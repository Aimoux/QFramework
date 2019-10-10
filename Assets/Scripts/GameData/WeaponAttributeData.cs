using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameData
{
    [System.Serializable]
    public class WeaponAttributeData 
    {
        //韧性相关不变??兴奋值不变??
        public int Level { get; set; }
        public int DamageBlunt { get; set; }//升级后伤害变化
        public int DamageSlash { get; set; }
        public int DamagePierce { get; set; }
        public int DamageMagic { get; set; }
        public int DamageFire { get; set; }
        public int DamageIce { get; set; }
        public int DamageElectric { get; set; }

        //SLASH = 0,
        //PIERCE =1,
        //BLUNT =1,//??钝器
        //MAGIC =2,
        //FIRE =3,
        //ICE =4,
        //ELECTRIC =5,



    }



}

