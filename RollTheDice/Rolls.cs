// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;

namespace RollTheDice
{
    /// <summary>
    /// Class that represence roll with its chance weight
    /// </summary>
    internal class Roll
    {
        /// <summary>
        /// Chance weight of the roll
        /// </summary>
        public uint ChanceWeight;

        /// <summary>
        /// Roll item
        /// </summary>
        public IRollItem RollItem;

        public Roll(uint chanceWeight, IRollItem rollItem)
        {
            this.ChanceWeight = chanceWeight;
            this.RollItem = rollItem;
        }
    }

    /// <summary>
    /// Container of all rolls and roll controller
    /// </summary>
    internal class Rolls
    {
        /// <summary>
        /// Randomizer
        /// </summary>
        private readonly System.Random _rd = new System.Random();

        /// <summary>
        /// Container of all rolls
        /// </summary>
        private Dictionary<string, Dictionary<RollType, List<Roll>>> _rolls;

        /// <summary>
        /// Contructor with initialize all variables
        /// </summary>
        public Rolls()
        {
            Log.Debug($"constructor Rolls {this}");
            _rolls = new Dictionary<string, Dictionary<RollType, List<Roll>>>();
        }

        /// <summary>
        /// Clear rolls
        /// </summary>
        public void Clear()
        {
            Log.Debug($"Cleaning rolls!");
            _rolls.Clear();
        }

        /// <summary>
        /// Add roll to roll collection
        /// </summary>
        /// <param name="triggerString">String type of roll</param>
        /// <param name="rollType">Roll type</param>
        /// <param name="rollItem">Roll item</param>
        /// <param name="chanceWeight">Chance weight for roll</param>
        public void AddRoll(string triggerString, RollType rollType, IRollItem rollItem, uint chanceWeight = 1)
        {
            Log.Debug($"Adding roll {triggerString}, {rollType}, {chanceWeight}");

            if (!_rolls.ContainsKey(triggerString))
                _rolls.Add(triggerString, new Dictionary<RollType, List<Roll>>());

            if (!_rolls[triggerString].ContainsKey(rollType))
                _rolls[triggerString].Add(rollType, new List<Roll>());

            _rolls[triggerString][rollType].Add(new Roll(chanceWeight, rollItem));

            Log.Debug($"size {_rolls[triggerString][rollType].Count}");
        }

        /// <summary>
        /// Get random roll
        /// </summary>
        /// <param name="player">Player which roll for</param>
        /// <param name="triggerString">String type of roll</param>
        /// <param name="goodChance">Chance to get good roll</param>
        /// <returns>Roll item or null if no items to roll</returns>
        public IRollItem GetRoll(Player player, string triggerString, float goodChance)
        {
            var rollType = GetRandomRollType(goodChance);
            var rollItem = GetRandomRollItem(player, triggerString, rollType);

            return rollItem;
        }

        /// <summary>
        /// Get random choosed roll item to roll
        /// </summary>
        /// <param name="player">Player which roll for</param>
        /// <param name="triggerString">String type of roll</param>
        /// <param name="rollType">Roll type</param>
        /// <returns>Roll item or null if no items to roll</returns>
        private IRollItem GetRandomRollItem(Player player, string triggerString, RollType rollType)
        {
            Log.Debug("Starting to call CanBeRolled of all rolls");
            var rolls = GetCanBeRolledRolls(player, triggerString, rollType);

            if (rolls.IsEmpty())
            {
                Log.Debug($"Trigger item {triggerString} roll type {rollType}. No can be rolled roll items.");
                return null;
            }

            if (rolls.Count == 1)
                return rolls.First().RollItem;

            uint weightsSum = (uint)rolls.Sum(roll => roll.ChanceWeight);
            uint randomNum = (uint)_rd.Next(0, (int)weightsSum + 1);

            foreach (var roll in rolls)
            {
                if (randomNum <= roll.ChanceWeight)
                    return roll.RollItem;

                randomNum -= roll.ChanceWeight;
            }

            Log.Debug($"Trigger item {triggerString} roll type {rollType}. Impossible code reached with weights sum {weightsSum} and total random num {randomNum}.");
            return null;
        }

        /// <summary>
        /// Get list of rolls that can be rolled at that moment
        /// </summary>
        /// <param name="player">Player which roll for</param>
        /// <param name="triggerString">String type of roll</param>
        /// <param name="rollType">Roll type</param>
        /// <returns>List of rolls</returns>
        private List<Roll> GetCanBeRolledRolls(Player player, string triggerString, RollType rollType)
        {
            var list = new List<Roll>();

            Log.Debug($"1---{triggerString}");

            Log.Debug($"***--{_rolls.Count}");

            if (!_rolls.ContainsKey(triggerString))
                return list;

            Log.Debug($"2---{rollType}");

            if (!_rolls[triggerString].ContainsKey(rollType))
                return list;

            Log.Debug($"size {_rolls[triggerString][rollType].Count}");

            foreach (var roll in _rolls[triggerString][rollType])
            {
                Log.Debug($"calling---{roll}");
                if (roll.RollItem.CanBeRolled(player))
                    list.Add(roll);
            }

            return list;
        }

        /// <summary>
        /// Pick randomly roll type
        /// </summary>
        /// <param name="goodChance">Chance to good roll</param>
        /// <returns>Picked roll type</returns>
        private RollType GetRandomRollType(float goodChance)
        {
            Log.Debug($"chance {goodChance}");

            if (_rd.NextDouble() <= goodChance)
                return RollType.Good;

            return RollType.Bad;
        }
    }
}
