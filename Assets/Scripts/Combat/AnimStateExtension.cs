using UnityEngine;
using Common;
using GameData;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public class AnimStateExtension : AnimState 
{
    static Dictionary<string, MethodInfo> methodMap;
    static Dictionary<string, MethodInfo> MethodMap
    {
        get
        {
            if (methodMap == null)
            {
                methodMap = new Dictionary<string, MethodInfo>();
                MethodInfo[] ms = typeof(AnimStateExtension).GetMethods();
                foreach (MethodInfo m in ms)
                {
                    methodMap[m.Name] = m;
                }
            }
            return methodMap;
        }
    }
    public AnimStateExtension(Role Controller, int ID) : base(Controller, ID)
    {


    }

    //public Vector2 ExDirection = Vector2.zero;//转向每帧判定,不再需要,零向量与其他向量夹角恒为90??

    public override void OnStateEnter()
    {
        string funcName = "OnStateEnter" + ID;
        string orgFuncName = "OnStateEnter" + State;
        if (MethodMap.ContainsKey(funcName))
        {
            MethodInfo func = MethodMap[funcName];
            func.Invoke(this, new object[] { });
        }
        else if (MethodMap.ContainsKey(orgFuncName))
        {
            MethodInfo func = MethodMap[orgFuncName];
            func.Invoke(this, new object[] { });
        }
        else
            base.OnStateEnter();

    }

     public override void OnStateUpdate()
    {
        string funcName = "OnStateUpdate" + ID;
        string orgFuncName = "OnStateUpdate" + State;
        if (MethodMap.ContainsKey(funcName))
        {
            MethodInfo func = MethodMap[funcName];
            func.Invoke(this, new object[] { });
        }
        else if (MethodMap.ContainsKey(orgFuncName))
        {
            MethodInfo func = MethodMap[orgFuncName];
            func.Invoke(this, new object[] { });
        }
        else
            base.OnStateUpdate();
    }

     public override void OnStateExit()
    {
        string funcName = "OnStateExit" + ID;
        string orgFuncName = "OnStateExit" + State;
        if (MethodMap.ContainsKey(funcName))
        {
            MethodInfo func = MethodMap[funcName];
            func.Invoke(this, new object[] { });
        }
        else if (MethodMap.ContainsKey(orgFuncName))
        {
            MethodInfo func = MethodMap[orgFuncName];
            func.Invoke(this, new object[] { });
        }
        else
            base.OnStateExit();
    }

    public override int CanTransit(AnimState next)
    {
        string funcName = "CanTransit" + ID;
        string orgFuncName = "CanTransit" + (int)State;
        if (MethodMap.ContainsKey(funcName))
        {
            MethodInfo func = MethodMap[funcName];
             return (int)func.Invoke(this, new object[] {next });
        }
        else if (MethodMap.ContainsKey(orgFuncName))
        {
            MethodInfo func = MethodMap[orgFuncName];
            return (int)func.Invoke(this, new object[] {next });
        }
        else
            return base.CanTransit(next);
    }

    #region Walk    
    public void OnStateUpdate1011()
    {
        base.OnStateUpdate();
        m_Controller.MoveToTarget();     
    }

    public void OnStateExit1011()
    {
        base.OnStateExit();
        m_Controller.Controller.StopNav();
    }

    #endregion

    #region Turn
    public void OnStateUpdate1015()//walk turn left
    {
        //reach angle early exit
        //no rotate in walk f/bl/r
        //rotate in turn
        //rotate yes in atk/jump/  turn back if ahead of target
        //rotate no need to involve nav 
        //turn rotate no need to nav cal dirdection

        //need nav desired v to know turn to what dir and then move
        //matched with move to target bt task

        /// if state finish but dang still too large
        /// push another turn 

        //默认位移\朝向由动作驱动
        TurningUpdate(ResultType.LEFTSIDE);

    }

    public void OnStateExit1015()
    {
        m_Controller.Controller.StopNav();
        base.OnStateExit();
    }

    public void OnStateUpdate1016()
    {
        TurningUpdate(ResultType.RIGHTSIDE);
    }

    public void OnStateExit1016()
    {
        m_Controller.Controller.StopNav();
        base.OnStateExit();
    }

    public void TurningUpdate(ResultType condition)
    {
        Vector2 dir = m_Controller.Controller.GetDirToPosbyNav(Target.Position, false);
        if (dir == Vector2.zero)
            OnStateBreak();

        float arriveDist = Data.AttackRange * Data.AttackRange;
        ResultType ret = m_Controller.HasReachTarget(Target, dir, Const.CollisionRadius, Const.ErrorAngle, arriveDist);

        if (ret != condition)
            OnStateBreak();
    }

    //public void OnStateUpdate1025()
    //{
    //    TurningUpdate(ResultType.LEFTSIDE);
    //}

    //public void OnStateUpdate1026()
    //{
    //    TurningUpdate(ResultType.LEFTSIDE);
    //}


    #endregion

    #region Transit 改用n*n配表的方法实现??如何应用体力限制??
    // public bool CanTransit2011(AnimState next)//单攻一
    // {
    //     switch (next.State)
    //     {
    //         case ANIMATIONSTATE.IDLE :
    //             return true;
    //         default:
    //             return false;
    //     }
    // }

    // public bool CanTransit2012(AnimState next)//单攻二
    // {
    //     switch (next.State)
    //     {
    //         case ANIMATIONSTATE.IDLE :
    //             return true;
    //         default:
    //             return false;
    //     }
    // }

    //  public bool CanTransit2013(AnimState next)//单攻三
    // {
    //     switch (next.State)
    //     {
    //         case ANIMATIONSTATE.IDLE :
    //             return true;
    //         default:
    //             return false;
    //     }
    // }

    // public bool CanTransit20211(AnimState next)//一型二连击一段
    // {
    //     switch (next.State)
    //     {
    //         case ANIMATIONSTATE.IDLE :
    //         case ANIMATIONSTATE.TWOCOMBO1PART2:
    //             return true;

    //         default:
    //             return false;
    //     }
    // }

    // public bool CanTransit20212(AnimState next)//一型二连击二段
    // {
    //     switch (next.State)
    //     {
    //         case ANIMATIONSTATE.IDLE :
    //             return true;

    //         default:
    //             return false;
    //     }

    // }

    // public bool CanTransit20221(AnimState next)//二型二连击一段
    // {
    //     switch (next.State)
    //     {
    //         case ANIMATIONSTATE.IDLE :
    //         case ANIMATIONSTATE.TWOCOMBO2PART2:
    //             return true;
    //         default:
    //             return false;
    //     }
    // }

    // public bool CanTransit20222(AnimState next)//二型二连击二段
    // {
    //     switch (next.State)
    //     {
    //         case ANIMATIONSTATE.IDLE :
    //             return true;
    //         default:
    //             return false;
    //     }
    // }

    #endregion


    #region Weapon _1
    //导致必须为每一个ID,写OnStateUpdate101XXX,否则会调用最基础的OnStateUpdate
    public void OnStateUpdate101101()//前进加耐力,避免"001"被替换为"1"
    {
        OnStateUpdate1011();//simi base walk forward



    }

  

   



    #endregion


}

//really necessary??
//reference tactic tree build??
//core design is avoid damage by move\evade\block\counter,(do damage safely) otherwise two bots will keep beating to death like fool 
//self status analysis before target status analysis...(personality related aggressive or conservative)
public class Assult
{
    public Role Target;
    public Role Caster;

    public Role FindTarget(Role defaultTarget)
    {



        return null;
    }

    public void Start()
    {

    }

    //不走cd,基本无用
    //public void Update()
    //{
       


    //}

    //move this logic to bt tree task??
    //行为中的移动模块将达到目标作为可以使用assult的前提
    //private bool PreReached;
    //public void PreMove()//若一直无法移动到预定位置,应放弃此决策
    //{
    //    //pre move && post move
    //    ResultType ret = CheckPosition(Target.Position);
    //    switch (ret)
    //    {
    //        //case ResultType.TOOFAR:
    //    }
    //}

    ResultType CheckPosition(Vector3 pos)
    {

        return ResultType.SUCCESS;
    }


    public void Break()
    {


    }

    public void End()
    {


    }
}

public class AssaultData
{
    public int ID;
    public float MaxRange;//行为树节点中完成此条件
    public float MinRange;
    public float MaxAngle;
    public int TargetType;
    public int CastType;
    public List<int> Acts;
    public string Actions;



}



