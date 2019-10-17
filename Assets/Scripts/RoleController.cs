using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

//deal with unity objetcs
//����ĵ���˳��㼶??:BT->Role->Controller
//Role��ʹ��GetComponent��ʽ��ȡ
public class RoleController : MonoBehaviour
{
    public Role role;
    public Animator anim;//��Ԥ�����а�??
    public NavMeshAgent nav;//Ԥ�����а�??
    public bool isGrounded { get { return IsOnGround(); } }//��ؼ���Ӧ�ó���??
    public UIFloatingBar floatingBar;
    public BehaviorTree tree;

    public Transform model;
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
    private NavMeshPath path = new NavMeshPath();
    private WalkState walk;


    public void StartNav(Vector3 pos)
    {
        if (!navigating)
        {
            nav.enabled = true;
            nav.isStopped = false;
            nav.updatePosition = true;
            nav.updateRotation = false;
            navigating = true;
        }

        if (walk == null)
            walk = new WalkState(role);

        if (role.Status != walk)
            role.PushState(walk);//�ܿز����ƶ����߼�λ��role��״̬ת����??

    }

    public void StopNav()
    {
        nav.isStopped = true;
        navigating = false;
    }

    //PlayerҲ�������ô˴�??������ײ�Լ���Խ�ϰ�??
    public ResultType MoveToTargetByNav(Vector3 pos)
    {
        if (!NavMesh.CalculatePath(transform.position, pos, NavMesh.AllAreas, path))
            return ResultType.Failure;

        if (path.status != NavMeshPathStatus.PathComplete)
            return ResultType.Failure;

        nav.SetDestination(pos);
        nav.speed = (anim.deltaPosition / Time.deltaTime).magnitude;//�����ж�??
        nav.nextPosition = transform.position;//Animator Apply root motion
        Vector3 targetFWD = nav.desiredVelocity;
        targetFWD = new Vector3(targetFWD.x, 0f, targetFWD.z).normalized;
        transform.forward = Vector3.Slerp(transform.forward, targetFWD, 0.8f);
        //nav.Warp(transform.position);//����䵼��desiredVelocity=0����   

        role.Position = transform.position;//���ƶ�״̬����δ���ֵ,���ö���??
        role.Forward = transform.forward;//�ܿ�/�Ϳ� ֱ�Ӵ���ģ�͵ĳ���λ��??
        //transform.forward = anim.deltaRotation * transform.forward;??

        return ResultType.Success;

        //�ٷ���������
        //nav.speed = (anim.deltaPosition / Time.deltaTime).magnitude;
        ////ʹ�˾������壬��ı�ApplyRootMotion����
        //transform.position = nav.nextPosition;
        ////ȡ��RigidBody������
        //transform.forward = anim.deltaRotation * transform.forward;
        ////˲��nav���������������
        //nav.Warp(transform.position);

    }

    public bool IsOnGround()
    {

        return true;
    }

}
