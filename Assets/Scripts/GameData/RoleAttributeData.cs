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
        public int Stimulation { get; set; }//最大兴奋值，血条下显示？
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Mental { get; set; }//精神抗力
    }

}
