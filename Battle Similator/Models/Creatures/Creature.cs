namespace Battle_Similator.Models.Creatures
{
    public abstract class Creature
    {
        protected private string name, id;
        protected private int level, hp, hpMax, strength, strengthMod, dexterity, dexterityMod, constitution, constitutionMod;
        protected private bool isAlive;

        public string Name { get { return name; } }
        public string ID { get { return id; } }
        public int Level { get { return level; } }
        public int HP { get { return hp; } }
        public int HPMax { get { return hpMax; } }
        public int Strength { get { return strength; } }
        public int Dexterity { get { return dexterity; } }
        public int Constitution { get { return constitution; } }
        public int StrengthMod { get { return strengthMod; } }
        public int DexterityMod { get { return dexterityMod; } }
        public int ConstitutionMod { get { return constitutionMod; } }
        public bool IsAlive { get {  return isAlive; } }

        public abstract void TakeDamage(int damage);

        protected private void updateAbilityModifiers()
        {
            this.strengthMod = this.strength / 3;
            this.dexterityMod = this.dexterity / 3;
            this.constitutionMod = this.constitution / 3;
        }
    }
}
