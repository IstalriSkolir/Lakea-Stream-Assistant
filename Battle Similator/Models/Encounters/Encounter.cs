using Battle_Similator.Models.Creatures;

namespace Battle_Similator.Models.Encounters
{
    public class Encounter
    {
        private Character character;
        private Monster monster;
        private Random random;
        private string encounterType;

        public Encounter(Character character, Monster monster, string encounterType)
        {
            this.character = character;
            this.monster = monster;
            random = new Random();
            this.encounterType = encounterType;
        }

        public EncounterResult Run()
        {
            int range = character.DexterityMod + monster.DexterityMod + 1;
            while (monster.IsAlive && character.IsAlive)
            {
                int ran = random.Next(1, range);
                if(ran <= character.DexterityMod)
                {
                    hit(character, monster);
                }
                else
                {
                    hit(monster, character);
                }
            }
            if (character.IsAlive)
            {
                return new EncounterResult(character, monster, encounterType, character.ID, monster.XPValue);
            }
            else
            {
                return new EncounterResult(character, monster, encounterType, monster.ID, 0, false);
            }
        }

        private void hit(Creature attacker, Creature target)
        {
            int initialDamage = random.Next(1, attacker.StrengthMod);
            float modifier = (float)Math.Sqrt((float)attacker.Level / (float)target.Level);
            float totalDamage = (float)initialDamage * modifier;
            int actualDamage = (int)Math.Round(totalDamage);
            if(actualDamage < 1)
            {
                actualDamage = 1;
            }
            target.TakeDamage(actualDamage);
        }
    }
}
