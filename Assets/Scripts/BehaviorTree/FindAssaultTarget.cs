using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using GameData;
using Common;


public class FindAssaultTarget : Action
{
    //public SharedRole sdtarget;//??useless??
    public SharedRole sdself;
    public MoveToTarget move1;
    public MoveToTarget move2;
    //public SharedAssault sdassault;
    // public override void OnStart()
    // {
    //     sdtarget.Value = sdassault.Value.FindTarget(sdself.Value.Target);//special assault target(min hp)
    // }

    public override TaskStatus OnUpdate()
    {
        Role target = sdself.Value.CurAssault.FindTarget(sdself.Value.Target);
        
        move1.Target = target;
        move2.Target = target;

        if(target == null)
            return TaskStatus.Failure;
        else
        {
           
            return TaskStatus.Success;
        }
           
    }
}