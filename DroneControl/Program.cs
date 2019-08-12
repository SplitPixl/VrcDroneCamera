using DroneControl.Controller;
using System;
using System.Threading;
using XInputDotNetPure;

namespace DroneControl
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string dest;
            try
            {
                dest = args[0];
            } catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Where to connect?");
                dest = Console.ReadLine();
            }

            Console.WriteLine(dest);
            Console.Clear();

            XInput input = new XInput();

            while (!Console.KeyAvailable || Console.ReadKey().Key != ConsoleKey.Escape)
            {
                Console.SetCursorPosition(0, 0);
                input.Update();
                GamePadState state = input.GetState();
                Thread.Sleep(1000 / 60);
            }
        }
    }
}
