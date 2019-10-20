using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    [Serializable]
    public class LevelMonsterData
    {
        public int ID { get; set; }
        public string Name { get; set; }//scene path
        public List<int> Monsters { get; set; }//索引
        public int Round { get; set; }//??

    }

}
