using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Resources.Lakea;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.WebSocket;
using System.Runtime.InteropServices;

namespace Lakea_Stream_Assistant.Static
{
    //Static class for writing to the console
    public sealed class Terminal
    {
        //Adds new message to the terminal display list
        public static void Output(string message)
        {
            string timeStamp = DateTime.Now.ToString("HH:mm:ss");
            Console.WriteLine($"{timeStamp} -> {message}");
        }
    }
}
