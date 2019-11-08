using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using GameData;
using Common;

public class MoveToTarget : Action
{
    //public SharedRole sdtarget;
    public SharedRole sdself;
    public Role Target;
    public float MaxRange = Const.DefaultWeaponRange;//默认攻击距离,战斗模块设置为技能伤害半径
    public float MinRange = Const.CollisionRadius;//默认碰撞,需要根据模型半径修正??特殊战斗招式用(太近了打不到)
    public float MaxAngle = Const.ErrorAngle;//默认角度差,战斗模块设置为技能伤害角度
    public int Attempt;

    public override void OnStart()
    {
        //累计失败次数,放弃决策??
        Attempt = 0;

    }

    public override TaskStatus OnUpdate()
    {

        // if(sdself.Value.OnTactic)//??
        //     return TaskStatus.Success;
        int type = DataManager.Instance.Animations[(int)sdself.Value.Status.State].AnimationType;
        if(type >1)
            return TaskStatus.Failure;

        ResultType ret = sdself.Value.IsTargetWithinRange(MaxRange, MinRange, MaxAngle);
        switch (ret)
        {
            case ResultType.TOOFAR:
                if (sdself.Value.Status.State == ANIMATIONSTATE.WALKFORWARD)
                {
                    //here better than WalkAnstUpdate??(walk to tar vs walk straight forward)
                    ResultType hasPth = sdself.Value.MoveToTarget(Target.Position);
                    if (hasPth != ResultType.SUCCESS)
                        sdself.Value.Status.OnStateBreak();
                }
                else
                {
                    sdself.Value.Status.OnStateBreak(ANIMATIONSTATE.WALKFORWARD);//此处已实现打断walk turn??
                    Attempt++;//when to quit tactic
                }
                break;

            case ResultType.TOONEAR:
                if (sdself.Value.Status.State == ANIMATIONSTATE.WALKBACK)
                {
                    sdself.Value.WalkBack();
                }
                else
                    sdself.Value.Status.OnStateBreak(ANIMATIONSTATE.WALKBACK);
                break;

            case ResultType.LEFTSIDE:
                if (sdself.Value.Status.State == ANIMATIONSTATE.WALKTURNLEFT)
                {
                    sdself.Value.WalkTurnLeft();
                }
                else
                    sdself.Value.Status.OnStateBreak(ANIMATIONSTATE.WALKTURNLEFT);
                break;

            case ResultType.RIGHTSIDE:
                if (sdself.Value.Status.State == ANIMATIONSTATE.WALKTURNRIGHT)
                {
                    sdself.Value.WalkTurnRight();
                }
                else
                    sdself.Value.Status.OnStateBreak(ANIMATIONSTATE.WALKTURNRIGHT);
                break;

            case ResultType.SUCCESS:
                sdself.Value.StopNav();
                break;
        }

        if (ret == Common.ResultType.SUCCESS)
            return TaskStatus.Success;
        else
            return TaskStatus.Failure;
    }
}

