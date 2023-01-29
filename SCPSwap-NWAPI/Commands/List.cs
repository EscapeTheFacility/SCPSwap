using System;
using System.Text;
using CommandSystem;
using NorthwoodLib.Pools;

namespace SCPSwap_NWAPI.Commands
{
    public class List : ICommand
    {
        public string Command { get; set; } = "list";
        
        public string[] Aliases { get; set; } = { "l" };
        
        public string Description { get; set; } = "Lists all valid swappable roles.";
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
            stringBuilder.AppendLine("Available Roles:");
            stringBuilder.Append(string.Join(Environment.NewLine, ValidSwaps.Names));
            response = StringBuilderPool.Shared.ToStringReturn(stringBuilder);
            return true;
        }
    }
}