using System;
using System.Collections.Generic;
using System.Reflection;

namespace CarStore.Infrastructure
{
    public class ValueType<T>
        where T : class
    {
        static List<PropertyInfo> properties;
        static Type type;

        public ValueType()
        {
            properties = new List<PropertyInfo>();
            type = typeof(T);
            var prop = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            //Sort(prop);
            foreach (var p in prop)
                properties.Add(p);
        }

        /*public override string ToString()
        {
            var s = "";
            for (int i = 0; i < properties.Count; i++)
            {
                if (i < properties.Count - 1)
                    s += properties[i].Name + ": " + type.GetProperty(properties[i].Name).GetValue(this) + "; ";
                else
                    s += properties[i].Name + ": " + type.GetProperty(properties[i].Name).GetValue(this);
            }
            return string.Join("", type.Name, "(", s, ")");
        }*/

        public override string ToString()
        {
            var s = "";
            for (int i = 0; i < properties.Count; i++)
            {
                if (i < properties.Count - 1)
                {
                    if (type.GetProperty(properties[i].Name).PropertyType.Name == "Int32")
                        s += type.GetProperty(properties[i].Name).GetValue(this) + ", ";
                    else
                        s += "'" + type.GetProperty(properties[i].Name).GetValue(this) + "'" + ", ";
                }
                else
                {
                    if (type.GetProperty(properties[i].Name).PropertyType.Name == "Int32")
                        s += type.GetProperty(properties[i].Name).GetValue(this);
                    else
                        s += "'" + type.GetProperty(properties[i].Name).GetValue(this) + "'";
                }
            }
            return s;
        }

        public static List<PropertyInfo> GetProperties()
        {
            return properties;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as T);
        }

        public bool Equals(T obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            else
            {
                foreach (var p in properties)
                {
                    var thisValue = type.GetProperty(p.Name).GetValue(this);
                    var thatValue = type.GetProperty(p.Name).GetValue(obj);
                    if (thisValue == null && thatValue == null) continue;
                    if (thisValue == null && thatValue != null || !thisValue.Equals(thatValue))
                        return false;
                }
                return true;
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                for (int i = 0; i < properties.Count; i++)
                {
                    //hash += (int)(type.GetProperty(properties[i].Name).GetValue(this).GetHashCode() * Math.Pow(k, i));
                    var value = type.GetProperty(properties[i].Name).GetValue(this);
                    hash = hash * 23 + (value == null ? 0 : value.GetHashCode());
                }

                return hash;
            }
        }

        public static void Sort(PropertyInfo[] array)
        {
            for (int i = array.Length - 1; i > 0; i--)
                for (int j = 1; j <= i; j++)
                {
                    var element1 = array[j - 1];
                    var element2 = array[j];
                    if (element1.Name.CompareTo(element2.Name) > 0)
                    {
                        array[j - 1] = element2;
                        array[j] = element1;
                    }
                }
        }
    }
}
