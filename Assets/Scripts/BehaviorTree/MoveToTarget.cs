using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using GameData;
using Common;

public class MoveToTarget : Action
{
    public SharedRole sdtarget;
    public SharedRole sdself;
    //public ResultType ret;

    public override void OnStart()
    {


    }

    //walk idle loop pose VS on frame end push again VS check nav each frame VS check nav only at end/start
    public override TaskStatus OnUpdate()
    {
        Common.ResultType ret = sdself.Value.HasReachTarget( );
        if (sdself.Value.Status.State == Common.ANIMATIONSTATE.IDLE)// || (sdself.Value.Status.State == Common.ANIMATIONSTATE.WALKFORWARD && ret == Common.ResultType.RUNNING))       
        {
            switch (ret)
            {
                case ResultType.LEFTSIDE:
                    sdself.Value.PushState((int)ANIMATIONSTATE.WALKTURNLEFT);//if too larget speed then run turn?
                    //if only one turnleft. pre turn and later turn has same vec value is dangerous??


                    break;

                case ResultType.RIGHTSIDE:
                    sdself.Value.PushState((int)ANIMATIONSTATE.WALKTURNRIGHT);
                    break;

                case ResultType.RUNNING:
                    sdself.Value.PushState((int)ANIMATIONSTATE.WALKFORWARD);
                    break;

                case ResultType.TOONEAR:
                    sdself.Value.PushState((int)ANIMATIONSTATE.WALKBACK);
                    break;
            }

        }

        if(ret == Common.ResultType.SUCCESS)
            return TaskStatus.Success;
        else if(ret == Common.ResultType.RUNNING)
            return TaskStatus.Running;
        else
            return TaskStatus.Failure;
    }

}