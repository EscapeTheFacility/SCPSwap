using System.ComponentModel;
using PlayerRoles;

namespace SCPSwap_NWAPI
{
    public class Config
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        [Description("The duration, in seconds, before a swap request gets automatically deleted.")]
        public float RequestTimeout { get; set; } = 20f;
        
        [Description("The duration, in seconds, after the round starts that swap requests can be sent.")]
        public float SwapTimeout { get; set; } = 60f;

        [Description("A collection of roles blacklisted from being swapped to, and cannot send swap requests.")]
        public RoleTypeId[] BlacklistedScps { get; set; } =
        {
            RoleTypeId.Scp0492,
        };
    }
}