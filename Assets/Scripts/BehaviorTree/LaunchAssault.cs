using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using GameData;
using Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class LaunchAssault: Action
{
    //public SharedRole sdtarget;
    public SharedRole sdself;

    // public override void OnStart()
    // {
    //     asds = sdself.Value.Assaults.Keys.ToList();
    //     int id = Random.Range(0, asds.Count);
    //     AssaultExtension ast = sdself.Value.GetAssaultById(asds[id]);
    //     sdself.Value.UseAssault(ast, ast.Target);    

    // }

    public override TaskStatus OnUpdate()
    {
        sdself.Value.UseAssault(sdself.Value.CurAssault, sdself.Value.CurAssault.Target);
        return TaskStatus.Success;
    }

}
