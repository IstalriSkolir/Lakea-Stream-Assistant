using Battle_Similator.Models.Creatures;

namespace Battle_Similator.Models.Encounters
{
    public class EncounterResult
    {
        private Character character;
        private Monster monster;
        private string encounterType;
        private string winner;
        private int xpGained;
        private bool levelUp;

        public Character Character { get { return character; } }
        public Monster Monster { get { return monster; } }
        public string EncounterType { get { return encounterType; } }
        public string Winner { get { return winner; } }
        public int XPGained { get { return xpGained; } }
        public bool LevelUp { get { return levelUp; } }

        public EncounterResult(Character character, Monster monster, string encounterType, string winner, int xpGained, bool levelUp)
        {
            this.character = character;
            this.monster = monster;
            this.encounterType = encounterType;
            this.winner = winner;
            this.xpGained = xpGained;
            this.levelUp = levelUp;
        }
    }
}
