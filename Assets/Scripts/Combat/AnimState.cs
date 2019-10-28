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




        }

        System.Activator.CreateInstance(System.Type.GetType("IdleSate"), Controller);

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
        IsBreak = true;
        m_Controller.Steady = m_Controller.Data.Steady;
        OnStateExit();
        //m_Controller.SetState()//反射创建AnimState类??

    }

    public virtual void OnStateBreak()//walk to target
    {
        IsBreak = true;
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

}