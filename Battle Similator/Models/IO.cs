using Battle_Similator.Models.Creatures;
using Battle_Similator.Models.Encounters;

namespace Battle_Similator.Models
{
    public class IO
    {
        public IO()
        {
            string path = Environment.CurrentDirectory + "\\Creatures\\Characters";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public void SaveCharacterData(Character character)
        {
            try
            {
                string filePath = Environment.CurrentDirectory + "\\Creatures\\Characters\\" + character.ID + ".txt";
                string characterString = "NAME:" + character.Name + "\nID:" + character.ID + "\nLEVEL:" + character.Level + "\nXP:" + character.XP + "\nHP:" + character.HPMax + 
                    "\nSTR:" + character.Strength + "\nDEX:" + character.Dexterity + "\nCON:" + character.Constitution;
                File.WriteAllText(filePath, characterString);
            }
            catch (Exception)
            {
                Environment.Exit((int)ErrorCode.IO_Save_Error);
            }
        }

        public Character LoadCharacterData(string id)
        {
            try
            {
                string filePath = Environment.CurrentDirectory + "\\Creatures\\Characters\\" + id + ".txt";
                if(File.Exists(filePath))
                {
                    string[] lines = File.ReadAllLines(filePath);
                    Dictionary<string, string> props = new Dictionary<string, string>();
                    foreach(string line in lines)
                    {
                        string[] parts = line.Split(":");
                        switch(parts[0])
                        {
                            case "NAME":
                                props.Add("NAME", parts[1]);
                                break;
                            case "ID":
                                props.Add("ID", parts[1]);
                                break;
                            case "LEVEL":
                                props.Add("LEVEL", parts[1]);
                                break;
                            case "XP":
                                props.Add("XP", parts[1]);
                                break;
                            case "HP":
                                props.Add("HP", parts[1]);
                                break;
                            case "STR":
                                props.Add("STR", parts[1]);
                                break;
                            case "DEX":
                                props.Add("DEX", parts[1]);
                                break;
                            case "CON":
                                props.Add("CON", parts[1]);
                                break;
                            default:
                                break;
                        }
                    }
                    return new Character(props["NAME"], props["ID"], Int32.Parse(props["XP"]), Int32.Parse(props["LEVEL"]), Int32.Parse(props["HP"]), Int32.Parse(props["STR"]),
                        Int32.Parse(props["DEX"]), Int32.Parse(props["CON"]));
                }
                else
                {
                    return null;
                }
            }
            catch(Exception)
            {
                Environment.Exit((int)ErrorCode.IO_Load_Error);
                return null;
            }
        }

        public string[] LoadMonstersByStrength(string strength)
        {
            try
            {
                string filepath = Environment.CurrentDirectory + "\\Creatures\\Monsters\\" + strength + "MONSTERS.txt";
                return File.ReadAllLines(filepath);
            }
            catch(Exception)
            {
                Environment.Exit((int)ErrorCode.IO_Load_Error);
                return null;
            }
        }

        public Monster LoadMonsterData(string id)
        {
            try
            {
                string filePath = Environment.CurrentDirectory + "\\Creatures\\Monsters\\" + id + ".txt";
                string[] lines = File.ReadAllLines(filePath);
                Dictionary<string, string> props = new Dictionary<string, string>();
                foreach (string line in lines)
                {
                    string[] parts = line.Split(":");
                    switch(parts[0])
                    {
                        case "NAME":
                            props.Add("NAME", parts[1]);
                            break;
                        case "ID":
                            props.Add("ID", parts[1]);
                            break;
                        case "LEVEL":
                            props.Add("LEVEL", parts[1]);
                            break;
                        case "HP":
                            props.Add("HP", parts[1]);
                            break;
                        case "STR":
                            props.Add("STR", parts[1]);
                            break;
                        case "DEX":
                            props.Add("DEX", parts[1]);
                            break;
                        case "CON":
                            props.Add("CON", parts[1]);
                            break;
                        default:
                            break;
                    }
                }
                return new Monster(props["NAME"], props["ID"], Int32.Parse(props["LEVEL"]), Int32.Parse(props["HP"]), Int32.Parse(props["STR"]), Int32.Parse(props["DEX"]),
                    Int32.Parse(props["CON"]));
            }
            catch (Exception)
            {
                Environment.Exit((int)ErrorCode.IO_Load_Error);
                return null;
            }
        }

        public void SaveResultData(EncounterResult result)
        {
            string filePath = Environment.CurrentDirectory + "\\BATTLERESULT.txt";
            string resultString = "CHARACTER_NAME:" + result.Character.Name + "\nCHARACTER_ID:" + result.Character.ID + "\nMONSTER_NAME:" + result.Monster.Name + "\nMONSTER_ID:" +
                result.Monster.ID + "\nWINNER:" + result.Winner + "\nXP_GAINED:" + result.XPGained;
            File.WriteAllText(filePath, resultString);
        }
    }
}
