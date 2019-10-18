
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using GameData;
using UnityEngine;
using QF;
using Newtonsoft.Json;

class DataManager : Singleton<DataManager>
{
    private Dictionary<int, WeaponData> _Weapons;

    public Dictionary<int, WeaponData> Weapons
    {
        get
        {
            if (this._Weapons == null)
                Load("Data/Weapons.json", ref _Weapons);
            return _Weapons;
        }
    }

    private Dictionary<int, RoleData> _Roles;

    public Dictionary<int, RoleData> Roles
    {
        get
        {
            if (this._Roles == null)
                Load("Data/Roles.json", ref this._Roles);
            return _Roles;
        }

    }

    private Dictionary<int, Dictionary<float, RoleAttributeData>> _RoleAttributes;
    public Dictionary<int, Dictionary<float, RoleAttributeData>> RoleAttributes
    {
        get
        {
            if (this._RoleAttributes == null)
            {
                Load("Data/RoleAttributes.json", ref this._RoleAttributes);
            }
            return _RoleAttributes;
        }
    }

    private Dictionary<int, Dictionary<float, WeaponAttributeData>> _WeaponAttribues;
    public Dictionary<int, Dictionary<float, WeaponAttributeData>> WeaponAttributes
    {
        get
        {
            if(this._WeaponAttribues == null)
            {
                Load("Data/WeaponAttributes.json", ref this._WeaponAttribues);
            }
            return _WeaponAttribues;
        }

    }

    private Dictionary<int, Dictionary<float, WeaponAddData>> _WeaponAdds;
    public Dictionary<int, Dictionary<float, WeaponAddData>> WeaponAdds
    {
        get
        {
            if(this._WeaponAdds == null)
            {
                Load("Data/WeaponAdds.json", ref this._WeaponAdds);
            }
            return _WeaponAdds;
        }

    }

    private Dictionary<int, LevelData> _Levels;
    public Dictionary<int, LevelData> Levels
    {
        get
        {
            if(_Levels == null)
            {
                Load("Data/Levels.json", ref _Levels);
            }
            return _Levels;
        }
    }

    private Dictionary<int, MonsterConfigData> _MonsterConfigs;
    public Dictionary<int, MonsterConfigData> MonsterConfigs
    {
        get
        {
            if(_MonsterConfigs == null)
            {
                Load("Data/MonsterConfigs.json", ref _MonsterConfigs);               
            }
            return _MonsterConfigs;
        }
    }

    private GlobalConfigData _GlobalConfig;
    public GlobalConfigData GlobalConfig
    {
        get
        {
            if(_GlobalConfig == null)
            {
                Load("Data/GlobalConfig.json", ref _GlobalConfig);
            }
            return _GlobalConfig;
        }
    }


    protected DataManager()
    {

    }

    //正式版数据位于streaming asset??
    private void Load<T>(string name, ref T outVal)
    {
        #if UNITY_EDITOR || UNITY_STANDALONE
        string filepath = name;
        filepath = "Assets/BuildOnlyAssets/" + filepath;
        Debug.Log("Json deserialize " + name);
        System.DateTime st = System.DateTime.Now;
        string strdata = System.IO.File.ReadAllText(filepath, Encoding.UTF8);
        //byte[] btdata = System.Text.Encoding.UTF8.GetBytes(strdata);
        //byte[] data = System.IO.File.ReadAllBytes(filepath);
        Debug.Log("read file " + name + " io cost: " + System.DateTime.Now.Subtract(st).TotalMilliseconds);
        st = System.DateTime.Now;
        try
        {
            outVal = JsonConvert.DeserializeObject<T>(strdata);
        }
        catch (Exception ex)
        {
            Debug.LogError("Json deserialize error " + name + "\r\n" + ex.ToString());
        }
        Debug.Log("Json deserialize " + name + " cost: " + System.DateTime.Now.Subtract(st).TotalMilliseconds);

//        #else
//        {
//            System.DateTime st = System.DateTime.Now;
//            byte[] data = ResourcesLoader.LoadFileData(name);//core??
//            Debug.Log("read file " + name + " io cost: " + System.DateTime.Now.Subtract(st).TotalMilliseconds);
//            st = System.DateTime.Now;
//            outVal = fastBinaryJSON.BJSON.ToObject<T>(data);//core??
//            Debug.Log("Json deserialize " + name + " cost: " + System.DateTime.Now.Subtract(st).TotalMilliseconds);
//        }
        #endif 
    }

    //    private void Load<T>(string name, ref T outVal)
    //    {
    //#if SERVER
    //		string path = System.Environment.GetEnvironmentVariable("CLIENT_DATA_PATH"); //System.AppDomain.CurrentDomain.BaseDirectory;
    //        if (string.IsNullOrEmpty(path)) path = ".";
    //        string json = System.IO.File.ReadAllText(path + "/" + name + ".txt");
    //        outVal = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
    //#else
    //
    //#if UNITY_EDITOR || UNITY_STANDALONE
    //        if (ResourcesLoader.LoadOriginalAssets)
    //        {
    //            string file = name + ".txt";
    //            file = "Assets/BuildOnlyAssets/" + file;
    //            Debug.Log("Json deserialize " + name);
    //            System.DateTime st = System.DateTime.Now;
    //            string data = System.IO.File.ReadAllText(file, Encoding.UTF8);
    //            //string data = ResourcesLoader.LoadFileAllText(name);
    //            //Debug.Log("read file " +name+" io cost: " + System.DateTime.Now.Subtract(st).TotalMilliseconds);
    //            //st = System.DateTime.Now;
    //            try
    //            {
    //                outVal = fastJSON.JSON.ToObject<T>(data);
    //            }
    //            catch (Exception ex)
    //            {
    //                Debug.LogError("Json deserialize error " + name + "\r\n" + ex.ToString());
    //            }
    //            Debug.Log("Json deserialize " + name + " cost: " + System.DateTime.Now.Subtract(st).TotalMilliseconds);
    //            return;
    //        }
    //#endif
    //        {
    //            System.DateTime st = System.DateTime.Now;
    //            byte[] data = ResourcesLoader.LoadFileData(name);//core??
    //            Debug.Log("read file " + name + " io cost: " + System.DateTime.Now.Subtract(st).TotalMilliseconds);
    //            st = System.DateTime.Now;
    //            outVal = fastBinaryJSON.BJSON.ToObject<T>(data);//core??
    //            Debug.Log("Json deserialize " + name + " cost: " + System.DateTime.Now.Subtract(st).TotalMilliseconds);
    //        }
    //#endif
    //    }
}

//class NumericsData
//{
//    public NumericData this[int index]
//    {
//        get
//        {
//            if (DataManager.Instance.DNumerics.ContainsKey(index))
//            {
//                return DataManager.Instance.DNumerics[index];
//            }
//            return null;
//        }
//        set { if (value == null) throw new ArgumentNullException("value"); }
//    }
//}