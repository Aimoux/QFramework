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
    private AnimStateExtension walkst;//避免过多的new

    public override void OnStart()
    {
        walkst = new AnimStateExtension(sdself.Value, (int)ANIMATIONSTATE.WALKFORWARD);
    }
    public override TaskStatus OnUpdate()
    {
        Common.ResultType ret = sdself.Value.HasReachTarget();
        if(sdself.Value.Status.State == Common.ANIMATIONSTATE.IDLE)// || (sdself.Value.Status.State == Common.ANIMATIONSTATE.WALKFORWARD && ret == Common.ResultType.RUNNING))       
            sdself.Value.PushState(walkst);        

        if(ret == Common.ResultType.SUCCESS)
            return TaskStatus.Success;
        else if(ret == Common.ResultType.RUNNING)
            return TaskStatus.Running;
        else
            return TaskStatus.Failure;
    }

}