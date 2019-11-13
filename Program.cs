using System;

namespace theHiddenGame {
    class MainClass {
        public static void Main(string[] args) {
            Console.WriteLine("-- theHiddenGame --");
            ConsoleKeyInfo keyInfo;
            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Escape) {
                switch (keyInfo.Key) {
                    case ConsoleKey.UpArrow:
                        Console.WriteLine("Up Arrow!");
                        break;

                    case ConsoleKey.DownArrow:
                        Console.WriteLine("Down Arrow!");
                        break;

                    case ConsoleKey.LeftArrow:
                        Console.WriteLine("Left Arrow!");
                        break;

                    case ConsoleKey.RightArrow:
                        Console.WriteLine("Right Arrow!");
                        break;
                }
            }
        }
    }
}
