using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using GameData;

public class Assault
{
    public Role Target;
    public Role Caster;
    public int ID;
    public AssaultData Data;
    public List<int> Actions = new List<int>();

    public Assault(int id, Role caster)
    {        
        Caster = caster;
        Data = DataManager.Instance.Assaults[id];
        string combo = Data.Actions;
        string fixedcomb = combo.Replace("\"", "");
        fixedcomb = fixedcomb.Replace("/", "");
        string[] states = fixedcomb.Split('+');
        foreach (string state in states)
        {
            int actId = System.Convert.ToInt32(state);
            Actions.Add(actId);
        }
        Actions.Add(0);//combo必须以Idle作为结束??
        Actions.Reverse();//堆栈逆序
    }

    public virtual void Init()
    {
       


    }

    public virtual void Start(Role target)
    {
        Caster.PushState((int)Common.ANIMATIONSTATE.SINGLEATK1);
        foreach (int act in Actions)
        {
            Caster.PushState(act);
        }          

    }

    //仇恨值更换对象
    public virtual Role FindTarget(Role defaultTarget)
    {
        return Caster.Target;
    }

    //不走cd,基本无用
    public virtual void Update(float dt)
    {



    }

    public virtual void Break()
    {

        End();
    }

    public virtual void End()
    {


    }
}


