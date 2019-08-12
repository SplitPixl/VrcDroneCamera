using System;
using System.Collections.Generic;
using System.Text;

namespace DroneControl.Types
{
    class Transform
    {
        Vector3 position;
        Vector3 rotation;

        public Transform(Vector3 pos, Vector3 rot) {
            position = pos;
            rotation = rot;
        }


    }
}
