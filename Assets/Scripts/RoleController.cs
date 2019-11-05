using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Common;
using GameData;
using QF.Res;

//deal with unity objetcs
//合理的调用顺序层级:BT->Role->Controller??
//Role中使用GetComponent方式获取
public class RoleController : MonoBehaviour
{
    public Role role;
    public Animator anim;//在预制体中绑定??
    public NavMeshAgent nav;//预制体中绑定??
    public bool isGrounded { get { return IsOnGround(); } }//落地检测的应用场合??
    public UIFloatingBar floatingBar;
    public BehaviorTree tree;

    public Transform model;
    public Transform wpPos;
    public Transform wpModel;
    public Vector3 position
    {
        get
        {
            return model.position;
        }
        set
        {
            model.position = value;
        }

    }

    public Vector3 forward
    {
        get
        {
            return model.forward;
        }
        set
        {
            model.forward = value;
        }
    }

    private bool navigating = false;
    private NavMeshPath path;
    //private WalkState walk;
    public ResLoader loader;

    private void Awake() 
    {
        path = new NavMeshPath();
    }

    private  void Start() 
    {
        
    }

    private void OnDestroy()
    {
        loader.Recycle2Cache();
        loader = null;
    }

    public void Init()
    {
        loader = role.Loader;

        anim.runtimeAnimatorController = loader.LoadSync<RuntimeAnimatorController>(role.CurWeapon.Data.Animator);

        model.GetComponent<RoleSensor>().Self = role;

        if(wpModel == null)
        {
            GameObject wpsrc = loader.LoadSync<GameObject>(role.CurWeapon.Data.Model);
            GameObject wp= GameObject.Instantiate(wpsrc) as GameObject;          
            wpModel = wp.transform;
        }

        wpModel.SetParent(wpPos);
        wpModel.localPosition = Vector3.zero;
        wpModel.localRotation = Quaternion.identity;
        wpModel.localScale = Vector3.one;

        wpModel.GetComponent<HitSensor>().wp = role.CurWeapon;

//tree at last??
        tree.ExternalBehavior = loader.LoadSync<ExternalBehavior>(role.Data.Behavior);
        tree.SetVariableValue(Const.SharedSelf, role);

    }
    public void StartNav()
    {
        if (!navigating)
        {
            nav.enabled = true;
            nav.isStopped = false;
            nav.updatePosition = true;
            nav.updateRotation = false;
            navigating = true;
        }
    }

    public void StopNav()
    {
        nav.isStopped = true;
        navigating = false;
    }

    public Vector2 GetDirToPosbyNav(Vector3 pos, bool stop = true)
    {    
        Vector2 dir = Vector2.zero;
        if (!NavMesh.CalculatePath(transform.position, pos, NavMesh.AllAreas, path) 
            || path.status != NavMeshPathStatus.PathComplete)//与nav stop与否无关??
            return dir;//没有路径时返回何值合适??

        StartNav();//是否必要??
        nav.SetDestination(pos);
        dir = new Vector2(nav.desiredVelocity.x, nav.desiredVelocity.z).normalized;

        if (stop)
            StopNav();

        return dir;
    }

    //Player也可以利用此处??多人碰撞以及跨越障碍??
    //nav仅用于朝向修正,真实位移\朝向完全由动画决定ApplyRootMotion??
    public ResultType MoveToPosByNav(Vector3 pos)
    {
        if (!NavMesh.CalculatePath(transform.position, pos, NavMesh.AllAreas, path))
            return ResultType.FAILURE;

        if (path.status != NavMeshPathStatus.PathComplete)
            return ResultType.FAILURE;

        nav.SetDestination(pos);
        nav.speed = (anim.deltaPosition / Time.deltaTime).magnitude;//浮空判断??
        nav.nextPosition = transform.position;//Animator Apply root motion
        Vector3 targetFWD = nav.desiredVelocity;
        targetFWD = new Vector3(targetFWD.x, 0f, targetFWD.z).normalized;
        //transform.forward = Vector3.Slerp(transform.forward, targetFWD, 0.8f);//lookrotate?
        //nav.Warp(transform.position);//此语句导致desiredVelocity=0？？   

        //apply root motion下,仍然可以代码控制pos,rote,二者共同作用
        //跑动中确有必要修正模型朝向(前进动画中朝向不变,但目标速度可能有垂直于追方朝向的分量)
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetFWD, Vector3.up), Const.MaxRoteAngDelta);
        //role.Position = transform.position;//非移动状态下如何传递值,利用动画??
        //role.Forward = transform.forward;//受控/滞空 直接传递模型的朝向位置??

        return ResultType.SUCCESS;

        //官方案例方案
        //nav.speed = (anim.deltaPosition / Time.deltaTime).magnitude;
        ////使此句有意义，需改变ApplyRootMotion？？
        //transform.position = nav.nextPosition;
        ////取消RigidBody件？？
        //transform.forward = anim.deltaRotation * transform.forward;
        ////瞬移nav，解决卡地形问题
        //nav.Warp(transform.position);

    }

    public bool IsOnGround()
    {

        return true;
    }





    //直接调用BTMove 的can see target??
    public float AngleToTarget(Vector3 pos)
    {
        Vector2 from = new Vector2(forward.x, forward.z);
        Vector2 ToTarget = new Vector2(pos.x - position.x, pos.z - position.z);
        float angle = Vector2.SignedAngle(from, ToTarget);
        return angle;
    }

    //角度范围判定
    public bool WithInAngle (Vector3 pos)
    {
        return AngleToTarget(pos) < DataManager.Instance.GlobalConfig.ViewAngle;
    }

    public void SetState(AnimState state)
    {
        //stateId equal + trigger，可解决anystate，trans to self的问题??
        anim.SetInteger(Const.StateID, (int)state.State);
    }

}
