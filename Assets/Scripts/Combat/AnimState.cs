using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using GameData;

public abstract class AnimState
{
    private  ANIMATIONSTATE m_State = ANIMATIONSTATE.IDLE; // 状态
    public ANIMATIONSTATE State
    {
        get{ return m_State; }
        set{ m_State = value; }// read only??
    }
    public int ID;

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
    public Role Target;//可以与Controller.Target不同
    #endregion

    public AnimState(Role Controller, int id)
    { 
        m_Controller = Controller;
        ID = id;
        Data = DataManager.Instance.Animations[ID];
        State = (ANIMATIONSTATE)Data.State;
        try 
        {
            int[] frames = DataManager.Instance.WeaponFrames[Controller.CurWeapon.Data.ID][(int)State];
            FrameCount = frames[0];
            FrameStart = frames[1];
            FrameEnd = frames[2];

            //根据物理时间步长进行换算
            FrameCount = Mathf.RoundToInt(FrameCount * Common.Const.DeltaFrame / Time.fixedDeltaTime);
            FrameStart = Mathf.RoundToInt(FrameStart * Common.Const.DeltaFrame / Time.fixedDeltaTime);
            FrameEnd = Mathf.RoundToInt(FrameEnd * Common.Const.DeltaFrame / Time.fixedDeltaTime);
        }
        catch 
        {
            Debug.LogError("wp id=: " + Controller.CurWeapon.Data.ID + " st=: " + State);
        }
        
       
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
        FindTarget();//??

        if( Data.AnimationType == (int)ANIMATIONTYPE.ATTACK)
        {
            //起始到打击检测结束进行出手韧性保护
            m_Controller.Steady *= m_Controller.CurWeapon.Data.ImpactRatio * Data.DefenseImpactRatio;
        }

        if(m_Controller.ID == 2)
            Debug.LogError(m_Controller.ID + " enter state: " + State);
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
        //为保证frame准确，不利用anim的loop，此处重新推??

    }

    // 更新
    public virtual void OnStateUpdate()
    {
        if(m_Controller.ID == 2)
            Debug.LogError(m_Controller.ID + " " + State + " " + CurFrame);

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
    //边走边嗑药、表情会运用到layer以及AvatarMask
    //嗑药动作放在UpBody层，表情放在Head层
    //为避免过度复杂，anim.SetLayerWeight(lyId, wgt), lerp。实际上可以在编辑器中直接指定Up及Head的weight为1??

    //手敲状态机??状态模式??
    //Idle, Move(?)到其他状态的转换不需要等待
    //非Idle到其他状态的转换大多需要等待
    //如能设置anystate 的 has exit time，则不需此处
    public virtual int CanTransit(AnimState next)
    {
        Dictionary<float, int> trans;
        if(DataManager.Instance.Transits.TryGetValue((int)State, out trans))
        {
            int cantrans;
            if(trans.TryGetValue((int)next.State, out cantrans))
                return cantrans;
            else
                Debug.LogError("find no next key: " + next.State);

        }
        else
            Debug.LogError("find no first key: " + State);

        return 0;
    }

    //被行为树调用
    //开场、受攻击、上个目标丢失
    //仇恨值系统,距离,对己伤害,目标状态(血量,体力)
    //角色状态:距离\血量\体力,具象化为定量的仇恨值,或是与仇恨值平级的存在??
    //寻找目标:按AnimStateExtension配置的TargetType(默认为最近?),仇恨值仅用于转移攻击目标判定?
    public virtual Role FindTarget()
    {
        Target = m_Controller.Target;
        return Target;

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
        get { return this.TargetSelectors.ContainsKey((TargetType)Data.TargetType) ? this.TargetSelectors[(TargetType)Data.TargetType] : null; }
    }


}

