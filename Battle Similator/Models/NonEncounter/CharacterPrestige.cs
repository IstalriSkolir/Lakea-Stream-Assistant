using Battle_Similator.Models.Creatures;
using Battle_Similator.Models.Resources;

namespace Battle_Similator.Models.NonEncounter
{
    public class CharacterPrestige
    {
        private IO io;

        public CharacterPrestige(IO io)
        {
            this.io = io;
        }

        public void Start(string characterID, string characterName)
        {
            Character character = io.LoadCharacterData(characterID, characterName);
            character.GainPrestige();
            string resultString = "CHARACTER_NAME:" + character.Name + "\nCHARACTER_ID:" + character.ID + "\nCHARACTER_LEVEL:" + character.Level + "\nCHARACTER_XP:" +
                character.XP + "\nCHARACTER_HP:" + character.HPMax + "\nCHARACTER_STR:" + character.Strength + "\nCHARACTER_DEX:" + character.Dexterity +
                "\nCHARACTER_CON:" + character.Constitution + "\nPRESTIGE:" + character.Prestige;
            io.SaveCharacterData(character);
            io.SaveResultData(resultString);
        }
    }
}
