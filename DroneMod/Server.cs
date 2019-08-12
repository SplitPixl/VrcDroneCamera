using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Server;
using VRCModLoader;
using UnityEngine;

namespace DroneMod
{
    class Server
    {
        WebSocketServer server = null;
        CameraManager camman = null;

        public Server()
        {
            int port = 35000; // ModPrefs.GetInt("DroneCam", "Port", 35000);
            server = new WebSocketServer(port);
        }

        public void Start()
        {
            GameObject cammanGO = new GameObject("CameraManager");
            camman = cammanGO.AddComponent<CameraManager>();
            UnityEngine.Object.DontDestroyOnLoad(cammanGO);
            server.AddWebSocketService("/control", () => new InputManager(camman));
            server.Start();
        }
    }
}
