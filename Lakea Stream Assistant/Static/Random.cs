namespace Lakea_Stream_Assistant.Static
{
    public static class Dice
    {
        private static System.Random random = new System.Random();

        public static int Roll(int max)
        {
            return random.Next(max);
        }

        public static int Roll(int min, int max)
        {
            return random.Next(min, max);
        } 
    }
}
