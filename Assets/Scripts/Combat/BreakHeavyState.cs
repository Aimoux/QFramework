using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakHeavyState : AnimState
{
    public BreakHeavyState(Role Controller):base(Controller)
    {
        this.State = Common.ANIMATIONSTATE.BREAKHEAVY;
        this.AnimCategory = Common.ANIMATIONTYPE.HITREACTION;
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
