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

    public Dictionary<TargetType, ITargetSelector> TargetSelectors;

    public ITargetSelector TargetSelector
    {
        get { return this.TargetSelectors.ContainsKey((TargetType)Data.TargetType) ? TargetSelectors[(TargetType)Data.TargetType] : null; }
    }

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
        //Caster.PushState((int)Common.ANIMATIONSTATE.SINGLEATK1);
        foreach (int act in Actions)
        {
            Caster.PushState(act);
        }          
    }

    //target type selector
    //role find near, here type=0, find most hated
    public virtual Role FindTarget(Role defaultTarget)
    {
        //嘲讽作用于仇恨值而不是buff??
        if (this.Data.TargetType == TargetType.Target)// || (this.Caster.AbilityEffects.Taunt && (RoleSide)this.Data.AffectedSide == RoleSide.Enemy))
        {
            if (defaultTarget != null)
            {
                this.Target = defaultTarget;
            }
            else
            {
                float sqrDist = 0;
                Role target = this.Caster.FindClosest(ref sqrDist);
                if (sqrDist <= Data.MaxRange)
                {
                    this.Target = target;
                }
                else if (this.Caster.AbilityEffects.Taunt)  //修改放技能瞬间被战场操控者嘲讽，却攻击距离打不到战场操控者的bug
                {
                    this.Target = this.Caster.Target;
                }
                else
                {
                    this.Target = null; //这里应该是空的
                }
            }
        }
        else if (this.Data.TargetType == TargetType.Self)
        {
            this.Target = this.Caster;
        }
        //else if (this.Data.TargetType == TargetType.DeadBody)
        //{
        //    this.Target = null;
        //    foreach (Role role in Logic.Instance.Roles.Where(role => role.RoleST == 6 && role.IsDeadBody && role.AniTotaltime < DataManager.Numerics[59].Param))
        //    {
        //        Target = role;
        //    }
        //    return Target;
        //}
        else if (this.TargetSelector != null)
        {
            float max = -float.MaxValue;
            Role result = null;
            foreach (Role role in Logic.Instance.GetSurvivors((RoleSide)Data.TargetSide))
            {
                //if (Data.TargetClass == RoleType.Demon && !role.Config.IsDemon)
                //    continue;

                //if (Data.TargetClass == RoleType.Natural && role.Config.IsDemon)
                //    continue;

                //if (role.AbilityEffects.Void && !role.CheckInvisibility())
                //    continue;

                //if (this.Caster.AbilityEffects.Charm && role == this.Caster)
                //    continue;

                double distance = (role.Position - this.Caster.Position).magnitude;
                if (distance >= Data.MinRange && distance <= Data.MaxRange)
                {
                    float v = this.TargetSelector.Select(role);
                    if (max < v)
                    {
                        max = v;
                        result = role;
                    }
                }
            }
            Target = result;
        }
        else
            Debug.LogError(string.Format("target type error: {0}", this.Data.TargetType));

        return Target;
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


