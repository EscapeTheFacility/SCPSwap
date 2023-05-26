using System;
using System.Linq;
using CommandSystem;
using NWAPIPermissionSystem;
using PluginAPI.Core;
using PlayerRoles;
using RemoteAdmin;
using SCPSwap_NWAPI.Models;

namespace SCPSwap_NWAPI.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ScpSwapParent : ParentCommand
    {
        public ScpSwapParent() => LoadGeneratedCommands();
        
        public override string Command => "scpswap";
        
        public override string[] Aliases { get; } = { "swap" };
        
        public override string Description => "Base command for ScpSwap.";
        
        public sealed override void LoadGeneratedCommands()
        {
            RegisterCommand(new Accept());
            RegisterCommand(new Cancel());
            RegisterCommand(new Decline());
            RegisterCommand(new List());
        }
        
        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get((PlayerCommandSender)sender);
            if (player == null)
            {
                response = "This command must be executed at the game level.";
                return false;
            }

            if (!Round.IsRoundStarted)
            {
                response = "The round has not yet started.";
                return false;
            }

            if (!player.CheckPermission("scpswap.swap"))
            {
                response = "You do not have access to this command.";
                return false;
            }

            if (Round.Duration > TimeSpan.FromSeconds(Plugin.Instance.Config.SwapTimeout))
            {
                response = "The swap period has ended.";
                return false;
            }

            if (arguments.IsEmpty())
            {
                response = $"Usage: .{Command} ScpNumber";
                return false;
            }

            if (player.Team != Team.SCPs)
            {
                response = "You must be an SCP to use this command.";
                return false;
            }

            if (Plugin.Instance.Config.BlacklistedScps.Contains(player.Role))
            {
                response = "You cannot swap as this SCP.";
                return false;
            }

            if (Swap.FromSender(player) != null)
            {
                response = "You already have a pending swap request!";
                return false;
            }

            Player receiver = GetReceiver(arguments.At(0), out Action<Player> spawnMethod);
            if (player == receiver)
            {
                response = "You can't swap with yourself.";
                return false;
            }

            if (spawnMethod == null)
            {
                response = "Unable to find the specified role. Please refer to the list command for available roles.";
                return false;
            }
            
            if (receiver != null)
            {
                if (Plugin.Instance.Config.BlacklistedScps.Contains(receiver.Role))
                {
                    response = "You cannot swap to this SCP.";
                    return false;
                }
                Swap.Send(player, receiver);
                response = "Request sent!";
                return true;
            }
            
            if (player.CheckPermission("scpswap.any"))
            {
                spawnMethod(player);
                response = "Swap successful.";
                return true;
            }

            response = "Unable to locate a player with the requested role.";
            return false;
        }

        private static Player GetReceiver(string request, out Action<Player> spawnMethod)
        {
            RoleTypeId roleSwap = ValidSwaps.Get(request);
            if (roleSwap != RoleTypeId.None)
            {
                spawnMethod = player => player.SetRole(roleSwap);
                return Player.GetPlayers().FirstOrDefault(player => player.Role == roleSwap);
            }

            spawnMethod = null;
            return null;
        }
    }
}