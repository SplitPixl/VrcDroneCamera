using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace DroneMod
{
    class InputManager : WebSocketBehavior
    {
        private CameraManager camman;

        public InputManager(CameraManager camman)
        {
            this.camman = camman;
        }

        protected override void OnOpen()
        {
            Console.WriteLine("[Drn] Camera Client Connected!");
            camman.Reset();
        }

        protected override void OnClose(CloseEventArgs e)
        {
            camman.SetMode(CameraManager.DroneMode.DISABLED);
        }

        protected override void OnError(ErrorEventArgs e)
        {
            Console.WriteLine(e);
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            try
            {
                //foreach (byte b in e.RawData)
                //{
                //    Console.Write(string.Format("{0,2:X}", b));
                //}
                //Console.WriteLine("\r");
                switch ((DataType)e.RawData[0])
                {
                    case DataType.SETMODE:
                        ProcessMode(e.RawData);
                        break;
                    case DataType.INPUT:
                        ProcessInput(e.RawData);
                        break;
                    case DataType.SETMOVESPEED:
                    case DataType.SETROTSPEED:
                    case DataType.SETMOVESMOOTH:
                    case DataType.SETROTSMOOTH:
                        ProcessFloatUpdate(e.RawData);
                        break;
                    case DataType.TELEPORT:
                        ProcessTeleportPos(e.RawData);
                        break;
                    case DataType.RESET:
                        ProcessReset();
                        break;
                    case DataType.CAMERASETTING:
                        ProcessCameraSetting(e.RawData);
                        break;
                    default:
                        Console.Write("Invalid OP Code: 0x{0:X}", e.RawData[0]);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void ProcessFloatUpdate(byte[] data)
        {
            float f = BitConverter.ToSingle(data, 1);

            switch ((DataType)data[0])
            {
                case DataType.SETMOVESPEED:
                    camman.translateSpeed = f;
                    break;
                case DataType.SETROTSPEED:
                    camman.rotateSpeed = f;
                    break;
                case DataType.SETMOVESMOOTH:
                    camman.translateSmooth = f;
                    break;
                case DataType.SETROTSMOOTH:
                    camman.rotateSmooth = f;
                    break;
            }
        }

        private void ProcessMode(byte[] rawData)
        {
            camman.SetMode((CameraManager.DroneMode)rawData[1]);
        }

        private void ProcessInput(byte[] data)
        {
            float translateX = (float)BitConverter.ToInt16(data, 1) / 32767;
            float translateY = (float)BitConverter.ToInt16(data, 3) / 32767;
            float translateZ = (float)BitConverter.ToInt16(data, 5) / 32767;
            Vector3 translate = new Vector3(translateX, translateY, translateZ);
            float rotateX = (float)BitConverter.ToInt16(data, 7) / 32767;
            float rotateY = (float)BitConverter.ToInt16(data, 9) / 32767;
            float rotateZ = (float)BitConverter.ToInt16(data, 11) / 32767;
            Vector3 rotate = new Vector3(rotateX, rotateY, rotateZ);

            camman.SetInput(translate, rotate);
        }

        private void ProcessTeleportPos(byte[] data)
        {
            float translateX = BitConverter.ToSingle(data, 1);
            float translateY = BitConverter.ToSingle(data, 5);
            float translateZ = BitConverter.ToSingle(data, 9);
            Vector3 translate = new Vector3(translateX, translateY, translateZ);
            float rotateX = BitConverter.ToSingle(data, 13);
            float rotateY = BitConverter.ToSingle(data, 17);
            float rotateZ = BitConverter.ToSingle(data, 21);
            Vector3 rotate = new Vector3(rotateX, rotateY, rotateZ);

            camman.TeleportPos(translate, rotate);
        }

        private void ProcessReset()
        {
            camman.Reset();
        }

        private void ProcessCameraSetting(byte[] data)
        {
            switch ((CameraSetting)data[1])
            {
                case CameraSetting.FOV:
                    float value = BitConverter.ToSingle(data, 2);
                    camman.SetFOV(value);
                    break;
            }
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
            CAMERASETTING = 8
        }

        enum CameraSetting
        {
            FOV = 0
        }

    }
}
