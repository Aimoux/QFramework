using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QF;
using QFramework;
using Common;
using GameData;

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


}

[Serializable]
public class SaveData//存档数据, 可否嵌套类??
{
    public HeroData data;


}

[Serializable]
public class HeroData
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