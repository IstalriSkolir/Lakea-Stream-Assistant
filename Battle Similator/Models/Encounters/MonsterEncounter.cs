using Battle_Similator.Models.Creatures;

namespace Battle_Similator.Models.Encounters
{
    public class MonsterEncounter
    {
        private IO io;

        public MonsterEncounter()
        {
            io = new IO();
        }

        public void Start(string monsterStrength, string characterID, string characterName)
        {
            Encounter encounter = preEncounter(monsterStrength, characterID, characterName);
            EncounterResult result = encounter.Run();
            postEncounter(result);
        }

        private Encounter preEncounter(string monsterStrength, string characterID, string characterName)
        {
            Monster monster = getMonster(monsterStrength);
            Character character = getCharacter(characterID, characterName);
            return new Encounter(character, monster);
        }

        private Monster getMonster(string strength)
        {
            string[] monsters = io.LoadMonstersByStrength(strength);
            Random random = new Random();
            int index = random.Next(0, monsters.Length);
            string id = monsters[index];
            return io.LoadMonsterData(id);
        }

        private Character getCharacter(string id, string name)
        {
            Character character = io.LoadCharacterData(id);
            if (character != null)
            {
                return character;
            }
            else
            {
                return new Character(name, id);
            }
        }

        private void postEncounter(EncounterResult result)
        {
            io.SaveCharacterData(result.Character);
            io.SaveResultData(result);
        }
    }
}
