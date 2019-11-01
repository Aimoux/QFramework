using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using GameData;
using System.Reflection;

//AnimStateExtension??通过配置实现仅仅外观特殊的武器,特定招式具有特别的效果?? AttackLiteXXXOnStateEnter() <=> TalentXXXStart()
public abstract class AnimState
{
    //考虑多层级继承? AttackLite: AttackState: AnimState, WalkForWard: Walk: AnimState?? 



    private  ANIMATIONSTATE m_State = ANIMATIONSTATE.IDLE; // 状态
    public ANIMATIONSTATE State
    {
        get{ return m_State; }
        set{ m_State = value; }// read only??
    }

    // 状态拥有者
    protected Role m_Controller = null;
    public AnimationData Data;
    public int FrameCount;//物理帧数fixed delta Time 时长
    public int CurFrame;
    public bool IsBreak;

    #region AttackAnim
    protected int FrameStart;//enable sensor
    protected int FrameEnd;//disable sensor
    public bool IsWeaponEnable
    {
        get
        {
            return Data.AnimationType == (int)ANIMATIONTYPE.ATTACK  && CurFrame >= FrameStart && CurFrame <= FrameEnd ;
        }
    }
    public int TalentId;//Anim与Talent关联极高,考虑TalentXXXOnFrameStart??
    #endregion

    public AnimState(Role Controller, ANIMATIONSTATE state)
    { 
        m_Controller = Controller;
        State = state;
        Data = DataManager.Instance.Animations[(int)state];
        int[] frames = DataManager.Instance.WeaponFrames[Controller.CurWeapon.Data.ID][(int)state];
        FrameCount = frames[0];
        FrameStart = frames[1];
        FrameEnd = frames[2];
    }

    public static AnimState Create(Role Controller, ANIMATIONSTATE state)
    {
        //create instance by name??
        switch (state)
        {
            case ANIMATIONSTATE.IDLE:
                return new IdleState(Controller);
            case ANIMATIONSTATE.WALKFORWARD:
                return new WalkForwardState(Controller);
            case ANIMATIONSTATE.WALKBACK:
                return new WalkBackState(Controller);
            case ANIMATIONSTATE.ATTACKLITE:
                return new AttackLiteState(Controller);
            case ANIMATIONSTATE.ATTACKHEAVY:
                return new AttackHeavyState(Controller);

        }

        // System.Activator.CreateInstance(System.Type.GetType("IdleSate"), Controller);

        return new IdleState(Controller);
    }

    public virtual void Init()//OnStateEnter已替代此功能??
    {
        CurFrame = 0;
    }

    // 开始
    public virtual void OnStateEnter()
    {
        CurFrame = 0;
        IsBreak = false;
        m_Controller.Controller.SetState(this);
        if( Data.AnimationType == (int)ANIMATIONTYPE.ATTACK)
        {
            //起始到打击检测结束进行出手韧性保护
            m_Controller.Steady *= m_Controller.CurWeapon.Data.ImpactRatio * Data.DefenseImpactRatio;
        }
    }

    //当前动作被打断（比如受攻击）
    public virtual void OnStateBreak(ANIMATIONSTATE state)
    {


        OnStateBreak();

    }

    public virtual void OnStateBreak()//walk to target
    {
        IsBreak = true;
        //m_Controller.Cmds.Clear();//??打断一个,整个策略放弃??
        //m_Controller.Steady = m_Controller.Data.Steady;
        OnStateExit();
        //m_Controller.SetState()//反射创建AnimState类??

    }


    // 結束
    public virtual void OnStateExit()
    {
        //CurFrame = FrameCount;//??push idle??
    }

    // 更新
    public virtual void OnStateUpdate()
    {
        CurFrame++;
        CurFrame = CurFrame > FrameCount ? FrameCount : CurFrame;

        if(CurFrame == FrameStart )//enable weapon sensor
        {
            OnFrameStart();
        }

        if(CurFrame == FrameEnd )//disable
        {
            OnFrameEnd();
        }
    }

    public virtual void OnFrameStart()//atk frame start
    {
        

    }

    //被打断时，此处是否仍应调用??
    public virtual void OnFrameEnd()//atk frame end
    {
        if( Data.AnimationType == (int)ANIMATIONTYPE.ATTACK)
        {
            //起始到打击检测结束进行出手韧性保护
            m_Controller.Steady /= (m_Controller.CurWeapon.Data.ImpactRatio * Data.DefenseImpactRatio);
        }

    }

    public override string ToString ()//??for what purpose??
    {
        return string.Format ("[AnimationState: StateName={0}]", State);
    }

    //为避免过多new，直接为每个state创建实例，之后使用这些实例not new?? 
    public virtual bool CanTransit(AnimState next)
    {
        return true;
    }

    //被行为树调用
    //开场、受攻击、上个目标丢失
    //仇恨值系统,距离,对己伤害,目标状态(血量,体力)
    //角色状态:距离\血量\体力,具象化为定量的仇恨值,或是与仇恨值平级的存在??
    //寻找目标:按AnimStateExtension配置的TargetType(默认为最近?),仇恨值仅用于转移攻击目标判定?
    public virtual Role FindTarget()
    {
        // Role target = null;
        // int hate = 0;
        // foreach (var kv in HatredDict)
        // {
        //     if (kv.Value > hate)
        //     {
        //         hate = kv.Value;
        //         target = kv.Key;
        //     }
        // }
        //if (Target != null)
        //    return Target;

        //foreach (Role role in Logic.Instance.GetAliveEnemies(Faction))
        //{
        //    Target = role;//interface selector??  vs hatred? 加权?
        //    return role;
        //}

        return null;



        //if (this.Data.TargetType == TargetType.Target || (this.Caster.AbilityEffects.Taunt && (RoleSide)this.Data.AffectedSide == RoleSide.Enemy))
        //{
        //    if (defaultTarget != null)
        //    {
        //        this.Target = defaultTarget;
        //    }
        //    else
        //    {
        //        float dist = 0;
        //        Role target = this.Caster.FindTarget(ref dist);
        //        if (dist <= this.MaxRange)
        //        {
        //            this.Target = target;
        //        }
        //        else if (this.Caster.AbilityEffects.Taunt)  //修改放技能瞬间被战场操控者嘲讽，却攻击距离打不到战场操控者的bug
        //        {
        //            this.Target = this.Caster.Target;
        //        }
        //        else
        //        {
        //            this.Target = null; //这里应该是空的
        //        }
        //    }
        //}
        //else if (this.Data.TargetType == TargetType.Self)
        //{
        //    this.Target = this.Caster;
        //}
        //else if (this.Data.TargetType == TargetType.DeadBody)
        //{
        //    this.Target = null;
        //    foreach (Role role in Logic.Instance.Roles.Where(role => role.RoleST == 6 && role.IsDeadBody && role.AniTotaltime < DataManager.Numerics[59].Param))
        //    {
        //        Target = role;
        //    }
        //    return Target;
        //}
        //else if (this.TargetSelector != null)
        //{
        //    float max = -float.MaxValue;
        //    Role result = null;
        //    foreach (Role role in Logic.Instance.GetSurvivors((RoleSide)this.TargetSide))
        //    {
        //        if (Data.TargetClass == RoleType.Demon && !role.Config.IsDemon)
        //            continue;

        //        if (Data.TargetClass == RoleType.Natural && role.Config.IsDemon)
        //            continue;

        //        if (role.AbilityEffects.Void && !role.CheckInvisibility())
        //            continue;

        //        if (this.Caster.AbilityEffects.Charm && role == this.Caster)
        //            continue;

        //        double distance = (role.Position - this.Caster.Position).magnitude;
        //        if (distance >= this.MinRange && distance <= this.MaxRange)
        //        {
        //            float v = this.TargetSelector.Select(role);
        //            if (max < v)
        //            {
        //                max = v;
        //                result = role;
        //            }
        //        }
        //    }
        //    this.Target = result;
        //}
        //else
        //    Logger.LogError(string.Format("target type error: {0}", this.Data.TargetType));

        //return this.Target;



    }

    public Dictionary<TargetType, ITargetSelector> TargetSelectors;

    public ITargetSelector TargetSelector
    {
        get { return this.TargetSelectors.ContainsKey(this.Data.TargetType) ? this.TargetSelectors[this.Data.TargetType] : null; }
    }


}

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

 public class AnimStateExtension : AnimState 
{
    static Dictionary<string, MethodInfo> methodMap;
    static Dictionary<string, MethodInfo> MethodMap
    {
        get
        {
            if (methodMap == null)
            {
                methodMap = new Dictionary<string, MethodInfo>();
                MethodInfo[] ms = typeof(AnimStateExtension).GetMethods();
                foreach (MethodInfo m in ms)
                {
                    methodMap[m.Name] = m;
                }
            }
            return methodMap;
        }
    }
    public AnimStateExtension(Role Controller) : base(Controller, ANIMATIONSTATE.ATTACKLITE)
    {


    }


    public bool CanTranslateTo201(AnimState anst)
    {

        return true;
    }

    public bool CanTranslateTo201_1(AnimState anst)
    {

        return true;
    }

    public override void OnStateUpdate()
    {
        string funcName = "OnStateUpdate" + Data.ID;
        string orgFuncName = "OnStateUpdate" + Data.State;
        if (MethodMap.ContainsKey(funcName))
        {
            MethodInfo func = MethodMap[funcName];
            func.Invoke(this, new object[] { });
        }
        else if (MethodMap.ContainsKey(orgFuncName))
        {
            MethodInfo func = MethodMap[orgFuncName];
            func.Invoke(this, new object[] { });
        }
        else
            base.OnStateUpdate();
    }


    public void OnStateUpdate101()//基础前进
    {
        //set anim;
    }

    public void OnStateUpdate102()//基础后退
    {

    }

    #region Weapon _1
    //导致必须为每一个ID,写OnStateUpdate101XXX,否则会调用最基础的OnStateUpdate
    public void OnStateUpdate101901()//前进加耐力,避免"001"被替换为"1"
    {
        OnStateUpdate101();//simi base walk forward



    }



    #endregion


}


public class AnimData
{
    public int ID;//for override
    public int State;//201,101层级?



}
