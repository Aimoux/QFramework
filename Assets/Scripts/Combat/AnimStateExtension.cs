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
        //onChangeDirection = InitMethod(typeof(Action), "Role" + RoleID + "ChangeDirection") as Action;
        //onEnterFrame = InitMethod(typeof(Action<double>), "Role" + RoleID + "OnEnterFrame") as Action<double>;
        //onUseTalent = InitMethod(typeof(Action<Talent, Role>), "Role" + RoleID + "UseTalent") as Action<Talent, Role>;
        //onFindTarget = InitMethod(typeof(CALL_FINDTARGET), "Role" + RoleID + "FindTarget") as CALL_FINDTARGET;

    }

    //private Action<Role> onStart = null;
    //private Action<float, double> onEnterFramePrefixT = null;
    //private Action<double> onEnterFramePrefixA = null;
    //private Action onHitFrame = null;
    //void InitAllMethod()
    //{
    //    onStart = InitMethod(typeof(Action<Role>), PrefixT + name + "Start") as Action<Role>;
    //    onEnterFramePrefixT = InitMethod(typeof(Action<float, double>), PrefixT + name + "OnEnterFrame") as Action<float, double>;
    //    onEnterFramePrefixA = InitMethod(typeof(Action<double>), PrefixA + name + "OnEnterFrame") as Action<double>;
    //    onHitFrame = InitMethod(typeof(Action), PrefixT + name + "OnHitFrame") as Action;
    //}

    public static Delegate InitMethod(Type funcType, string funcName, Dictionary<string, MethodInfo> MethodMap, object firstArgv)
    {
        MethodInfo func = null;
        if (MethodMap.TryGetValue(funcName, out func))
        {
            return Delegate.CreateDelegate(funcType, firstArgv, func);
        }

        return null;
    }


    //public override void OnHitFrame()
    //{
    //    //string funcName = PrefixT + name + "OnHitFrame";
    //    //if (MethodMap.ContainsKey(funcName))
    //    //{
    //    //    MethodInfo func = MethodMap[funcName];
    //    //    func.Invoke(this, new object[] { });
    //    //}
    //    if (onHitFrame != null)
    //    {
    //        onHitFrame();
    //    }
    //    else
    //    {
    //        base.OnHitFrame();
    //    }
    //}

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

