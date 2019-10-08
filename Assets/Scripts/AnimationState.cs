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

    public int FrameCount;//物理帧数fixed delta Time 时长


    // 构造
    public AnimState(Role Controller)
    { 
        m_Controller = Controller; 
    }

    // 开始
    public virtual void OnStateEnter()
    {}

    // 結束
    public virtual void OnStateExit()
    {}

    // 更新
    public virtual void OnStateUpdate()
    {
        FrameCount--;
        FrameCount = FrameCount < 0 ? 0 : FrameCount;
    }

    public override string ToString ()
    {
        return string.Format ("[AnimationState: StateName={0}]", State);
    }

    //为避免过多new，直接为每个state创建实例，之后使用这些实例not new?? 
    public virtual bool CanTransit(AnimState next)
    {

        return true;
    }

}