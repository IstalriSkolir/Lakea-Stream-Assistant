namespace Battle_Similator.Models.Creatures
{
    public class Monster : Creature
    {
        private int xpValue;
        private string difficulty;

        public int XPValue { get { return xpValue; } }
        public string Difficulty {  get { return difficulty; } }

        public Monster(string name, string id, int level, int hp, int hpMax, int strength, int dexterity, int contitution, int xpValue)
        {
            this.name = name;
            this.id = id;
            this.level = level;
            this.hp = hp;
            this.hpMax = hpMax;
            this.strength = strength;
            this.dexterity = dexterity;
            this.constitution = contitution;
            this.xpValue = xpValue;
            this.isAlive = true;
            this.updateAbilityModifiers();
            this.determineDifficulty();
        }

        public override void TakeDamage(int damage)
        {
            this.hp -= damage;
            if (this.hp <= 0)
            {
                this.isAlive = false;
            }
        }

        private void determineDifficulty()
        {
            if (this.level <= 10)
            {
                this.difficulty = "WEAK";
            }
            else if (this.level <= 30)
            {
                this.difficulty = "NORMAL";
            }
            else if (this.level <= 50)
            {
                this.difficulty = "HARD";
            }
            else
            {
                this.difficulty = "RANDOM";
            }
        }
    }
}
