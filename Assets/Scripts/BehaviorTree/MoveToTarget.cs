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
        sdtarget.Value = sdself.Value.FindTarget();
        if (sdtarget.Value == null)
        {
            Debug.Log(this.gameObject.name + " has null target: ");
            return TaskStatus.Failure;
        }      
        else if(sdtarget.Value.Status.State != Common.ANIMATIONSTATE.WALK)       
            sdself.Value.PushState(walkst);

        return TaskStatus.Success;
    }

}