using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using Common;
using System.Linq;
using UnityEngine.AI;//??待取消

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
            if (value > Data.MaxLevel)
                _Level = Data.MaxLevel;
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

    public int Steady { get; set; }
    //韧性（打断）
    public Vector3 Position { get; set; }
    public Vector3 Forward { get; set; }

    public WeaponData weapon { get; set; }
    //策略模式，更换武器，使用接口??
    //    public int StateID
    //    {
    //        get
    //        { 
    //            return anim.GetInteger(Common.CommonAnim.StateID);
    //        }
    //        set
    //        {
    //            anim.SetInteger(Common.CommonAnim.StateID, value);
    //        }
    //    }

    public Animator anim;

    //drop out stack
    private Stack<AnimState> Cmds = new Stack<AnimState>();
    private AnimState IdleSt;

    public Hero hero;

    //是否有必要??
    //RoleController
    //RoleJoint


    public Role()
    {


    }

    //涉及到Unity实体的逻辑全部移到RoleController??
    public Role(Animator animator)
    {
        this.anim = animator;
        this.Status = new IdleState(this);//至于构造方法中，可选初始状态
        IdleSt = Status;
    }

    public Role(int id, int lv)//NPC only, extra config: clothes ,accorsory, weapon
    {
        ID = id;
        Level = lv;

    }

    //同样使用状态模式来更换武器、服装??
    //WeaponState
    //ClothState


    //动画机逻辑处理（攻击、条件）位于此处??
    public AnimState Status { get; private set; }
    //状态设计模式
    private bool m_bRunBegin = false;//OnXXEnter的触发判定

    //操作缓存
    public void PushState(AnimState animState)
    {
        Cmds.Push(animState);
    }

    // 设定状态，缓存状态堆栈，转换判定?? 连击判定??
    public void SetState(AnimState animState)
    {
        Debug.Log(anim.name + " SetState:" + animState.State.ToString());
        m_bRunBegin = false;

        anim.SetInteger(CommonAnim.StateID, (int)animState.State);
        // 通知前一个State結束
        if (Status != null)
            Status.OnStateExit();

        // 设定
        Status = animState;
    }

    public delegate void EnableSensor();//事件，指定帧触发检测。模型上的trigger始终勾选，但利用此处设置的bool值??
    public delegate void DisableSnesor();

    //每个Tick被调用，入口尚未添加??
    public void StateUpdate()
    {
        // 通知新的State開始
        if (Status != null && m_bRunBegin == false)
        {
            Status.OnStateEnter();
            m_bRunBegin = true;
        }

        if (Status != null)
            Status.OnStateUpdate();

        if (Status.FrameCount <= 0)
        {
            //if cmds 皆不可用（或没有），自动转向idle??
            if (Cmds.Count == 0)
            {
                Cmds.Push(IdleSt);//Idle可合并入walk(vertical =0)??
            }

            while (Cmds.Count > 0)
            {
                //有无必要lock??
                AnimState next = Cmds.Pop();
                if (CanTrans(next))
                {
                    SetState(next);
                    Cmds.Clear();//生效的只有一个??
                    break;
                }
            }
        }

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
    }

    //初始化属性
    protected void InitAttributes()
    {
        //base
        Attributes = BaseAttributes.Clone();
        InitEquipAttributes();

    }

    //装备属性
    protected void InitEquipAttributes()
    {



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

    //被击中    
    public virtual void OnMasoch(Weapon wp)
    {
        //Pop HUD "How Dare You!"
        CalculateLostHP(wp.ForceDict);

        //回魔计算
        float mpGain = Attributes.MPDmgRecovery;
        Remedy(RoleAttribute.MP, mpGain, this);

        //耐力扣除

        //韧性计算

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
        Role target = null;
        int hate = 0;
        foreach (var kv in HatredDict)
        {
            if (kv.Value > hate)
            {
                hate = kv.Value;
                target = kv.Key;
            }
        }
        return target;
    }

    //if(idle or move) ->FindTarget ->Move(Rotate) ->Atk {atk1, atk2, atk3}{gen random avail comb sets and push}
    //if(out range) ->Find->Move()->...cycle
    //can not rotate in atk motion(other than attached in anim clip)

    //behaviour pattern gen and select( supported by sufficient anim combo)

    //virtual behavior
    //override behav for each npc??

    //Atk Combo ids
    public ANIMATIONSTATE[] GenTactic()
    {
        //每把武器的combo不同,读取自weapon配置??
        ANIMATIONSTATE[] LightCombo = new ANIMATIONSTATE[]
        { ANIMATIONSTATE.ATTACKLITE, ANIMATIONSTATE.ATTACKLITE };

        ANIMATIONSTATE[] HybridCombo = new ANIMATIONSTATE[]
        {
            ANIMATIONSTATE.ATTACKLITE, ANIMATIONSTATE.ATTACKHEAVY
        };


        return LightCombo;
    }

    //update rotation??
    public RoleController Controller;
    public virtual bool MoveToTarget(Role target)
    {
        float dist = SquarePlanarDist(target);
        float arriveDist = (Data.Radius + target.Data.Radius) * (Data.Radius + target.Data.Radius);
        if (dist > arriveDist)
        {
            Controller.StartNav(target.Position);
            ResultType ret = Controller.MoveToTargetByNav(target.Position);
            return ret == ResultType.Success;
        }
        else
        {
            return true;
        }

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

        return new Role();
    }



}

//deal with unity objetcs
//合理的调用顺序层级??:BT->Role->Controller
public class RoleController : MonoBehaviour
{
    public Role role;
    public Animator anim;
    public NavMeshAgent nav;
    public bool IsGrounded { get { return IsOnGround(); } }//落地检测的应用场合??
    //public Vector3 pos;//??不直接取transform,而是通过逻辑计算:位移+速度时间积分??

    private bool navigating = false;
    private NavMeshPath path = new NavMeshPath();
    private WalkState walk;
    //start move
    public void StartNav(Vector3 pos)
    {
        if (!navigating)
        {
            nav.enabled = true;
            nav.isStopped = false;
            nav.updatePosition = true;
            nav.updateRotation = false;
            navigating = true;
        }

        if (walk == null)
            walk = new WalkState(role);

        if (role.Status != walk)
            role.PushState(walk);//受控不能移动的逻辑位于role的状态转换中??

    }

    public void StopNav()
    {
        nav.isStopped = true;
        navigating = false;
    }

    //Player也可以利用此处??多人碰撞以及跨越障碍??
    public ResultType MoveToTargetByNav(Vector3 pos)
    {
        if (!NavMesh.CalculatePath(transform.position, pos, NavMesh.AllAreas, path))
            return ResultType.Failure;

        if (path.status != NavMeshPathStatus.PathComplete)
            return ResultType.Failure;

        nav.SetDestination(pos);
        nav.speed = (anim.deltaPosition / Time.deltaTime).magnitude;//浮空判断??
        nav.nextPosition = transform.position;//Animator Apply root motion
        Vector3 targetFWD = nav.desiredVelocity;
        targetFWD = new Vector3(targetFWD.x, 0f, targetFWD.z).normalized;
        transform.forward = Vector3.Slerp(transform.forward, targetFWD, 0.8f);
        //nav.Warp(transform.position);//此语句导致desiredVelocity=0？？   

        role.Position = transform.position;//非移动状态下如何传递值,利用动画??
        role.Forward = transform.forward;//受控/滞空 直接传递模型的朝向位置??
        //transform.forward = anim.deltaRotation * transform.forward;??

        return ResultType.Success;

        //官方案例方案
        //nav.speed = (anim.deltaPosition / Time.deltaTime).magnitude;
        ////使此句有意义，需改变ApplyRootMotion？？
        //transform.position = nav.nextPosition;
        ////取消RigidBody件？？
        //transform.forward = anim.deltaRotation * transform.forward;
        ////瞬移nav，解决卡地形问题
        //nav.Warp(transform.position);

    }

    public bool IsOnGround()
    {

        return true;
    }

}