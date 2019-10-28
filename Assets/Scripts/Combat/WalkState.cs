using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class WalkState : AnimState
{
    public WalkState(Role Controller, ANIMATIONSTATE state):base (Controller, state)
    {

    }
    public WalkState(Role Controller)
        : base(Controller, ANIMATIONSTATE.WALKFORWARD)
    {

    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        m_Controller.Controller.anim.SetFloat(Common.Const.Vertical, 1f);// 1 walk, 2 run

    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        m_Controller.Controller.StopNav();
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        m_Controller.MoveToTarget();
        // if(m_Controller.MoveToTarget())
        // OnStateBreak();        
    }




}

