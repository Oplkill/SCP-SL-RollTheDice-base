// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using Exiled.Events.EventArgs.Server;

namespace RollTheDice
{
    internal class EventHandlers
    {
        private readonly Rolls _rolls = RTD.Rolls;

        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            _rolls.Clear();
        }
    }
}