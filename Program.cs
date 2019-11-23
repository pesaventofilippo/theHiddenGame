using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace theHiddenGame {
    class SnakeBody {
        public int x;
        public int y;


        public SnakeBody(int x, int y) {
            this.x = x;
            this.y = y;
        }
    }



    class Snake {
        public Queue<SnakeBody> body;
        public int length;
        private string direction;
        private bool toGrow;


        public Snake(int x, int y) {
            direction = "right";
            length = 1;
            toGrow = false;
            body = new Queue<SnakeBody>();
            body.Enqueue(new SnakeBody(x, y));
        }


        public void Move() {
            SnakeBody head = body.Last();
            int xDiff = 0;
            int yDiff = 0;

            switch (direction) {
                case "up":
                    Thread.Sleep(150);
                    if (head.y > 0)
                        yDiff--;
                    else
                        Game.GameOver();
                    break;

                case "down":
                    Thread.Sleep(150);
                    if (head.y < Console.WindowHeight - 1)
                        yDiff++;
                    else
                        Game.GameOver();
                    break;

                case "left":
                    Thread.Sleep(80);
                    if (head.x > 0)
                        xDiff--;
                    else
                        Game.GameOver();
                    break;

                case "right":
                    Thread.Sleep(80);
                    if (head.x < Console.WindowWidth - 1)
                        xDiff++;
                    else
                        Game.GameOver();
                    break;
            }

            SnakeBody newHead = new SnakeBody(head.x + xDiff, head.y + yDiff);
            foreach (SnakeBody b in body) {
                if (newHead.x == b.x && newHead.y == b.y)
                    Game.GameOver();
            }

            body.Enqueue(newHead);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(head.x, head.y);
            Console.Write('x');
            Console.SetCursorPosition(newHead.x, newHead.y);
            Console.Write('x');

            if (!toGrow) {
                SnakeBody last = body.Dequeue();
                Console.SetCursorPosition(last.x, last.y);
                Console.Write(' ');
            }
            else toGrow = false;
        }


        public void ChangeDirection(ConsoleKey key) {
            switch (key) {
                case ConsoleKey.UpArrow:
                    if (direction != "down")
                        direction = "up";
                    break;
                case ConsoleKey.DownArrow:
                    if (direction != "up")
                        direction = "down";
                    break;
                case ConsoleKey.LeftArrow:
                    if (direction != "right")
                        direction = "left";
                    break;
                case ConsoleKey.RightArrow:
                    if (direction != "left")
                        direction = "right";
                    break;
            }
        }


        public bool IsEating(Apple apple) {
            return (body.Last().x == apple.x && body.Last().y == apple.y);
        }


        public void Grow() {
            length++;
            toGrow = true;
        } 
    }



    class Apple {
        public int x;
        public int y;


        public Apple() {
            Random r = new Random();
            x = r.Next(1, Console.WindowWidth - 2);
            y = r.Next(1, Console.WindowHeight - 2);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(x, y);
            Console.Write('+');
        }


        ~Apple() {
            Console.SetCursorPosition(x, y);
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
            CenterText($"Snake Length: {snake.body.Count}", 7);
            Console.ForegroundColor = ConsoleColor.Gray;
            CenterText($"Press space to retry.", 9);

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.Spacebar)
                Main();
            else {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                CenterText("Thanks for playing!", 3);
                Thread.Sleep(2000);
            }
        }


        public static void GameOver() {
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
