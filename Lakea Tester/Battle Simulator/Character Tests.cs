using Battle_Similator.Models.Creatures;

namespace Lakea_Tester.Battle_Simulator
{
    [TestClass]
    public class Character_Tests
    {
        public int RandomSeed;
        Character Char1;
        Character Char2;
        Character Char3;
        Character Char4;
        Dictionary<string, string> CharProps = new Dictionary<string, string>()
        {
            { "NAME", "CHARDICT" },
            { "ID", "ID4" },
            { "LEVEL", "5" },
            { "XP", "300" },
            { "HP", "32" },
            { "STR", "12" },
            { "DEX", "13" },
            { "CON", "14" },
            { "DEATHS", "5" },
            { "MONSTERS_KILLED", "15" },
            { "BOSSES_FOUGHT", "3" },
            { "BOSSES_BEATEN", "1" },
            { "MONSTER_WIN_RATE", "75" }
        };

        public Character_Tests()
        {
            Char1 = new Character("Char1", "ID1");
            Char2 = new Character("Char2", "ID2", 0, 1, 20, 9, 9, 9);
            Char3 = new Character("Char3", "ID3", 300, 5, 35, 12, 13, 12);
            Char4 = new Character(CharProps);
            RandomSeed = 100;
        }

        [TestMethod]
        public void CreateCharactersTest()
        {
            Char4 = new Character(CharProps);
            Assert.AreEqual("Char1", Char1.Name, "Char1.Name: " + Char1.Name + ", Expected: Char1");
            Assert.AreEqual("Char2", Char2.Name, "Char2.Name: " + Char2.Name + ", Expected: Char2");
            Assert.AreEqual(CharProps["NAME"], Char4.Name, "Char4.Name: " + Char4.Name + ", Expected: " + CharProps["NAME"]);
            Assert.AreEqual("ID1", Char1.ID, "Char1.ID: " + Char1.ID + ", Expected: ID1");
            Assert.AreEqual("ID2", Char2.ID, "Char2.ID: " + Char2.ID + ", Expected: ID2");
            Assert.AreEqual(CharProps["ID"], Char4.ID, "Char4.ID: " + Char4.ID + ", Expected: " + CharProps["ID"]);
            Assert.AreEqual(0, Char1.XP, "Char1.XP: " + Char1.XP + ", Expected: 0");
            Assert.AreEqual(0, Char2.XP, "Char2.XP: " + Char2.XP + ", Expected: 0");
            Assert.AreEqual(int.Parse(CharProps["XP"]), Char4.XP, "Char4.XP: " + Char4.XP + ", Expected: " + CharProps["XP"]);
            Assert.AreEqual(1, Char1.Level, "Char1.Level: " + Char1.Level + ", Expected: 1");
            Assert.AreEqual(1, Char2.Level, "Char2.Level: " + Char2.Level + ", Expected: 1");
            Assert.AreEqual(int.Parse(CharProps["LEVEL"]), Char4.Level, "Char4.Level: " + Char4.Level + ", Expected: " + CharProps["LEVEL"]);
            Assert.AreEqual(20, Char1.HP, "Char1.HP: " + Char1.HP + ", Expected: 20");
            Assert.AreEqual(20, Char2.HP, "Char2.HP: " + Char2.HP + ", Expected: 20");
            Assert.AreEqual(int.Parse(CharProps["HP"]), Char4.HP, "Char4.HP: " + Char4.HP + ", Expected: " + CharProps["HP"]);
            Assert.AreEqual(20, Char1.HPMax, "Char1.HPMax: " + Char1.HPMax + ", Expected: 20");
            Assert.AreEqual(20, Char2.HPMax, "Char2.HPMax: " + Char2.HPMax + ", Expected: 20");
            Assert.AreEqual(int.Parse(CharProps["HP"]), Char4.HPMax, "Char4.HPMax: " + Char4.HPMax + ", Expected: " + CharProps["HP"]);
            Assert.AreEqual(9, Char1.Strength, "Char1.Strength: " + Char1.Strength + ", Expected: 9");
            Assert.AreEqual(9, Char2.Strength, "Char2.Strength: " + Char2.Strength + ", Expected: 9");
            Assert.AreEqual(12, Char3.Strength, "Char3.Strength: " + Char3.Strength + ", Expected: 12");
            Assert.AreEqual(int.Parse(CharProps["STR"]), Char4.Strength, "Char4.Strength: " + Char4.Strength + ", Expected: " + CharProps["STR"]);
            Assert.AreEqual(9, Char1.Dexterity, "Char1.Dexterity: " + Char1.Dexterity + ", Expected: 9");
            Assert.AreEqual(9, Char2.Dexterity, "Char2.Dexterity: " + Char2.Dexterity + ", Expected: 9");
            Assert.AreEqual(13, Char3.Dexterity, "Char3.Dexterity: " + Char3.Dexterity + ", Expected: 13");
            Assert.AreEqual(int.Parse(CharProps["DEX"]), Char4.Dexterity, "Char4.Dexterity: " + Char4.Dexterity + ", Expected: " + CharProps["DEX"]);
            Assert.AreEqual(9, Char1.Constitution, "Char1.Constitution: " + Char1.Constitution + ", Expected: 9");
            Assert.AreEqual(9, Char2.Constitution, "Char2.Constitution: " + Char2.Constitution + ", Expected: 9");
            Assert.AreEqual(12, Char3.Constitution, "Char3.Constitution: " + Char3.Constitution + ", Expected: 12");
            Assert.AreEqual(int.Parse(CharProps["CON"]), Char4.Constitution, "Char4.Constitution: " + Char4.Constitution + ", Expected: " + CharProps["CON"]);
            Assert.AreEqual(3, Char1.StrengthMod, "Char1.StrengthMod: " + Char1.StrengthMod + ", Expected: 3");
            Assert.AreEqual(3, Char2.StrengthMod, "Char2.StrengthMod: " + Char2.StrengthMod + ", Expected: 3");
            Assert.AreEqual(4, Char3.StrengthMod, "Char3.StrengthMod: " + Char3.StrengthMod + ", Expected: 4");
            Assert.AreEqual(3, Char1.DexterityMod, "Char1.DexterityMod: " + Char1.DexterityMod + ", Expected: 3");
            Assert.AreEqual(3, Char2.DexterityMod, "Char2.DexterityMod: " + Char2.DexterityMod + ", Expected: 3");
            Assert.AreEqual(4, Char3.DexterityMod, "Char3.DexterityMod: " + Char3.DexterityMod + ", Expected: 4");
            Assert.AreEqual(3, Char1.ConstitutionMod, "Char1.ConstitutionMod: " + Char1.ConstitutionMod + ", Expected: 3");
            Assert.AreEqual(3, Char2.ConstitutionMod, "Char2.ConstitutionMod: " + Char2.ConstitutionMod + ", Expected: 3");
            Assert.AreEqual(4, Char3.ConstitutionMod, "Char3.ConstitutionMod: " + Char3.ConstitutionMod + ", Expected: 4");
            Assert.AreEqual(int.Parse(CharProps["DEATHS"]), Char4.Deaths, "Char4.Deaths: " + Char4.Deaths + ", Expected: " + CharProps["DEATHS"]);
            Assert.AreEqual(int.Parse(CharProps["MONSTERS_KILLED"]), Char4.MonstersKilled, "Char4.MonstersKilled: " + Char4.MonstersKilled + ", Expected: " + CharProps["MONSTERS_KILLED"]);
            Assert.AreEqual(int.Parse(CharProps["BOSSES_FOUGHT"]), Char4.BossesFought, "Char4.BossesFought: " + Char4.BossesFought + ", Expected: " + CharProps["BOSSES_FOUGHT"]);
            Assert.AreEqual(int.Parse(CharProps["BOSSES_BEATEN"]), Char4.BossesBeaten, "Char4.BossesBeaten: " + Char4.BossesBeaten + ", Expected: " + CharProps["BOSSES_BEATEN"]);
            Assert.AreEqual(float.Parse(CharProps["MONSTER_WIN_RATE"]), Char4.MonsterWinRate, "Char4.MonsterWinRate: " + Char4.MonsterWinRate + ", Expected: " + CharProps["MONSTER_WIN_RATE"]);
        }

        [TestMethod]
        public void CharacterTakeDamageTest()
        {
            Char1.TakeDamage(3);
            Char2.TakeDamage(5);
            Char2.TakeDamage(6);
            Char3.TakeDamage(30);
            Assert.AreEqual(17, Char1.HP, "Char3.HP: " + Char1.HP + ", Expected: 17");
            Assert.AreEqual(9, Char2.HP, "Char2.HP: " + Char2.HP + ", Expected: 9");
            Assert.AreEqual(5, Char3.HP, "Char3.HP: " + Char3.HP + ", Expected: 5");
        }

        [TestMethod]
        public void CharacterDeathNoResetTest()
        {
            Character character = new Character("Character", "Character_ID", 300, 5, 30, 12, 12, 12);
            character.ResetOnDeath = false;
            character.TakeDamage(30);
            Assert.IsFalse(character.IsAlive, "character.Alive");
            Assert.AreEqual(0, character.Deaths, "character.Deaths");
            Assert.AreEqual(5, character.Level, "character.Level");
            Assert.AreEqual(300, character.XP, "character.XP");
            Assert.AreEqual(30, character.HPMax, "character.MaxHP");
            Assert.AreEqual(12, character.Strength, "character.Strength");
            Assert.AreEqual(12, character.Dexterity, "character.Dexterity");
            Assert.AreEqual(12, character.Constitution, "character.Constitution");
            Assert.AreEqual(4, character.StrengthMod, "character.StrengthMod");
            Assert.AreEqual(4, character.DexterityMod, "character.DexterityMod");
            Assert.AreEqual(4, character.ConstitutionMod, "character.ConstitutionMod");
        }

        [TestMethod]
        public void CharacterDeathResetLevel10()
        {
            Character character = new Character("Character", "Character_ID", 1350, 10, 50, 15, 15, 15, RandomSeed);
            character.TakeDamage(50);
            Assert.IsFalse(character.IsAlive, "character.IsAlive");
            Assert.AreEqual(1, character.Deaths, "character.Deaths");
            Assert.AreEqual(9, character.Level, "character.Level");
            Assert.AreEqual(1145, character.XP, "character.XP");
            Assert.AreEqual(47, character.HPMax, "character.HPMax");
            Assert.AreEqual(14, character.Strength, "character.Strength");
            Assert.AreEqual(15, character.Dexterity, "character.Dexterity");
            Assert.AreEqual(14, character.Constitution, "character.Constitution");
        }

        [TestMethod]
        public void CharacterDeathResetLevel30()
        {
            Character character = new Character("Character", "Character_ID", 13050, 30, 100, 25, 25, 25, RandomSeed);
            character.TakeDamage(100);
            Assert.IsFalse(character.IsAlive, "character.IsAlive");
            Assert.AreEqual(1, character.Deaths, "character.Deaths");
            Assert.AreEqual(25, character.Level, "character.Level");
            Assert.AreEqual(9135, character.XP, "character.XP");
            Assert.AreEqual(73, character.HPMax, "character.HPMax");
            Assert.AreEqual(23, character.Strength, "character.Strength");
            Assert.AreEqual(21, character.Dexterity, "character.Dexterity");
            Assert.AreEqual(21, character.Constitution, "character.Constition");
        }

        [TestMethod]
        public void CharacterDeathResetLevel60()
        {
            Character character = new Character("Character", "Character_ID", 53100, 60, 300, 40, 40, 40, RandomSeed);
            character.TakeDamage(300);
            Assert.IsFalse(character.IsAlive, "character.IsAlive");
            Assert.AreEqual(1, character.Deaths, "character.Deaths");
            Assert.AreEqual(42, character.Level, "character.Level");
            Assert.AreEqual(26550, character.XP, "character.XP");
            Assert.AreEqual(179, character.HPMax, "character.HPMax");
            Assert.AreEqual(33, character.Strength, "character.Strength");
            Assert.AreEqual(24, character.Dexterity, "character.Dexterity");
            Assert.AreEqual(27, character.Constitution, "character.Constition");
        }

        [TestMethod]
        public void CharacterIncreaseXPNoLevelUpTest()
        {
            Char1 = new Character("Char1", "ID1");
            Char2 = new Character("Char2", "ID2");
            Char3 = new Character("Char3", "ID3", 90, 3, 30, 10, 11, 10);
            Char1.IncreaseXP(5);
            Char2.IncreaseXP(10);
            Char3.IncreaseXP(10);
            Char3.IncreaseXP(25);
            Assert.AreEqual(1, Char1.Level, "Char1.Level: " + Char1.Level + ", Expected: 1");
            Assert.AreEqual(5, Char1.XP, "Char1.XP: " + Char1.XP + ", Expected: 5");
            Assert.AreEqual(1, Char2.Level, "Char2.Level: " + Char2.Level + ", Expected: 1");
            Assert.AreEqual(10, Char2.XP, "Char2.XP: " + Char2.XP + ", Expected: 10");
            Assert.AreEqual(3, Char3.Level, "Char3.Level: " + Char3.Level + ", Expected: 3");
            Assert.AreEqual(125, Char3.XP, "Char3.XP: " + Char3.XP + ", Expected: 125");
        }

        [TestMethod]
        public void CharacterIncreaseXPLevelUpTest()
        {
            Char1 = new Character("Char1", "ID1");
            Char2 = new Character("Char2", "ID2");
            Char3 = new Character("Char3", "ID3", 90, 3, 30, 10, 11, 10);
            Char1.IncreaseXP(30);
            Char2.IncreaseXP(180);
            Char3.IncreaseXP(90);
            Assert.AreEqual(2, Char1.Level, "Char1.Level: " + Char1.Level + ", Expected: 2");
            Assert.AreEqual(30, Char1.XP, "Char1.XP: " + Char1.XP + ", Expected: 30");
            Assert.IsTrue(Char1.HPMax >= 21 && Char1.HPMax <= 24, "Char1.HPMax: " + Char1.HPMax + ", Expected: 21>x>24");
            int abilityTotal = Char1.Strength + Char1.Dexterity + Char1.Constitution;
            Assert.AreEqual(29, abilityTotal, "Char1 Ability Total: " + abilityTotal + ", Expected: 29, Str: " + Char1.Strength + ", Dex: " +
                Char1.Dexterity + ", Con: " + Char1.Constitution);
            int abilityModTotal = Char1.StrengthMod + Char1.DexterityMod + Char1.ConstitutionMod;
            Assert.IsTrue(abilityModTotal >= 9 && abilityModTotal <= 10, "Char1 Ability Mod Total: " + abilityModTotal + ", Expected: 9>x>10, StrMod: " + 
                Char1.StrengthMod + ", DexMod: " + Char1.DexterityMod + ", ConMod: " + Char1.ConstitutionMod);
            Assert.AreEqual(4, Char2.Level, "Char2.Level: " + Char2.Level + ", Expected: 4");
            Assert.AreEqual(180, Char2.XP, "Char2.XP: " + Char2.XP + ", Expected: 180");
            Assert.IsTrue(Char2.HPMax >= 23 && Char2.HPMax <= 31, "Char2.HPMax: " + Char2.HPMax + ", Expected: 23>x>31");
            abilityTotal = Char2.Strength + Char2.Dexterity + Char2.Constitution;
            Assert.AreEqual(33, abilityTotal, "Char2 Ability Total: " + abilityTotal + ", Expected: 33, Str: " + Char2.Strength + ", Dex: " +
                Char2.Dexterity + ", Con: " + Char2.Constitution);
            abilityModTotal = Char2.StrengthMod + Char2.DexterityMod + Char2.ConstitutionMod;
            Assert.IsTrue(abilityModTotal >= 9 && abilityModTotal <= 11, "Char2 Ability Mod Total: " + abilityModTotal + ", Expected: 9>x>11, StrMod: " + 
                Char2.StrengthMod + ", DexMod: " + Char2.DexterityMod + ", ConMod: " + Char2.ConstitutionMod);
            Assert.AreEqual(4, Char3.Level, "Char3.Level: " + Char3.Level + ", Expected: 4");
            Assert.AreEqual(180, Char3.XP, "Char3.XP: " + Char3.XP + ", Expected: 180");
            Assert.IsTrue(Char3.HPMax >= 31 && Char3.HPMax <= 35, "Char.HPMax: " + Char3.HPMax + ", Expected: 31>x>35");
            abilityTotal = Char3.Strength + Char3.Dexterity + Char3.Constitution;
            Assert.AreEqual(33, abilityTotal, "Char3 Ability Total: " + abilityTotal + ", Expected: 33, Str: " + Char3.Strength + ", Dex: " +
                Char3.Dexterity + ", Con: " + Char3.Constitution);
            abilityModTotal = Char3.StrengthMod + Char3.DexterityMod + Char3.ConstitutionMod;
            Assert.IsTrue(abilityModTotal >= 9 && abilityModTotal <= 10, "Char3 Ability Mod Total: " + abilityModTotal + ", Expected: 9>x>11, StrMod: " +
                Char3.StrengthMod + ", DexMod: " + Char3.DexterityMod + ", ConMod: " + Char3.ConstitutionMod);
        }

        [TestMethod]
        public void CharacterLevelUpOver100()
        {
            Character character = new Character("Character", "Character_ID", 148500, 100, 700, 80, 80, 80);
            character.IncreaseXP(3000);
            Assert.AreEqual(151500, character.XP, "character.XP: " + character.XP + ", Expected: 151500");
            Assert.AreEqual(101, character.Level, "character.Level: " + character.Level + ", Expected: 101");
            Assert.AreEqual(700, character.HPMax, "character.HPMax: " + character.HPMax + ", Expected: 700");
            Assert.AreEqual(80, character.Strength, "character.Strength: " + character.Strength + ", Expected: 80");
            Assert.AreEqual(80, character.Dexterity, "character.Dexterity: " + character.Dexterity + ", Expected: 80");
            Assert.AreEqual(80, character.Constitution, "character.Constitution: " + character.Constitution + ", Expected: 80");
        }

        [TestMethod]
        public void CharacterStatisticsTest()
        {
            Char1 = new Character("Char1", "ID1");
            Char2 = new Character(CharProps);
            Char1.NewMonsterKilled();
            Char2.NewMonsterKilled();
            Char1.NewBossFought();
            Char2.NewBossFought();
            Char1.NewBossBeaten();
            Char2.NewBossBeaten();
            Char1.UpdateMonsterWinRate();
            Char2.UpdateMonsterWinRate();
            Assert.AreEqual(1, Char1.MonstersKilled, "Char1.MonstersKilled: " + Char1.MonstersKilled + ", Expected: 1");
            Assert.AreEqual(16, Char2.MonstersKilled, "Char2.MonstersKilled: " + Char2.MonstersKilled + ", Expected: " + (int.Parse(CharProps["MONSTERS_KILLED"]) + 1));
            Assert.AreEqual(1, Char1.BossesFought, "Char1.BossesFought: " + Char1.BossesFought + ", Expected: 1");
            Assert.AreEqual(4, Char2.BossesFought, "Char2.BossesFought: " + Char2.BossesFought + ", Expected: " + (int.Parse(CharProps["BOSSES_FOUGHT"]) + 1));
            Assert.AreEqual(1, Char1.BossesBeaten, "Char1.BossesBeaten: " + Char1.BossesBeaten + ", Expected: 1");
            Assert.AreEqual(2, Char2.BossesBeaten, "Char2.BossesBeaten: " + Char2.BossesBeaten + ", Expected: " + (int.Parse(CharProps["BOSSES_BEATEN"]) + 1));
            Assert.AreEqual(100, Char1.MonsterWinRate, "Char1.MonsterWinRate: " + Char1.MonsterWinRate + ", Expected: 100");
            Assert.AreEqual(76.2f, Char2.MonsterWinRate, "Char2.MonsterWinRate: " + Char2.MonsterWinRate + ", Expected: 76.2");
        }
    }
}
