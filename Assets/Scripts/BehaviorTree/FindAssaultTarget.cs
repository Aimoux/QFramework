using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using GameData;
using Common;


public class FindAssaultTarget : Action
{
    public SharedRole sdtarget;//??useless??
    public SharedRole sdself;
    public SharedAssault sdassault;
    public override void OnStart()
    {
        sdtarget.Value = sdassault.Value.FindTarget(sdself.Value.Target);//special assault target(min hp)
    }

    public override TaskStatus OnUpdate()
    {

        return TaskStatus.Success;
    }
}