using Battle_Similator.Models.Creatures;

namespace Lakea_Tester.Battle_Simulator
{
    [TestClass]
    public class Monster_Tests
    {
        public Monster Monster1;
        public Monster Monster2;
        public Monster Monster3;

        public Monster_Tests()
        {
            Monster1 = new Monster("Monster1", "ID1", 1, 20, 20, 9, 9, 9, 20);
            Monster2 = new Monster("Monster2", "ID2", 3, 26, 26, 12, 9, 10, 60);
            Monster3 = new Monster("Monster3", "ID3", 5, 30, 30, 12, 12, 11, 100);
        }

        [TestMethod]
        public void CreateMonsterTest()
        {
            Assert.AreEqual("Monster1", Monster1.Name, "Monster1.Name");
            Assert.AreEqual("Monster2", Monster2.Name, "Monster2.Name");
            Assert.AreEqual("Monster3", Monster3.Name, "Monster3.Name");
            Assert.AreEqual("ID1", Monster1.ID, "Monster1.ID");
            Assert.AreEqual("ID2", Monster2.ID, "Monster2.ID");
            Assert.AreEqual("ID3", Monster3.ID, "Monster3.ID");
            Assert.AreEqual(1, Monster1.Level, "Monster1.Level");
            Assert.AreEqual(3, Monster2.Level, "Monster2.Level");
            Assert.AreEqual(5, Monster3.Level, "Monster3.Level");
            Assert.AreEqual(20, Monster1.HP, "Monster1.HP");
            Assert.AreEqual(26, Monster2.HP, "Monster2.HP");
            Assert.AreEqual(30, Monster3.HP, "Monster3.HP");
            Assert.AreEqual(20, Monster1.HPMax, "Monster1.HPMax");
            Assert.AreEqual(26, Monster2.HPMax, "Monster2.HPMax");
            Assert.AreEqual(30, Monster3.HPMax, "Monster3.HPMax");
            Assert.AreEqual(9, Monster1.Strength, "Monster1.Strength");
            Assert.AreEqual(12, Monster2.Strength, "Monster2.Strength");
            Assert.AreEqual(12, Monster3.Strength, "Monster3.Strength");
            Assert.AreEqual(9, Monster1.Dexterity, "Monster1.Dexterity");
            Assert.AreEqual(9, Monster2.Dexterity, "Monster2.Dexterity");
            Assert.AreEqual(12, Monster3.Dexterity, "Monster3.Dexterity");
            Assert.AreEqual(9, Monster1.Constitution, "Monster1.Constitution");
            Assert.AreEqual(10, Monster2.Constitution, "Monster2.Constitution");
            Assert.AreEqual(11, Monster3.Constitution, "Monster3.Constitution");
            Assert.AreEqual(3, Monster1.StrengthMod, "Monster1.StrengthMod");
            Assert.AreEqual(4, Monster2.StrengthMod, "Monster2.StrengthMod");
            Assert.AreEqual(4, Monster3.StrengthMod, "Monster3.StrengthMod");
            Assert.AreEqual(3, Monster1.DexterityMod, "Monster1.DexterityMod");
            Assert.AreEqual(3, Monster2.DexterityMod, "Monster2.DexterityMod");
            Assert.AreEqual(4, Monster3.DexterityMod, "Monster3.DexterityMod");
            Assert.AreEqual(3, Monster1.ConstitutionMod, "Monster1.ConstitutionMod");
            Assert.AreEqual(3, Monster2.ConstitutionMod, "Monster2.ConstitutionMod");
            Assert.AreEqual(3, Monster3.ConstitutionMod, "Monster3.ConstitutionMod");
            Assert.AreEqual(20, Monster1.XPValue, "Monster1.XPValue");
            Assert.AreEqual(60, Monster2.XPValue, "Monster2.XPValue");
            Assert.AreEqual(100, Monster3.XPValue, "Monster3.XPValue");
        }

        [TestMethod]
        public void MonsterTakeDamageTest()
        {
            Monster1.TakeDamage(10);
            Monster2.TakeDamage(25);
            Monster3.TakeDamage(5);
            Monster3.TakeDamage(20);
            Assert.AreEqual(10, Monster1.HP, "Monster1.HP");
            Assert.IsTrue(Monster1.IsAlive, "Monster1.IsAlive");
            Assert.AreEqual(1, Monster2.HP, "Monster2.HP");
            Assert.IsTrue(Monster2.IsAlive, "Monster2.IsAlive");
            Assert.AreEqual(5, Monster3.HP, "Monster3.HP");
            Assert.IsTrue(Monster3.IsAlive, "Monster3.IsAlive");
        }

        [TestMethod]
        public void MonsterDeathTest()
        {
            Monster1 = new Monster("Monster1", "ID1", 1, 20, 20, 9, 9, 9, 20);
            Monster2 = new Monster("Monster2", "ID2", 3, 26, 26, 12, 9, 10, 60);
            Monster3 = new Monster("Monster3", "ID3", 5, 30, 30, 12, 12, 11, 100);
            Monster1.TakeDamage(20);
            Monster2.TakeDamage(26);
            Monster3.TakeDamage(30);
            Assert.AreEqual(0, Monster1.HP, "Monster1.HP");
            Assert.IsFalse(Monster1.IsAlive, "Monster1.IsAlive");
            Assert.AreEqual(0, Monster2.HP, "Monster2.HP");
            Assert.IsFalse(Monster2.IsAlive, "Monster2.IsAlive");
            Assert.AreEqual(0, Monster3.HP, "Monster3.HP");
            Assert.IsFalse(Monster3.IsAlive, "Monster3.IsAlive");
        }
    }
}
