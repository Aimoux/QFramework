using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class HitTarget : Action
{
    public SharedRole sdrole;//wespons && curweapon??

    private AttackLiteState atkLiteSt;

    public override void OnStart()
	{
        atkLiteSt = new AttackLiteState(sdrole.Value);
	}

	public override TaskStatus OnUpdate()
	{
        sdrole.Value.PushState(atkLiteSt);//指令推送与指令处理分离??
		return TaskStatus.Success;
	}
}