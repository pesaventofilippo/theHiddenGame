using System;
using System.Threading;

namespace theHiddenGame {
    class Snake {
        public int x;
        public int y;
        public int prevX;
        public int prevY;
        public int length;
        public string direction;


        public Snake(int x, int y) {
            this.x = x;
            this.y = y;
            this.prevX = x;
            this.prevY = y;
            this.length = 1;
            this.direction = "right";
        }


        public void Move() {
            switch (this.direction) {
                case "up":
                    Thread.Sleep(150);
                    if (this.y > 0)
                        this.y--;
                    else
                        Game.GameOver();
                    break;
                case "down":
                    Thread.Sleep(150);
                    if (this.y < Console.WindowHeight - 1)
                        this.y++;
                    else
                        Game.GameOver();
                    break;
                case "left":
                    Thread.Sleep(80);
                    if (this.x > 0)
                        this.x--;
                    else
                        Game.GameOver();
                    break;
                case "right":
                    Thread.Sleep(80);
                    if (this.x < Console.WindowWidth - 1)
                        this.x++;
                    else
                        Game.GameOver();
                    break;
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(this.x, this.y);
            Console.Write('x');
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(this.prevX, this.prevY);
            Console.Write(' ');
            this.prevX = this.x;
            this.prevY = this.y;
        }


        public void ChangeDirection(ConsoleKey key) {
            switch (key) {
                case ConsoleKey.UpArrow:
                    if (this.direction != "down")
                        this.direction = "up";
                    break;
                case ConsoleKey.DownArrow:
                    if (this.direction != "up")
                        this.direction = "down";
                    break;
                case ConsoleKey.LeftArrow:
                    if (this.direction != "right")
                        this.direction = "left";
                    break;
                case ConsoleKey.RightArrow:
                    if (this.direction != "left")
                        this.direction = "right";
                    break;
            }
        }


        public bool IsEating(Apple apple) {
            return (this.x == apple.x && this.y == apple.y);
        }


        public void Grow() {
            this.length++;
        } 
    }



    class Apple {
        public int x;
        public int y;


        public Apple() {
            Random r = new Random();
            this.x = r.Next(1, Console.WindowWidth - 2);
            this.y = r.Next(1, Console.WindowHeight - 2);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(this.x, this.y);
            Console.Write('+');
        }


        ~Apple() {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(this.x, this.y);
            Console.Write(' ');
        }
    }



    class Game {
        static Snake snake;
        static Apple apple;
        static int score;
        static int highScore;
        static bool gameOver;
        static Thread keyThread;


        public static void Main() {
            Console.CursorVisible = false;
            Console.Title = "theHiddenGame";

            if (highScore == 0)
                StartScreen();

            Console.Clear();
            snake = new Snake(Console.WindowWidth / 2, Console.WindowHeight / 2);
            apple = new Apple();
            score = 0;
            gameOver = false;

            keyThread = new Thread(new ThreadStart(GetKeyPresses));
            keyThread.Start();

            while (!gameOver) {
                snake.Move();
                CheckCollisions();
            }
        }


        public static void StartScreen() {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            CenterText("--- theHiddenGame ---", 1);
            CenterText("by pesaventofilippo", 2);
            Console.ForegroundColor = ConsoleColor.White;
            CenterText("The goal of this game is to move your snake around,", 4);
            CenterText("eating as much apples as you can without crashing into the", 5);
            CenterText("field borders or into your snake's body.", 6);
            CenterText("Press any key to start playing!", 8);
            Console.ForegroundColor = ConsoleColor.Gray;
            CenterText("This game is open-source!", Console.WindowHeight - 3);
            CenterText("https://github.com/pesaventofilippo/theHiddenGame", Console.WindowHeight - 2);
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
        }


        public static void EndScreen() {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            CenterText("Game Over!", 3);
            Console.ForegroundColor = ConsoleColor.White;
            CenterText($"Score: {score}", 5);
            CenterText($"High Score: {highScore}", 6);
            CenterText($"Snake Length: {snake.length}", 7);
            Console.ForegroundColor = ConsoleColor.Gray;
            CenterText($"Press space to retry.", 9);
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.Spacebar)
                Main();
            else {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                CenterText("Thanks for playing!", 3);
                Thread.Sleep(3000);
            }
        }


        public static void GameOver() {
            Yeah();
            gameOver = true;
            keyThread.Abort();
            if (score > highScore)
                highScore = score;
            EndScreen();
        }


        public static void GetKeyPresses() {
            while (!gameOver) {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key != ConsoleKey.Escape) {
                    snake.ChangeDirection(keyInfo.Key);
                }
            }
        }


        public static void CheckCollisions() {
            if (snake.IsEating(apple)) {
                score += snake.length;
                snake.Grow();
                Console.Beep();
                apple = new Apple();
            }
        }


        public static void CenterText(string text, int y) {
            Console.SetCursorPosition(Console.WindowWidth / 2 - text.Length / 2 - 1, y);
            Console.Write(text);
        }
    }
}
