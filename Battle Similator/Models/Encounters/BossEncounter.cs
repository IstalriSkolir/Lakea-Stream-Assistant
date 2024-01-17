using Battle_Similator.Models.Creatures;
using Battle_Similator.Models.Resources;

namespace Battle_Similator.Models.Encounters
{
    public class BossEncounter
    {
        private IO io;
        private HealthBarImage healthBar;
        private string[] bossList;

        public BossEncounter(IO io, string config, string resourcePath)
        {
            this.io = io;
            healthBar = new HealthBarImage(io, config, resourcePath);
            this.bossList = io.LoadBossList();
        }

        public void Start(string characterID, string characterName)
        {
            Encounter encounter = preEncounter(characterID, characterName);
            EncounterResult result = encounter.Run();
            postEncounter(result);
        }

        private Encounter preEncounter(string characterID, string characterName)
        {
            Character character = io.LoadCharacterData(characterID, characterName);
            character.ResetOnDeath = false;
            Monster monster;
            if (io.CurrentBossFileExists())
            {
                monster = io.LoadNPCData("Bosses", "CURRENTBOSS");
            }
            else
            {
                monster = io.LoadNPCData("Bosses", bossList[0]);
            }
            return new Encounter(character, monster, "BOSSBATTLE");
        }

        private void postEncounter(EncounterResult result)
        {
            string bossBeaten = "\nBOSS_BEATEN:";
            string allBossesBeaten = "\nALL_BOSSES_BEATEN:";
            if(result.Winner.Equals(result.Character.ID))
            {
                bossBeaten += "TRUE";
                string finalBoss = bossList[bossList.Length - 1];
                string currentBoss = result.Monster.ID + "-" + result.Monster.Name;
                if(finalBoss.Equals(currentBoss))
                {
                    allBossesBeaten += "TRUE";
                }
                else
                {
                    allBossesBeaten += "FALSE";
                    string thisBoss = result.Monster.ID + "-" + result.Monster.Name;
                    string nextBossString = "";
                    int index = 0;
                    foreach (string boss in bossList)
                    {
                        if (thisBoss.Equals(boss))
                        {
                            nextBossString = bossList[index + 1];
                        }
                        else
                        {
                            index++;
                        }
                    }
                    Monster nextBoss = io.LoadNPCData("Bosses", nextBossString);
                    io.SaveCurrentBossData(nextBoss);
                }
                distributeXP(result.Character.ID, result.Monster.XPValue);
            }
            else
            {
                bossBeaten += "FALSE";
                allBossesBeaten += "FALSE";
                io.SaveCurrentBossData(result.Monster);
                io.AppendCurrentBossFighters(result.Character.ID);
            }
            string resultsString = "ENCOUNTER_TYPE:" + result.EncounterType + "\nCHARACTER_NAME:" + result.Character.Name + "\nCHARACTER_ID:" + 
                result.Character.ID + "\nMONSTER_NAME:" + result.Monster.Name + "\nMONSTER_ID:" + result.Monster.ID + "\nWINNER:" + result.Winner + "\nXP_GAINED:" +
                result.XPGained + "\nLEVEL_UP:" + result.LevelUp.ToString().ToUpper() + "\nCHARACTER_LEVEL:" + result.Character.Level + bossBeaten + allBossesBeaten;
            io.SaveResultData(resultsString);
            io.SaveCharacterData(result.Character);
            healthBar.GenerateHealthBarImage(result.Monster);
        }

        private void distributeXP(string id, int totalXP)
        {
            string[] fightersArray = io.LoadCurrentBossFighters();
            List<string> fighters = new List<string> { id };
            foreach(string fighter in fightersArray)
            {
                if (!fighters.Contains(fighter))
                {
                    fighters.Add(fighter);
                }
            }
            int xpGain = totalXP / fighters.Count;
            xpGain += 5 - (xpGain % 5);
            foreach(string fighter in fighters)
            {
                Character character = io.LoadCharacterData(fighter);
                character.IncreaseXP(xpGain);
                io.SaveCharacterData(character);
            }
            io.DeleteCurrentBossFighters();
        }
    }
}
