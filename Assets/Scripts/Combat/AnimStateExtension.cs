using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using GameData;
using System.Reflection;

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

