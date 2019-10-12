using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameData
{
    public enum RoleAttribute : int
    {
        None = 0,
        /// <summary>
        /// 力量
        /// </summary>
        Strength,
        /// <summary>
        /// 智力
        /// </summary>
        Intelligence,
        /// <summary>
        /// 敏捷
        /// </summary>
        Agility,

        /// <summary>
        /// HP（生命值）
        /// </summary>
        HP,

        /// <summary>
        /// MP（魔法值）
        /// </summary>
        MP,
        /// <summary>
        /// 物理攻击力
        /// </summary>
        AttackDamage,
        /// <summary>
        /// 魔法强度（攻击力）
        /// </summary>
        AbilityPower,
        /// <summary>
        /// 物理防御力
        /// </summary>
        Armor,
        /// <summary>
        /// 魔法抗性（防御力）
        /// </summary>
        MagicResistance,
        /// <summary>
        /// 物理暴击
        /// </summary>
        Critical,
        /// <summary>
        /// 魔法暴击
        /// </summary>
        MagicCritical,
        /// <summary>
        /// 生命恢复速度（持续恢复）
        /// </summary>
        HPRegen,
        /// <summary>
        /// 魔法恢复速度（持续恢复）
        /// </summary>
        MPRegen,
        /// <summary>
        /// 持续减血速度
        /// </summary>
        HPDecrease,

        /// <summary>
        /// 生命回复（攻击时）
        /// </summary>
        HPAtkRecovery,
        /// <summary>
        /// 魔法回复（攻击时）
        /// </summary>
        MPAtkRecovery,
        /// <summary>
        /// 魔法回复（受伤时）
        /// </summary>
        MPDmgRecovery,
        /// <summary>
        /// 物理命中
        /// </summary>
        HitRate,
        /// <summary>
        /// 物理闪避
        /// </summary>
        Dodge,
        /// <summary>
        /// 物理防御穿透
        /// </summary>
        ArmorPenetration,
        /// <summary>
        /// 无视魔法防御
        /// </summary>
        MagicResistIgnore,
        /// <summary>
        /// 降低能量消耗
        /// </summary>
        ManaCostReduction,
        /// <summary>
        /// 治疗能力
        /// </summary>
        Heal,
        /// <summary>
        /// 吸取能力（攻击时吸取）
        /// </summary>
        LifeSteal,
        /// <summary>
        /// 生命回复（战斗结束）
        /// </summary>
        HPSupply,
        /// <summary>
        /// 魔法回复（战斗结束）
        /// </summary>
        MPSupply,
        /// <summary>
        /// 攻击速度
        /// </summary>
        AttackSpeed,
        /// <summary>
        /// 移动速度
        /// </summary>
        MoveSpeed,
        PowerFactor,
        /// <summary>
        /// 物理免伤
        /// </summary>
        PhysicsImmunization,
        /// <summary>
        /// 魔法免伤
        /// </summary>
        MagicImmunization,
        /// <summary>
        /// 技能等级提升
        /// </summary>
        SkillLevel,
        /// <summary>
        /// 沉默抵抗率
        /// </summary>
        SilenceResistRate,
        /// <summary>
        /// 男性减伤
        /// </summary>
        MaleDmgReduction,
        /// <summary>
        /// 坚韧（提高BUFF抗性）
        /// </summary>
        Toughness,
        /// <summary>
        /// 暴击系数
        /// </summary>
        CriticalFactor,
        SHealHp,
        STMP,
        /// <summary>
        /// 魔法箭矢，六级学院书籍《太阳井的耻辱》对风行增 幅，使风法的普通射击造成(5%)  魔法强度的额外魔法伤害
        /// </summary>
        MagicArrows,
        /// <summary>
        /// 激怒，六级学院书籍《野兽之王》对拍拍熊增幅，使拍拍熊被暴击进入狂暴状态，额外造成当前生命值x%的物理伤害
        /// </summary>
        Wild,
        /// <summary>
        /// 剧毒之球，六级学院书籍《驯龙高手》对亚龙增幅，使亚龙的敌人生命每减少上限x%,亚龙造成的伤害提升Y%
        /// </summary>
        PoisonousBall,
        /// <summary>
        /// 重复释放魔法技能几率
        /// </summary>
        RepeatAbilityPowerTalentRate,
        BasicNum,
        ExtraBasicNum,
        BaseRatio,
        CastNum,
        ShieldValue,
        Speed,
        Time,
        Param1,
        Param2,
        Param3,
        Param4,
        Param5,
        Param6,
        /*STMP,
        SHealHp,*/
        //符文
        CriticalDamageAddonRate,         //提升物理暴击伤害百分比
        MagicCriticalDamageAddonRate,    //提升法术暴击伤害百分比
        AOEDamageAddonRate,              //提升AOE伤害百分比
        HolyDamageAddonRate,             //提升神圣伤害百分比
        ContinuousDamageAddonRate,       //提升持续伤害百分比
        MagicCriticalDamageReduceRate,   //降低魔法暴击伤害百分比
        CriticalDamageReduceRate,        //降低物理暴击伤害百分比
        AOEDamageReduceRate,             //降低AOE伤害百分比
        HolyDamageReduceRate,            //降低神圣伤害百分比
        ContinuousDamageReduceRate,      //降低持续伤害百分比
        AttackOffsetAddon,               //提升物理攻击抵消
        MagicAttackOffsetAddon,          //提升魔法攻击抵消
        DefendHitRecoverAddon,           //提升抵抗硬直
        SummonedAttrAddon,               //提升召唤物属性
        MagicBloodSuckingAddon,          //提升法术吸血
        DeathReduceEnemyEnergyRecover,   //死亡减少敌方回能
        //符文 End
        MAX,
        PLUSMAX = MAX + 15,
    }
}
