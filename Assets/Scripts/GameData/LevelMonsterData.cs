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
        public int Round { get; set; }
        public string Path { get; set; }//scene path
        public List<int> Monsters { get; set; }//怪物配置id,excel数组元素个数需相等,0表示空
        public List<float> Positions { get; set; }//位置,需要与怪物数量匹配


    }

}
