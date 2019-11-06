using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{

    [System.Serializable]
    public class AssaultData
    {
        public int ID { get; set; }
        public int Type { get; set; }
        public float MaxRange { get; set; }
        public float MinRange { get; set; }
        public float MaxAngle { get; set; }
        public int TargetType { get; set; }
        public int CastType { get; set; }
        public int Weight { get; set; }//默认释放概率??  wi/sum(wij)??
        public List<int> Acts { get; set; }
        public string Actions { get; set; }
        public float CommonCoolDown { get; set; }//??

    }
}