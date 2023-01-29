using System;
using PluginAPI.Core;
using UnityEngine;

namespace SCPSwap_NWAPI.Models
{
    
    /// <summary>
    /// Container to make translations for console messages more fun.
    /// </summary>
    [Serializable]
    public class ConsoleMessage
    {
        public ConsoleMessage()
        {
        }

        public ConsoleMessage(string message, string color)
        {
            Message = message;
            Color = color;
        }
        
        public string Message { get; set; }
        
        public string Color { get; set; }
        
        public void SendTo(Player player) => player.SendConsoleMessage(Message, Color);
    }

    /// <summary>
    /// Container to store broadcast information in config.
    /// </summary>
    [Serializable]
    public class GameBroadcastMessage
    {
        public GameBroadcastMessage()
        {
            
        }

        public GameBroadcastMessage(string message, ushort duration)
        {
            Message = message;
            Duration = duration;
        }
        
        public string Message { get; set; }
        
        public ushort Duration { get; set; }
    }
}