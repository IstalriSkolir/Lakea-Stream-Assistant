using Battle_Similator.Models.Creatures;

namespace Battle_Similator.Models.Encounters
{
    public class Encounter
    {
        private Character character;
        private Monster monster;
        private Random random;
        private string encounterType;

        public Encounter(Character character, Monster monster, string encounterType, int randomSeed = -1)
        {
            this.character = character;
            this.monster = monster;
            this.encounterType = encounterType;
            if(randomSeed != -1)
            {
                random = new Random(randomSeed);
            }
            else
            {
                random = new Random();
            }
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
            int halfStrengthMod = attacker.StrengthMod / 2;
            int roll1 = random.Next(1, halfStrengthMod);
            int roll2 = random.Next(1, halfStrengthMod);
            int initialDamage = roll1 + roll2;
            float modifier = (float)Math.Sqrt((float)getCreatureLevel(attacker) / (float)getCreatureLevel(target));
            float totalDamage = (float)initialDamage * modifier;
            int actualDamage = (int)Math.Round(totalDamage);
            if(actualDamage < 1)
            {
                actualDamage = 1;
            }
            target.TakeDamage(actualDamage);
        }

        private int getCreatureLevel(Creature creature)
        {
            if(creature.GetType() == typeof(Character))
            {
                if(creature.Level <= 100)
                {
                    return creature.Level;
                }
                else
                {
                    return 100;
                }
            }
            else
            {
                return creature.Level;
            }
        }
    }
}
