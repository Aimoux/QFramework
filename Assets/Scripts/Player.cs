using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QF;
using QFramework;
using Common;
using GameData;
using Newtonsoft.Json;

//玩家数据单例，非mono
//player 与role的分工
//player 全局数据 vs role 战斗实时数据
//存档??
public class Player :Singleton<Player>
{
    public int Level { get; set; }
    public int CfgHP { get; set;}//加点后的HP
    public int CfgStamina {get;set;}
    public int CfgStimulation { get; set; }
    public int CfgStrength { get; set; }
    public int CfgDexterity { get; set; }
    public int CfgMental { get; set; }

    public SaveData save;//deserialize from userdata??


    public void Save()
    {
        SaveManager.Instance.Save(save, "");


    }

    public void Load()
    {
        SaveManager.Instance.Load<SaveData>("", ref save);
    }



  
}

[Serializable]
public class SaveData//存档数据, 可否嵌套类??
{
    public UserData data;//自己任务的存档
    public List<Hero> allies;//已解锁同伴存档


}

[Serializable]
public class UserData
{
    public int Level { get; set; }
    public int CfgHP { get; set; }//加点后的HP
    public int CfgStamina { get; set; }
    public int CfgStimulation { get; set; }
    public int CfgStrength { get; set; }
    public int CfgDexterity { get; set; }
    public int CfgMental { get; set; }
    public int Exp { get; set; }//经验值
    public int Virtue { get; set; }//善恶值

    List<Item> Items { get; set; }//物品
    public int Helmet { get; set; }//当前护甲
    public int Chest { get; set; }
    public int Glove { get; set; }
    public int Boot { get; set; }
    public int Amulet { get; set; }
    public int Circle { get; set; }

}

public class SaveManager: Singleton<SaveManager>
{
    public void Load<T>(string filePath, ref T outVal)
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        filePath = "Assets/BuildOnlyAssets/DeafultSave";
        byte[] btdata = System.IO.File.ReadAllBytes(filePath);
        string strdata = System.Text.Encoding.UTF8.GetString(btdata);
        System.DateTime st = System.DateTime.Now;
        Debug.Log("read file " + filePath + " io cost: " + System.DateTime.Now.Subtract(st).TotalMilliseconds);
        st = System.DateTime.Now;
        try
        {
            outVal = JsonConvert.DeserializeObject<T>(strdata);
        }
        catch (Exception ex)
        {
            Debug.LogError("Json deserialize error " + filePath + "\r\n" + ex.ToString());
        }
        Debug.Log("Json deserialize " + filePath + " cost: " + System.DateTime.Now.Subtract(st).TotalMilliseconds);
#endif 
    }

    public void Save(object data, string filePath)//const string path
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        System.DateTime st = System.DateTime.Now;       
        string strdata = JsonConvert.SerializeObject(data);
        byte[] btdata = System.Text.Encoding.UTF8.GetBytes(strdata);
        System.IO.File.WriteAllBytes(filePath, btdata);
        Debug.Log("save cost: " + filePath + " cost: " + System.DateTime.Now.Subtract(st).TotalMilliseconds);
#endif 
    }




}
