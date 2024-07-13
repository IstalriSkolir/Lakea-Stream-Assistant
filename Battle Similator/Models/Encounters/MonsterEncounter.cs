using Battle_Similator.Models.Creatures;
using Battle_Similator.Models.Resources;
using System.Threading;

namespace Battle_Similator.Models.Encounters
{
    public class MonsterEncounter
    {
        private IO io;

        public MonsterEncounter(IO io)
        {
            this.io = io;
        }

        public void Start(string characterID, string characterName, string subTier, string monsterStrength = "", string monsterFile = "", bool randomMonster = true)
        {
            Encounter encounter = null;
            if (randomMonster)
            {
                encounter = preEncounter(characterID, characterName, subTier, randomMonster, monsterStrength: monsterStrength);
            }
            else
            {
                encounter = preEncounter(characterID, characterName,subTier, randomMonster, monsterFile: monsterFile);
            }
            EncounterResult result = encounter.Run();
            postEncounter(result);
        }

        private Encounter preEncounter(string characterID, string characterName, string subTier, bool randomMonster, string monsterStrength = "", string monsterFile = "")
        {
            Character character = io.LoadCharacterData(characterID, characterName);
            character.SetTwitchSubTier(subTier);
            if(randomMonster)
            {
                Monster monster = getMonster(monsterStrength);
                return new Encounter(character, monster, monsterStrength + "MONSTER");
            }
            else
            {
                Monster monster = io.LoadNPCData(CreatureType.Monster, monsterFile);
                return new Encounter(character, monster, monster.Difficulty + "MONSTER");
            }
        }

        private Monster getMonster(string strength)
        {
            string[] monsters = io.LoadMonstersByStrength(strength);
            Random random = new Random();
            int index = random.Next(0, monsters.Length);
            string id = monsters[index];
            return io.LoadNPCData(CreatureType.Monster, id);
        }

        private void postEncounter(EncounterResult result)
        {
            if (result.Winner.Equals(result.Character.ID))
            {
                result.Character.NewMonsterKilled();
                bool levelUp = result.Character.IncreaseXP(result.XPGained);
                result.LevelUp = levelUp;
            }
            result.Character.UpdateMonsterWinRate();
            string resultString = "ENCOUNTER_TYPE:" + result.EncounterType + "\nCHARACTER_NAME:" + result.Character.Name + "\nCHARACTER_ID:" + result.Character.ID +
                "\nMONSTER_NAME:" + result.Monster.Name + "\nMONSTER_ID:" + result.Monster.ID + "\nWINNER:" + result.Winner + "\nXP_GAINED:" + result.XPGained +
                "\nLEVEL_UP:" + result.LevelUp.ToString().ToUpper() + "\nCHARACTER_LEVEL:" + result.Character.Level;
            io.SaveResultData(resultString);
            io.SaveCharacterData(result.Character);
        }
    }
}
