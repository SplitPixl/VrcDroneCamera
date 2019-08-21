using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using VRCModLoader;

namespace DroneMod
{
    [VRCModInfo("DroneMod", "2.0", "SplitPixl")]
    public class DroneMod : VRCMod
    {
        private bool initialising = true;
        Server server;
        // All the following methods are optional
        // They also works like Unity's magic methods
        void OnApplicationStart() {
            VRCTools.ModPrefs.RegisterPrefBool("DroneCam", "enabled", true, "Enabled");
            VRCTools.ModPrefs.RegisterPrefInt("DroneCam", "port", 35000, "Webserver Port");
            VRCTools.ModPrefs.RegisterPrefBool("DroneCam", "openatstart", true, "Open on Start");
            VRCTools.ModPrefs.RegisterPrefBool("DroneCam", "nocontroller", false, "Disable Gamepad Input");
            VRCTools.ModPrefs.RegisterPrefBool("DroneCam", "verboselogging", false, "Verbose Logging");
            server = new Server();
        }
        void OnApplicationQuit() { }
        void OnLevelWasLoaded(int level)
        {
            if (level == 0 && initialising)
            {
                initialising = false;
                if (VRCTools.ModPrefs.GetBool("DroneCam", "enabled"))
                {
                    server.Start();
                }

                if (VRCTools.ModPrefs.GetBool("DroneCam", "nocontroller"))
                {
                    GameObject app = GameObject.Find("/_Application");
                    app.GetComponent<VRCInputProcessorController>().enabled = false;
                }
            }
        }
        void OnLevelWasInitialized(int level) { }
        void OnUpdate() { }
        void OnFixedUpdate() { }
        void OnLateUpdate() { }
        void OnGUI() { }
    }
}
