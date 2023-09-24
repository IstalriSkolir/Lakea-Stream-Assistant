using Battle_Similator.Models.Creatures;

namespace Battle_Similator.Models.Encounters
{
    public class EncounterResult
    {
        private Character character;
        private Monster monster;
        private string winner;
        private int xpGained;

        public Character Character { get { return character; } }
        public Monster Monster { get { return monster; } }
        public string Winner { get { return winner; } }
        public int XPGained { get { return xpGained; } }

        public EncounterResult(Character character, Monster monster, string winner, int xpGained)
        {
            this.character = character;
            this.monster = monster;
            this.winner = winner;
            this.xpGained = xpGained;
        }
    }
}
