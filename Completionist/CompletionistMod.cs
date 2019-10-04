namespace Completionist
{
    using System.Linq;
    using Completionist.MonsterSlayer;
    using StardewModdingAPI;
    using StardewModdingAPI.Events;
    using StardewValley.Monsters;

    public class CompletionistMod : Mod
    {

        private MonsterSlayerTracker killTracker;
        private Messaging messaging;
        private Babelfish translator;

        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.SaveLoaded += InitializeModState;
            helper.Events.World.NpcListChanged += World_NpcListChanged;
            helper.Events.GameLoop.UpdateTicked += (s, e) => messaging?.PruneMessages();
            translator = new Babelfish(helper.Translation);
        }

        /// <summary>
        /// Determine whether a monster on the 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void World_NpcListChanged(object sender, NpcListChangedEventArgs e)
        {
            if (Context.IsWorldReady &&  e.Removed.Where(n => n is Monster)?.Count() > 0)
            {
                killTracker.MonstersSlain();
            }
        }

        /// <summary>
        /// Sets or resets the local mod state when a savegame is loaded
        /// </summary>
        /// <param name="sender">the event trigger</param>
        /// <param name="e">the event parameters</param>
        private void InitializeModState(object sender, SaveLoadedEventArgs e)
        {
            messaging = new Messaging();
            killTracker = new MonsterSlayerTracker(messaging, translator);
        }
    }
}
