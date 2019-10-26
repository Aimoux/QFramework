using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class AttackHeavyState : AnimState
{
    public AttackHeavyState(Role Controller):base(Controller)
    {
        this.State = Common.ANIMATIONSTATE.ATTACKHEAVY;
         GameData.WeaponFrameData data = DataManager.Instance.WeaponFrames[Controller.CurWeapon.Data.ID];
        this.FrameCount = data.AttackHeavyCount;
        this.FrameStart = data.AttackHeavyStart;
        this.FrameEnd = data.AttackHeavyEnd;
        this.IsAttackState = true;
        this.AnimCategory = ANIMATIONTYPE.ATTACK;
        this.Impact = ImpactType.HEAVY;
        this.DamageRatio = 1.5f;
        this.ImpactAtkRatio = 1.5f;
        this.ImpactDefRatio = 1.5f;
    }

}
