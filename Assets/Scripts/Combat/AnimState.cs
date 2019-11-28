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
    protected Role Caster = null;
    public AnimationData Data;
    public int FrameCount;//物理帧数fixed delta Time 时长
    public int CurFrame;
    //public bool IsBreak;

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

    public AnimState(Role Controller, int id, int Count, int Start, int End)
    { 
        Caster = Controller;
        ID = id;
        Data = DataManager.Instance.Animations[ID];
        State = (ANIMATIONSTATE)Data.State;

        FrameCount = Mathf.RoundToInt(Count * Common.Const.DeltaFrame / Time.fixedDeltaTime);
        FrameStart = Mathf.RoundToInt(Start * Common.Const.DeltaFrame / Time.fixedDeltaTime);
        FrameEnd = Mathf.RoundToInt(End * Common.Const.DeltaFrame / Time.fixedDeltaTime);
       
    }

    public virtual void Init()//OnStateEnter已替代此功能??
    {
        CurFrame = 0;
    }

    // 开始
    public virtual void OnStateEnter()
    {
        CurFrame = 0;
        //IsBreak = false;
        Caster.Controller.SetState(this);

        switch(Data.AnimationType)
        {
            case (int)ANIMATIONTYPE.MOTION:
                OnMotionEnter();
                break;

            case (int)ANIMATIONTYPE.ATTACK:
                OnAttackEnter();
                break;

            case (int)ANIMATIONTYPE.STUNT:
                OnStuntEnter();
                break;

            case (int)ANIMATIONTYPE.HITREACTION:
                OnHitEnter();
                break;

            case (int)ANIMATIONTYPE.DEATH:
                OnDeathEnter();
                break;
        }

        // if(Caster.ID == 2)
        //     Debug.LogError(Caster.ID + " enter state: " + State);
    }

    private void OnMotionEnter()
    {
        Caster.StartNav();
    }

    private void OnAttackEnter()
    {
        //起始到打击检测结束进行出手韧性保护
        Caster.Steady *= Caster.CurWeapon.Data.ImpactRatio * Data.DefenseImpactRatio;
        Caster.Stamina -= (int)(Caster.CurWeapon.Data.StaminaCost * Data.StaminaCostRatio);
        Caster.Stamina = Caster.Stamina < 0 ? 0 : Caster.Stamina;
        Caster.StopNav();
    }

    private void OnStuntEnter()//enter:攻方timeline.play, 关键帧(enable):目标timeline.play??
    {
        Caster.PlayTimeLine(Caster.CurAssault.StuntPath);
    }

    //vs Role.OnStatusBreak()
    private void OnHitEnter()
    {
        //Caster.BreakAssault();

    }

    private void OnDeathEnter()
    {
        Caster.IsDead = true;
    }

    public virtual void OnStateBreak()//walk to target
    {
        //IsBreak = true;
        //m_Controller.Cmds.Clear();//??打断一个,整个策略放弃??
        //m_Controller.Steady = m_Controller.Data.Steady;
        OnStateExit();
        //m_Controller.SetState()//反射创建AnimState类??

    }


    // 结束
    public virtual void OnStateExit()
    {
        //motion以及idle设置为loop true
        // if(Data.AnimationType ==(int)ANIMATIONTYPE.MOTION)
        // {
        //     Caster.StopNav();
        // }

        switch(Data.AnimationType)
        {
            case (int)ANIMATIONTYPE.ATTACK:
                OnAttackExit();
                break;

            case (int)ANIMATIONTYPE.STUNT:
                OnStuntExit();
                break;
        }

    }

    private void OnAttackExit()
    {
        if(Caster.CurAssault != null)
        {
            //避免Actions中出现重复state??
            if(ID == Caster.CurAssault.Actions[0])
            {
                Caster.CurAssault.End();
            }
        }
    }

    private void OnMotionExit()
    {

    }

    private void OnStuntExit()
    {
        Caster.StopTimeLine();
    }

    // 更新
    public virtual void OnStateUpdate()
    {
        // if(Caster.ID == 2)
        //     Debug.LogError(Caster.ID + " " + State + " " + CurFrame);

        //break trans 判定??

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
            Caster.Steady /= (Caster.CurWeapon.Data.ImpactRatio * Data.DefenseImpactRatio);
        }

    }

    public override string ToString ()//??for what purpose??
    {
        return string.Format ("[AnimationState: StateName={0}]", State);
    }
  
    //边走边嗑药、表情会运用到layer以及AvatarMask
    //嗑药动作放在UpBody层，表情放在Head层


    public virtual int CanTransit(AnimState next)
    {
        Dictionary<int, int> trans;
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


}

