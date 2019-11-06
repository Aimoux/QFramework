using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using GameData;
using Common;


using System.Collections;
using System.Collections.Generic;
using System.Linq;





public class MoveToTarget : Action
{
    public SharedRole sdtarget;
    public SharedRole sdself;
    public float MaxRange = Const.DefaultWeaponRange;//Ĭ�Ϲ�������,ս��ģ������Ϊ�����˺��뾶
    public float MinRange = Const.CollisionRadius;//Ĭ����ײ,��Ҫ����ģ�Ͱ뾶����??����ս����ʽ��(̫���˴򲻵�)
    public float MaxAngle = Const.ErrorAngle;//Ĭ�ϽǶȲ�,ս��ģ������Ϊ�����˺��Ƕ�
    public int Attempt;

    public override void OnStart()
    {
        //�ۼ�ʧ�ܴ���,��������??
        Attempt = 0;

    }

    public override TaskStatus OnUpdate()
    {
        ResultType ret = sdself.Value.IsTargetWithinRange(MaxRange, MinRange, MaxAngle);
        switch (ret)
        {
            case ResultType.TOOFAR:
                if (sdself.Value.Status.State == ANIMATIONSTATE.WALKFORWARD)
                {
                    //here better than WalkAnstUpdate??(walk to tar vs walk straight forward)
                    ResultType hasPth = sdself.Value.MoveToTarget(sdtarget.Value.Position);
                    if (hasPth != ResultType.SUCCESS)
                        sdself.Value.Status.OnStateBreak();
                }
                else
                {
                    sdself.Value.Status.OnStateBreak(ANIMATIONSTATE.WALKFORWARD);//�˴���ʵ�ִ��walk turn??
                    Attempt++;//when to quit tactic
                }
                break;

            case ResultType.TOONEAR:
                if (sdself.Value.Status.State == ANIMATIONSTATE.WALKBACK)
                {
                    sdself.Value.WalkBack();
                }
                else
                    sdself.Value.Status.OnStateBreak(ANIMATIONSTATE.WALKBACK);
                break;

            case ResultType.LEFTSIDE:
                if (sdself.Value.Status.State == ANIMATIONSTATE.WALKTURNLEFT)
                {
                    sdself.Value.WalkTurnLeft();
                }
                else
                    sdself.Value.Status.OnStateBreak(ANIMATIONSTATE.WALKTURNLEFT);
                break;

            case ResultType.RIGHTSIDE:
                if (sdself.Value.Status.State == ANIMATIONSTATE.WALKTURNRIGHT)
                {
                    sdself.Value.WalkTurnRight();
                }
                else
                    sdself.Value.Status.OnStateBreak(ANIMATIONSTATE.WALKTURNRIGHT);
                break;

            case ResultType.SUCCESS:
                sdself.Value.StopNav();
                break;
        }

        if (ret == Common.ResultType.SUCCESS)
            return TaskStatus.Success;
        else
            return TaskStatus.Failure;
    }
}

//����ת = LookAt+WalkLeft/Right??
public class MoveToPosition : Action
{
    public Vector3 targetpos;
    public SharedRole sdself;

    public override TaskStatus OnUpdate()
    {
        ResultType ret = sdself.Value.MoveToPosition(targetpos);
        switch (ret)
        {
            case ResultType.TOOFAR:
                if (sdself.Value.Status.State == ANIMATIONSTATE.WALKFORWARD)
                {
                    ResultType hasPath = sdself.Value.MoveToTarget(targetpos);
                    if (hasPath != ResultType.SUCCESS)
                        sdself.Value.Status.OnStateBreak();
                }
                else
                    sdself.Value.Status.OnStateBreak(ANIMATIONSTATE.WALKFORWARD);
                break;

            case ResultType.LEFTSIDE:
                if (sdself.Value.Status.State == ANIMATIONSTATE.WALKTURNLEFT)
                {
                    sdself.Value.WalkTurnLeft();
                }
                else
                    sdself.Value.Status.OnStateBreak(ANIMATIONSTATE.WALKTURNLEFT);
                break;

            case ResultType.RIGHTSIDE:
                if (sdself.Value.Status.State == ANIMATIONSTATE.WALKTURNRIGHT)
                {
                    sdself.Value.WalkTurnRight();
                }
                else
                    sdself.Value.Status.OnStateBreak(ANIMATIONSTATE.WALKTURNRIGHT);
                break;

            case ResultType.SUCCESS:
                sdself.Value.StopNav();
                break;

            case ResultType.FAILURE://no path??
                sdself.Value.Status.OnStateBreak();
                break;
        }

        if (ret == Common.ResultType.SUCCESS)
            return TaskStatus.Success;
        else
            return TaskStatus.Failure;
    }

}

public class FindAssaultTarget : Action
{
    public SharedRole sdtarget;
    public SharedRole sdself;
    public override void OnStart()
    {

    }

    public override TaskStatus OnUpdate()
    {

        return TaskStatus.Success;
    }
}



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
