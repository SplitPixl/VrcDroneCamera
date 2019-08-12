using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace ControlClient
{
    class Client
    {
        WebSocket socket;

        public Client(string url)
        {
            socket = new WebSocket(url);
            socket.OnOpen += Socket_OnOpen;
            socket.OnMessage += Socket_OnMessage;
            socket.OnError += Socket_OnError;
            socket.OnClose += Socket_OnClose;
        }

        public void Connect()
        {
            Console.WriteLine("Connecting...");
            socket.Connect();
        }

        private void Socket_OnOpen(object sender, EventArgs e)
        {
            Console.WriteLine("Connected!");
            SendSetMode(DroneMode.FLY);
            Input input = new Input(this);
        }

        private void Socket_OnMessage(object sender, MessageEventArgs e)
        {

        }

        private void Socket_OnError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine("Connection Closed, Press any key to quit.");
            Console.ReadKey();
            Environment.Exit(1);
        }

        private void Socket_OnClose(object sender, CloseEventArgs e)
        {
            Console.WriteLine("Connection Closed, Press any key to quit.");
            Console.ReadKey();
            Environment.Exit(0);
        }

        enum DataType
        {
            SETMODE = 0,
            INPUT = 1,
            SETMOVESPEED = 2,
            SETROTSPEED = 3,
            SETMOVESMOOTH = 4,
            SETROTSMOOTH = 5,
            TELEPORT = 6,
            RESET = 7,
            SETFOLLOWGO = 8,
            SETFOLLOWPLAYER = 9,
            SHUTTER = 10
        }

        public enum DroneMode
        {
            DISABLED = 0,
            FLY = 1,
            FOLLOW = 2,
        }

        public void SendSetMode(DroneMode mode)
        {
            byte[] data = { (byte)DataType.SETMODE, (byte)mode };
            Console.SetCursorPosition(0, 2);
            foreach (byte b in data)
            {
                Console.Write(string.Format("{0,2:X}", b));
            }
            socket.Send(data);
        }

        public void SendInput(short tx, short ty, short tz, short rx, short ry, short rz)
        {
            byte[] data = { (byte)DataType.INPUT };
            data = data
                .Concat(BitConverter.GetBytes(tx)) // translate X
                .Concat(BitConverter.GetBytes(ty)) // translate Y
                .Concat(BitConverter.GetBytes(tz)) // translate Z
                .Concat(BitConverter.GetBytes(rx)) // rotate X
                .Concat(BitConverter.GetBytes(ry)) // rotate Y
                .Concat(BitConverter.GetBytes(rz)) // rotate Z
                .ToArray();
            Console.SetCursorPosition(0, 1);
            foreach (byte b in data)
            {
                Console.Write(string.Format("{0,2:X}", b));
            }
            socket.Send(data);
        }

        public void SendSetMoveSpeed(float speed)
        {
            byte[] data = { (byte)DataType.SETMOVESPEED };
            data = data.Concat(BitConverter.GetBytes(speed))
                .ToArray();
            Console.SetCursorPosition(0, 2);
            foreach (byte b in data)
            {
                Console.Write(string.Format("{0,2:X}", b));
            }
            socket.Send(data);
        }

        public void SendSetRotateSpeed(float speed)
        {
            byte[] data = { (byte)DataType.SETROTSPEED };
            data = data.Concat(BitConverter.GetBytes(speed))
                .ToArray();
            Console.SetCursorPosition(0, 2);
            foreach (byte b in data)
            {
                Console.Write(string.Format("{0,2:X}", b));
            }
            socket.Send(data);
        }

        public void SendSetMoveSmooth(float smooth)
        {
            byte[] data = { (byte)DataType.SETMOVESMOOTH };
            data = data.Concat(BitConverter.GetBytes(smooth))
                .ToArray();
            Console.SetCursorPosition(0, 2);
            foreach (byte b in data)
            {
                Console.Write(string.Format("{0,2:X}", b));
            }
            socket.Send(data);
        }

        public void SendSetRotateSmooth(float smooth)
        {
            byte[] data = { (byte)DataType.SETROTSMOOTH };
            data = data.Concat(BitConverter.GetBytes(smooth))
                .ToArray();
            Console.SetCursorPosition(0, 2);
            foreach (byte b in data)
            {
                Console.Write(string.Format("{0,2:X}", b));
            }
            socket.Send(data);
        }

        public void SendTeleportPos(float tx, float ty, float tz, float rx, float ry, float rz)
        {
            byte[] data = { (byte)DataType.TELEPORT };
            data = data
                .Concat(BitConverter.GetBytes(tx)) // translate X
                .Concat(BitConverter.GetBytes(ty)) // translate Y
                .Concat(BitConverter.GetBytes(tz)) // translate Z
                .Concat(BitConverter.GetBytes(rx)) // rotate X
                .Concat(BitConverter.GetBytes(ry)) // rotate Y
                .Concat(BitConverter.GetBytes(rz)) // rotate Z
                .ToArray();
            Console.SetCursorPosition(0, 1);
            foreach (byte b in data)
            {
                Console.Write(string.Format("{0,2:X}", b));
            }
            socket.Send(data);
        }

        internal void SendReset()
        {
            byte[] data = { (byte)DataType.RESET };
            Console.SetCursorPosition(0, 2);
            foreach (byte b in data)
            {
                Console.Write(string.Format("{0,2:X}", b));
            }
            socket.Send(data);
        }

        internal void Disconnect()
        {
            SendSetMode(DroneMode.DISABLED);
            socket.Close();
        }

    }
}
