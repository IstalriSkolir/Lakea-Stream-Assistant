using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;
using System.Diagnostics;
using System.Globalization;

namespace Lakea_Stream_Assistant.EventProcessing.Battle_Simulator
{
    // This class handles the calls to the Battle Simulator Application
    public class BattleManager
    {
        private ProcessStartInfo battleSimInfo;
        private Process battleSim;
        private BattleFileParser fileParser;
        private List<string> queue;
        private bool active;
        private bool bossesFirstFight;
        private int bossCount;
        private string resourcePath;

        // Constructor sets the path and other properties for the Battle Simulator Application
        public BattleManager(string resourcePath)
        {
            this.fileParser = new BattleFileParser(resourcePath);
            this.queue = new List<string>();
            this.active = false;
            this.bossesFirstFight = true;
            this.bossCount = 1;
            this.battleSimInfo = new ProcessStartInfo("\"" + Environment.CurrentDirectory + "\\Applications\\Battle Simulator\\Battle Similator.exe\"");
            this.battleSimInfo.CreateNoWindow = true;
            this.battleSim = new Process();
            this.battleSim.StartInfo = battleSimInfo;
            this.battleSim.EnableRaisingEvents = true;
            this.battleSim.Exited += battleSimulatorExited;
            if (resourcePath == null || resourcePath == string.Empty || resourcePath.Equals("default"))
            {
                this.resourcePath = Environment.CurrentDirectory + "\\Resources\\Applications\\Battle Simulator\\Creatures\\";
            }
            else
            {
                this.resourcePath = resourcePath + "\\\\Creatures\\\\";
            }
            Other("ENVIRONMENTRESET", "NA", "NA");
        }

        // Get the character sheet of a user
        public Dictionary<string, string> GetCharacterSheet(string accountID, string displayName)
        {
            try
            {
                Dictionary<string, string> args = new Dictionary<string, string>();
                Dictionary<string, string> character = fileParser.GetCharacterData(accountID, displayName);
                int level = Int32.Parse(character["LEVEL"]);
                int nextLevel = 0;
                for (int count = 1; count <= level; count++)
                {
                    nextLevel += count * 30;
                }
                int xpNeeded = nextLevel - Int32.Parse(character["XP"]);
                int strMod = Int32.Parse(character["STR"]) / 3;
                int dexMod = Int32.Parse(character["DEX"]) / 3;
                int conMod = Int32.Parse(character["CON"]) / 3;
                args.Add("Message", displayName + " -> LEVEL: " + character["LEVEL"] + ", XP: " + character["XP"] + ", XP_NEEDED: " + xpNeeded + ", HP: " +
                    character["HP"] + ", STR: " + character["STR"] + "(+" + strMod + "), DEX: " + character["DEX"] + "(+" + dexMod + "), CON: " + character["CON"] +
                    "(+" + conMod + ")");
                return args;
            }
            catch (Exception ex) 
            {
                Terminal.Output("Lakea: Character Sheet Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        // Get the character statistics of a user
        public Dictionary<string, string> GetCharacterStatistics(string accountID, string displayName)
        {
            Dictionary<string, string> character = fileParser.GetCharacterData(accountID, displayName);
            Dictionary<string, string> args = new Dictionary<string, string>()
            {
                { "Message", displayName + " -> DEATHS: " + character["DEATHS"] + ", MONSTERS_KILLED: " + character["MONSTERS_KILLED"] + ", MONSTER_WIN_RATE: " +
                    character["MONSTER_WIN_RATE"] + ", BOSSES_FOUGHT: " + character["BOSSES_FOUGHT"] + ", BOSSES_BEATEN: " + character["BOSSES_BEATEN"] + ", PRESTIGE: " +
                    character["PRESTIGE"]}
            };
            return args;
        }

        #region Run Battle Simulator

        // Add an event to the Battle Sim Queue
        public void Other(string eve, string accountID, string displayName)
        {
            try
            {
                TwitchSubTier subTier = TwitchSubTier.None; 
                if(eve == "CHARACTERPRESTIGE")
                {
                    Dictionary<string, string> character = fileParser.GetCharacterData(accountID, displayName);
                    int level = Int32.Parse(character["LEVEL"]);
                    if(level < 100)
                    {
                        Twitch.WriteToChat("You're not a high enough level yet " + displayName + ", you can't prestige until you're level 100!");
                        return;
                    }
                }
                else if(eve == "CHARACTERTRAINING")
                {
                    subTier = Twitch.GetUserSubscriptionTier(accountID).Result;
                }
                string item = "\"LAKEA\" \"" + eve + "\" \"" + accountID + "\" \"" + displayName + "\" \"" + subTier.ToString() + "\" \"" + resourcePath + "\"";
                queue.Add(item);
                if (!active)
                {
                    runBattleSimulator();
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Train Item Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        // Add a battle to the Battle Sim Queue
        public void Battle(string type, string accountID, string displayName, List<string> battleArgs)
        {
            try
            {
                Dictionary<string, string> character = fileParser.GetCharacterData(accountID, displayName);
                int level = Int32.Parse(character["LEVEL"]);
                if(level >= 5)
                {
                    TwitchSubTier subTier = Twitch.GetUserSubscriptionTier(accountID).Result;
                    string item = "\"LAKEA\" \"" + type + "\" \"" + accountID + "\" \"" + displayName + "\" \"" + subTier.ToString() + "\" \"" + resourcePath + "\"";
                    foreach (string arg in battleArgs)
                    {
                        item += " \"" + arg + "\"";
                    }
                    queue.Add(item);
                    if(!active)
                    {
                        runBattleSimulator();
                    }
                }
                else
                {
                    Twitch.WriteToChat("Your not a high enough level yet " + displayName + "! Train with me some more before you get yourself killed!");
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Battle Item Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        // Run the Battle Simulator
        private void runBattleSimulator()
        {
            try
            {
                active = true;
                string parameters = queue[0];
                queue.RemoveAt(0);
                Terminal.Output("Lakea: Starting Battle Simulator -> " + parameters);
                Logs.Instance.NewLog(LogLevel.Info, "Starting Battle Simulator -> " + parameters);
                if (parameters.Contains("BOSSBATTLE") && bossesFirstFight)
                {
                    bossesFirstFight = false;
                    Dictionary<string, string> args = new Dictionary<string, string>()
                    {
                        { "EventID", "Boss_" + bossCount + "_First_Battle" }
                    };
                    StreamAssistant.EventHandler.NewEvent(new IncomingEvent(EventSource.Battle_Simulator, EventType.Battle_Simulator_Encounter, args));
                    Thread.Sleep(5000);
                }
                battleSimInfo.Arguments = parameters;
                battleSim.Start();
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Battle Simulator Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        #endregion

        #region Battle Simulator Ended

        // When Battle Simulator finishes, read the exit code to determine output
        private void battleSimulatorExited(object sender, EventArgs e)
        {
            Process process = (Process)sender;
            int exitCode = process.ExitCode;
            if(exitCode != 0)
            {
                Terminal.Output("Lakea: Battle Simulator Ended");
                Logs.Instance.NewLog(LogLevel.Info, "Battle Simulator Ended");
                Dictionary<string, string> results = fileParser.GetResultData();
                if (queue.Count > 0)
                {
                    runBattleSimulator();
                }
                else
                {
                    active = false;
                }
                switch (exitCode)
                {
                    case 0:
                        break;
                    case (int)ExitCode.Character_Training:
                        trainingEnded(results);
                        break;
                    case (int)ExitCode.Monster_Battle:
                        monsterBattleEnded(results);
                        break;
                    case (int)ExitCode.Boss_Battle:
                        bossBattleEnded(results);
                        break;
                    case (int)ExitCode.Boss_Healthbar_Update:
                        break;
                    case (int)ExitCode.Character_Prestige:
                        characterPrestiageEnded(results);
                        break;
                    default:
                        Terminal.Output("Lakea: Battle Simulator Error");
                        Logs.Instance.NewLog(LogLevel.Error, "Battle Simulator Error Code -> " + exitCode + ", " + (ExitCode)exitCode);
                        break;
                }
            }
            else
            {
                active = false;
            }
        }

        // Read the training results and send them to Twitch
        private void trainingEnded(Dictionary<string, string> results)
        {
            if(results.Count > 0)
            {
                string message = results["CHARACTER_NAME"] + " trained with me and gained " + results["XP_GAINED"] + "XP!";
                if (results["LEVEL_UP"].Equals("TRUE"))
                {
                    message = message.Substring(0, message.Length - 1) + ", they've reached level " + results["CHARACTER_LEVEL"] + "!";
                }
                Twitch.WriteToChat(message);
            }
        }

        // Read the battle results and send them to Twitch
        private void monsterBattleEnded(Dictionary<string, string> results)
        {
            if (results.Count > 0)
            {
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                string message = string.Empty;
                string monster = results["MONSTER_NAME"].Replace("_", " ");
                monster = textInfo.ToTitleCase(monster.ToLower());
                if (results["WINNER"].Equals(results["CHARACTER_ID"]))
                {
                    message = results["CHARACTER_NAME"] + " fought a " + monster + " and won! They gained " + results["XP_GAINED"] + "XP!";
                    if (results["LEVEL_UP"].Equals("TRUE"))
                    {
                        message = message.Substring(0, message.Length - 1) + " and reached level " + results["CHARACTER_LEVEL"] + "!";
                    }
                }
                else if (results["WINNER"].Equals(results["MONSTER_ID"]))
                {
                    message = results["CHARACTER_NAME"] + " was knocked out while fighting a " + monster + "! They should have trained with me more! " + results["CHARACTER_NAME"] + " has lost " + results["CHARACTER_XP_LOST"] + "XP!";
                    if (results["CHARACTER_LEVELS_LOST"] != "0")
                    {
                        message = message.Remove(message.Length - 1);
                        message += " and " + results["CHARACTER_LEVELS_LOST"] + " levels!";
                    }
                }
                Twitch.WriteToChat(message);
            }
        }

        // Read the boss battle results and process the events
        private void bossBattleEnded(Dictionary<string, string> results)
        {
            if(results.Count > 0)
            {
                Dictionary<string, string> args = new Dictionary<string, string>();
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                string boss = results["MONSTER_NAME"].Replace("_", " ");
                boss = textInfo.ToTitleCase(boss.ToLower());
                if ("TRUE".Equals(results["ALL_BOSSES_BEATEN"]))
                {
                    Twitch.WriteToChat(results["CHARACTER_NAME"] + " fought " + boss + " and won! All the bosses have been defeated!");
                    args.Add("EventID", "All_Bosses_Defeated");
                }
                else if ("TRUE".Equals(results["BOSS_BEATEN"]))
                {
                    bossesFirstFight = true;
                    Twitch.WriteToChat(results["CHARACTER_NAME"] + " fought " + boss + " and won! Get ready for the next boss!");
                    args.Add("EventID", "Boss_" + bossCount + "_Defeated");
                    bossCount++;
                }
                else
                {
                    Twitch.WriteToChat(results["CHARACTER_NAME"] + " fought " + boss + " and lost, better luck next time ranger!");
                    args.Add("EventID", "Boss_Battle_Ended");
                }
                StreamAssistant.EventHandler.NewEvent(new IncomingEvent(EventSource.Battle_Simulator, EventType.Battle_Simulator_Encounter, args));
            }
        }

        // Read the prestige results and send them to Twitch
        private void characterPrestiageEnded(Dictionary<string, string> results)
        {
            int level = Int32.Parse(results["CHARACTER_LEVEL"]);
            int nextLevel = 0;
            for (int count = 1; count <= level; count++)
            {
                nextLevel += count * 30;
            }
            int strMod = Int32.Parse(results["CHARACTER_STR"]) / 3;
            int dexMod = Int32.Parse(results["CHARACTER_DEX"]) / 3;
            int conMod = Int32.Parse(results["CHARACTER_CON"]) / 3;
            string message = "@" + results["CHARACTER_NAME"] + " Just reached " + results["PRESTIGE"] + " prestige! LEVEL: " + results["CHARACTER_LEVEL"] + ", XP: " +
                results["CHARACTER_XP"] + ", NEXT_LEVEL: " + nextLevel + ", HP: " + results["CHARACTER_HP"] + ", STR: " + results["CHARACTER_STR"] + "(+" + strMod + 
                "), DEX: " + results["CHARACTER_DEX"] + "(+" + dexMod + "), CON: " + results["CHARACTER_CON"] + "(+" + conMod + ")";
            Twitch.WriteToChat(message);
        }

        #endregion
    }
}
