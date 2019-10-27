using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    [Serializable]
    public class MonsterConfigData
    {
        public int MID { get; set; }//key
        public int ID { get; set; }//role id
        public int Level { get; set; }
        public Dictionary <int, int> Weapons { get; set; }//不同个体可以数目不同,转换时会自动排序,不可有重复的key
        public int MainWeapon {get;set;}
        public int MainWeaponLevel {get; set;}
        public float RatioDPS { get; set; }
        public float RatioHP { get; set; }
        public float RatioMP { get; set; }//??
        public float Scale { get; set; }
        public int BossType { get; set; }//??
        public int RewardMoney { get; set; }
        public int RewardEXP { get; set; }//??
        public Dictionary<int, float> Drops { get; set; }//掉落物品及概率
        public List<int> Equips { get; set; }//同样用字典,每个部位对应一件装备


    }

}
