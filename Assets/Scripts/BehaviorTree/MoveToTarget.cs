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
    private WalkState walkst;//避免过多的new

    public override void OnStart()
    {
        walkst = new WalkState(sdself.Value );
    }
    public override TaskStatus OnUpdate()
    {
        if(sdself.Value.Status.State != Common.ANIMATIONSTATE.WALKFORWARD)       
            sdself.Value.PushState(walkst);

        Common.ResultType ret = sdself.Value.HasReachTarget();

        if(ret == Common.ResultType.SUCCESS)
            return TaskStatus.Success;
        else if(ret == Common.ResultType.RUNNING)
            return TaskStatus.Running;
        else
            return TaskStatus.Failure;
    }

}