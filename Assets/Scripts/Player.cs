using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QF;
using QFramework;

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

public class SaveData
{
    public int Level { get; set; }
    public int CfgHP { get; set;}//加点后的HP
    public int CfgStamina {get;set;}
    public int CfgStimulation { get; set; }
    public int CfgStrength { get; set; }
    public int CfgDexterity { get; set; }
    public int CfgMental { get; set; }


}
