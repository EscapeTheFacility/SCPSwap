using System;
using CommandSystem;
using PluginAPI.Core;
using SCPSwap_NWAPI.Models;

namespace SCPSwap_NWAPI.Commands
{
    public class Cancel : ICommand
    {
        public string Command { get; set; } = "cancel";
        
        public string[] Aliases { get; set; } = { "c" };
        
        public string Description { get; set; } = "Cancels an active swap request.";
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player playerSender = Player.Get(sender);
            if (playerSender == null)
            {
                response = "This command must be from the game level.";
                return false;
            }

            Swap swap = Swap.FromSender(playerSender);
            if (swap == null)
            {
                response = "You do not have an active swap request.";
                return false;
            }

            swap.Cancel();
            response = "Swap request cancelled!";
            return true;
        }
    }
}