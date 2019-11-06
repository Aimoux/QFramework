using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using GameData;
using Common;


public class FindAssaultTarget : Action
{
    public SharedRole sdtarget;
    public SharedRole sdself;
    public override void OnStart()
    {

    }

    public override TaskStatus OnUpdate()
    {

        return TaskStatus.Success;
    }
}