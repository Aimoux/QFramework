using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackLiteState : AnimState
{
    public AttackLiteState(Role Controller):base(Controller)
    {
        this.State = Common.ANIMATIONSTATE.ATTACKLITE;
         GameData.WeaponFrameData data = DataManager.Instance.WeaponFrames[Controller.CurWeapon.Data.ID];
        this.FrameCount = data.AttackLiteCount;
        this.CurFrame = FrameCount;
        this.FrameStart = data.AttackLiteStart;
        this.FrameEnd = data.AttackLiteEnd;
        this.IsAttackState = true;
    }

    // 开始
    public override void OnStateEnter()
    {
        //PBaseDefenseGame.Instance.Initinal();
    }

    // 結束
    public override void OnStateExit()
    {
        //PBaseDefenseGame.Instance.Release();
    }

    // 更新
    public override void OnStateUpdate()
    {   
        // 游戏逻辑
        //PBaseDefenseGame.Instance.Update();
        // Render由Unity負責

        // 游戏是否结束
        //if( PBaseDefenseGame.Instance.ThisGameIsOver())
            //m_Controller.SetState(new WalkState(m_Controller), "MainMenuScene" );
    }
}
