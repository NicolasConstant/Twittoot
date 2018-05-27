using System;
using System.IO;

namespace Twittoot.Logic
{
    public class IntroDisplay
    {
        public void Run()
        {
            var displayLines = File.ReadAllLines("Intro.json");
            Console.ForegroundColor = ConsoleColor.Cyan;
            foreach (var line in displayLines)
                Console.WriteLine(line);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }
    }
}