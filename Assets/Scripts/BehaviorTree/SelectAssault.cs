using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Common;
using GameData;

public class SelectAssault : Action
{
    public SharedRole sdself;
	public override TaskStatus OnUpdate()
	{
        Assault ast = sdself.Value.SelectAssault();
		if(ast == null)
            return TaskStatus.Failure;
		else
            return TaskStatus.Success;

    }
}