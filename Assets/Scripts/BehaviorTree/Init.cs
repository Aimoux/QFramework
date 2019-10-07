using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class Init : Action
{
    public SharedAnimator sdanim;
//缩并到role中??
    public SharedNavAgent sdnav;
    public SharedTransform sdtarget;
    public SharedRole sdrole;

    public override void OnStart()
    {
        sdtarget.SetValue(GamingManager.Instance.PlayerRoot);
        sdanim.SetValue(GetComponent<Animator>());
        sdnav.SetValue(GetComponent<NavMeshAgent>());
        sdrole.SetValue(new Role(sdanim.Value));//logic manager 中初始化??

        //string mod = DataManager.Instance.Roles[1].Model;
        //int hp = DataManager.Instance.RoleAttributes[1].Dict[1].HP;
        int hp = DataManager.Instance.RoleAttributes[1][1].HP;
        Debug.LogError("hp=: " + hp);
       
    }

    public override TaskStatus OnUpdate()
    {
        this.Disabled = true;
        return TaskStatus.Success;

    }
}