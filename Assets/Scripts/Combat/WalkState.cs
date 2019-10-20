using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : AnimState
{
    public WalkState(Role Controller)
        : base(Controller)
    {
        this.State = Common.ANIMATIONSTATE.WALK;
        GameData.WeaponFrameData data = DataManager.Instance.WeaponFrames[Controller.CurWeapon.Data.ID];
        this.FrameCount = data.WalkCount;
        this.CurFrame = FrameCount;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        m_Controller.Controller.anim.SetFloat(Common.Const.Vertical, 1f);// 1 walk, 2 run

    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        m_Controller.MoveToTarget();        
    }


}

