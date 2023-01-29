using PlayerRoles;
using PluginAPI.Core;
using UnityEngine;

namespace SCPSwap_NWAPI.Models
{
    /// <summary>
    /// A container to swap data between players.
    /// </summary>
    public class SwapData
    {
        private readonly RoleTypeId role;
        private readonly Vector3 position;
        private readonly float health;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SwapData"/> class.
        /// </summary>
        /// <param name="player">The player to generate the data from.</param>
        public SwapData(Player player)
        {
            role = player.Role;
            position = player.Position;
            health = player.Health;
        }

        /// <summary>
        /// Spawns a player with the contained swap data.
        /// </summary>
        /// <param name="player">The player to swap.</param>
        public void Swap(Player player)
        {
            player.SetRole(role);

            player.Position = position;
            player.Health = health;
        }
    }
}