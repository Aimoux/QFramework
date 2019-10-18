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
        public int Stamina { get; set; }//����ʾ
        public int MP { get; set; }//����˷�ֵMP��Ѫ������ʾ��
        public int Strength { get; set; }//���������˺�����
        public int Dexterity { get; set; }//���������˺��������ҩ�ٶ�??
        public int Mental { get; set; }//�ؼ������ؼ��ֿ��ж���Ԫ���˺�����??
        public int MPAtkRecovery { get; set; }//��������ʱ�ظ�MP(aoe����Ӧֻ��һ��,���趨����?)
        public int MPDmgRecovery { get; set; }//������ʱ�ظ�MP
        public int Armor { get; set; }
        public int FireResistance { get; set; }
        public int IceResistance { get; set; }
        public int ElectricResistance { get; set; }
        public int MagicResistance { get; set; }

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
            //clone.Values = (float[])this.Values.Clone();//�̳���GameDataBase
            return clone;
        }






    }

}