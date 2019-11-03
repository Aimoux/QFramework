using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class AttackTarget : Action
{
	public SharedRole sdtarget;
    public SharedRole sdself;
    //public ResultType ret;
    //private AttackLiteState litest;//避免过多的new
	public override void OnStart()
	{
        //litest = new AttackLiteState(sdself.Value);
    }
	public override TaskStatus OnUpdate()
	{
		//if(sdself.Value.Status != litest)
           //sdself.Value.PushState(litest);
		   
        return TaskStatus.Success;
	}
}