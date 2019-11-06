using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using GameData;
using Common;


using System.Collections;
using System.Collections.Generic;
using System.Linq;





public class MoveToTarget : Action
{
    public SharedRole sdtarget;
    public SharedRole sdself;
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
        ResultType ret = sdself.Value.IsTargetWithinRange(MaxRange, MinRange, MaxAngle);
        switch (ret)
        {
            case ResultType.TOOFAR:
                if (sdself.Value.Status.State == ANIMATIONSTATE.WALKFORWARD)
                {
                    //here better than WalkAnstUpdate??(walk to tar vs walk straight forward)
                    ResultType hasPth = sdself.Value.MoveToTarget(sdtarget.Value.Position);
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

//二人转 = LookAt+WalkLeft/Right??
public class MoveToPosition : Action
{
    public Vector3 targetpos;
    public SharedRole sdself;

    public override TaskStatus OnUpdate()
    {
        ResultType ret = sdself.Value.MoveToPosition(targetpos);
        switch (ret)
        {
            case ResultType.TOOFAR:
                if (sdself.Value.Status.State == ANIMATIONSTATE.WALKFORWARD)
                {
                    ResultType hasPath = sdself.Value.MoveToTarget(targetpos);
                    if (hasPath != ResultType.SUCCESS)
                        sdself.Value.Status.OnStateBreak();
                }
                else
                    sdself.Value.Status.OnStateBreak(ANIMATIONSTATE.WALKFORWARD);
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

            case ResultType.FAILURE://no path??
                sdself.Value.Status.OnStateBreak();
                break;
        }

        if (ret == Common.ResultType.SUCCESS)
            return TaskStatus.Success;
        else
            return TaskStatus.Failure;
    }

}

public class FindAssaultTarget : Action
{
    public SharedRole sdtarget;
    public SharedRole sdself;
    public override void OnStart()
    {

    }

    public override TaskStatus OnUpdate()
    {

        return TaskStatus.Success;
    }
}



public class LaunchAssault: Action
{
    public SharedRole sdtarget;
    public SharedRole sdself;
    private List<int> asds;

    public override void OnStart()
    {
        asds = sdself.Value.Assaults.Keys.ToList();
        int id = Random.Range(0, asds.Count);
        AssaultExtension ast = sdself.Value.GetAssaultById(asds[id]);
        sdself.Value.UseAssault(ast, sdtarget.Value);      

    }

    public override TaskStatus OnUpdate()
    {



        return TaskStatus.Success;
    }




}
