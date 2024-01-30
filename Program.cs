using System;
using System.Data;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using static System.Console;

namespace SnakeGame
{

    class Program

    {
        private const int GameAreaWidth = 30;
        private const int GameAreaHeight = 30;

        private const int ScreenAreaWidth = GameAreaWidth * 3;
        private const int ScreenAreaHeight = GameAreaHeight * 3;

      


        private const ConsoleColor BorderColor = ConsoleColor.White;
        private const ConsoleColor BoosterColor = ConsoleColor.Blue;

        private const ConsoleColor HeadColor = ConsoleColor.Green;
        
        static ConsoleColor BodyColor = ConsoleColor.Gray;

        private static readonly Random random= new Random();
        private const ConsoleColor FoodColor = ConsoleColor.Yellow;
        static void Main()
        {
            SetWindowSize(ScreenAreaWidth, ScreenAreaHeight);
            SetBufferSize(ScreenAreaWidth, ScreenAreaHeight);
            CursorVisible = false;

            while(true)
            {
                StartGame();

                Thread.Sleep(2500);
                ReadKey();
            }
            
        }



        static void StartGame()
        {

            int Frame = 200;

            Clear();

            DrawBorder();

            var snake = new Snake(15, 15, HeadColor, BodyColor);

            Pixel boost = GenBooster(snake);
            Pixel food = GenFood(snake);
            food.Draw();
            

            int score = 0;
            int counter = 0;

            Stopwatch sw = new Stopwatch();
            

            Control control = Control.Right;



            while (true)
            {
                sw.Restart();

                Control oldControl = control;

                while (sw.ElapsedMilliseconds <= Frame)
                {
                    if (control == oldControl) control = ReadControl(control);
                }


                if(snake.Head.X==food.X && snake.Head.Y == food.Y)
                {
                    Frame = 200;
                    snake.Move(control,true);
                    food = GenFood(snake);
                    food.Draw();

                    score++;

                    //boost
                    if (score > 4 && score % 5 == 0)
                    {
                        boost = GenBooster(snake);
                        boost.Draw();
                    }
                    //


                    Task.Run(()=> Beep(200,200));
                    
                }
                else snake.Move(control);


                if (snake.Head.X == boost.X && snake.Head.Y == boost.Y)
                {          
                   
                    int choice = random.Next(0, 2);
                    switch (choice)
                    {
                        case 0: 
                            Frame= 100;
                            break;
                        case 1:
                            Frame = 400;
                            break;
                    }                   
                }
                




                if (snake.Head.X == GameAreaWidth - 1
                    || snake.Head.X == 0
                    || snake.Head.Y == GameAreaHeight - 1
                    || snake.Head.Y == 0)
                    //|| snake.Body.Any(b => b.X == snake.Head.X && b.Y == snake.Head.Y))
                    break;

                if (snake.Body.Any(b => b.X == snake.Head.X && b.Y == snake.Head.Y))
                {
                    Task.Run(() => Beep(200,200));
                    snake.Clear();
                    snake = new Snake(snake.Head.X, snake.Head.Y,HeadColor, BodyColor = (ConsoleColor)random.Next(1,10)) ;
                    snake.Move(oldControl);
                    counter++;
                    if (counter== 3) { break; }                   
                }

            }

            snake.Clear();

            SetCursorPosition(ScreenAreaWidth / 2, ScreenAreaHeight / 2);
            Write("Конец игры, Счет: {0}\n",score);
            Task.Run(() => Beep(2500, 700));
        }

        static Pixel GenFood(Snake snake)
        {
            Pixel food;
            do
            {
                food = new Pixel(random.Next(1, GameAreaWidth - 2), random.Next(1, GameAreaHeight - 2), FoodColor);
            } while (snake.Head.X == food.X && snake.Head.Y == food.Y
            || snake.Body.Any(p => p.X == food.X && p.Y == food.Y));

            return food;
        }



        static Pixel GenBooster(Snake snake)
        {
            Pixel booster;
            do
            {
                booster = new Pixel(random.Next(1, GameAreaWidth - 2), random.Next(1, GameAreaHeight - 2), BoosterColor);
            } while (snake.Head.X == booster.X && snake.Head.Y == booster.Y
            || snake.Body.Any(p => p.X == booster.X && p.Y == booster.Y));

            return booster;
        }




        static Control ReadControl(Control currentControl)
        {
            if (!KeyAvailable)
                return currentControl;

            ConsoleKey key = ReadKey(true).Key;

            currentControl = key switch
            {
                ConsoleKey.UpArrow when currentControl != Control.Down => Control.Up,
                ConsoleKey.DownArrow when currentControl != Control.Up => Control.Down,
                ConsoleKey.LeftArrow when currentControl != Control.Right => Control.Left,
                ConsoleKey.RightArrow when currentControl != Control.Left => Control.Right,
                _ => currentControl
            };

            return currentControl;
        }





        static void DrawBorder()
        {
            for (int i = 0; i<GameAreaWidth; i++)
            {
                new Pixel(i,0,BorderColor).Draw();
                new Pixel(i, GameAreaWidth - 1, BorderColor).Draw();
            }

            for (int i = 0; i < GameAreaHeight; i++)
            {
                new Pixel(0, i, BorderColor).Draw();
                new Pixel(GameAreaHeight - 1, i, BorderColor).Draw();
            }
        }


    }
}