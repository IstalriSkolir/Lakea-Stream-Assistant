namespace Battle_Similator.Models.Creatures
{
    public class Character : Creature
    {
        private long xp;

        public long XP { get { return xp; } }

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
            this.updateAbilityModifiers();
        }

        public Character(string name, string id, long xp, int level, int hp, int strength, int dexterity, int contitution)
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
            this.updateAbilityModifiers();
        }

        public void IncreaseXP(long value)
        {
            this.xp += value;
        }
    }
}
