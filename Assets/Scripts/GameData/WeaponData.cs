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
        public float Weight { get; set; }
        public float Length { get; set; }
        public int AddStrLv { get; set; }//���������ȼ� SABCDE
        public int AddDexLv { get; set; }//���������ȼ�
        public int AddMntLv { get; set; }//��־�����ȼ�
        public int MPAtkRecovery { get; set; }//������������������




        public int SteadyEx { get; set; }//��������??
        public int SteadyDmg { get; set; }//��������
        public int StaminaCost { get; set; }//�����������ģ��ػ�����


        }


}

