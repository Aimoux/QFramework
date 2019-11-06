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
    public SharedRole sdtarget;
    public SharedRole sdself;
    private List<int> asds;

    public override void OnStart()
    {
        asds = sdself.Value.Assaults.Keys.ToList();
        int id = Random.Range(0, asds.Count);
        AssaultExtension ast = sdself.Value.GetAssaultById(asds[id]);
        sdself.Value.UseAssault(ast, sdtarget.Value);      

    }

    public override TaskStatus OnUpdate()
    {



        return TaskStatus.Success;
    }

}
