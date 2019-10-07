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
        public int Stimulation { get; set; }//����˷�ֵ��Ѫ������ʾ��
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Mental { get; set; }//������
    }

}
