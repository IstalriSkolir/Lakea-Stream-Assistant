namespace Battle_Similator.Models.Creatures
{
    public class Character : Creature
    {
        private int xp;
        private int nextLevel;
        private int deaths;
        private int monstersKilled;
        private int bossesFought;
        private int bossesBeaten;
        private float monsterWinRate;
        private bool resetOnDeath = true;

        public int XP { get { return xp; } }
        public int Deaths { get { return deaths; } }
        public int MonstersKilled { get { return monstersKilled; } }
        public int BossesFought { get { return bossesFought; } }
        public int BossesBeaten { get { return bossesBeaten; } }
        public float MonsterWinRate { get { return monsterWinRate; } }
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
            this.deaths = 0;
            this.monstersKilled = 0;
            this.bossesFought = 0;
            this.bossesBeaten = 0;
            this.isAlive = true;
            this.updateAbilityModifiers();
            this.calculateNextLevel();
        }

        public Character(string name, string id, int xp, int level, int hp, int str, int dex, int con)
        {
            this.name= name;
            this.id = id;
            this.xp = xp;
            this.level = level;
            this.hp = hp;
            this.hpMax = hp;
            this.strength = str;
            this.dexterity = dex;
            this.constitution = con;
            this.deaths = 0;
            this.monstersKilled = 0;
            this.bossesFought = 0;
            this.bossesBeaten = 0;
            this.isAlive = true;
            this.updateAbilityModifiers();
            this.calculateNextLevel();
        }

        public Character(Dictionary<string, string> props)
        {
            this.name = props["NAME"];
            this.id = props["ID"];
            this.xp = int.Parse(props["XP"]);
            this.level = int.Parse(props["LEVEL"]);
            this.hp = int.Parse(props["HP"]);
            this.hpMax = int.Parse(props["HP"]);
            this.strength = int.Parse(props["STR"]);
            this.dexterity = int.Parse(props["DEX"]);
            this.constitution = int.Parse(props["CON"]);
            if(props.ContainsKey("DEATHS")) { this.deaths = int.Parse(props["DEATHS"]); } else { this.deaths = 0; }
            if(props.ContainsKey("MONSTERS_KILLED")) { this.monstersKilled = int.Parse(props["MONSTERS_KILLED"]); } else { this.monstersKilled= 0; }
            if(props.ContainsKey("BOSSES_FOUGHT")) { this.bossesFought = int.Parse(props["BOSSES_FOUGHT"]); } else { this.bossesFought = 0; }
            if(props.ContainsKey("BOSSES_BEATEN")) { this.bossesBeaten = int.Parse(props["BOSSES_BEATEN"]); } else { this.bossesBeaten= 0; }
            if(props.ContainsKey("MONSTER_WIN_RATE")) { this.monsterWinRate = float.Parse(props["MONSTER_WIN_RATE"]); } else { this.monsterWinRate = 0; }
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

        public void NewMonsterKilled()
        {
            monstersKilled++;
        }

        public void NewBossFought()
        {
            bossesFought++;
        }

        public void NewBossBeaten()
        {
            bossesBeaten++;
        }

        public void UpdateMonsterWinRate()
        {
            float totalBattles = (float)(monstersKilled + deaths);
            monsterWinRate = (float)Math.Round(((monstersKilled / totalBattles) * 100), 1);
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
            }
            hpMax += rand.Next(1, ConstitutionMod + 1);
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
                deaths++;
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
