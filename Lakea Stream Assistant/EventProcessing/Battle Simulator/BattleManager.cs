using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.EventProcessing.Processing;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;
using System.Diagnostics;
using System.Globalization;
using System.Reflection.Emit;

namespace Lakea_Stream_Assistant.EventProcessing.Battle_Simulator
{
    //This class handles the calls to the Battle Simulator Application
    public class BattleManager
    {
        private EventInput eventInput;
        private ProcessStartInfo battleSimInfo;
        private Process battleSim;
        private BattleFileParser fileParser;

        //Constructor sets the path and other properties for the Battle Simulator Application
        public BattleManager(EventInput eventInput)
        {
            this.eventInput = eventInput;
            this.fileParser = new BattleFileParser();
            battleSimInfo = new ProcessStartInfo("\"" + Environment.CurrentDirectory + "\\Applications\\Battle Simulator\\Battle Similator.exe\"");
            battleSimInfo.CreateNoWindow = true;
            battleSim = new Process();
            battleSim.StartInfo = battleSimInfo;
            battleSim.EnableRaisingEvents = true;
            battleSim.Exited += battleSimulatorExited;
        }

        //Get the character sheet of a user
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
                int strMod = Int32.Parse(character["STR"]) / 3;
                int dexMod = Int32.Parse(character["DEX"]) / 3;
                int conMod = Int32.Parse(character["CON"]) / 3;
                args.Add("Message", "@" + displayName + " -> LEVEL: " + character["LEVEL"] + ", XP: " + character["XP"] + ", NEXT_LEVEL: " + nextLevel + ", HP: " +
                    character["HP"] + ", STR: " + character["STR"] + "(+" + strMod + ")" + ", DEX: " + character["DEX"] + "(+" + dexMod + ")" + ", CON: " +
                    character["CON"] + "(+" + conMod + ")");
                return args;
            }
            catch (Exception ex) 
            {
                Terminal.Output("Lakea: Character Sheet Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        #region Run Battle Simulator

        //Train a Character to gain XP
        public void TrainCharacter(string accountID, string displayName)
        {
            try
            {
                Terminal.Output("Lakea: Starting Battle Simulator -> CHARACTERTRAINING , " + accountID + ", " + displayName);
                Logs.Instance.NewLog(LogLevel.Info, "Starting Battle Simulator -> CHARACTERTRAINING, " + accountID + ", " + displayName);
                battleSimInfo.Arguments = "\"LAKEA\" \"CHARACTERTRAINING\" \"" + accountID + "\" \"" + displayName + "\"";
                battleSim.Start();
            }
            catch(Exception ex)
            {
                Terminal.Output("Lakea: Train Character Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        //Call the Battle Simulator to run a monster encounter passing in monster strength and user data
        public void MonsterBattle(string monsterStrength, string accountID, string displayName)
        {
            try
            {
                Dictionary<string,string> character = fileParser.GetCharacterData(accountID, displayName);
                int level = Int32.Parse(character["LEVEL"]);
                if(level >= 5)
                {
                    Terminal.Output("Lakea: Starting Battle Simulator -> " + monsterStrength + ", " + accountID + ", " + displayName);
                    Logs.Instance.NewLog(LogLevel.Info, "Starting Battle Simulator -> " + monsterStrength + ", " + accountID + ", " + displayName);
                    battleSimInfo.Arguments = "\"LAKEA\" \"" + monsterStrength + "\" \"" + accountID + "\" \"" + displayName + "\"";
                    battleSim.Start();
                }
                else
                {
                    Dictionary<string, string> args = new Dictionary<string, string>
                    {
                        { "Message", "Your not a high enough level yet @" + displayName + "! Train with me some more before you get yourself killed!" }
                    };
                    eventInput.NewEvent(new EventItem(EventSource.Lakea, EventType.Battle_Simulator_Encounter, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, "Battle Simulator Encounter", "Battle_Simulator_Monster", args: args));
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Battle Simulator Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        #endregion

        #region Battle Simulator Ended

        //When Battle Simulator finishes, read the exit code to determine output
        private void battleSimulatorExited(object sender, EventArgs e)
        {
            Process process = (Process)sender;
            int exitCode = process.ExitCode;
            switch (exitCode)
            {
                case 0:
                    break;
                case (int)ExitCode.Character_Training:
                    trainingEnded();
                    break;
                case (int)ExitCode.Monster_Battle:
                    monsterBattleEnded();
                    break;
                default:
                    Terminal.Output("Lakea: Battle Simulator Error");
                    Logs.Instance.NewLog(LogLevel.Error, "Battle Simulator Error Code -> " + exitCode + ", " + (ExitCode)exitCode);
                    break;
            }
        }

        //Read the training results and send them to Twitch
        private void trainingEnded()
        {
            Terminal.Output("Lakea: Battle Simulator Ended");
            Logs.Instance.NewLog(LogLevel.Info, "Battle Simulator Ended");
            Dictionary<string, string> results = fileParser.GetTrainingData();
            if(results.Count > 0)
            {
                Dictionary<string, string> args = new Dictionary<string, string>();
                string message = "@" + results["CHARACTER_NAME"] + " trained with me and gained " + results["XP_GAINED"] + "XP!";
                if (results["LEVEL_UP"].Equals("TRUE"))
                {
                    message = message.Substring(0, message.Length - 1) + ", they've reached level " + results["CHARACTER_LEVEL"] + "!";
                }
                args.Add("Message", message);
                eventInput.NewEvent(new EventItem(EventSource.Battle_Simulator, EventType.Battle_Simulator_Training, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, "Battle Simulator Training", "Battle_Simulator_Training", args: args));
            }
        }

        //Read the battle results and send them to Twitch
        private void monsterBattleEnded()
        {
            Terminal.Output("Lakea: Battle Simulator Ended");
            Logs.Instance.NewLog(LogLevel.Info, "Battle Simulator Ended");
            Dictionary<string, string> results = fileParser.GetBattleData();
            if (results.Count > 0)
            {
                Dictionary<string, string> args = new Dictionary<string, string>();
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                string monster = results["MONSTER_NAME"].Replace("_", " ");
                monster = textInfo.ToTitleCase(monster.ToLower());
                if (results["WINNER"].Equals(results["CHARACTER_ID"]))
                {
                    string message = "@" + results["CHARACTER_NAME"] + " fought a " + monster + " and won! They gained " + results["XP_GAINED"] + "XP!";
                    if (results["LEVEL_UP"].Equals("TRUE"))
                    {
                        message = message.Substring(0, message.Length - 1) + " and reached level " + results["CHARACTER_LEVEL"] + "!";
                    }
                    args.Add("Message", message);
                }
                else if (results["WINNER"].Equals(results["MONSTER_ID"]))
                {
                    args.Add("Message", "@" + results["CHARACTER_NAME"] + " died while fighting a " + monster + "! They should have trained with me more!");
                }
                eventInput.NewEvent(new EventItem(EventSource.Battle_Simulator, EventType.Battle_Simulator_Encounter, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, "Battle Simulator Encounter", "Battle_Simulator_Monster", args: args));
            }
        }

        #endregion
    }
}
