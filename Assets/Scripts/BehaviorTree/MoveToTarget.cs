using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using GameData;
using Common;

public class MoveToTarget : Action
{
    public SharedRole sdtarget;
    public SharedRole sdself;
    //public ResultType ret;

    public override void OnStart()
    {


    }

    //walk idle loop pose VS on frame end push again VS check nav each frame VS check nav only at end/start
    public override TaskStatus OnUpdate()
    {
        Common.ResultType ret = sdself.Value.HasReachTarget( );
        if (sdself.Value.Status.State == Common.ANIMATIONSTATE.IDLE)// || (sdself.Value.Status.State == Common.ANIMATIONSTATE.WALKFORWARD && ret == Common.ResultType.RUNNING))       
        {
            switch (ret)
            {
                case ResultType.LEFTSIDE:
                    sdself.Value.PushState((int)ANIMATIONSTATE.WALKTURNLEFT);//if too larget speed then run turn?
                    //if only one turnleft. pre turn and later turn has same vec value is dangerous??


                    break;

                case ResultType.RIGHTSIDE:
                    sdself.Value.PushState((int)ANIMATIONSTATE.WALKTURNRIGHT);
                    break;

                case ResultType.RUNNING:
                    sdself.Value.PushState((int)ANIMATIONSTATE.WALKFORWARD);
                    break;

                case ResultType.TOONEAR:
                    sdself.Value.PushState((int)ANIMATIONSTATE.WALKBACK);
                    break;
            }

        }

        if(ret == Common.ResultType.SUCCESS)
            return TaskStatus.Success;
        else if(ret == Common.ResultType.RUNNING)
            return TaskStatus.Running;
        else
            return TaskStatus.Failure;
    }

}

public class MoveToTargetsss : Action
{
    public SharedRole sdtarget;
    public SharedRole sdself;
    public MoveToPosition move;
    public float distance;

    public override TaskStatus OnUpdate()
    {
        MoveToTargetDist();
        return TaskStatus.Failure;
    }

    private void MoveToTargetDist()
    {
        Vector3 dir = sdtarget.Value.Position - sdself.Value.Position;
        dir = new Vector3(dir.x, 0f, dir.z);
        if (Vector3.Dot(dir, dir) > distance)
            return;

        dir = dir.normalized;
        Vector3 pos = sdtarget.Value.Position + distance * dir;
        move.targetpos = pos;

    }

}

public class EvadeTarget : Action
{
    public SharedRole sdtarget;
    public SharedRole sdself;
    public MoveToPosition move;
    public float distance;

    public override TaskStatus OnUpdate()
    {
        EvadeTargetDist();
        return TaskStatus.Failure;
    }

    private void EvadeTargetDist()
    {
        Vector3 dir = sdself.Value.Position - sdtarget.Value.Position;
        dir = new Vector3(dir.x, 0f, dir.z);
        if (Vector3.Dot(dir, dir) > distance)
            return;

        dir = dir.normalized;
        Vector3 pos = sdtarget.Value.Position + distance * dir;
        move.targetpos = pos;
    }
}

//¶þÈË×ª = LookAt+WalkLeft/Right??
public class MoveToPosition: Action 
{
    public Vector3 targetpos;
    public SharedRole sdself;

    public override TaskStatus OnUpdate()
    {

        return TaskStatus.Failure;
    }

    //look into standard asset to find solution
    private TaskStatus MoveToPos()
    {
        sdself.Value.Controller.MoveToPosByNav(targetpos);

        Common.ResultType ret = sdself.Value.HasReachTarget();
        if (sdself.Value.Status.State == Common.ANIMATIONSTATE.IDLE)// || (sdself.Value.Status.State == Common.ANIMATIONSTATE.WALKFORWARD && ret == Common.ResultType.RUNNING))       
        {
            switch (ret)
            {
                case ResultType.LEFTSIDE:
                    sdself.Value.PushState((int)ANIMATIONSTATE.WALKTURNLEFT);//if too larget speed then run turn?
                    //if only one turnleft. pre turn and later turn has same vec value is dangerous??
                    break;

                case ResultType.RIGHTSIDE:
                    sdself.Value.PushState((int)ANIMATIONSTATE.WALKTURNRIGHT);
                    break;

                case ResultType.RUNNING:
                    sdself.Value.PushState((int)ANIMATIONSTATE.WALKFORWARD);
                    break;

                case ResultType.TOONEAR:
                    sdself.Value.PushState((int)ANIMATIONSTATE.WALKBACK);
                    break;
            }

        }

        if (ret == Common.ResultType.SUCCESS)
            return TaskStatus.Success;
        else if (ret == Common.ResultType.RUNNING)
            return TaskStatus.Running;
        else
            return TaskStatus.Failure;


    }

    public void RotateTo(Vector3 pos)
    {
        Vector3 v1 = pos - transform.position;
        v1.y = 0;
        float angle = Vector3.Angle(transform.forward, v1);
        if (angle < 0.1)
        {
            return;
        }
        Vector3 cross = Vector3.Cross(transform.forward, v1);
        //transform.Rotate(cross, Mathf.Min(turnSpeed, Mathf.Abs(angle)));
    }

}
