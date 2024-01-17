using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;

namespace Lakea_Stream_Assistant.EventProcessing.Battle_Simulator
{
    //This class reads the data files for the Battle Simulator
    public class BattleFileParser
    {
        private string characterPath;
        private string monsterPath;
        private string resultPath;

        //Constructor sets the file paths
        public BattleFileParser(string resourcePath)
        {
            if (resourcePath == null || resourcePath == string.Empty || resourcePath.Equals("default"))
            {
                characterPath = Environment.CurrentDirectory + "\\Applications\\Battle Simulator\\Creatures\\Characters\\";
                monsterPath = Environment.CurrentDirectory + "\\Applications\\Battle Simulator\\Creatures\\Monsters\\";
                resultPath = Environment.CurrentDirectory + "\\Applications\\Battle Simulator\\";
            }
            else
            {
                characterPath = resourcePath + "\\Creatures\\Characters\\";
                monsterPath = resourcePath + "\\Creatures\\Monsters\\";
                resultPath = resourcePath;
            }
        }

        //Load a character from a file into a dictionary
        public Dictionary<string, string> GetCharacterData(string accountID, string displayName)
        {
            try
            {
                Dictionary<string, string> characterDict = new Dictionary<string, string>();
                if (File.Exists(characterPath + accountID + ".txt"))
                {
                    string[] characterArray = File.ReadAllLines(characterPath + accountID + ".txt");
                    foreach (string property in characterArray)
                    {
                        string[] parts = property.Split(":");
                        switch (parts[0])
                        {
                            case "NAME":
                                characterDict.Add("NAME", parts[1]);
                                break;
                            case "ID":
                                characterDict.Add("ID", parts[1]);
                                break;
                            case "LEVEL":
                                characterDict.Add("LEVEL", parts[1]);
                                break;
                            case "XP":
                                characterDict.Add("XP", parts[1]);
                                break;
                            case "HP":
                                characterDict.Add("HP", parts[1]);
                                break;
                            case "STR":
                                characterDict.Add("STR", parts[1]);
                                break;
                            case "DEX":
                                characterDict.Add("DEX", parts[1]);
                                break;
                            case "CON":
                                characterDict.Add("CON", parts[1]);
                                break;
                            default:
                                Terminal.Output("Lakea: Unrecognised Property in Character File -> " + property);
                                Logs.Instance.NewLog(LogLevel.Warning, "Unrecognised Property in Character File -> " + property);
                                break;
                        }
                    }
                }
                else
                {
                    characterDict = new Dictionary<string, string>
                    {
                        { "NAME", displayName },
                        { "ID", accountID },
                        { "LEVEL", "1" },
                        { "XP", "0" },
                        { "HP", "20" },
                        { "STR", "9" },
                        { "DEX", "9" },
                        { "CON", "9" }
                    };
                }
                return characterDict;
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Character Data Load Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
                return new Dictionary<string, string>();
            }
        }

        //Load a monster from a file into a dictionary
        public Dictionary<string, string> GetMonsterData(string monsterID)
        {
            try
            {
                Dictionary<string, string> monsterDict = new Dictionary<string, string>();
                if (File.Exists(monsterPath + monsterID + ".txt"))
                {
                    string[] monsterArray = File.ReadAllLines(monsterPath + monsterID + ".txt");
                    foreach (string property in monsterArray)
                    {
                        string[] parts = property.Split(":");
                        switch (parts[0])
                        {
                            case "NAME":
                                monsterDict.Add("NAME", parts[1]);
                                break;
                            case "ID":
                                monsterDict.Add("ID", parts[1]);
                                break;
                            case "LEVEL":
                                monsterDict.Add("LEVEL", parts[1]);
                                break;
                            case "HP":
                                monsterDict.Add("HP", parts[1]);
                                break;
                            case "STR":
                                monsterDict.Add("STR", parts[1]);
                                break;
                            case "DEX":
                                monsterDict.Add("DEX", parts[1]);
                                break;
                            case "CON":
                                monsterDict.Add("CON", parts[1]);
                                break;
                            default:
                                Terminal.Output("Lakea: Unrecognised Property in Monster File -> " + property);
                                Logs.Instance.NewLog(LogLevel.Warning, "Unrecognised Property in Monster File -> " + property);
                                break;
                        }
                    }
                }
                else
                {
                    monsterDict = new Dictionary<string, string>
                    {
                        { "NAME", "UNKNOWN" },
                        { "ID", monsterID },
                        { "LEVEL", "1" },
                        { "HP", "20" },
                        { "STR", "9" },
                        { "DEX", "9" },
                        { "CON", "9" }
                    };
                }
                return monsterDict;
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Monster Data Load Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
                return new Dictionary<string, string>();
            }
        }

        //Load the battle results from a file into a dictionary
        public Dictionary<string, string> GetResultData()
        {
            try
            {
                string[] resultsArray = File.ReadAllLines(resultPath + "\\RESULT.txt");
                Dictionary<string, string> resultsDict = new Dictionary<string, string>();
                foreach (string result in resultsArray)
                {
                    string[] parts = result.Split(":");
                    switch (parts[0])
                    {
                        case "BOSS_BEATEN":
                            resultsDict.Add("BOSS_BEATEN", parts[1]);
                            break;
                        case "ALL_BOSSES_BEATEN":
                            resultsDict.Add("ALL_BOSSES_BEATEN", parts[1]);
                            break;
                        case "CHARACTER_CON":
                            resultsDict.Add("CHARACTER_CON", parts[1]);
                            break;
                        case "CHARACTER_DEX":
                            resultsDict.Add("CHARACTER_DEX", parts[1]);
                            break;
                        case "CHARACTER_HP":
                            resultsDict.Add("CHARACTER_HP", parts[1]);
                            break;
                        case "CHARACTER_ID":
                            resultsDict.Add("CHARACTER_ID", parts[1]);
                            break;
                        case "CHARACTER_LEVEL":
                            resultsDict.Add("CHARACTER_LEVEL", parts[1]);
                            break;
                        case "CHARACTER_NAME":
                            resultsDict.Add("CHARACTER_NAME", parts[1]);
                            break;
                        case "CHARACTER_STR":
                            resultsDict.Add("CHARACTER_STR", parts[1]);
                            break;
                        case "CHARACTER_XP":
                            resultsDict.Add("CHARACTER_XP", parts[1]);
                            break;
                        case "ENCOUNTER_TYPE":
                            resultsDict.Add("ENCOUNTER_TYPE", parts[1]);
                            break;
                        case "LEVEL_UP":
                            resultsDict.Add("LEVEL_UP", parts[1]);
                            break;
                        case "MONSTER_ID":
                            resultsDict.Add("MONSTER_ID", parts[1]);
                            break;
                        case "MONSTER_NAME":
                            resultsDict.Add("MONSTER_NAME", parts[1]);
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
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Battle Result Load Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
                return new Dictionary<string, string>();
            }
        }
    }
}
