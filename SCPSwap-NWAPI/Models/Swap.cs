using System.Collections.Generic;
using MEC;
using PlayerRoles;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;

namespace SCPSwap_NWAPI.Models
{
    /// <summary>
    /// Handles the swapping of players.
    /// </summary>
    public class Swap
    {
        private static readonly List<Swap> Swaps = new List<Swap>();
        private static readonly List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();
        private CoroutineHandle coroutine;

        private Swap(Player sender, Player receiver)
        {
            Sender = sender;
            Receiver = receiver;

            SendRequestMessages();
            coroutine = Timing.RunCoroutine(RunTimeout());
            Coroutines.Add(coroutine);
            EventManager.RegisterEvents<Swap>(Plugin.Instance);
        }

        public Swap()
        {
            
        }
        
        /// <summary>
        /// Gets the sender of the swap request.
        /// </summary>
        public Player Sender { get; }

        /// <summary>
        /// Gets the person who was sent the swap request.
        /// </summary>
        public Player Receiver { get; }
        
        /// <summary>
        /// Gets a swap request based on the sender.
        /// </summary>
        /// <param name="player">The sender of the request.</param>
        /// <returns>The sent swap request or null if one isn't found.</returns>
        public static Swap FromSender(Player player)
        {
            foreach (Swap swap in Swaps)
            {
                if (swap.Sender == player)
                    return swap;
            }

            return null;
        }
        
        /// <summary>
        /// Gets a swap request based on the receiver.
        /// </summary>
        /// <param name="player">The receiver of the request.</param>
        /// <returns>The sent swap request or null if one isn't found.</returns>
        public static Swap FromReceiver(Player player)
        {
            foreach (Swap swap in Swaps)
            {
                if (swap.Receiver == player)
                    return swap;
            }

            return null;
        }
        
        /// <summary>
        /// Creates a new instance of the <see cref="Swap"/> class with the given Sender and Receiver.
        /// </summary>
        /// <param name="sender">The sender of the request.</param>
        /// <param name="receiver">The receiver of the request.</param>
        public static void Send(Player sender, Player receiver)
        {
            Swaps.Add(new Swap(sender, receiver));
        }
        
        /// <summary>
        /// Clears all active swap requests.
        /// </summary>
        public static void Clear()
        {
            Swaps.Clear();
            foreach (CoroutineHandle coroutine in Coroutines)
                Timing.KillCoroutines(coroutine);

            Coroutines.Clear();
        }
        
        /// <summary>
        /// Performs the swap between the <see cref="Sender"/> and the <see cref="Receiver"/>.
        /// </summary>
        public void Run()
        {
            PartiallyDestroy();
            SwapData senderData = new SwapData(Sender);
            SwapData receiverData = new SwapData(Receiver);

            senderData.Swap(Receiver);
            receiverData.Swap(Sender);

            Plugin.Instance.Messages.SwapSuccessful.SendTo(Sender);
            Plugin.Instance.Messages.SwapSuccessful.SendTo(Receiver);
            Swaps.Remove(this);
        }
        
        /// <summary>
        /// Broadcasts the swap cancellation then destroys the swap.
        /// </summary>
        public void Cancel()
        {
            Sender.SendBroadcast("Swap request cancelled!", 5, shouldClearPrevious: true);
            Destroy();
        }
        
        /// <summary>
        /// Broadcasts the swap decline then destroys the swap.
        /// </summary>
        public void Decline()
        {
            Sender.SendBroadcast($"{Receiver.DisplayNickname ?? Receiver.Nickname} has declined your swap request.", 5, shouldClearPrevious: true);
            Destroy();
        }
        
        private void PartiallyDestroy()
        {
            if (coroutine.IsRunning)
                Timing.KillCoroutines(coroutine);

            EventManager.UnregisterEvents<Swap>(Plugin.Instance);
        }

        private void Destroy()
        {
            PartiallyDestroy();
            Swaps.Remove(this);
        }

        private void SendRequestMessages()
        {
            string consoleMessage = Plugin.Instance.Messages.RequestConsoleMessage.Message;
            consoleMessage = consoleMessage.Replace("$SenderName", Sender.DisplayNickname ?? Sender.Nickname);
            consoleMessage = consoleMessage.Replace("$RoleName", Sender.Role.ToString());
            Receiver.SendConsoleMessage(consoleMessage, Plugin.Instance.Messages.RequestConsoleMessage.Color);
            Receiver.SendBroadcast(Plugin.Instance.Messages.RequestBroadcast.Message, Plugin.Instance.Messages.RequestBroadcast.Duration);
        }

        [PluginEvent(ServerEventType.PlayerChangeRole)]
        private void OnChangingRole(Player player, PlayerRoleBase oldRole, RoleTypeId newRole, RoleChangeReason reason)
        {
            if (player == Sender || player == Receiver)
                Cancel();
        }

        private IEnumerator<float> RunTimeout()
        {
            yield return Timing.WaitForSeconds(Plugin.Instance.Config.RequestTimeout);
            Plugin.Instance.Messages.TimeoutSender.SendTo(Sender);
            Plugin.Instance.Messages.TimeoutReceiver.SendTo(Receiver);
            Destroy();
        }
    }
}