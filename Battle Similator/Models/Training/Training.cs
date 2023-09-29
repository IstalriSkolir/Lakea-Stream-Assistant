using Battle_Similator.Models.Creatures;

namespace Battle_Similator.Models.Training
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

        public void Start(string characterID, string characterName)
        {
            Character character = io.LoadCharacterData(characterID, characterName);
            int xpGained = (random.Next(1, 7) * 5) + ((character.Level / 2) * 5);
            bool levelUp = character.IncreaseXP(xpGained);
            io.SaveCharacterData(character);
            io.SaveTrainingResultData(character, xpGained, levelUp);
        }
    }
}
