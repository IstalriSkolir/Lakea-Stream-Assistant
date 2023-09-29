using Battle_Similator;
using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.EventProcessing.Processing;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;
using System.Diagnostics;
using System.Globalization;

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
                args.Add("Message", "@" + displayName + " -> LEVEL: " + character["LEVEL"] + ", XP: " + character["XP"] + ", HP: " + character["HP"] + ", STR: " +
                    character["STR"] + ", DEX: " + character["DEX"] + ", CON: " + character["CON"]);
                return args;
            }
            catch (Exception ex) 
            {
                Terminal.Output("Lakea: Character Sheet Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        //Call the Battle SImulator to run a monster encounter passing in monster strength and user data
        public void MonsterBattle(string monsterStrength, string accountNumber, string displayName)
        {
            try
            {
                Dictionary<string,string> character = fileParser.GetCharacterData(accountNumber, displayName);
                int level = Int32.Parse(character["LEVEL"]);
                if(level >= 5)
                {
                    Terminal.Output("Lakea: Starting Battle Simulator -> " + accountNumber + ", " + displayName + ", " + monsterStrength);
                    Logs.Instance.NewLog(LogLevel.Info, "Starting Battle Simulator -> " + accountNumber + ", " + displayName + ", " + monsterStrength);
                    battleSimInfo.Arguments = "\"LAKEA\" \"" + monsterStrength + "\" \"" + accountNumber + "\" \"" + displayName + "\"";
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

        //When Battle Simulator finishes, read the results file and send results to Twitch
        private void battleSimulatorExited(object sender, EventArgs e)
        {
            Process process = (Process)sender;
            int exitCode = process.ExitCode;
            if (exitCode != 0)
            {
                Terminal.Output("Lakea: Battle Simulator Error");
                Logs.Instance.NewLog(LogLevel.Error, "Battle Simulator Error Code -> " + exitCode + ", " + (ErrorCode)exitCode);
            }
            else
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
                        args.Add("Message", "@" + results["CHARACTER_NAME"] + " fought a " + monster + " and won! They gained " + results["XP_GAINED"] + "XP!");
                    }
                    else if (results["WINNER"].Equals(results["MONSTER_ID"]))
                    {
                        args.Add("Message", "@" + results["CHARACTER_NAME"] + " died while fighting a " + monster + "! They should have trained with me more!");
                    }
                    eventInput.NewEvent(new EventItem(EventSource.Battle_Simulator, EventType.Battle_Simulator_Encounter, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, "Battle Simulator Encounter", "Battle_Simulator_Monster", args: args));
                }
            }
        }
    }
}
