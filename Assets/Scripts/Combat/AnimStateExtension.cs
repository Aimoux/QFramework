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
    //只在前行中修正方向,左\右\后完全由动画决定??
    //public void OnStateUpdate1011()
    //{
    //    base.OnStateUpdate();
    //}

    // public void OnStateExit1011()
    // {
    //     base.OnStateExit();
    //     Caster.Controller.StopNav();
    // }

    #endregion

    #region Turn
    //public void OnStateUpdate1015()//walk turn left
    //{
    //    //默认位移\朝向由动作驱动
    //    base.OnStateUpdate();
    //}

    // public void OnStateExit1015()
    // {
    //     Caster.Controller.StopNav();
    //     base.OnStateExit();
    // }

    // public void OnStateExit1016()
    // {
    //     Caster.Controller.StopNav();
    //     base.OnStateExit();
    // }

    //public void OnStateUpdate1025()
    //{
    //    TurningUpdate(ResultType.LEFTSIDE);
    //}

    //public void OnStateUpdate1026()
    //{
    //    TurningUpdate(ResultType.LEFTSIDE);
    //}


    #endregion

    #region Transit 改用n*n配表的方法实现
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
    #endregion


    #region Weapon _1
    //导致必须为每一个ID,写OnStateUpdate101XXX,否则会调用最基础的OnStateUpdate
    public void OnStateUpdate101101()//前进加耐力,避免"001"被替换为"1"
    {
        base.OnStateUpdate();//simi base walk forward
    }
    #endregion


}

//really necessary??
//reference tactic tree build??
//core design is avoid damage by move\evade\block\counter,(do damage safely) otherwise two bots will keep beating to death like fool 
//self status analysis before target status analysis...(personality related aggressive or conservative)



