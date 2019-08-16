using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Reflection;
using System;

namespace DebuggerMod.Serial
{
    public class GameObjectSerial
    {
        public string name;
        public bool active;
        public int instanceId;
        public string tag;
        public int layer;
        public List<ComponentSerial> components;
        public List<int> children;
        public string path;
        public int? parent;
        public GameObject source;

        public static string GetPath(Transform getPathTransform)
        {
            string path = "";
            while (getPathTransform.parent != null)
            {
                getPathTransform = getPathTransform.parent;
                path = string.Format("/{0}{1}", getPathTransform.name, path);
            }
            return path;
        }

        public GameObjectSerial(GameObject go)
        {
            source = go;
            path = "/" + go.name;
            name = go.name;
            active = go.activeSelf;
            instanceId = go.GetInstanceID();
            tag = go.tag;
            layer = go.layer;
            components = new List<ComponentSerial>();
            children = new List<int>();
            parent = go.transform.parent?.GetInstanceID();

            List<string> avoidablePropertyNames = GetAvoidablePropertyNames();

            foreach (Component comp in go.GetComponents<Component>())
            {
                Dictionary<string, ComponentSerialValue> compFields = new Dictionary<string, ComponentSerialValue>();
                FieldInfo[] fields = comp.GetType().GetFields();

                foreach (FieldInfo field in fields)
                {
                    ComponentSerialValue value = new ComponentSerialValue(field, comp);
                    compFields.Add(field.Name, value);
                }

                Dictionary<string, ComponentSerialValue> props = new Dictionary<string, ComponentSerialValue>();
                PropertyInfo[] properties = comp.GetType().GetProperties();

                foreach (PropertyInfo prop in properties)
                {
                    if (avoidablePropertyNames.Contains(prop.Name))
                    {
                        continue;
                    }
                    else
                    {
                        ComponentSerialValue value = new ComponentSerialValue(prop, comp);
                        props.Add(prop.Name, value);
                    }
                }

                List<ComponentMethod> methds = new List<ComponentMethod>();
                MethodInfo[] methods = comp.GetType().GetMethods();

                foreach (MethodInfo method in methods)
                {
                    List<string> args = new List<string>();

                    foreach (Type arg in method.GetGenericArguments())
                    {
                        args.Add(arg.ToString());
                    }
                    ComponentMethod methd = new ComponentMethod()
                    {
                        type = method.ReturnType.ToString(),
                        name = method.Name,
                        args = args
                    };
                }

                ComponentSerial compSerial = new ComponentSerial()
                {
                    type = comp.GetType().ToString(),
                    instanceId = comp.GetInstanceID(),
                    fields = compFields,
                    properties = props,
                    //methods = methds
                };

                components.Add(compSerial);
            }

            for (int i = 0; i < go.transform.childCount; i++)
            {
                children.Add(go.transform.GetChild(i).gameObject.GetInstanceID());
            }
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public override string ToString()
        {
            string resp = "";

            resp += string.Format("[{0}] [{1}] {2}\n", active, source.activeInHierarchy, path);
            foreach (ComponentSerial comp in components)
            {
                resp += string.Format(">{0}", comp.type);
                resp += "=>Fields:\n";
                foreach (string name in comp.fields.Keys)
                {
                    ComponentSerialValue val;
                    comp.fields.TryGetValue(name, out val);
                    resp += string.Format("->({0}{1}: {2}\n", val.type, name, val.value);
                }
                resp += "=>Properties:\n";
                foreach (string name in comp.properties.Keys)
                {
                    ComponentSerialValue val;
                    comp.fields.TryGetValue(name, out val);
                    resp += string.Format("->({0}{1}: {2}\n", val.type, name, val.value);
                }
            }

            return resp;
        }

        private static List<string> GetAvoidablePropertyNames()
        {
            List<string> avoidablePropertyNames = new List<string>();
            foreach (PropertyInfo avoidableProperty in typeof(Component).GetProperties())
            {
                avoidablePropertyNames.Add(avoidableProperty.Name);
            }
            return avoidablePropertyNames;
        }
    }
    public class ComponentSerial
    {
        public string type;
        public int instanceId;
        public Dictionary<string, ComponentSerialValue> fields;
        public Dictionary<string, ComponentSerialValue> properties;
        //public List<ComponentMethod> methods;
    }

    public class ComponentMethod
    {
        public string type;
        public string name;
        public List<string> args;
    }
    public class ComponentSerialValue
    {
        public string type;
        public object value;

        static List<Type> prims = new List<Type>{
        typeof(bool),
        typeof(int),
        typeof(float),
        typeof(double),
        typeof(string)
    };

        static Dictionary<Type, Type> specials = new Dictionary<Type, Type> {
        { typeof(Vector2), typeof(SerialVector2) },
        { typeof(Vector3), typeof(SerialVector3) },
        { typeof(Vector4), typeof(SerialVector4) },
        { typeof(Quaternion), typeof(SerialQuaternion) }
    };

        public ComponentSerialValue(PropertyInfo prop, Component component)
        {
            type = prop.PropertyType.ToString();
            try
            {
                object val = prop.GetValue(component, null);
                value = toVal(prop.PropertyType, val);
            }
            catch (Exception e)
            {
                value = null;
            }
        }

        public ComponentSerialValue(FieldInfo prop, Component component)
        {
            type = prop.FieldType.ToString();
            try
            {
                object val = prop.GetValue(component);
                value = toVal(prop.FieldType, val);
            }
            catch (Exception e)
            {
                value = null;
            }

        }

        private static object toVal(Type type, object val)
        {
            if (val == null)
            {
                return null;
            }
            else if (prims.Contains(type))
            {
                return val;
            }
            else if (type == typeof(Vector3))
            {
                object serialVal = specials[type].GetConstructors()[0].Invoke(new object[] { val });
                return serialVal;
            } else if (type == typeof(Transform))
            {
                return GameObjectSerial.GetPath((Transform)val);
            }
            else if (type == typeof(GameObject))
            {
                return GameObjectSerial.GetPath(((GameObject)val).transform);
            }
            else
            {
                return val.ToString();
            }
        }
    }

    public class SerialVector2
    {
        public float x;
        public float y;

        public SerialVector2(Vector2 vec)
        {
            x = vec.x;
            y = vec.y;
        }
    }

    public class SerialVector3
    {
        public float x;
        public float y;
        public float z;

        public SerialVector3(Vector3 vec)
        {
            x = vec.x;
            y = vec.y;
            z = vec.z;
        }
    }

    public class SerialVector4
    {
        public float w;
        public float x;
        public float y;
        public float z;

        public SerialVector4(Vector4 vec)
        {
            w = vec.w;
            x = vec.x;
            y = vec.y;
            z = vec.z;
        }
    }

    public class SerialQuaternion
    {
        public float w;
        public float x;
        public float y;
        public float z;

        public SerialQuaternion(Quaternion vec)
        {
            w = vec.w;
            x = vec.x;
            y = vec.y;
            z = vec.z;
        }
    }
}
