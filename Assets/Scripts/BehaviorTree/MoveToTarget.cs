using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using GameData;
using Common;

public class MoveToTarget : Action
{
    public SharedAnimator sdanim;
    public SharedNavAgent sdnav;
    public SharedTransform sdtarget;
    public SharedRole sdrole;
    //public ResultType ret;

    private NavMeshPath path = new NavMeshPath();
    public float vertical = 1;
    public float MoveSpeedMultiplier = 1f;


    private WalkState walkst;//避免过多的new

    public override void OnStart()
    {
        StartNav();
        walkst = new WalkState(sdrole.Value );
    }

    public override TaskStatus OnUpdate()
    {
        if (sdtarget.Value == null)
        {
            Debug.Log(this.gameObject.name + " Move to Invalid Target: " + sdtarget.Value.name);
            return TaskStatus.Failure;
        }      
            
        float dist = (sdtarget.Value.position - transform.position).magnitude;
        //float arriveDist = Caster.Value.Radius + Target.Value.Radius;
        if (dist > 2f)//??
        {
            if (sdnav.Value.isStopped)
                StartNav();
           
            if (MoveToTargetByNav() != ResultType.Success)
                return TaskStatus.Failure;

            //Debug.Log(this.gameObject.name + " still moving to target: " + Target.Value.obj.gameObject.name);
            return TaskStatus.Running;
        }
        else
        {
            //UnityMethods.Instance.SetRoleAnimation(Caster.Value.anim, "CanAttack", AnimType.Bool, true);
            EndNav();
            return TaskStatus.Success;
        }

    }

    public void StartNav()
    {
        //SetRoleAnimation(role.anim, "Vertical", AnimType.Float, vertical);//每帧调用不合适？？
        //SetRoleAnimation(role.anim, "CanAttack", AnimType.Bool, false);//查询当前anim state 判定？？
        sdanim.Value.SetFloat(Common.CommonAnim.Vertical, 1f);
        sdnav.Value.enabled = true;
        sdnav.Value.isStopped = false;
        sdnav.Value.updatePosition = true;
        sdnav.Value.updateRotation = false;        
    }

    // 风险：start时路径存在，行进update过程中，路径不通？？
    public ResultType MoveToTargetByNav()
    {
        if (!NavMesh.CalculatePath(sdtarget.Value.position, transform.position, NavMesh.AllAreas, path))
            return ResultType.Failure;

        if (path.status != NavMeshPathStatus.PathComplete)
            return ResultType.Failure;

        //每帧new一个state不合理??
        if (sdrole.Value.Status != null && sdrole.Value.Status.State != Common.ANIMATIONSTATE.WALK)
        {
            sdrole.Value.SetState(walkst);//状态模式动画机管理??
        }
        sdnav.Value.SetDestination(sdtarget.Value.position);
        sdnav.Value.speed = (sdanim.Value.deltaPosition / Time.deltaTime).magnitude;//time interval??
        sdnav.Value.nextPosition = transform.position;//Animator Apply root motion
        Vector3 targetFWD = sdnav.Value.desiredVelocity;
        targetFWD = new Vector3(targetFWD.x, 0f, targetFWD.z).normalized;
        transform.forward = Vector3.Slerp(transform.forward, targetFWD, 0.8f);
        //nav.Warp(transform.position);//此语句导致desiredVelocity=0？？ 

        return ResultType.Success;

        //官方案例方案
        //nav.speed = (anim.deltaPosition / Time.deltaTime).magnitude;
        ////使此句有意义，需改变ApplyRootMotion？？
        //transform.position = nav.nextPosition;
        ////取消RigidBody件？？
        //transform.forward = anim.deltaRotation * transform.forward;
        ////瞬移nav，解决卡地形问题
        //nav.Warp(transform.position);

    }

    public void EndNav()
    {
        sdnav.Value.isStopped = true;

    }

    //施法期间，保持朝向目标
    //    public void UpdateRote(BTRoleData Caster, BTRoleData Target)
    //    {
    //        Vector3 targetFWD = Target.obj.transform.position - Caster.obj.transform.position;
    //        targetFWD = new Vector3(targetFWD.x, 0f, targetFWD.z).normalized;
    //        Caster.obj.transform.forward = Vector3.Slerp(Caster.obj.transform.forward, targetFWD, 0.8f);
    //    }

    //    public float ComputeDist(BTRoleData caster, BTRoleData target)
    //    {
    //        float dist = (caster.obj.transform.position - target.obj.transform.position).magnitude;
    //        return dist;
    //    }

}