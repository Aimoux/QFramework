using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class BreakLiteState : AnimState
{
    public BreakLiteState(Role Controller):base(Controller, ANIMATIONSTATE.BREAKLITE)
    {

    }

    // 开始
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        
    }

    // 结束
    public override void OnStateExit()
    {
        base.OnStateExit();
    }

    // 更新
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
    }
}
