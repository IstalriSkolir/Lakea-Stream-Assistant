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
            Assert.AreEqual("Char1", Char1.Name, "Char1.Name");
            Assert.AreEqual("Char2", Char2.Name, "Char2.Name");
            Assert.AreEqual(CharProps["NAME"], Char4.Name, "Char4.Name");
            Assert.AreEqual("ID1", Char1.ID, "Char1.ID");
            Assert.AreEqual("ID2", Char2.ID, "Char2.ID");
            Assert.AreEqual(CharProps["ID"], Char4.ID, "Char4.ID");
            Assert.AreEqual(0, Char1.XP, "Char1.XP");
            Assert.AreEqual(0, Char2.XP, "Char2.XP");
            Assert.AreEqual(int.Parse(CharProps["XP"]), Char4.XP, "Char4.XP");
            Assert.AreEqual(1, Char1.Level, "Char1.Level");
            Assert.AreEqual(1, Char2.Level, "Char2.Level");
            Assert.AreEqual(int.Parse(CharProps["LEVEL"]), Char4.Level, "Char4.Level");
            Assert.AreEqual(20, Char1.HP, "Char1.HP");
            Assert.AreEqual(20, Char2.HP, "Char2.HP");
            Assert.AreEqual(int.Parse(CharProps["HP"]), Char4.HP, "Char4.HP");
            Assert.AreEqual(20, Char1.HPMax, "Char1.HPMax");
            Assert.AreEqual(20, Char2.HPMax, "Char2.HPMax");
            Assert.AreEqual(int.Parse(CharProps["HP"]), Char4.HPMax, "Char4.HPMax");
            Assert.AreEqual(9, Char1.Strength, "Char1.Strength");
            Assert.AreEqual(9, Char2.Strength, "Char2.Strength");
            Assert.AreEqual(12, Char3.Strength, "Char3.Strength");
            Assert.AreEqual(int.Parse(CharProps["STR"]), Char4.Strength, "Char4.Strength");
            Assert.AreEqual(9, Char1.Dexterity, "Char1.Dexterity");
            Assert.AreEqual(9, Char2.Dexterity, "Char2.Dexterity");
            Assert.AreEqual(13, Char3.Dexterity, "Char3.Dexterity");
            Assert.AreEqual(int.Parse(CharProps["DEX"]), Char4.Dexterity, "Char4.Dexterity");
            Assert.AreEqual(9, Char1.Constitution, "Char1.Constitution");
            Assert.AreEqual(9, Char2.Constitution, "Char2.Constitution");
            Assert.AreEqual(12, Char3.Constitution, "Char3.Constitution");
            Assert.AreEqual(int.Parse(CharProps["CON"]), Char4.Constitution, "Char4.Constitution");
            Assert.AreEqual(3, Char1.StrengthMod, "Char1.StrengthMod");
            Assert.AreEqual(3, Char2.StrengthMod, "Char2.StrengthMod");
            Assert.AreEqual(4, Char3.StrengthMod, "Char3.StrengthMod");
            Assert.AreEqual(3, Char1.DexterityMod, "Char1.DexterityMod");
            Assert.AreEqual(3, Char2.DexterityMod, "Char2.DexterityMod");
            Assert.AreEqual(4, Char3.DexterityMod, "Char3.DexterityMod");
            Assert.AreEqual(3, Char1.ConstitutionMod, "Char1.ConstitutionMod");
            Assert.AreEqual(3, Char2.ConstitutionMod, "Char2.ConstitutionMod");
            Assert.AreEqual(4, Char3.ConstitutionMod, "Char3.ConstitutionMod");
            Assert.AreEqual(int.Parse(CharProps["DEATHS"]), Char4.Deaths, "Char4.Deaths");
            Assert.AreEqual(int.Parse(CharProps["MONSTERS_KILLED"]), Char4.MonstersKilled, "Char4.MonstersKilled");
            Assert.AreEqual(int.Parse(CharProps["BOSSES_FOUGHT"]), Char4.BossesFought, "Char4.BossesFought");
            Assert.AreEqual(int.Parse(CharProps["BOSSES_BEATEN"]), Char4.BossesBeaten, "Char4.BossesBeaten");
            Assert.AreEqual(float.Parse(CharProps["MONSTER_WIN_RATE"]), Char4.MonsterWinRate, "Char4.MonsterWinRate");
        }

        [TestMethod]
        public void CharacterTakeDamageTest()
        {
            Char1.TakeDamage(3);
            Char2.TakeDamage(5);
            Char2.TakeDamage(6);
            Char3.TakeDamage(30);
            Assert.AreEqual(17, Char1.HP, "Char3.HP");
            Assert.AreEqual(9, Char2.HP, "Char2.HP");
            Assert.AreEqual(5, Char3.HP, "Char3.HP");
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
            Assert.AreEqual(1, Char1.Level, "Char1.Level");
            Assert.AreEqual(5, Char1.XP, "Char1.XP");
            Assert.AreEqual(1, Char2.Level, "Char2.Level");
            Assert.AreEqual(10, Char2.XP, "Char2.XP");
            Assert.AreEqual(3, Char3.Level, "Char3.Level");
            Assert.AreEqual(125, Char3.XP, "Char3.XP");
        }

        [TestMethod]
        public void CharacterIncreaseXPLevelUpTest()
        {
            Char1 = new Character("Char1", "ID1", RandomSeed);
            Char2 = new Character("Char2", "ID2", 90, 3, 30, 10, 11, 10, RandomSeed);
            Char1.IncreaseXP(30);
            Char2.IncreaseXP(360);
            Assert.AreEqual(2, Char1.Level, "Char1.Level");
            Assert.AreEqual(30, Char1.XP, "Char1.XP");
            Assert.AreEqual(23, Char1.HPMax, "Char1.HPMax");
            Assert.AreEqual(10, Char1.Strength, "Char1.Strength");
            Assert.AreEqual(9, Char1.Dexterity, "Char1.Dexterity");
            Assert.AreEqual(10, Char1.Constitution, "Char1.Constitution");
            Assert.AreEqual(3, Char1.StrengthMod, "Char1.Strength");
            Assert.AreEqual(3, Char1.DexterityMod, "Char1.Dexterity");
            Assert.AreEqual(3, Char1.ConstitutionMod, "Char1.Constitution");
            Assert.AreEqual(6, Char2.Level, "Char2.Level");
            Assert.AreEqual(450, Char2.XP, "Char2.XP");
            Assert.AreEqual(39, Char2.HPMax, "Char2.HPMax");
            Assert.AreEqual(11, Char2.Strength, "Char2.Strength");
            Assert.AreEqual(13, Char2.Dexterity, "Char2.Dexterity");
            Assert.AreEqual(13, Char2.Constitution, "Char2.Constitution");
            Assert.AreEqual(3, Char2.StrengthMod, "Char2.StrengthMod");
            Assert.AreEqual(4, Char2.DexterityMod, "Char2.DexterityMod");
            Assert.AreEqual(4, Char2.ConstitutionMod, "Char2.ConstitutionMod");
        }

        [TestMethod]
        public void CharacterLevelUpOver100()
        {
            Character character = new Character("Character", "Character_ID", 148500, 100, 700, 80, 80, 80);
            character.IncreaseXP(3000);
            Assert.AreEqual(151500, character.XP, "character.XP");
            Assert.AreEqual(101, character.Level, "character.Level");
            Assert.AreEqual(700, character.HPMax, "character.HPMax");
            Assert.AreEqual(80, character.Strength, "character.Strength");
            Assert.AreEqual(80, character.Dexterity, "character.Dexterity");
            Assert.AreEqual(80, character.Constitution, "character.Constitution");
            Assert.AreEqual(26, character.StrengthMod, "character.StrengthMod");
            Assert.AreEqual(26, character.DexterityMod, "character.DexterityMod");
            Assert.AreEqual(26, character.ConstitutionMod, "character.Constitution");
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
            Assert.AreEqual(1, Char1.MonstersKilled, "Char1.MonstersKilled");
            Assert.AreEqual(16, Char2.MonstersKilled, "Char2.MonstersKilled");
            Assert.AreEqual(1, Char1.BossesFought, "Char1.BossesFought");
            Assert.AreEqual(4, Char2.BossesFought, "Char2.BossesFought");
            Assert.AreEqual(1, Char1.BossesBeaten, "Char1.BossesBeaten");
            Assert.AreEqual(2, Char2.BossesBeaten, "Char2.BossesBeaten");
            Assert.AreEqual(100, Char1.MonsterWinRate, "Char1.MonsterWinRate");
            Assert.AreEqual(76.2f, Char2.MonsterWinRate, "Char2.MonsterWinRate");
        }
    }
}
