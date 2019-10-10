using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    [System.Serializable]
    public class WeaponData
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Category { get; set; }
        public string Icon { get; set; }
        public string Model { get; set; }
        public string Animator { get; set; }
        public int RequireLevel { get; set; }
        public int RequireStr { get; set; }
        public int RequireDex { get; set; }
        public float BaseDamage { get; set; }//����˺����ػ��˺�����ֵ�����봫��??
        public float StrBonus { get; set; }
        public float DexBonus { get; set; }
        public float Weight { get; set; }
        public float Length { get; set; }
        public int DamageType { get; set; }//�ӿ������̡�ħ������������Ѫ

    }


}

