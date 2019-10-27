using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using Common;
using System.Linq;

//角色基类，继承Q的Entity??
//战斗即时数据，逻辑层注意与表现层分离??
public class Role
{
    public int ID { get; set; }
    private RoleData _Data;
    public RoleData Data
    {
        get
        {
            if (_Data == null)
                _Data = DataManager.Instance.Roles[ID].Clone();
            return _Data;
        }
    }

    private int _Level { get; set; }
    public int Level
    {
        get { return _Level; }
        set
        {
            if (value < Data.MinLevel)
                _Level = Data.MinLevel;
            else if (value > Data.MaxLevel)
                _Level = Data.MaxLevel;
            else
                _Level = value;
        }
    }

    //NPC读表，玩家读配置?
    private RoleAttributeData _BaseAttributes;
    public RoleAttributeData BaseAttributes
    {
        get
        {
            if (_BaseAttributes == null)
                _BaseAttributes = DataManager.Instance.RoleAttributes[ID][Level].Clone();

            return _BaseAttributes;
        }
    }

    //初始化后的开场数值，非即时数值:基础+装备
    public RoleAttributeData Attributes
    {
        get;
        set;


    }

    public RoleController Controller;

    public int Index { get; set; }//场景唯一ID,tick顺序??
    public int FactOrg { get; set; }//原阵容
    public int Faction { get; set; }//动态阵容
    public int HP { get; set; }
    //实时HP
    public int Stamina { get; set; }
    //实时耐力
    public int MP { get; set; }
    //实时兴奋
    public int Strength { get; set; }
    //降智打击及buff
    public int Dexterity { get; set; }

    public int Mental { get; set; }

    public float Steady { get; set; }//韧性（打断）

    public Vector3 Position
    {
        get
        {
            return Controller.position;
        }
        set
        {
            Controller.position = value;
        }
    }
    public Vector3 Forward
    {
        get
        {
            return Controller.forward;
        }
        set
        {
            Controller.forward = value;
        }
    }

    public WeaponData weapon { get; set; }
    public Weapon CurWeapon { get; set; }
    private Stack<AnimState> Cmds = new Stack<AnimState>();
    private AnimState IdleSt;
    public QF.Res.ResLoader Loader;

    public Hero hero;//是否有必要??
    public bool IsPause;
    public bool OnTactic//是否正在执行决策(决策打断或正常结束时,需要新的策略)
    {
        get { return Cmds.Count > 0; }//最后的动作一开始,就可以进行下一个决策??
    }

    //读取存档(队友装备)
    public static Role Create(Hero hero)
    {
        return new Role(hero);

    }

    public Role(Hero hero)
    {
        this.hero = hero;
        ID = hero.ID;
        Level = hero.Level;

        int wpid = hero.Weapons.First().Key;
        int wplv = hero.Weapons.First().Value;
        CurWeapon = new Weapon(wpid, wplv, this);
        IdleSt = new IdleState(this);
        Status = IdleSt;

        Loader = QF.Res.ResLoader.Allocate();
        //use preload async in manager to ensure sync here ??
        GameObject model = Loader.LoadSync<GameObject>(Data.Model);
        Controller = GameObject.Instantiate(model).GetComponent<RoleController>();
        Controller.role = this;
        Controller.Init();
        Init();
    }

    //同样使用状态模式来更换武器、服装??
    //WeaponState
    //ClothState
    public void SetWeapon(Weapon wp)
    {
        CurWeapon = wp;//create instance by name

    }

    public delegate void EnableSensor();//事件，指定帧触发检测。模型上的trigger始终勾选，但利用此处设置的bool值??
    public delegate void DisableSnesor();
    //动画机逻辑处理（攻击、条件）位于此处??
    public AnimState Status { get; private set; }
    //状态设计模式
    private bool m_bRunBegin = false;//OnXXEnter的触发判定

    public Role Target;

    //每个Tick被调用，入口尚未添加??
    private void StateUpdate()
    {
        // 通知新的State開始
        if (Status != null && m_bRunBegin == false)
        {
            Status.OnStateEnter();
            m_bRunBegin = true;
        }

        if (Status != null)
            Status.OnStateUpdate();

        if (Status.CurFrame >= Status.FrameCount || Status.IsBreak)//to add break cur status??
        {
            //if cmds 皆不可用（或没有），自动转向idle??
            if (Cmds.Count == 0)
            {
                //Cmds.Push(IdleSt);//Idle可合并入walk(vertical =0)??
                GenTactic();
            }

            while (Cmds.Count > 0)
            {
                //有无必要lock??
                AnimState next = Cmds.Pop();
                if (CanTrans(next))
                {
                    SetState(next);
                    //Cmds.Clear();//生效的只有一个??
                    break;
                }
            }
        }

    }

        //操作缓存
    public void PushState(AnimState animState)
    {
        Cmds.Push(animState);
    }

    // 设定状态，缓存状态堆栈，转换判定?? 连击判定??
    public void SetState(AnimState animState)
    {
        m_bRunBegin = false;

        //Controller.SetState(animState);//移至on state enter 中??
        // 通知前一个State結束
        if (Status != null)
            Status.OnStateExit();//避免重复??

        // 设定
        Status = animState;
    }


    //边走边嗑药、表情会运用到layer以及AvatarMask
    //嗑药动作放在UpBody层，表情放在Head层
    //为避免过度复杂，anim.SetLayerWeight(lyId, wgt), lerp。实际上可以在编辑器中直接指定Up及Head的weight为1??

    //手敲状态机??状态模式??
    //Idle, Move(?)到其他状态的转换不需要等待
    //非Idle到其他状态的转换大多需要等待
    //如能设置anystate 的 has exit time，则不需此处
    public bool CanTrans(AnimState next)
    {
        return true;
        switch (Status.State)//当前
        {
            case Common.ANIMATIONSTATE.ATTACKHEAVY:

                switch (next.State)
                {
                    case Common.ANIMATIONSTATE.ATTACKHEAVY:
                        return false;

                    default:
                        return false;

                }





        }
        return true;

    }

    //初始化属性
    //获取武器??
    //初始化状态
    //LoadScene->EnterLevel->Logic.InitHeroes->Role.Init



    public virtual void Init()
    {
        InitAttributes();
        InitWeapon();


    }

    //初始化属性
    protected void InitAttributes()
    {
        //base
        Attributes = BaseAttributes.Clone();
        InitEquipAttributes();
        HP = Attributes.HP;
        MP = Attributes.MP;
        Stamina = Attributes.Stamina;
        Strength = Attributes.Strength;
        Dexterity = Attributes.Dexterity;
        Mental = Attributes.Mental;
        Steady = Data.Steady;

    }

    //装备属性
    protected void InitEquipAttributes()
    {



    }

    protected void InitWeapon()//
    {





    }

//role驱动行为树更合适，行为树仅提供决策，大部分逻辑仍在role中完成??
    public virtual void Update(float dt)
    {
        StateUpdate();
        CurWeapon.Update(dt);
        //决策放在逻辑帧的最后??
        BehaviorDesigner.Runtime.BehaviorManager.instance.Tick(Controller.tree);

    }

    public virtual void OnSadism(Weapon wp, Role Masoch)//打到别人
    {
        //Pop HUD "I Got You!"

        //命中回蓝,不设上限,可叠加??
        float mpGain = wp.Data.MPAtkRecovery + Attributes.MPAtkRecovery;
        Remedy(RoleAttribute.MP, mpGain, this);
    }

    //命中时被武器调用
    public virtual void CalculateForce(WeaponData data, Dictionary<DamageType, float> forceDict)
    {
        int strDmg = DataManager.Instance.WeaponAdds[data.AddStrLv][this.Strength].StrengthAdd;//源于力量的附加伤害值
        int dexDmg = DataManager.Instance.WeaponAdds[data.AddDexLv][this.Dexterity].DexterityAdd;//源于敏捷
        int mtlDmg = DataManager.Instance.WeaponAdds[data.AddMntLv][this.Mental].MentalAdd;//源于意志

        if (forceDict[DamageType.BLUNT] > 0)//属性修正公式，钝击仅受力量、敏捷加成
        {
            forceDict[DamageType.BLUNT] += (strDmg + dexDmg);
        }
        if (forceDict[DamageType.PIERCE] > 0)//刺击
        {
            forceDict[DamageType.PIERCE] += (strDmg + dexDmg);
        }
        if (forceDict[DamageType.SLASH] > 0)//斩击
        {
            forceDict[DamageType.SLASH] += (strDmg + dexDmg);
        }
        if (forceDict[DamageType.ELECTRIC] > 0)//电击仅受意志加成
        {
            forceDict[DamageType.ELECTRIC] += mtlDmg;
        }
        if (forceDict[DamageType.FIRE] > 0)
        {
            forceDict[DamageType.FIRE] += mtlDmg;
        }
        if (forceDict[DamageType.ICE] > 0)
        {
            forceDict[DamageType.ICE] += mtlDmg;
        }

        if (forceDict[DamageType.MAGIC] > 0)
        {
            forceDict[DamageType.MAGIC] += mtlDmg;
        }

    }

    //计算削韧
    public virtual float CalculateImpact(Weapon wp)
    {
        //受击方韧性
        if(Status.Data.AnimationType != (int)ANIMATIONTYPE.ATTACK)//非攻击状态韧性为0
        {
            Steady = 0f;
        }

        //攻击方削韧
        AnimState atk = wp.Owner.Status;
        float force = wp.Data.ImpactDamage *atk.Data.AttackImpactRatio ;

        //韧性打空后恢复原值
        //每30s回满一次暂无必要??
        Steady -= force;
        if(Steady <= 0)
        {
            int type = DataManager.Instance.WeaponImpacts[wp.Data.Category][(float)atk.State];
            HitReaction(type);
        }    
        return force;
    }

//受击硬直处理
    public virtual void HitReaction(int impact)//受击需要分层状态机??
    {
        //如果硬直类似亚特大的带有交互性质，考虑用TimeLine??
        //暂不考虑自身状态对最终硬直的影响??
        Status.OnStateBreak((ANIMATIONSTATE)impact);

        AnimState BreakState;
        switch ((ANIMATIONSTATE)impact)//pool?
        {
            case ANIMATIONSTATE.BREAKLITE :
                BreakState = new BreakLiteState(this);
                break;

            case ANIMATIONSTATE.BREAKHEAVY :
                BreakState = new BreakHeavyState(this);
                break;

            default :
             BreakState = new BreakLiteState(this);
                break;
        }
        SetState(BreakState);//??
    }

    //被击中    
    public virtual void OnMasoch(Weapon wp)
    {
        //Pop HUD "How Dare You!"
        CalculateLostHP(wp.ForceDict);
        CalculateImpact(wp);//韧性计算

        //回魔计算
        float mpGain = Attributes.MPDmgRecovery;
        Remedy(RoleAttribute.MP, mpGain, this);

        //耐力扣除(仅限防御状态??)

  

        //更新仇恨值
        HatredDict[wp.Owner] += 10;
    }

    //受自身姿态影响：防御/背刺
    //受武器伤害字典影响，受武器特殊效果影响
    //受攻击方特殊技能影响
    //仅作用于HP??
    protected virtual float CalculateLostHP(Dictionary<DamageType, float> forceDict)
    {
        //物理攻击考虑护甲
        float physicalForce = forceDict[DamageType.BLUNT] + forceDict[DamageType.PIERCE] + forceDict[DamageType.SLASH];
        float amr = Attributes.Armor;
        float lostHP = 0f;
        if (amr >= 0f)
        {
            lostHP = physicalForce / (1f + 0.06f * physicalForce);
        }
        else
        {
            lostHP = physicalForce * (2f - 1f / (1f - 0.06f * amr));
        }

        //非物理攻击考虑抗性,天生基础抗性25%(配表)
        //考虑用this索引器,方便计算,规定好顺序,不轻易变更
        float[] emtForces = new float[] { forceDict[DamageType.FIRE], forceDict[DamageType.ICE],
            forceDict[DamageType.ELECTRIC], forceDict[DamageType.MAGIC]};
        float[] emtResists = new float[] {Attributes.FireResistance, Attributes.IceResistance,
        Attributes.ElectricResistance, Attributes.MagicResistance };

        for (int i = 0; i < emtForces.Length; i++)
        {
            lostHP += emtForces[i] * 0.75f * (1f - emtResists[i]);
        }


        //考虑自身姿态的影响及后果（扣除耐力，增加exte）
        //特殊效果的处理


        return 0f;
    }


    protected virtual float Remedy(RoleAttribute type, float force, Role from)
    {

        if (type == RoleAttribute.HP)
        {


        }
        else if (type == RoleAttribute.MP)
        {

        }


        return 0f;
    }

    private Dictionary<Role, int> HatredDict = new Dictionary<Role, int>();

    //被行为树调用
    //开场、受攻击、上个目标丢失
    public virtual Role FindTarget()
    {
        // Role target = null;
        // int hate = 0;
        // foreach (var kv in HatredDict)
        // {
        //     if (kv.Value > hate)
        //     {
        //         hate = kv.Value;
        //         target = kv.Key;
        //     }
        // }
        if(Target != null)
            return Target;

        foreach(Role role in Logic.Instance.GetAliveEnemies(Faction))
        {
            Target = role;//interface selector??
            return role;
        }

        return null;
    }

    //if(idle or move) ->FindTarget ->Move(Rotate) ->Atk {atk1, atk2, atk3}{gen random avail comb sets and push}
    //if(out range) ->Find->Move()->...cycle
    //can not rotate in atk motion(other than attached in anim clip)

    //behaviour pattern gen and select( supported by sufficient anim combo)

    //virtual behavior
    //override behav for each npc??

    //Atk Combo ids
    public virtual void GenTactic()
    {
        if(OnTactic)
            return;

        //random walkst dir
        if(Target != null)
        {
            if(HasReachTarget() == ResultType.RUNNING)
                Cmds.Push(new WalkState(this));
            else
            {
                Cmds.Push(new AttackLiteState(this));//每把武器的combo不同,读取自weapon配置??
                Cmds.Push(new AttackHeavyState(this));
            }
           
        }
    }


    public virtual bool MoveToTarget()
    {
        if(HasReachTarget() == ResultType.RUNNING )
        {
            Controller.StartNav(Target.Position);
            ResultType ret = Controller.MoveToTargetByNav(Target.Position);
            return false;
            //return ret == ResultType.SUCCESS;
        }
        else 
        {
            Controller.StopNav();
        }
         

        return true;

    }

    public ResultType HasReachTarget()
    {
         float dist = SquarePlanarDist(Target);
        float arriveDist = (Data.Radius + Target.Data.Radius) * (Data.Radius + Target.Data.Radius);
        if (dist > arriveDist)
        {
            return ResultType.RUNNING;
        }
        else if(dist < 0/3f)//DataManager.Instance.GlobalConfig.MinRange)
            return ResultType.TooNear;

        return ResultType.SUCCESS;

    }

    private float SquarePlanarDist(Role target)
    {
        if (target == null)
        {
            Debug.Log("null target for planar dist");
            return 0f;
        }

        float dist = (target.Position.x - Position.x) * (target.Position.x - Position.x) +
            (target.Position.z - Position.z) * (target.Position.z - Position.z);

        return dist;
    }

    public virtual Role GetMostHated()
    {

        return null;
    }

    public virtual void End(Role attacker)
    {

        this.Status = null;//应移植controller??
        Logic.Instance.RoleDead(this);


    }



}
