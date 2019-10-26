using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    [System.Serializable]
    public class WeaponAddData : MonoBehaviour
    {
        public int StrengthAdd { get; set; }//武器伤害的力量修正，代码控制仅对物理类型伤害有效
        public int DexterityAdd { get; set; }//敏捷修正
        public int MentalAdd { get; set; }//代码控制对非物理性伤害生效

    }
}
