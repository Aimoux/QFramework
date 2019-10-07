using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class FindTarget : Conditional
{
	public override TaskStatus OnUpdate()
	{
		return TaskStatus.Success;
	}
}