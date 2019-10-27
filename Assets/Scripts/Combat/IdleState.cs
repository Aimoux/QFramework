using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using Common;

public class IdleState : AnimState
{
    public IdleState(Role  Controller):base(Controller, ANIMATIONSTATE.IDLE)
    {


    }

    // 开始
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        // 可以在此进行游戏数据的加载和初始化等
    }

    // 更新
    public override void OnStateUpdate()
    {
        // 转换场景
        //m_Controller.SetState(new WalkState(m_Controller), "MainMenuScene");

        base.OnStateUpdate();
    }

    //手敲状态机
    //缺陷：增加状态A时，除了添加A->X，还要找出所有能够转向A的X，并修改之。
    public override bool CanTransit(AnimState next)
    {
        switch (next.State)
        {
            case Common.ANIMATIONSTATE.ATTACKHEAVY:
                return false;


        }

        return true;

    }


}
