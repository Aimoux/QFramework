using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using GameData;
using Common;

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
                    //sdself.Value.Status.OnStateBreak(ANIMATIONSTATE.WALKFORWARD);
                    sdself.Value.PushState((int)ANIMATIONSTATE.WALKFORWARD);
                break;

            case ResultType.LEFTSIDE:
                if (sdself.Value.Status.State == ANIMATIONSTATE.WALKTURNLEFT)
                {
                    sdself.Value.WalkTurnLeft();
                }
                else
                    //sdself.Value.Status.OnStateBreak(ANIMATIONSTATE.WALKTURNLEFT);
                    sdself.Value.PushState((int)ANIMATIONSTATE.WALKTURNLEFT);
                break;

            case ResultType.RIGHTSIDE:
                if (sdself.Value.Status.State == ANIMATIONSTATE.WALKTURNRIGHT)
                {
                    sdself.Value.WalkTurnRight();
                }
                else
                    //sdself.Value.Status.OnStateBreak(ANIMATIONSTATE.WALKTURNRIGHT);
                    sdself.Value.PushState((int)ANIMATIONSTATE.WALKTURNRIGHT);
                break;

            case ResultType.SUCCESS:
                //sdself.Value.StopNav();
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
