// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using Exiled.API.Features;

namespace RollTheDice
{
    /// <summary>
    /// Roll interface
    /// </summary>
    public interface IRollItem
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">Player which roll for</param>
        /// <returns>True - roll can be rolled, False - not</returns>
        bool CanBeRolled(Player player);

        /// <summary>
        /// Execute roll
        /// </summary>
        /// <param name="player">Player which roll for</param>
        void CallRoll(Player player);
    }
}
