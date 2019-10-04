namespace Completionist
{
    using System.Collections.Generic;
    using System.Linq;
    using StardewValley;

    /// <summary>
    /// Wrapper for adding messages to the HUD.
    /// </summary>
    /// <remarks>
    /// Its intended use is showing a message when a monster is killed towards the monster slaying goals.
    /// Rather than showing separate messages when the player kills the same type of mob in quick succession, this class keeps track of existing on-screen messages and updates those as necessary
    /// </remarks>
    public class Messaging
    {
        private readonly Dictionary<string, HUDMessage> messages;

        public Messaging()
        {
            messages = new Dictionary<string, HUDMessage>();
        }

        public void ShowMessage(string message, string kind)
        {
            if (messages.ContainsKey(kind) && messages[kind].timeLeft > 0)
            {
                messages[kind].timeLeft = 3500;
                messages[kind].message = message;
            }
            else
            {
                var msg = new HUDMessage(message, HUDMessage.achievement_type);
                messages[kind] = msg;
                Game1.addHUDMessage(msg); // Not a big fan of the direct dependency on the Game1 class, may want to inject a reference to the method at a later point
            }
        }

        public void PruneMessages()
        {
            var keysToRemove = messages.Keys.Where(k => messages[k].timeLeft <= 0).ToArray();
            foreach (var key in keysToRemove)
            {
                messages.Remove(key);
            }
        }
    }
}
