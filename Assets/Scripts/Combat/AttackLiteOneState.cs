using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class AttackLiteOneState :AnimState {

    public AttackLiteOneState(Role Controller):base(Controller)
    {
        this.State = Common.ANIMATIONSTATE.ATTACKLITE;
         GameData.WeaponFrameData data = DataManager.Instance.WeaponFrames[Controller.CurWeapon.Data.ID];
        this.FrameCount = data.AttackLiteCount;
        this.FrameStart = data.AttackLiteStart;
        this.FrameEnd = data.AttackLiteEnd;
        this.IsAttackState = true;
        this.AnimCategory = ANIMATIONTYPE.ATTACK;
        this.Impact = ImpactType.LITE;
        this.DamageRatio = 1f;//配表global??
        this.ImpactAtkRatio = 1f;
        this.ImpactDefRatio = 1f;
    }
   


}
