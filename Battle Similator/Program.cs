using Battle_Similator.Models.Encounters;
using Battle_Similator.Models.NonEncounter;
using Battle_Similator.Models.NonEncounters;
using Battle_Similator.Models.Resources;

namespace Battle_Similator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Environment.Exit((int)ExitCode.No_Args_Given);
            }
            else if(args.Length < 5 && args[1] != "ENVIRONMENTRESET")
            {
                Environment.Exit((int)ExitCode.Not_Enough_Args);
            }
            else
            {
                IO io = new IO(args[0], args[4]);
                switch(args[1])
                {
                    case "ENVIRONMENTRESET":
                        EnvironmentReset reset = new EnvironmentReset(io, args[0], args[4]);
                        reset.Start();
                        exitCode(0);
                        break;
                    case "CHARACTERTRAINING":
                        Training trainer = new Training(io);
                        trainer.Start(args[2], args[3]);
                        exitCode((int)ExitCode.Character_Training);
                        break;
                    case "WEAKMONSTER":
                        MonsterEncounter weakMonster = new MonsterEncounter(io);
                        weakMonster.Start("WEAK", args[2], args[3]);
                        exitCode((int)ExitCode.Monster_Battle);
                        break;
                    case "NORMALMONSTER":
                        MonsterEncounter normalMonster = new MonsterEncounter(io);
                        normalMonster.Start("NORMAL", args[2], args[3]);
                        exitCode((int)ExitCode.Monster_Battle);
                        break;
                    case "HARDMONSTER":
                        MonsterEncounter hardMonster = new MonsterEncounter(io);
                        hardMonster.Start("HARD", args[2], args[3]);
                        exitCode((int)ExitCode.Monster_Battle);
                        break;
                    case "RANDOMMONSTER":
                        MonsterEncounter randomMonster = new MonsterEncounter(io);
                        randomMonster.Start("RANDOM", args[2], args[3]);
                        exitCode((int)ExitCode.Monster_Battle);
                        break;
                    case "BOSSBATTLE":
                        BossEncounter bossEncounter = new BossEncounter(io, args[0], args[4]);
                        bossEncounter.Start(args[2], args[3]);
                        exitCode((int)ExitCode.Boss_Battle);
                        break;
                    case "CHARACTERPRESTIGE":
                        CharacterPrestige prestige = new CharacterPrestige(io);
                        prestige.Start(args[2], args[3]);
                        exitCode((int)ExitCode.Character_Prestige);
                        break;
                    case "CHARACTERRESET":
                        CharacterReset characterReset = new CharacterReset(io);
                        characterReset.Start(args[2], args[3]);
                        exitCode((int)ExitCode.Character_Reset);
                        break;
                    default:
                        Environment.Exit((int)ExitCode.Invalid_Args);
                        break;
                }
            }
        }

        static void exitCode(int exitCode)
        {
            Environment.Exit(exitCode);
        }
    }
}