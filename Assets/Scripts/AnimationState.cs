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
    {}

    public override string ToString ()
    {
        return string.Format ("[AnimationState: StateName={0}]", State);
    }

}