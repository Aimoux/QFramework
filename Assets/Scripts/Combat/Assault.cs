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

    }


    //仇恨值更换对象
    public virtual Role FindTarget(Role defaultTarget)
    {               
        return Caster.Target;
    }

    public virtual void Init()
    {
        foreach(int id in Data.Acts)
        {
            Caster.GetAnimStateById(id).Init();//??
        }
    }

    public virtual void Start(Role target)
    {
        Caster.PushState((int)Common.ANIMATIONSTATE.SINGLEATK1);
        foreach (int act in Actions)
        {
            Caster.PushState(act);
        }          

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


