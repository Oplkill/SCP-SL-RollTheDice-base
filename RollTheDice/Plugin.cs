// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using Exiled.API.Features;
using System;
using Server = Exiled.Events.Handlers.Server;

namespace RollTheDice
{
    public class RTD : Plugin<Config, Translations>
    {
        public static RTD Instance;
        public override Version Version => new Version(0, 0, 1);
        public override string Author => "Oplkill";
        public override string Name => "RollTheDice base";
        public override string Prefix => "RTD base";

        internal static Rolls Rolls;

        private EventHandlers _eventHandler;

        /// <summary>
        /// Add roll to roll collection
        /// </summary>
        /// <param name="triggerString">String type of roll</param>
        /// <param name="rollType">Roll type</param>
        /// <param name="rollItem">Roll item</param>
        /// <param name="chanceWeight">Chance weight for roll</param>
        public void AddRoll(string triggerString, RollType rollType, IRollItem rollItem, uint chanceWeight = 1)
        {
            Rolls.AddRoll(triggerString, rollType, rollItem, chanceWeight);
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
            return Rolls.GetRoll(player, triggerString, goodChance);
        }

        public override void OnEnabled()
        {
            Rolls = new Rolls();
            Instance = this;
            RegisterEvents();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            UnRegisterEvents();
            Rolls = null;
            Instance = null;
            base.OnDisabled();
        }

        private void RegisterEvents()
        {
            _eventHandler = new EventHandlers();
            Server.RoundEnded += _eventHandler.OnRoundEnded;
        }

        private void UnRegisterEvents()
        {
            Server.RoundEnded -= _eventHandler.OnRoundEnded;
            _eventHandler = null;
        }
    }
}