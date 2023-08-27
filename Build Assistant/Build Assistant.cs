using Build_Assistant.Singletons;

namespace Build_Assistant
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\nBuild Assistant: Loading...");
            List<string> commands = new List<string> { "MoveBuildDirectory" };
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("No arguements given, Build Assistant needs to be given one of the following arguements:");
                foreach (string command in commands)
                    Console.WriteLine("- " + command);
                Console.WriteLine("Build Assistant: Closing...\n");
                return;
            }
            switch (args[0])
            {
                case "MoveBuildDirectory":
                    MoveBuildDirectory.Instance.Run();
                    break;
                default:
                    Console.WriteLine("Build Assistant Error -> Invalid Arguement '" + args[0] + "'");
                    break;
            }
            Console.WriteLine("Build Assistant: Closing...\n");
        }
    }
}