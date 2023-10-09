using Battle_Similator.Models.Creatures;
using Battle_Similator.Models.Resources;

namespace Battle_Similator.Models.NonEncounters
{
    public class CharacterReset
    {
        private IO io;

        public CharacterReset(IO io)
        {
            this.io = io;
        }

        public void Start(string characterID, string characterName)
        {
            Character character = new Character(characterName, characterID);
            character.TakeDamage(9999);
            string resultString = "CHARACTER_NAME:" + character.Name + "\nCHARACTER_ID:" + character.ID + "\nCHARACTER_LEVEL:" + character.Level + "\nCHARACTER_XP:" +
                character.XP + "\nCHARACTER_HP:" + character.HPMax + "\nCHARACTER_STR:" + character.Strength + "\nCHARACTER_DEX:" + character.Dexterity +
                "\nCHARACTER_CON:" + character.Constitution;
            io.SaveCharacterData(character);
            io.SaveResultData(resultString);
        }
    }
}
