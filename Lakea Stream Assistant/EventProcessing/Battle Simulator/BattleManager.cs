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

        //Constructor sets the path and other properties for the Battle Simulator Application
        public BattleManager(EventInput eventInput)
        {
            this.eventInput = eventInput;
            battleSimInfo = new ProcessStartInfo("\"" + Environment.CurrentDirectory + "\\Applications\\Battle Simulator\\Battle Similator.exe\"");
            battleSimInfo.CreateNoWindow = true;
            battleSim = new Process();
            battleSim.StartInfo = battleSimInfo;
            battleSim.EnableRaisingEvents = true;
            battleSim.Exited += battleSimulatorExited;
        }

        //Call the Battle SImulator to run a monster encounter passing in monster strength and user data
        public void MonsterBattle(string monsterStrength, string accountNumber, string displayName)
        {
            try
            {
                Terminal.Output("Lakea: Starting Battle Simulator -> " + accountNumber + ", " + displayName + ", " + monsterStrength);
                Logs.Instance.NewLog(LogLevel.Info, "Starting Battle Simulator -> " + accountNumber + ", " + displayName + ", " + monsterStrength);
                battleSimInfo.Arguments = "\"LAKEA\" \"" + monsterStrength + "\" \"" + accountNumber + "\" \"" + displayName + "\"";
                battleSim.Start();
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
            Terminal.Output("Lakea: Battle Simulator Ended");
            Logs.Instance.NewLog(LogLevel.Info, "Battle Simulator Ended");
            Dictionary<string, string> results = loadBattleResult();
            Dictionary<string, string> args = new Dictionary<string, string>();
            if(results.Count> 0)
            {
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
                eventInput.NewEvent(new EventItem(EventSource.Battle_Simulator, EventType.Battle_Simulator_Encounter, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, "Battle Simulator Encounter", "Battle_Simulator_Weak_Monster", args: args));
            }
        }

        //Reads the BATTLERESULT.txt file and returns data in a dictionary
        private Dictionary<string, string> loadBattleResult()
        {
            try
            {
                string filePath = Environment.CurrentDirectory + "\\Applications\\Battle Simulator\\BATTLERESULT.txt";
                string[] resultsArr = File.ReadAllLines(filePath);
                Dictionary<string, string> resultsDict = new Dictionary<string, string>();
                foreach (string result in resultsArr)
                {
                    string[] parts = result.Split(":");
                    switch (parts[0])
                    {
                        case "ENCOUNTER_TYPE":
                            resultsDict.Add("ENCOUNTER_TYPE", parts[1]);
                            break;
                        case "CHARACTER_NAME":
                            resultsDict.Add("CHARACTER_NAME", parts[1]);
                            break;
                        case "CHARACTER_ID":
                            resultsDict.Add("CHARACTER_ID", parts[1]);
                            break;
                        case "MONSTER_NAME":
                            resultsDict.Add("MONSTER_NAME", parts[1]);
                            break;
                        case "MONSTER_ID":
                            resultsDict.Add("MONSTER_ID", parts[1]);
                            break;
                        case "WINNER":
                            resultsDict.Add("WINNER", parts[1]);
                            break;
                        case "XP_GAINED":
                            resultsDict.Add("XP_GAINED", parts[1]);
                            break;
                        default:
                            Terminal.Output("Lakea: Unrecognised Property in Battle Results -> " + result);
                            Logs.Instance.NewLog(LogLevel.Warning, "Unrecognised Property in Battle Results -> " + result);
                            break;
                    }
                }
                return resultsDict;
            }
            catch(Exception ex)
            {
                Terminal.Output("Lakea: Battle Result Load Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
                return new Dictionary<string, string>();
            }
        } 
    }
}
