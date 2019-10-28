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

    public override bool CanTransit(AnimState next)
    {
        bool can = base.CanTransit(next);
        switch(next.State)
        {
            case Common.ANIMATIONSTATE.IDLE:
            case Common.ANIMATIONSTATE.WALK://等待补充体力死否受控制??
                return true;

            case Common.ANIMATIONSTATE.ATTACKLITE:
                return true;




        }



        return false;
    }
}
