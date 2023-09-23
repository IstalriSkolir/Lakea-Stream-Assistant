namespace Battle_Similator.Models.Creatures
{
    public class Monster : Creature
    {
        private long xpValue;

        public long XPValue { get { return xpValue; } }

        public Monster(string name, string id, int level, int hp, int strength, int dexterity, int contitution)
        {
            this.name = name;
            this.id = id;
            this.level = level;
            this.hp = hp;
            this.hpMax = hp;
            this.strength = strength;
            this.dexterity = dexterity;
            this.constitution = contitution;
            this.xpValue = (level * 5);
            this.updateAbilityModifiers();
        }
    }
}
