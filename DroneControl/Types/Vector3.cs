using System;
using System.Collections.Generic;
using System.Text;

namespace DroneControl.Types
{
    class Vector3
    {
        float x;
        float y;
        float z;

        public Vector3(float x = 0, float y = 0, float z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vector3 Add(Vector3 v1, Vector3 v2)
        {
            return new Vector3(
                v1.x + v2.x,
                v1.y + v2.y,
                v1.z + v2.z);
        }

        public static Vector3 Subtract(Vector3 v1, Vector3 v2)
        {
            return new Vector3(
                v1.x - v2.x,
                v1.y - v2.y,
                v1.z - v2.z);
        }

        public static Vector3 Multiply(Vector3 v1, Vector3 v2)
        {
            return new Vector3(
                v1.x * v2.x,
                v1.y * v2.y,
                v1.z * v2.z);
        }

        public static Vector3 Divide(Vector3 v1, Vector3 v2)
        {
            return new Vector3(
                v1.x / v2.x,
                v1.y / v2.y,
                v1.z / v2.z);
        }

        public new string ToString => string.Format("x: {0,7:N3} y: {1,7:N3}, z: {2,7:N3}", x, y, z);
    }
}
