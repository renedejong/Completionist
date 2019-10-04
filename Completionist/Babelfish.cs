namespace Completionist
{
    using StardewModdingAPI;

    /// <summary>
    /// Translation (i18n) helper
    /// </summary>
    public class Babelfish
    {
        private readonly ITranslationHelper translator;

        public Babelfish(ITranslationHelper translator)
        {
            this.translator = translator;
        }

        public Translation NumberOfMonstersSlain(int amountSlain, int amountNeeded, string monsterName) => translator.Get("monsters.slain", (amountSlain, amountNeeded, monsterName));

        public Translation Bats => translator.Get("monster.bats");
        public Translation CaveInsects => translator.Get("monster.cave-insects");
        public Translation Duggies => translator.Get("monster.duggies");
        public Translation DustSprites => translator.Get("monster.dust-sprites");
        public Translation Slimes => translator.Get("monster.slimes");
        public Translation Skeletons => translator.Get("monster.skeletons");
        public Translation VoidSpirits => translator.Get("monster.void-spirits");
    }
}
