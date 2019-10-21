using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

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

    protected int FrameCount;//物理帧数fixed delta Time 时长
    public int CurFrame;
    protected int FrameStart;//enable sensor
    protected int FrameEnd;//disable sensor
    protected bool IsAttackState = false;
    public bool IsWeaponEnable
    {
        get
        {
            return IsAttackState && CurFrame >= FrameStart || CurFrame <= FrameEnd ;
        }
    }


    // 构造
    public AnimState(Role Controller)
    { 
        m_Controller = Controller; 
        
    }

    // 开始
    public virtual void OnStateEnter()
    {
        CurFrame = FrameCount;
        m_Controller.Controller.SetState(this);
    }

    // 結束
    public virtual void OnStateExit()
    {
        //CurFrame = FrameCount;//??push idle??
    }

    // 更新
    public virtual void OnStateUpdate()
    {
        CurFrame--;
        CurFrame = CurFrame < 0 ? 0 : CurFrame;

        if(CurFrame == FrameStart )//enable weapon sensor
        {
            OnFrameStart();
        }

        if(CurFrame == FrameEnd )//disable
        {
            OnFrameEnd();
        }
    }

    public virtual void OnFrameStart()
    {

    }

    public virtual void OnFrameEnd()
    {

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