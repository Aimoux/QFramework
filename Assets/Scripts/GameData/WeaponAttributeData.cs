using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameData
{
    [System.Serializable]
    public class WeaponAttributeData 
    {
        //������ز���??�˷�ֵ����??
        public int Level { get; set; }
        public int DamageBlunt { get; set; }//�������˺��仯
        public int DamageSlash { get; set; }
        public int DamagePierce { get; set; }
        public int DamageMagic { get; set; }
        public int DamageFire { get; set; }
        public int DamageIce { get; set; }
        public int DamageElectric { get; set; }

        //SLASH = 0,
        //PIERCE =1,
        //BLUNT =1,//??����
        //MAGIC =2,
        //FIRE =3,
        //ICE =4,
        //ELECTRIC =5,



    }



}

