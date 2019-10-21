using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class FindTarget : Conditional
{
    public SharedRole sdtarget;
    public SharedRole sdself;

	public override void OnStart()
    {
        

    }

	public override TaskStatus OnUpdate()
	{
		if(sdtarget.Value == null)
		{
			sdtarget.Value = sdself.Value.FindTarget();
		}

		if(sdtarget.Value == null)
            return TaskStatus.Failure;
		else
            return TaskStatus.Success;

	}
}