using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XInput.Wrapper;

namespace ControlClient
{
    class Input
    {
        Client client;
        bool going = true;
        int deadzone = 2750;

        public Input(Client client)
        {
            this.client = client;
            DoTheThing();
        }

        private short ShortClamp(int input)
        {
            return (short)Math.Max(Math.Min(input, short.MaxValue), -short.MaxValue);
        }

        private short CalcDeadzone(int input)
        {
            if (Math.Abs(input) < deadzone)
            {
                return 0;
            } else
            {
                return ShortClamp(input);
            }
        }

        private void DoTheThing()
        {
            Console.Clear();
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, Console.WindowHeight - 2);
            Console.Write("Menu - Press highlighted key to select");
            X.StartPolling(X.Gamepad_1);
            while (going)
            {
                GetInput();
                Menu();
            }
            Environment.Exit(0);
        }

        private void GetInput()
        {
            short lx = CalcDeadzone(X.Gamepad_1.LStick.X);
            short ly = CalcDeadzone(X.Gamepad_1.LStick.Y);
            short rx = CalcDeadzone(X.Gamepad_1.RStick.X);
            short ry = CalcDeadzone(X.Gamepad_1.RStick.Y);
            byte tl = (byte)X.Gamepad_1.LTrigger;
            byte tr = (byte)X.Gamepad_1.RTrigger;
            Console.SetCursorPosition(0, 0);
            string hh = string.Format("LX: {0,6} | LY: {1,6} | RX: {2,6} | RY:{3,6} | TL: {4,6} | TR: {5,6}", lx, ly, rx, ry, tl, tr);
            Console.Write(hh);
            short my = ShortClamp((int)(short.MaxValue * (((float)tr - tl) / 255)));
            client.SendInput(lx, my, ly, (short)-ry, rx, 0);
            Thread.Sleep(1000 / 60);
        }

        private void WriteHighlited(string a, string b)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(a);
            Console.ResetColor();
            Console.Write(b);
        }

        private void Menu()
        {
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            WriteHighlited("1", " Movement / ");
            WriteHighlited("2", " Rotation speed, ");
            WriteHighlited("3", " Movement / ");
            WriteHighlited("4", " Rotation smoothing ");
            WriteHighlited("5", " Teleport ");
            WriteHighlited("6", " Reset Position ");
            WriteHighlited("ESC", " Quit ");

            if (Console.KeyAvailable)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.D1:
                        SubmenuMovementSpeed();
                        break;
                    case ConsoleKey.D2:
                        SubmenuRotationSpeed();
                        break;
                    case ConsoleKey.D3:
                        SubmenuMovementSmooth();
                        break;
                    case ConsoleKey.D4:
                        SubmenuRotationSmooth();
                        break;
                    case ConsoleKey.D5:
                        SubmenuTeleport();
                        break;
                    case ConsoleKey.D6:
                        ResetPosition();
                        break;
                    case ConsoleKey.Escape:
                        Quit();
                        break;
                }
            }
        }

        private void SubmenuMovementSpeed()
        {
            Console.CursorVisible = true;
            client.SendInput(0, 0, 0, 0, 0, 0);
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("New Movement Speed:");
            string resp = Console.ReadLine();
            float speed = float.Parse(resp);
            client.SendSetMoveSpeed(speed);
            Console.SetCursorPosition(0, Console.WindowHeight - 2);
            Console.Write(string.Format("Set movement speed to {0}", speed));
            Console.CursorVisible = false;
        }

        private void SubmenuRotationSpeed()
        {
            Console.CursorVisible = true;
            client.SendInput(0, 0, 0, 0, 0, 0);
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("New Rotation Speed:");
            string resp = Console.ReadLine();
            float speed = float.Parse(resp);
            client.SendSetRotateSpeed(speed);
            Console.SetCursorPosition(0, Console.WindowHeight - 2);
            Console.Write(string.Format("Set rotate speed to {0}", speed));
            Console.CursorVisible = false;
        }

        private void SubmenuMovementSmooth()
        {
            Console.CursorVisible = true;
            client.SendInput(0, 0, 0, 0, 0, 0);
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("New Movement Smoothness:");
            string resp = Console.ReadLine();
            float speed = float.Parse(resp);
            client.SendSetMoveSmooth(speed);
            Console.SetCursorPosition(0, Console.WindowHeight - 2);
            Console.Write(string.Format("Set movement smooth to {0}", speed));
            Console.CursorVisible = false;
        }

        private void SubmenuRotationSmooth()
        {
            Console.CursorVisible = true;
            client.SendInput(0, 0, 0, 0, 0, 0);
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("New Rotation Smoothness:");
            string resp = Console.ReadLine();
            float speed = float.Parse(resp);
            client.SendSetRotateSmooth(speed);
            Console.SetCursorPosition(0, Console.WindowHeight - 2);
            Console.Write(string.Format("Set rotate smooth to {0}", speed));
            Console.CursorVisible = false;
        }

        private void SubmenuTeleport()
        {
            Console.CursorVisible = true;
            client.SendInput(0, 0, 0, 0, 0, 0);
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("New Position");
            Console.Write("X: ");
            float px = float.Parse(Console.ReadLine());
            Console.Write("Y: ");
            float py = float.Parse(Console.ReadLine());
            Console.Write("Z: ");
            float pz = float.Parse(Console.ReadLine());
            Console.WriteLine("New Rotation");
            Console.Write("X: ");
            float rx = float.Parse(Console.ReadLine());
            Console.Write("Y: ");
            float ry = float.Parse(Console.ReadLine());
            Console.Write("Z: ");
            float rz = float.Parse(Console.ReadLine());
            client.SendTeleportPos(px, py, pz, rx, ry, rz);
            Console.Clear();
            Console.SetCursorPosition(0, Console.WindowHeight - 2);
            Console.Write(string.Format("Teleported to new position"));
            Console.CursorVisible = false;
        }

        private void ResetPosition()
        {
            client.SendReset();
        }

        private void Quit()
        {
            client.Disconnect();
            Environment.Exit(0);
        }
    }
}
