using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    [System.Serializable]
    public class AnimationData 
    {
        public int ID { get; set; }
        public int State { get; set; }
        public string Name { get; set; }
        public int AnimationType { get; set; }
        public int TargetType { get; set; }
        public float AttackRange { get; set; }
        public float AttackDamageRatio { get; set; }
        public float AttackImpactRatio { get; set; }
        public float DefenseImpactRatio { get; set; }

    }


}

