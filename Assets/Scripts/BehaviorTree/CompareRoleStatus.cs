using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Common;

public class CompareRoleStatus : Conditional
{	
    public SharedRole sdself;
	public override TaskStatus OnUpdate()
	{
        ANIMATIONSTATE st = sdself.Value.Status.State;
		if(st == ANIMATIONSTATE.IDLE)
        	return TaskStatus.Success;
		else
            return TaskStatus.Failure;
    }
}