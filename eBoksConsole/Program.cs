using System;
using MinBoks;

namespace eBoksConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Begynder at hente data fra EBoks");

            EboksServer eboksServer = new EboksServer();
            eboksServer.ConsoleRun();

            Console.WriteLine("Press any key to stop.");
            Console.ReadLine();
        }
    }
}
