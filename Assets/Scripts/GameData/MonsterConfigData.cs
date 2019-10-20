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
        public Dictionary <int, int> Weapons { get; set; }//error in convert??
        public int MainWeapon {get;set;}
        public int MainWeaponLevel {get; set;}
        public float RatioDPS { get; set; }
        public float RatioHP { get; set; }
        public float RatioMP { get; set; }//??
        public float Scale { get; set; }
        public int BossType { get; set; }//??
        public int RewardMoney { get; set; }
        public int RewardEXP { get; set; }//??
        public int RewardItem1 { get; set; }//??掉落概率??
        public int RewardItem2 { get; set; }//??怪物图鉴系统中查看掉落??
        public float DropChance1 { get; set; }
        public float DropChance2 { get; set; }

        public List<int> Items { get; set; }//how to best dec??
        public List<float> Chances { get; set; }
        public List<int> Equips { get; set; }

        public List<int> WPLvs { get; set; }

    }

}
