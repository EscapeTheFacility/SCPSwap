using System;
using CommandSystem;
using PluginAPI.Core;
using SCPSwap_NWAPI.Models;

namespace SCPSwap_NWAPI.Commands
{
    public class Accept : ICommand
    {
        public string Command { get; set; } = "accept";
        
        public string[] Aliases { get; set; } = { "yes", "y" };
        
        public string Description { get; set; } = "Accepts an active swap request.";
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player playerSender = Player.Get(sender);
            if (playerSender == null)
            {
                response = "This command must be from the game level.";
                return false;
            }

            Swap swap = Swap.FromReceiver(playerSender);
            if (swap == null)
            {
                response = "You do not have a pending swap request.";
                return false;
            }

            swap.Run();
            response = "Swap successful!";
            return true;
        }
    }
}