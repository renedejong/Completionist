namespace Completionist.MonsterSlayer
{
    using System.Collections.Generic;
    using System.Linq;
    using StardewModdingAPI.Events;
    using StardewValley;
    using StardewValley.Monsters;

    public class MonsterSlayerTracker
    {
        // These names and numbers are derived from StardewValley.Locations.AdventureGuild.areAllMonsterSlayerQuestsComplete() 
        private static readonly string[] slimes = new[] { "Green Slime", "Frost Jelly", "Sludge" };
        private static readonly string[] voidSpirits = new[] { "Shadow Guy", "Shadow Shaman", "Shadow Brute" };
        private static readonly string[] skeletons = new[] { "Skeleton", "Skeleton Mage" };
        private static readonly string[] caveInsects = new[] { "Grub", "Fly", "Bug" };
        private static readonly string[] bats = new[] { "Bat", "Frost Bat", "Lava Bat" };
        private static readonly string[] duggies = new[] { "Duggy" };
        private static readonly string[] dustSprites = new[] { "Dust Spirit" };

        private readonly Babelfish translator;
        private readonly Messaging messaging;
        private readonly Dictionary<string, string> monsterNames;

        public const int MaxNumberOfCaveInsects = 125;
        public const int MaxNumberOfBats = 200;
        public const int MaxNumberOfDuggies = 30;
        public const int MaxNumberOfDustSpirits = 500;
        public const int MaxNumberOfSlimes = 1000;
        public const int MaxNumberOfSkeletons = 50;
        public const int MaxNumberOfVoidSpirits = 150;

        /// <summary>
        /// Monster slaying goal tracker
        /// </summary>
        /// <param name="messaging">the HUD messaging client</param>
        /// <param name="translator">the translation client</param>
        public MonsterSlayerTracker(Messaging messaging, Babelfish translator)
        {
            this.messaging = messaging;
            this.translator = translator;

            Count = GetCount();

            // Initialize a local dictionary with translated monster names
            monsterNames = new Dictionary<string, string>
            {
                ["bats"] = translator.Bats,
                ["cave insects"] = translator.CaveInsects,
                ["duggies"] = translator.Duggies,
                ["dust sprites"] = translator.DustSprites,
                ["skeletons"] = translator.Skeletons,
                ["slimes"] = translator.Slimes,
                ["void spirits"] = translator.VoidSpirits
            };
        }

        /// <summary>
        /// Gets or sets the amount of monsters slain by kind
        /// </summary>
        public SlayerCount Count { get; set; }

        /// <summary>
        /// Check if any NPCs were removed that are classified as monster, if there are, update the kill count statistics
        /// </summary>
        /// <param name="e">the event data</param>
        public void OnNpcListChanged(NpcListChangedEventArgs e)
        {
            if (e.Removed.Any(n => n is Monster))
            {
                CheckForSlainMonsters();
            }
        }

        /// <summary>
        /// Query the game stats for the kill counts of the monster slaying goals.
        /// Displays a message if the player gained a kill towards a goal.
        /// </summary>
        public void CheckForSlainMonsters()
        {
            var newCount = GetCount();

            // Inline function to reduce a bunch of slightly differing if-blocks.
            void diff(int oldAmount, int newAmount, int max, string name)
            {
                // Only add a line if the amount of monsters slain differs, and doesn't exceed the monster slayer goal yet
                if (oldAmount < newAmount && oldAmount < max)
                {
                    // Show an on-screen message, along the lines of "killed <x> of <y> <monster>"
                    messaging.ShowMessage(translator.NumberOfMonstersSlain(newAmount, max, monsterNames[name]), name);
                }
            }

            diff(Count.Bats, newCount.Bats, MaxNumberOfBats, "bats");
            diff(Count.CaveInsects, newCount.CaveInsects, MaxNumberOfCaveInsects, "cave insects");
            diff(Count.Duggies, newCount.Duggies, MaxNumberOfDuggies, "duggies");
            diff(Count.DustSprites, newCount.DustSprites, MaxNumberOfDustSpirits, "dust sprites");
            diff(Count.Skeletons, newCount.Skeletons, MaxNumberOfSkeletons, "skeletons");
            diff(Count.Slimes, newCount.Slimes, MaxNumberOfSlimes, "slimes");
            diff(Count.VoidSpirits, newCount.VoidSpirits, MaxNumberOfVoidSpirits, "void spirits");

            Count = newCount;
        }

        /// <summary>
        /// Get the latest monster slayer kill counts
        /// </summary>
        /// <returns></returns>
        private static SlayerCount GetCount()
        {
            // Iterate through a collection of monster names, get their individual kill counts and sum that
            int sumKills(string[] names) => names.Sum(n => Game1.stats.getMonstersKilled(n));

            return new SlayerCount
            {
                Bats = sumKills(bats),
                CaveInsects = sumKills(caveInsects),
                Duggies = sumKills(duggies),
                DustSprites = sumKills(dustSprites),
                Skeletons = sumKills(skeletons),
                Slimes = sumKills(slimes),
                VoidSpirits = sumKills(voidSpirits)
            };
        }
    }
}
