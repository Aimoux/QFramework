using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    [System.Serializable]
    public class RoleAttributeData
    {
        public int Level { get; set; }
        public int HP { get; set; }
        public int Stamina { get; set; }//不显示
        public int MP { get; set; }//最大兴奋值MP，血条下显示？
        public int Strength { get; set; }//武器需求，伤害修正
        public int Dexterity { get; set; }//武器需求，伤害修正，嗑药速度??
        public int Mental { get; set; }//特技需求，特技抵抗判定，元素伤害修正??
        public int MPAtkRecovery { get; set; }//攻击别人时回复MP(aoe攻击应只算一次,并设定上限?)
        public int MPDmgRecovery { get; set; }//被攻击时回复MP
        public int Armor { get; set; }
        public int FireResistance { get; set; }
        public int IceResistance { get; set; }
        public int ElectricResistance { get; set; }
        public int MagicResistance { get; set; }
        public int StaminaRegen { get; set; }//耐力回复速度，受体力影响??

        //MAGIC = 3,
        //FIRE = 4,
        //ICE = 5,
        //ELECTRIC = 6,

        //public static string GetExtraNameField(string name)
        //{
        //    return name + PF_S;
        //}

        //public static int GetPlusValueField(int name)
        //{
        //    return (int)RoleAttribute.MAX - 1 + name;
        //}

        //public static int GetPredictNameField(int name)
        //{
        //    return (int)RoleAttribute.PLUSMAX - 1 + (int)name;
        //}

        //public static int GetPSNameField(int name, int stars)
        //{
        //    return (int)RoleAttribute.MAX + ((int)name - 1) * 5 + stars - 1;
        //}

        public RoleAttributeData Clone()
        {
            RoleAttributeData clone = this.MemberwiseClone() as RoleAttributeData;
            //clone.Values = (float[])this.Values.Clone();//继承自GameDataBase
            return clone;
        }






    }

}
