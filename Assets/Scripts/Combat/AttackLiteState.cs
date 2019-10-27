using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class AttackLiteState : AnimState
{
    public AttackLiteState(Role Controller):base(Controller, ANIMATIONSTATE.ATTACKLITE)
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
