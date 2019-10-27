using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class BreakHeavyState : AnimState
{
    public BreakHeavyState(Role Controller):base(Controller, ANIMATIONSTATE.BREAKHEAVY)
    {

    }

    // 开始
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        
    }

    // 結束
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
