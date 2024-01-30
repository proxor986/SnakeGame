using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class Snake
    {
        public Pixel Head { get; private set; }

        private readonly ConsoleColor _headColor;
        private readonly ConsoleColor _bodyColor;


        public Snake(int initialX, int initialY, ConsoleColor headColor, ConsoleColor bodyColor, int bodyLenght = 3)
        {
            _headColor = headColor;
            _bodyColor = bodyColor;

            Head = new Pixel(initialX, initialY, _headColor);

            for (int i = bodyLenght; i > 0; i--) 
            {
                Body.Enqueue(new Pixel(Head.X - i - 1 , initialY, _bodyColor));
            }

            Draw();
        }

        public Queue<Pixel> Body { get; } = new Queue<Pixel>();

        public void Draw()
        {
            Head.Draw();
            foreach (Pixel pixel in Body)
            {
                pixel.Draw();
            }
        }

        public void Clear()
        {
            Head.Clear();
            foreach (Pixel pixel in Body)
            {
                pixel.Clear();
            }
        }

        public void Move(Control control, bool eat = false)
        {
            Clear();

            Body.Enqueue(new Pixel(Head.X, Head.Y, _bodyColor));

            if(!eat)
                Body.Dequeue();

            //Body.Dequeue();

            Head = control switch
            {
                Control.Right => new Pixel(Head.X + 1, Head.Y, _headColor),
                Control.Left => new Pixel(Head.X - 1, Head.Y, _headColor),
                Control.Up => new Pixel(Head.X, Head.Y - 1, _headColor),
                Control.Down => new Pixel(Head.X, Head.Y + 1, _headColor),
                _ => Head
            };
            Draw();

        }

    }
}
