using Battle_Similator.Models.Creatures;

namespace Battle_Similator.Models.Misc
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
            io.SaveCharacterData(character);
            io.SaveResetData(character);
        }
    }
}
