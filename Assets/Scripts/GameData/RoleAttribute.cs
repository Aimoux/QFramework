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
        /// ����
        /// </summary>
        Strength,
        /// <summary>
        /// ����
        /// </summary>
        Intelligence,
        /// <summary>
        /// ����
        /// </summary>
        Agility,

        /// <summary>
        /// HP������ֵ��
        /// </summary>
        HP,

        /// <summary>
        /// MP��ħ��ֵ��
        /// </summary>
        MP,
        /// <summary>
        /// ��������
        /// </summary>
        AttackDamage,
        /// <summary>
        /// ħ��ǿ�ȣ���������
        /// </summary>
        AbilityPower,
        /// <summary>
        /// ���������
        /// </summary>
        Armor,
        /// <summary>
        /// ħ�����ԣ���������
        /// </summary>
        MagicResistance,
        /// <summary>
        /// ������
        /// </summary>
        Critical,
        /// <summary>
        /// ħ������
        /// </summary>
        MagicCritical,
        /// <summary>
        /// �����ָ��ٶȣ������ָ���
        /// </summary>
        HPRegen,
        /// <summary>
        /// ħ���ָ��ٶȣ������ָ���
        /// </summary>
        MPRegen,
        /// <summary>
        /// ������Ѫ�ٶ�
        /// </summary>
        HPDecrease,

        /// <summary>
        /// �����ظ�������ʱ��
        /// </summary>
        HPAtkRecovery,
        /// <summary>
        /// ħ���ظ�������ʱ��
        /// </summary>
        MPAtkRecovery,
        /// <summary>
        /// ħ���ظ�������ʱ��
        /// </summary>
        MPDmgRecovery,
        /// <summary>
        /// ��������
        /// </summary>
        HitRate,
        /// <summary>
        /// ��������
        /// </summary>
        Dodge,
        /// <summary>
        /// ���������͸
        /// </summary>
        ArmorPenetration,
        /// <summary>
        /// ����ħ������
        /// </summary>
        MagicResistIgnore,
        /// <summary>
        /// ������������
        /// </summary>
        ManaCostReduction,
        /// <summary>
        /// ��������
        /// </summary>
        Heal,
        /// <summary>
        /// ��ȡ����������ʱ��ȡ��
        /// </summary>
        LifeSteal,
        /// <summary>
        /// �����ظ���ս��������
        /// </summary>
        HPSupply,
        /// <summary>
        /// ħ���ظ���ս��������
        /// </summary>
        MPSupply,
        /// <summary>
        /// �����ٶ�
        /// </summary>
        AttackSpeed,
        /// <summary>
        /// �ƶ��ٶ�
        /// </summary>
        MoveSpeed,
        PowerFactor,
        /// <summary>
        /// ��������
        /// </summary>
        PhysicsImmunization,
        /// <summary>
        /// ħ������
        /// </summary>
        MagicImmunization,
        /// <summary>
        /// ���ܵȼ�����
        /// </summary>
        SkillLevel,
        /// <summary>
        /// ��Ĭ�ֿ���
        /// </summary>
        SilenceResistRate,
        /// <summary>
        /// ���Լ���
        /// </summary>
        MaleDmgReduction,
        /// <summary>
        /// ���ͣ����BUFF���ԣ�
        /// </summary>
        Toughness,
        /// <summary>
        /// ����ϵ��
        /// </summary>
        CriticalFactor,
        SHealHp,
        STMP,
        /// <summary>
        /// ħ����ʸ������ѧԺ�鼮��̫�����ĳ��衷�Է����� ����ʹ�編����ͨ������(5%)  ħ��ǿ�ȵĶ���ħ���˺�
        /// </summary>
        MagicArrows,
        /// <summary>
        /// ��ŭ������ѧԺ�鼮��Ұ��֮������������������ʹ�����ܱ����������״̬��������ɵ�ǰ����ֵx%�������˺�
        /// </summary>
        Wild,
        /// <summary>
        /// �綾֮������ѧԺ�鼮��ѱ�����֡�������������ʹ�����ĵ�������ÿ��������x%,������ɵ��˺�����Y%
        /// </summary>
        PoisonousBall,
        /// <summary>
        /// �ظ��ͷ�ħ�����ܼ���
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
        //����
        CriticalDamageAddonRate,         //�����������˺��ٷֱ�
        MagicCriticalDamageAddonRate,    //�������������˺��ٷֱ�
        AOEDamageAddonRate,              //����AOE�˺��ٷֱ�
        HolyDamageAddonRate,             //������ʥ�˺��ٷֱ�
        ContinuousDamageAddonRate,       //���������˺��ٷֱ�
        MagicCriticalDamageReduceRate,   //����ħ�������˺��ٷֱ�
        CriticalDamageReduceRate,        //�����������˺��ٷֱ�
        AOEDamageReduceRate,             //����AOE�˺��ٷֱ�
        HolyDamageReduceRate,            //������ʥ�˺��ٷֱ�
        ContinuousDamageReduceRate,      //���ͳ����˺��ٷֱ�
        AttackOffsetAddon,               //��������������
        MagicAttackOffsetAddon,          //����ħ����������
        DefendHitRecoverAddon,           //�����ֿ�Ӳֱ
        SummonedAttrAddon,               //�����ٻ�������
        MagicBloodSuckingAddon,          //����������Ѫ
        DeathReduceEnemyEnergyRecover,   //�������ٵз�����
        //���� End
        MAX,
        PLUSMAX = MAX + 15,
    }
}
