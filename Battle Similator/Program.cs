using Battle_Similator.Models;
using Battle_Similator.Models.Creatures;

namespace Battle_Similator
{
    internal class Program
    {
        private static IO io; 

        static void Main(string[] args)
        {
            io = new IO();
            //Character character = new Character("IstalriSkolir", "106861102");
            //io.SaveCharacterData(character);
            Character character = io.LoadCharacterData("106861102");
            Monster monster1 = io.LoadMonsterData("0000-Rat");
            Monster monster2 = io.LoadMonsterData("0003-Spider");
            Monster monster3 = io.LoadMonsterData("0005-Snake");
        }
    }
}