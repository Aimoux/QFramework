using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public interface ITargetSelector
{
    TargetType Type { get; }
    float Select(Role role);
}

class RandomSelector : ITargetSelector
{
    public TargetType Type
    {
        get { return TargetType.Random; }
    }

    public float Select(Role role)
    {
        return (float)Random.Range(0f,1f);//??
    }
}

class WeakestSelector : ITargetSelector
{
    public TargetType Type
    {
        get { return TargetType.Weakest; }
    }

    public float Select(Role role)
    {
        return -role.HP / role.Attributes.HP;
    }
}
class MaxHPSelector : ITargetSelector
{
    public TargetType Type
    {
        get { return TargetType.MaxHP; }
    }

    public float Select(Role role)
    {
        return role.HP;
    }
}

class NearestSelector : ITargetSelector
{
    public TargetType Type
    {
        get { return TargetType.Nearest; }
    }

    private Role Caster;

    public NearestSelector(Role caster)
    {
        this.Caster = caster;
    }

    public float Select(Role role)
    {
        return -(role.Position - this.Caster.Position).magnitude;
    }
}
class FarthestSelector : ITargetSelector
{
    public TargetType Type
    {
        get { return TargetType.Farthest; }
    }
    private Role Caster;
    public FarthestSelector(Role caster)
    {
        this.Caster = caster;
    }

    public float Select(Role role)
    {
        return (role.Position - this.Caster.Position).magnitude;
    }
}

class MaxMPSelector : ITargetSelector
{
    public TargetType Type
    {
        get { return TargetType.MaxMP; }
    }

    public float Select(Role role)
    {
        return role.MP % 1000;
    }
}

class MinMPSelector : ITargetSelector
{
    public TargetType Type
    {
        get { return TargetType.MinMP; }
    }

    public float Select(Role role)
    {
        if (role.MP >= 1000) //修复选择最小MP找到了满MP  因为满MP对一千取余为0 大于 90对1000取余   WangJunlong 2019-5-21
            return -role.MP;
        return -role.MP % 1000;
    }
}

//class MaxIntelligenceSelector : ITargetSelector
//{
//    public TargetType Type
//    {
//        get { return TargetType.MaxIntelligence; }
//    }

//    public float Select(Role role)
//    {
//        return role.Attributes.Intelligence;
//    }
//}
class MaxStrengthSelector : ITargetSelector
{
    public TargetType Type
    {
        get { return TargetType.MaxStrength; }
    }

    public float Select(Role role)
    {
        return role.Attributes.Strength;
    }
}
//class MaxAgilitySelector : ITargetSelector
//{
//    public TargetType Type
//    {
//        get { return TargetType.MaxAgility; }
//    }

//    public float Select(Role role)
//    {
//        return role.Attributes.Agility;
//    }
//}
class MinHPSelector : ITargetSelector
{
    public TargetType Type
    {
        get { return TargetType.MinHP; }
    }

    public float Select(Role role)
    {
        return -role.HP;
    }
}

//class MaxADSelector : ITargetSelector
//{
//    public TargetType Type
//    {
//        get { return TargetType.MaxAttackDamage; }
//    }

//    public float Select(Role role)
//    {
//        return role.Attributes.AttackDamage;
//    }
//}

//class MaxAPSelector : ITargetSelector
//{
//    public TargetType Type
//    {
//        get { return TargetType.MaxAbilityPower; }
//    }

//    public float Select(Role role)
//    {
//        return role.Attributes.AbilityPower;
//    }
//}



