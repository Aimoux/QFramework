using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : AnimState
{
    public WalkState(Role Controller)
        : base(Controller)
    {
        this.State = Common.ANIMATIONSTATE.WALK;
    }

    // 开始
    public override void OnStateEnter()
    {
        // 取得开始按钮
//        Button tmpBtn = UITool.GetUIComponent<Button>("StartGameBtn");
//        if (tmpBtn != null)
//            tmpBtn.onClick.AddListener(() => OnStartGameBtnClick(tmpBtn));
    }

    // 开始战斗
    //    private void OnStartGameBtnClick(Button theButton)
    //    {
    //        //Debug.Log ("OnStartBtnClick:"+theButton.gameObject.name);
    //        m_Controller.SetState(new AttackLiteState(m_Controller), "BattleScene");
    //    }

}

