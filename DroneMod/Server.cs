using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRCTools;
using UnityEngine;
using System.IO;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;
using MimeTypes;

namespace DroneMod
{
    class Server
    {
        HttpServer server = null;
        CameraManager camman = null;

        public Server()
        {
            int port = ModPrefs.GetInt("DroneCam", "port");
            server = new HttpServer(port);

            string webroot = Path.GetFullPath(@".\UserData\DroneCam\www");
            server.OnGet += (sender, e) => {
                var req = e.Request;
                var res = e.Response;

                if (ModPrefs.GetBool("DroneCam", "verboselogging"))
                {
                    Console.WriteLine(string.Format("[DroneCam][Server] GET {0}", req.RawUrl));
                }

                var path = req.RawUrl;
                if (path == "/")
                    path += "index.html";

                var abspath = webroot + path.Replace("/", "\\");
                
                if (File.Exists(abspath))
                {
                    try
                    {
                        res.ContentType = MimeTypeMap.GetMimeType(path.Split('.').Last());
                        res.ContentEncoding = Encoding.UTF8;
                        byte[] contents = File.ReadAllBytes(abspath);
                        res.WriteContent(contents);
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine(err);
                        res.StatusCode = 500;
                        res.WriteContent(Encoding.UTF8.GetBytes(err.ToString()));
                    }
                } else
                {
                    res.Redirect("http://" + req.UserHostName + "/");
                }

            }; ;
        }

        public void Start()
        {
            GameObject cammanGO = new GameObject("CameraManager");
            camman = cammanGO.AddComponent<CameraManager>();
            UnityEngine.Object.DontDestroyOnLoad(cammanGO);
            server.AddWebSocketService("/control", () => new InputManager(camman));
            server.Start();
            Console.WriteLine(string.Format("[DroneCam] Server running on http://{0}:{1}/", server.Address, server.Port));
            if (ModPrefs.GetBool("DroneCam", "openatstart"))
            {
                System.Diagnostics.Process.Start(string.Format("http://localhost:{1}/", server.Address, server.Port));
            }
        }
    }
}
