using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    [System.Serializable]
    public class RoleData : Cloneable
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Gender { get; set; }
        public string Model { get; set; }
        public string Behavior { get; set; }
        public string Portrait { get; set; }
        public string Picture { get; set; }
        public int Personality { get; set; }
        public int MaxLevel { get; set; }
        public int MinLevel { get; set; }
        public float Radius { get; set; }

        public RoleData Clone()
        {
            RoleData clone = this.MemberwiseClone() as RoleData;
            return clone;
        }
    }


}

