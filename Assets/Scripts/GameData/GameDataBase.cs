using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GameData
{
    public class Property
    {
        public string Name;
        private MethodInfo Getter;
        private MethodInfo Setter;

        public Property(PropertyInfo prop)
        {
            this.Name = prop.Name;
            this.Getter = prop.GetGetMethod();
            this.Setter = prop.GetSetMethod();
        }

        public object GetValue(object owner)
        {
            return this.Getter.Invoke(owner, null);
        }

        public void SetValue(object owner, object value)
        {
            this.Setter.Invoke(owner, new object[] { value });
        }
    }
    [Serializable]
    public class DataBase<T>
    {

        private static Dictionary<string, Property> propertys = null;
        public Dictionary<string, Property> Propertys
        {
            get
            {
                if (propertys == null)
                {
                    propertys = new Dictionary<string, Property>();
                    foreach (PropertyInfo prop in this.GetType().GetProperties().Where(p => !p.PropertyType.Name.Equals("Object") && !p.Name.Equals("Item") && !p.Name.Equals("Propertys")))
                    {
                        propertys[prop.Name] = new Property(prop);
                    }
                }
                return propertys;
            }
        }

        public object this[string propertyName]
        {
            get
            {
                if (!this.Propertys.ContainsKey(propertyName))
                    return null;
                return this.Propertys[propertyName].GetValue(this);
            }
            set
            {
                if (!this.Propertys.ContainsKey(propertyName))
                    return;
                this.Propertys[propertyName].SetValue(this, value);
            }
        }

        public bool HasProperty(string propertyName)
        {
            return this.Propertys.ContainsKey(propertyName);
        }

    }

    public class GameDataBase<T>
    {
        private static Dictionary<string, Property> propertys = null;
        private static Dictionary<int, Property> epropertys = null;

        protected void InitProperties()
        {
            if (propertys == null)
            {
                propertys = new Dictionary<string, Property>();
                foreach (PropertyInfo prop in this.GetType().GetProperties().Where(p => !p.PropertyType.Name.Equals("Object") && !p.Name.Equals("Item") && !p.Name.Equals("Propertys") && !p.Name.Equals("EPropertys")))
                {
                    propertys[prop.Name] = new Property(prop);
                }
            }
            if (epropertys == null)
            {
                epropertys = new Dictionary<int, Property>();
                foreach (PropertyInfo prop in this.GetType().GetProperties().Where(p => !p.PropertyType.Name.Equals("Object") && !p.Name.Equals("Item") && !p.Name.Equals("Propertys") && !p.Name.Equals("EPropertys")))
                {
                    if (this is RoleAttributeData)
                    {
                        RoleAttribute key = (RoleAttribute)Enum.Parse(typeof(RoleAttribute), prop.Name);
                        epropertys[(int)key] = new Property(prop);
                    }
                    //if (this is AbilityEffects)
                    //{
                    //    ControlEffect key = (ControlEffect)Enum.Parse(typeof(ControlEffect), prop.Name);
                    //    epropertys[(int)key] = new Property(prop);
                    //}
                }
            }
        }
        public Dictionary<string, Property> Propertys
        {
            get
            {
                if (propertys == null)
                {
                    InitProperties();
                }
                return propertys;
            }
        }
        public Dictionary<int, Property> EPropertys
        {
            get
            {
                if (epropertys == null)
                {
                    InitProperties();
                }
                return epropertys;
            }
        }

        public T this[string propertyName]//索引器
        {
            get
            {
                if (!Propertys.ContainsKey(propertyName))
                    return default(T);
                return (T)Propertys[propertyName].GetValue(this);
            }
            set
            {
                if (!Propertys.ContainsKey(propertyName))
                    return;
                Propertys[propertyName].SetValue(this, value);
            }
        }

        public T this[int enumValue]
        {
            get
            {
                if (!EPropertys.ContainsKey(enumValue))
                    return default(T);
                return (T)EPropertys[enumValue].GetValue(this);
            }
            set
            {
                if (!EPropertys.ContainsKey(enumValue))
                    return;
                EPropertys[enumValue].SetValue(this, value);
            }
        }

        public void Reset()
        {
            foreach (Property prop in Propertys.Values)
            {
                prop.SetValue(this, default(T));
            }
        }

        public string GetStringValue(string name)
        {
            if (!this.Propertys.ContainsKey(name))
                return string.Empty;
            return (string)Propertys[name].GetValue(this);
        }

        public T1 GetEnumValue<T1>(string name, T1 defaultValue)
        {
            if (!this.Propertys.ContainsKey(name))
                return defaultValue;

            string value = (string)Propertys[name].GetValue(this);
            if (string.IsNullOrEmpty(value))
                return defaultValue;
            try
            {
                T1 t = (T1)Enum.Parse(typeof(T1), value, true);
                return t;
            }
            catch (Exception ex)
            {
                return defaultValue;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Property prop in Propertys.Values)
            {
                sb.AppendLine(prop.Name + ":" + prop.GetValue(this));
            }
            return sb.ToString();
        }
    }

    [Serializable]
    public class GameDataArray<T>
    {
        public T[] Values;

        public GameDataArray(int capacity)
        {
            Values = new T[capacity];
        }

        public T this[int enumValue]
        {
            get
            {
                if (enumValue >= Values.Length)
                    return default(T);
                return Values[enumValue];
            }
            set
            {
                Values[enumValue] = value;
            }
        }

        public void Reset()
        {
            for (int i = 0; i < this.Values.Length; i++)
            {
                this.Values[i] = default(T);
            }
        }
    }

    [Serializable]
    public class Cloneable
    {
        /// <summary>
        /// 属性赋值器
        /// </summary>
        /// <typeparam name="TD">目的类型</typeparam>
        /// <typeparam name="TS">源类型</typeparam>
        /// <param name="d">目的实例</param>
        /// <param name="s">源实例</param>
        /// <returns>目的类型实例</returns>
        TD PropertySetter<TD, TS>(TD d, TS s)
        {
            var tsType = s.GetType();
            var tdType = typeof(TD);
            var tsProperties = tsType.GetProperties();
            var tdProperties = tdType.GetProperties();
            foreach (var tsp in tsProperties)
            {
                foreach (var tdp in tdProperties)
                {
                    if (!tdp.Name.Equals(tsp.Name) || tdp.PropertyType != tsp.PropertyType)
                        continue;
                    tdp.SetValue(d, tsp.GetValue(s, null), null);
                    break;
                }
            }
            return d;
        }
        /// <summary>
        /// 属性赋值器
        /// </summary>
        /// <typeparam name="TD">目的类型</typeparam>
        /// <typeparam name="TS">源类型</typeparam>
        /// <param name="d">目的实例</param>
        /// <param name="s">源实例</param>
        /// <returns>目的类型实例</returns>
        TD UnrealPropertySetter<TD, TS>(TD d, TS s)
        {
            var tsType = s.GetType();
            var tdType = typeof(TD);
            var tsProperties = tsType.GetProperties();
            var tdProperties = tdType.GetProperties();
            foreach (var tsp in tsProperties)
            {
                foreach (var tdp in tdProperties)
                {
                    if (!tdp.Name.Equals(tsp.Name) || tdp.PropertyType != tsp.PropertyType || (tdp.Name == "Item"))
                        continue;
                    if (!tdp.CanWrite)
                        continue;
                    tdp.SetValue(d, tsp.GetValue(s, null), null);
                    break;
                }
            }
            return d;
        }

        /// <summary>
        /// 克隆
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="t">实例</param>
        /// <returns>克隆后的实例</returns>
        T Clone<T>(T t) where T : new()
        {
            var tType = typeof(T);
            var clonedT = new T();//Activator.CreateInstance<T>();
            foreach (var tProperty in tType.GetProperties())
            {
                tProperty.SetValue(clonedT, tProperty.GetValue(t, null), null);
            }
            return clonedT;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public T DeepClone<T>(T t)
        {
            var stream = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, t);
            stream.Position = 0;
            return (T)formatter.Deserialize(stream);
        }


    }




}
