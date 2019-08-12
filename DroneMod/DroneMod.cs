using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRCModLoader;

namespace DroneMod
{
    [VRCModInfo("DroneMod", "2.0", "SplitPixl")]
    public class DroneMod : VRCMod
    {
        private bool initialising = true;
        Server server = new Server();
        // All the following methods are optional
        // They also works like Unity's magic methods
        void OnApplicationStart() { }
        void OnApplicationQuit() { }
        void OnLevelWasLoaded(int level)
        {
            if (level == 0 && initialising)
            {
                initialising = false;
                server.Start();
            }
        }
        void OnLevelWasInitialized(int level) { }
        void OnUpdate() { }
        void OnFixedUpdate() { }
        void OnLateUpdate() { }
        void OnGUI() { }
    }
}
