using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    [System.Serializable]
    public class WeaponData //不随武器升级而增长
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Category { get; set; }//影响冲击力
        public string Icon { get; set; }
        public string Model { get; set; }
        public string Animator { get; set; }
        public int FrameConfig { get; set; }
        public int RequireLevel { get; set; }
        public int RequireStr { get; set; }
        public int RequireDex { get; set; }
        public float Weight { get; set; }
        //武器的攻击范围是固定的,但是同一武器的不同招数的范围允许不同(原地劈砍 vs 前跃攻击)
        public float Length { get; set; }// vs max range, min range?? 战斗检测用unity 的trigger
        public int AddStrLv { get; set; }//力量修正等级 SABCDE
        public int AddDexLv { get; set; }//敏捷修正等级
        public int AddMntLv { get; set; }//意志修正等级
        public float MPAtkRecovery { get; set; }//命中回魔
        public float ImpactRatio { get; set; }//出手保护韧性因子与招式保护因子共同作用
        public float ImpactDamage { get; set; }//基础韧性削减与招式削韧因子共同作用
        public int StaminaCost { get; set; }//基础体力消耗，重击倍数
        public Dictionary<int, string> Combos { get; set; }//可用组合招式
        public Dictionary<int, int> Actions { get; set; }//atk + assaultID


        }


}

