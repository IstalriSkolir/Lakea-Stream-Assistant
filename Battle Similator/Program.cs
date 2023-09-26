﻿using Battle_Similator.Models;
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
            else if(args.Length < 4)
            {
                Environment.Exit((int)ErrorCode.Not_Enough_Args);
            }
            else
            {
                IO io = new IO(args[0]);
                switch(args[1])
                {
                    case "WEAKMONSTER":
                        MonsterEncounter weakMonster = new MonsterEncounter(io);
                        weakMonster.Start("WEAK", args[2], args[3]);
                        break;
                    default:
                        Environment.Exit((int)ErrorCode.Invalid_Args);
                        break;
                }
            }
        }
    }
}