using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : AnimState
{
    public IdleState(Role  Controller):base(Controller)
    {
        this.State = Common.ANIMATIONSTATE.IDLE;
    }

    // 开始
    public override void OnStateEnter()
    {
        // 可以在此进行游戏数据的加载和初始化等
    }

    // 更新
    public override void OnStateUpdate()
    {
        // 转换场景
        //m_Controller.SetState(new WalkState(m_Controller), "MainMenuScene");
    }

}
