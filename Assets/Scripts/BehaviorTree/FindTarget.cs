using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Common;
using GameData;
using System.Collections;
using System.Collections.Generic;

public class FindTarget : Conditional
{
    //public SharedRole sdtarget;
    public SharedRole sdself;

    // public override void OnStart()
    // {


    // }

    public override TaskStatus OnUpdate()
    {
        // if (sdtarget.Value == null)
        // {
        //     float dist = 0f;
        //     sdtarget.Value = sdself.Value.FindClosest(ref dist);
        // }

        // if (sdtarget.Value == null)
        //     return TaskStatus.Failure;
        // else
        //     return TaskStatus.Success;

        float dist = 0f;
        Role target = sdself.Value.FindClosest(ref dist);

        if(target == null)
            return TaskStatus.Failure;
        else
            return TaskStatus.Success;

    }

}