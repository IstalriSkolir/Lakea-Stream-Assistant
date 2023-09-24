using Battle_Similator.Models.Encounters;

namespace Battle_Similator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Environment.Exit((int)ErrorCode.No_Args_Given);
            }
            else if(args.Length < 3)
            {
                Environment.Exit((int)ErrorCode.Not_Enough_Args);
            }
            else
            {
                switch(args[0])
                {
                    case "WEAKMONSTER":
                        MonsterEncounter weakMonster = new MonsterEncounter();
                        weakMonster.Start("WEAK", args[1], args[2]);
                        break;
                    default:
                        Environment.Exit((int)ErrorCode.Invalid_Args);
                        break;
                }
            }
        }
    }
}