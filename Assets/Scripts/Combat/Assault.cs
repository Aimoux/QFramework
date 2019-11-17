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
    public string StuntPath;

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
        // string fixedcomb = combo.Replace("\"", "");
        // fixedcomb = fixedcomb.Replace("/", "");
        string[] states = combo.Split('+');
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
    //仇恨来源:初见\伤害(anst or weapon 固有+lostHP)\第三方辅助\
    //仇恨清除,仇恨转移:
    //4、“目标更换事件”触发条件一：一旦怪物的仇恨表中开始有数据存在（即不为空），就会触发一次“目标更换事件”；
    //5、“目标更换事件”触发条件二：如果怪物或NPC仇恨列表中“当前战斗目标”和“当前最大仇恨值目标”不同，且“当前第一、第二仇恨值比值”>110%，则会触发“目标更换事件”；
    //6、“目标更换事件”触发条件三：一旦怪物或NPC仇恨列表中有一个目标仇恨值达到仇恨上限，即65535时，则会触发“目标更换事件”（注意：该事件会在“极限衰减”过程之前完成）；
    //7、“目标更换事件”触发条件四：怪物或NPC仇恨列表中，“当前战斗目标”的仇恨值发生衰减或清除事件（不管是因为什么原因），都会触发“目标更换事件”；
    //8、“目标更换事件”触发条件五：怪物或NPC在响应“呼救”和“召集”技能或者怪物在受到“愤怒嘲讽”技能时，会立刻触发一次“目标更换事件”（可以通过让技能携带一个具有“目标更换事件”触发功能的脚本来实现）。

    public virtual Role FindTarget(Role defaultTarget)
    {
        //嘲讽作用于仇恨值而不是buff??
        if (this.Data.TargetType == TargetType.MAXHATRED)
        {
            float hate = 0f;
            foreach(Role role in Caster.Hates.Keys)
            {
                if(Caster.Hates[role] > hate)
                {
                    hate = Caster.Hates[role];
                    Target = role;
                }
            }
        }
        else if (this.Data.TargetType == TargetType.TARGET)
        {
            if (defaultTarget != null)
            {
                this.Target = defaultTarget;
            }
            else
            {
                float dist = 0f;
                Target = Caster.FindClosest(ref dist);
            }
        }
        else if (this.Data.TargetType == TargetType.SELF)
        {
            this.Target = this.Caster;
        }
        else if (this.TargetSelector != null)
        {
            float max = -float.MaxValue;
            Role result = null;
            foreach (Role role in Logic.Instance.GetAliveEnemies(Data.TargetSide))
            {
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


