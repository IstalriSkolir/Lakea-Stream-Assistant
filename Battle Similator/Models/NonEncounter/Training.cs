using Battle_Similator.Models.Creatures;
using Battle_Similator.Models.Resources;

namespace Battle_Similator.Models.NonEncounters
{
    public class Training
    {
        private IO io;
        Random random;

        public Training(IO io)
        {
            this.io = io;
            random = new Random();
        }

        public void Start(string characterID, string characterName, string subTier)
        {
            Character character = io.LoadCharacterData(characterID, characterName);
            character.SetTwitchSubTier(subTier);
            int xpGained = (random.Next(1, 9) * 5) + (character.Level * 5);
            if(xpGained > 100)
            {
                xpGained = 100;
            }
            xpGained = character.ApplySubBonusXP(xpGained);
            bool levelUp = character.IncreaseXP(xpGained);
            string resultString = "CHARACTER_NAME:" + character.Name + "\nCHARACTER_ID:" + character.ID + "\nXP_GAINED:" + xpGained + "\nLEVEL_UP:" +
                levelUp.ToString().ToUpper() + "\nCHARACTER_LEVEL:" + character.Level;
            io.SaveCharacterData(character);
            io.SaveResultData(resultString);
        }
    }
}
