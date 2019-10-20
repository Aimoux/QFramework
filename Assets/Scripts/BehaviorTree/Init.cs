using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class Init : Action
{    
    public SharedRole sdself;

    public override void OnStart()
    {
        Debug.LogError("init task start: " + sdself.Value.Data.Name);



    }

    public override TaskStatus OnUpdate()
    {
        this.Disabled = true;
        return TaskStatus.Success;

    }
}