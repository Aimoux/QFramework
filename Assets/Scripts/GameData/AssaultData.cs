using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{

    [System.Serializable]
    public class AssaultData
    {
        public int ID { get; set; }
        public int Type { get; set; }//特技\武器招数??
        public float MaxRange { get; set; }
        public float MinRange { get; set; }
        public float MaxAngle { get; set; }
        public int TargetSide { get; set; }//自己0,友军1,敌人-1
        public Common.TargetType TargetType { get; set; }//可以使用枚举??
        public int CastType { get; set; }
        public int Weight { get; set; }//权重概率
        public string Actions { get; set; }
        public int ManaCost { get; set; }
        public float CommonCoolDown { get; set; }//??

    }
}