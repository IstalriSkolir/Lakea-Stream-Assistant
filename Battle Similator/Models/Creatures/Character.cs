namespace Battle_Similator.Models.Creatures
{
    public class Character : Creature
    {
        private int xp;
        private int nextLevel;
        private bool resetOnDeath = true;

        public int XP { get { return xp; } }
        public bool ResetOnDeath { get { return resetOnDeath; } set { resetOnDeath = value; } }

        public Character(string name, string id)
        {
            this.name = name;
            this.id = id;
            this.level = 1;
            this.xp = 0;
            this.hp = 20;
            this.hpMax = this.hp;
            this.strength = 9;
            this.dexterity = 9;
            this.constitution = 9;
            this.isAlive = true;
            this.updateAbilityModifiers();
            this.calculateNextLevel();
        }

        public Character(string name, string id, int xp, int level, int hp, int strength, int dexterity, int contitution)
        {
            this.name = name;
            this.id = id;
            this.xp = xp;
            this.level = level;
            this.hp = hp;
            this.hpMax = hp;
            this.strength = strength;
            this.dexterity = dexterity;
            this.constitution = contitution;
            this.isAlive = true;
            this.updateAbilityModifiers();
            this.calculateNextLevel();
        }

        public override void TakeDamage(int damage)
        {
            this.hp -= damage;
            if (this.hp <= 0)
            {
                death();
            }
        }

        public bool IncreaseXP(int value)
        {
            bool leveledUp = false;
            xp += value;
            if(xp >= nextLevel)
            {
                levelUp();
                leveledUp = true;
            }
            return leveledUp;
        }

        private void calculateNextLevel()
        {
            nextLevel = 0;
            for(int count = 1; count <= level; count++)
            {
                nextLevel += count * 30;
            }
        }

        private void levelUp()
        {
            Random rand = new Random();
            level++;
            for(int point = 0; point < 2; point++)
            {
                int ran = rand.Next(0, 3);
                switch(ran)
                {
                    case 0:
                        strength++;
                        break;
                    case 1:
                        dexterity++;
                        break;
                    case 2:
                        constitution++;
                        break;
                }
                updateAbilityModifiers();
                hpMax += rand.Next(1, ConstitutionMod);
            }
            calculateNextLevel();
            if(xp >= nextLevel)
            {
                levelUp();
            }
        }

        private void death()
        {
            isAlive = false;
            if (resetOnDeath)
            {
                level = 1;
                xp = 300;
                hpMax = 20;
                strength = 9;
                dexterity = 9;
                constitution = 9;
                levelUp();
            }
        }
    }
}
