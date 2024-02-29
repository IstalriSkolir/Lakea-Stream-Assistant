using Battle_Similator.Models.Creatures;
using Battle_Similator.Models.Encounters;

namespace Lakea_Tester.Battle_Simulator
{
    [TestClass]
    public class Encounter_Tests
    {
        public int RandomSeed;

        public Encounter_Tests()
        {
            RandomSeed = 100;
        }

        [TestMethod]
        public void EncounterRunLoopTest()
        {
            int runCount = 100;
            Random random = new Random();
            for(int i = 0;  i < runCount; i++)
            {
                Character character = new Character("Character", "Character_ID");
                character.IncreaseXP(random.Next(30, 971550));
                Monster monster = new Monster("Monster", "Monster_ID", random.Next(1, 255), random.Next(20, 3000), random.Next(20, 3000), random.Next(9, 300), 
                    random.Next(9, 300), random.Next(9, 300), 20 * random.Next(1, 255));
                Encounter encounter = new Encounter(character, monster, "TESTING");
                EncounterResult result = encounter.Run();
                Assert.IsNotNull(result, "result");
                Assert.IsNotNull(result.Character, "result.Character");
                Assert.IsNotNull(result.Monster, "result.Monster");
                Assert.IsTrue((result.Winner == result.Character.ID || result.Winner == result.Monster.ID), "result.Winner");
                if(result.Winner == result.Character.ID)
                {
                    Assert.AreEqual(result.Monster.XPValue, result.XPGained, "result.XPGained");
                    Assert.IsTrue(result.Character.IsAlive, "result.Character.IsAlive");
                    Assert.IsFalse(result.Monster.IsAlive, "result.Monster.IsAlive");
                }
                else if(result.Winner == result.Monster.ID)
                {
                    Assert.AreEqual(0, result.XPGained, "result.XPGained");
                    Assert.IsTrue(result.Monster.IsAlive, "result.Monster.IsAlive");
                    Assert.IsFalse(result.Character.IsAlive, "result.Character.IsAlive");
                }
            }
        }

        [TestMethod]
        public void EncounterRunKnownSeed()
        {
            Character character = new Character("Character", "Character_ID", 300, 5, 30, 12, 12, 11);
            Monster monster = new Monster("Monster", "Monster_ID", 5, 30, 30, 12, 12, 11, 100);
            Encounter encounter = new Encounter(character, monster, "TESTING", RandomSeed);
            EncounterResult result = encounter.Run();
            Assert.IsNotNull(result, "result");
            Assert.IsNotNull(result.Character, "result.Character");
            Assert.IsNotNull(result.Monster, "result.Monster");
            Assert.AreEqual(result.Winner, result.Monster.ID, "result.Winner");
            Assert.IsTrue(result.Monster.IsAlive, "result.Monster.IsAlive");
            Assert.IsFalse(result.Character.IsAlive, "result.Character.IsAlive");
            Assert.IsTrue(result.Monster.HP > 0, "result.Monster.HP");
            Assert.IsTrue(result.Character.HP <= 0, "result.Character.HP");
        }

        [TestMethod]
        public void EncounterCharacterWins()
        {
            Character character = new Character("Character", "Character_ID", 0, 50, 300, 120, 120, 110);
            Monster monster = new Monster("Monster", "Monster_ID", 5, 30, 30, 12, 12, 11, 100);
            Encounter encounter = new Encounter(character, monster, "TESTING", RandomSeed);
            EncounterResult result = encounter.Run();
            Assert.IsNotNull(result, "result");
            Assert.IsNotNull(result.Character, "result.Character");
            Assert.IsNotNull(result.Monster, "result.Monster");
            Assert.AreEqual(result.Winner, result.Character.ID, "result.Winner");
            Assert.IsTrue(result.Character.IsAlive, "result.Character.IsAlive");
            Assert.IsFalse(result.Monster.IsAlive, "result.Monster.IsAlive");
            Assert.IsTrue(result.Character.HP > 0, "result.Character.HP");
            Assert.IsTrue(result.Monster.HP <= 0, "result.Monster.HP");
            Assert.AreEqual(100, result.XPGained, "result.XPGained");
            Assert.IsFalse(result.LevelUp, "result.LevelUp");
        }

        [TestMethod]
        public void EncounterMonsterWins()
        {
            Character character = new Character("Character", "Character_ID");
            Monster monster = new Monster("Monster", "Monster_ID", 50, 300, 300, 30, 30, 30, 1000);
            Encounter encounter = new Encounter(character, monster, "TESTING", RandomSeed);
            EncounterResult result = encounter.Run();
            Assert.IsNotNull(result, "result");
            Assert.IsNotNull(result.Character, "result.Character");
            Assert.IsNotNull(result.Monster, "result.Monster");
            Assert.AreEqual(result.Winner, result.Monster.ID, "result.Winner");
            Assert.IsTrue(result.Monster.IsAlive, "result.Monster.IsAlive");
            Assert.IsFalse(result.Character.IsAlive, "result.Character.IsAlive");
            Assert.IsTrue(result.Monster.HP > 0, "result.Monster.HP");
            Assert.IsTrue(result.Character.HP <= 0, "result.Character.HP");
            Assert.AreEqual(0, result.XPGained, "result.XPGained");
            Assert.IsFalse(result.LevelUp, "result.LevelUp");
        }

        [TestMethod]
        public void EncounterCharacterOverLevel100()
        {
            Character character = new Character("Character", "Character_ID", 0, 200, 1000, 100, 100, 100);
            Monster monster = new Monster("Monster", "Monster_ID", 150, 1000, 1000, 100, 100, 100, 0);
            Encounter encounter = new Encounter(character, monster, "TESTING", RandomSeed);
            EncounterResult result = encounter.Run();
            Assert.IsNotNull(result, "result");
            Assert.IsNotNull(result.Character, "result.Character");
            Assert.IsNotNull(result.Monster, "result.Monster");
            Assert.AreEqual(result.Winner, result.Monster.ID, "result.Winner");
            Assert.IsTrue(result.Monster.IsAlive, "result.Monster.IsAlive");
            Assert.IsFalse(result.Character.IsAlive, "result.Character.IsAlive");
            Assert.IsTrue(result.Monster.HP > 0, "result.Monster.HP");
            Assert.IsTrue(result.Character.HP <= 0, "result.Character.HP");
            Assert.IsFalse(result.LevelUp, "result.LevelUp");
        }
    }
}
