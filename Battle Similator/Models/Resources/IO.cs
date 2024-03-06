using Battle_Similator.Models.Creatures;

namespace Battle_Similator.Models.Resources
{
    public class IO
    {
        string path;

        public IO(string config, string resourcePath)
        {
            if (config == "LAKEA")
            {
                path = resourcePath;
            }
            else if (config == "DEBUG")
            {
                path = Environment.CurrentDirectory + "\\Creatures\\";
            }
            else
            {
                Environment.Exit((int)ExitCode.Invalid_Args);
            }
        }

        #region Character Data

        public void SaveCharacterData(Character character)
        {
            try
            {
                string filePath = path + "Characters\\" + character.ID + ".txt";
                string characterString = "NAME:" + character.Name + "\nID:" + character.ID + "\nLEVEL:" + character.Level + "\nXP:" + character.XP + "\nHP:" + character.HPMax +
                    "\nSTR:" + character.Strength + "\nDEX:" + character.Dexterity + "\nCON:" + character.Constitution + "\nDEATHS:" + character.Deaths + "\nMONSTERS_KILLED:" +
                    character.MonstersKilled + "\nBOSSES_FOUGHT:" + character.BossesFought + "\nBOSSES_BEATEN:" + character.BossesBeaten + "\nMONSTER_WIN_RATE:" + 
                    character.MonsterWinRate + "\nPRESTIGE:" + character.Prestige;
                File.WriteAllText(filePath, characterString);
            }
            catch (Exception)
            {
                Environment.Exit((int)ExitCode.IO_Save_Error);
            }
        }

        public Character LoadCharacterData(string id, string name = "")
        {
            try
            {
                string filePath = path + "Characters\\" + id + ".txt";
                if (File.Exists(filePath))
                {
                    
                    string[] lines = File.ReadAllLines(filePath);
                    Dictionary<string, string> props = new Dictionary<string, string>();
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(":");
                        switch (parts[0])
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
                            case "DEATHS":
                                props.Add("DEATHS", parts[1]);
                                break;
                            case "MONSTERS_KILLED":
                                props.Add("MONSTERS_KILLED", parts[1]);
                                break;
                            case "BOSSES_FOUGHT":
                                props.Add("BOSSES_FOUGHT", parts[1]);
                                break;
                            case "BOSSES_BEATEN":
                                props.Add("BOSSES_BEATEN", parts[1]);
                                break;
                            case "MONSTER_WIN_RATE":
                                props.Add("MONSTER_WIN_RATE", parts[1]);
                                break;
                            case "PRESTIGE":
                                props.Add("PRESTIGE", parts[1]);
                                break;
                            default:
                                break;
                        }
                    }
                    return new Character(props);
                }
                else
                {
                    return new Character(name, id);
                }
            }
            catch (Exception)
            {
                Environment.Exit((int)ExitCode.IO_Load_Error);
                return null;
            }
        }

        #endregion

        #region Monster Data

        public string[] LoadMonstersByStrength(string strength)
        {
            try
            {
                string filepath = path + "Monsters\\" + strength + "MONSTERS.txt";
                return File.ReadAllLines(filepath);
            }
            catch (Exception)
            {
                Environment.Exit((int)ExitCode.IO_Load_Error);
                return null;
            }
        }

        public string[] LoadBossList()
        {
            try
            {
                string filepath = path + "Bosses\\BOSSLIST.txt";
                return File.ReadAllLines(filepath);
            }
            catch (Exception ex)
            {
                Environment.Exit((int)ExitCode.IO_Load_Error);
                return null;
            }
        }

        public bool CurrentBossFileExists()
        {
            try
            {
                return File.Exists(path + "Bosses\\CURRENTBOSS.txt");
            }
            catch (Exception ex)
            {
                Environment.Exit((int)ExitCode.IO_Load_Error);
                return false;
            }
        }

        public Dictionary<string, string> LoadBossProfilePicturePaths()
        {
            try
            {
                string[] pathsArray = File.ReadAllLines(path + "Bosses\\BOSSPROFILEPICPATHS.txt");
                Dictionary<string, string> pathsDict = new Dictionary<string, string>();
                foreach (string path in pathsArray)
                {
                    string[] parts = path.Split(":", 2);
                    pathsDict.Add(parts[0], parts[1]);
                }
                return pathsDict;
            }
            catch (Exception ex)
            {
                Environment.Exit((int)ExitCode.IO_Load_Error);
                return null;
            }
        }

        public void SaveCurrentBossData(Monster boss)
        {
            try
            {
                string filePath = path + "Bosses\\CURRENTBOSS.txt";
                string bossString = "NAME:" + boss.Name + "\nID:" + boss.ID + "\nLEVEL:" + boss.Level + "\nCURRENT_HP:" + boss.HP + "\nMAX_HP:" + boss.HPMax +
                    "\nSTR:" + boss.Strength + "\nDEX:" + boss.Dexterity + "\nCON:" + boss.Constitution;
                File.WriteAllText(filePath, bossString);
            }
            catch (Exception ex)
            {
                Environment.Exit((int)ExitCode.IO_Save_Error);
            }
        }

        public Monster LoadNPCData(string type, string id)
        {
            try
            {
                string filePath = path + type + "\\" + id + ".txt";
                string[] lines = File.ReadAllLines(filePath);
                Dictionary<string, string> props = new Dictionary<string, string>();
                foreach (string line in lines)
                {
                    string[] parts = line.Split(":");
                    switch (parts[0])
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
                        case "CURRENT_HP":
                            props.Add("CURRENT_HP", parts[1]);
                            break;
                        case "MAX_HP":
                            props.Add("MAX_HP", parts[1]);
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
                if ("Bosses".Equals(type))
                {
                    return new Monster(props["NAME"], props["ID"], int.Parse(props["LEVEL"]), int.Parse(props["CURRENT_HP"]), int.Parse(props["MAX_HP"]),
                        int.Parse(props["STR"]), int.Parse(props["DEX"]), int.Parse(props["CON"]), int.Parse(props["LEVEL"]) * 60);
                }
                else
                {
                    return new Monster(props["NAME"], props["ID"], int.Parse(props["LEVEL"]), int.Parse(props["HP"]), int.Parse(props["HP"]),
                        int.Parse(props["STR"]), int.Parse(props["DEX"]), int.Parse(props["CON"]), int.Parse(props["LEVEL"]) * 20);
                }
            }
            catch (Exception)
            {
                Environment.Exit((int)ExitCode.IO_Load_Error);
                return null;
            }
        }

        #endregion

        #region NonCreature Data

        public void SaveResultData(string result)
        {
            try
            {
                string filePath = path + "..\\RESULT.txt";
                File.WriteAllText(filePath, result);
            }
            catch (Exception ex)
            {
                Environment.Exit((int)ExitCode.IO_Save_Error);
            }
        }

        public void AppendCurrentBossFighters(string id)
        {
            try
            {
                string filePath = path + "Bosses\\CURRENTBOSSFIGHTERS.txt";
                File.AppendAllText(filePath, id + "\n");
            }
            catch(Exception ex)
            {
                Environment.Exit((int)ExitCode.IO_Save_Error);
            }
        }

        public string[] LoadCurrentBossFighters()
        {
            try
            {
                string filePath = path + "Bosses\\CURRENTBOSSFIGHTERS.txt";
                if(File.Exists(filePath))
                {
                    return File.ReadAllLines(filePath);
                }
                else
                {
                    return new string[0];
                }
            }
            catch(Exception ex )
            {
                Environment.Exit((int)ExitCode.IO_Load_Error);
                return null;
            }
        }

        public void DeleteCurrentBossFighters()
        {
            try
            {
                string filePath = path + "Bosses\\CURRENTBOSSFIGHTERS.txt";
                if(File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch
            {
                Environment.Exit((int)ExitCode.IO_Save_Error);
            }
        }

        #endregion
    }
}
