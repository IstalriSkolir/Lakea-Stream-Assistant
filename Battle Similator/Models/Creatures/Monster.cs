namespace Battle_Similator.Models.Creatures
{
    public class Monster : Creature
    {
        private int xpValue;

        public int XPValue { get { return xpValue; } }

        public Monster(string name, string id, int level, int hp, int hpMax, int strength, int dexterity, int contitution)
        {
            this.name = name;
            this.id = id;
            this.level = level;
            this.hp = hp;
            this.hpMax = hpMax;
            this.strength = strength;
            this.dexterity = dexterity;
            this.constitution = contitution;
            this.xpValue = (level * 5);
            this.isAlive = true;
            this.updateAbilityModifiers();
        }

        public override void TakeDamage(int damage)
        {
            this.hp -= damage;
            if (this.hp <= 0)
            {
                this.isAlive = false;
            }
        }
    }
}
