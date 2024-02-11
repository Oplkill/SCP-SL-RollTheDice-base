// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace RollTheDice.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class GetSerial : ICommand, IUsageProvider
    {
        public string Command { get; } = "blablabla";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "Description";
        public string[] Usage { get; } = { "usage descr" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission("bc.getserial"))
            {
                response = "You do not have permission to use this command";
                return false;
            }

            response = "balblabla";


            return false;
        }
    }
}