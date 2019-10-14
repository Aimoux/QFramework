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
        public int AddStrLv { get; set; }//力量修正等级 SABCDE
        public int AddDexLv { get; set; }//敏捷修正等级
        public int AddMntLv { get; set; }//意志修正等级
        public int MPAtkRecovery { get; set; }//不随武器升级而增长




        public int SteadyEx { get; set; }//出手韧性??
        public int SteadyDmg { get; set; }//韧性削减
        public int StaminaCost { get; set; }//基础体力消耗，重击倍数


        }


}

