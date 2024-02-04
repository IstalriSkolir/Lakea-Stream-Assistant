using Battle_Similator.Models.Creatures;

namespace Lakea_Tester.Battle_Simulator
{
    [TestClass]
    public class Character_Tests
    {
        Character Char1;
        Character Char2;
        Character Char3;

        public Character_Tests()
        {
            Char1 = new Character("Char1", "ID1");
            Char2 = new Character("Char2", "ID2", 0, 1, 20, 9, 9, 9);
            Char3 = new Character("Char3", "ID3", 300, 5, 35, 12, 13, 12);
        }

        [TestMethod]
        public void CreateCharactersTest()
        {
            Assert.AreEqual("Char1", Char1.Name, "Char1.Name: " + Char1.Name + ", Expected: Char1");
            Assert.AreEqual("Char2", Char2.Name, "Char2.Name: " + Char2.Name + ", Expected: Char2");
            Assert.AreEqual("ID1", Char1.ID, "Char1.ID: " + Char1.ID + ", Expected: ID1");
            Assert.AreEqual("ID2", Char2.ID, "Char2.ID: " + Char2.ID + ", Expected: ID2");
            Assert.AreEqual(0, Char1.XP, "Char1.XP: " + Char1.XP + ", Expected: 0");
            Assert.AreEqual(0, Char2.XP, "Char2.XP: " + Char2.XP + ", Expected: 0");
            Assert.AreEqual(1, Char1.Level, "Char1.Level: " + Char1.Level + ", Expected: 1");
            Assert.AreEqual(1, Char2.Level, "Char2.Level: " + Char2.Level + ", Expected: 1");
            Assert.AreEqual(20, Char1.HP, "Char1.HP: " + Char1.HP + ", Expected: 20");
            Assert.AreEqual(20, Char2.HP, "Char2.HP: " + Char2.HP + ", Expected: 20");
            Assert.AreEqual(20, Char1.HPMax, "Char1.HPMax: " + Char1.HPMax + ", Expected: 20");
            Assert.AreEqual(20, Char2.HPMax, "Char2.HPMax: " + Char2.HPMax + ", Expected: 20");
            Assert.AreEqual(9, Char1.Strength, "Char1.Strength: " + Char1.Strength + ", Expected: 9");
            Assert.AreEqual(9, Char2.Strength, "Char2.Strength: " + Char2.Strength + ", Expected: 9");
            Assert.AreEqual(12, Char3.Strength, "Char3.Strength: " + Char3.Strength + ", Expected: 12");
            Assert.AreEqual(9, Char1.Dexterity, "Char1.Dexterity: " + Char1.Dexterity + ", Expected: 9");
            Assert.AreEqual(9, Char2.Dexterity, "Char2.Dexterity: " + Char2.Dexterity + ", Expected: 9");
            Assert.AreEqual(13, Char3.Dexterity, "Char3.Dexterity: " + Char3.Dexterity + ", Expected: 13");
            Assert.AreEqual(9, Char1.Constitution, "Char1.Constitution: " + Char1.Constitution + ", Expected: 9");
            Assert.AreEqual(9, Char2.Constitution, "Char2.Constitution: " + Char2.Constitution + ", Expected: 9");
            Assert.AreEqual(12, Char3.Constitution, "Char3.Constitution: " + Char3.Constitution + ", Expected: 12");
            Assert.AreEqual(3, Char1.StrengthMod, "Char1.StrengthMod: " + Char1.StrengthMod + ", Expected: 3");
            Assert.AreEqual(3, Char2.StrengthMod, "Char2.StrengthMod: " + Char2.StrengthMod + ", Expected: 3");
            Assert.AreEqual(4, Char3.StrengthMod, "Char3.StrengthMod: " + Char3.StrengthMod + ", Expected: 4");
            Assert.AreEqual(3, Char1.DexterityMod, "Char1.DexterityMod: " + Char1.DexterityMod + ", Expected: 3");
            Assert.AreEqual(3, Char2.DexterityMod, "Char2.DexterityMod: " + Char2.DexterityMod + ", Expected: 3");
            Assert.AreEqual(4, Char3.DexterityMod, "Char3.DexterityMod: " + Char3.DexterityMod + ", Expected: 4");
            Assert.AreEqual(3, Char1.ConstitutionMod, "Char1.ConstitutionMod: " + Char1.ConstitutionMod + ", Expected: 3");
            Assert.AreEqual(3, Char2.ConstitutionMod, "Char2.ConstitutionMod: " + Char2.ConstitutionMod + ", Expected: 3");
            Assert.AreEqual(4, Char3.ConstitutionMod, "Char3.ConstitutionMod: " + Char3.ConstitutionMod + ", Expected: 4");
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
        public void CharacterDeathTest()
        {
            Char1 = new Character("Char1", "ID1");
            Char2 = new Character("Char2", "ID2", 300, 10, 40, 15, 17, 15);
            Char3 = new Character("Char3", "ID3", 300, 10, 40, 15, 17, 15);
            Char2.ResetOnDeath = false;
            Char1.TakeDamage(20);
            Char2.TakeDamage(40);
            Char3.TakeDamage(40);
            Assert.IsFalse(Char1.IsAlive, "Char1.IsAlive: " + Char1.IsAlive + ", Expected: False");
            Assert.IsFalse(Char2.IsAlive, "Char2.IsAlive: " + Char2.IsAlive + ", Expected: False");
            Assert.AreEqual(10, Char2.Level, "Char2.Level: " + Char2.Level + ", Expected: 10");
            Assert.IsFalse(Char3.IsAlive, "Char3.IsAlive: " + Char3.IsAlive + ", Expected: False");
            Assert.AreEqual(5, Char3.Level, "Char3.Level: " + Char3.Level + ", Expected: 5");
            Assert.AreEqual(300, Char3.XP, "Char3.XP: " + Char3.XP + ", Expected: 300");
            Assert.IsTrue(Char3.HPMax >= 20 && Char3.HPMax <= 50, "Char3.HPMax: " + Char3.HPMax + ", Expected: 20>x>50");
            int abilityTotal = Char3.Strength + Char3.Dexterity + Char3.Constitution;
            Assert.AreEqual(35, abilityTotal, "Char3 Ability Total: " + abilityTotal + ", Expected: 35, Str: " + Char3.Strength + ", Dex: " +
                Char3.Dexterity + ", Con: " + Char3.Constitution);
            int abilityModTotal = Char3.StrengthMod + Char3.DexterityMod + Char3.ConstitutionMod;
            Assert.IsTrue(abilityModTotal >= 10 && abilityModTotal <= 13,
                "Char3 Ability Mod Total: " + abilityModTotal + ", Expected: 10>x>13, StrMod: " + Char3.StrengthMod + ", DexMod: " + 
                Char3.DexterityMod + ", ConMod: " + Char3.ConstitutionMod);
        }

        [TestMethod]
        public void CharacterIncreaseXPNoLevelUp()
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
        public void CharacterIncreaseXPLevelUp()
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
    }
}
