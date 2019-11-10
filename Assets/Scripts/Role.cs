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

    public delegate void EnableSensor();//事件，指定帧触发检测。模型上的trigger始终勾选，但利用此处设置的bool值??
    public delegate void DisableSnesor();
    //动画机逻辑处理（攻击、条件）位于此处??
    public AnimState Status { get; private set; }
    //状态设计模式
    private bool m_bRunBegin = false;//OnXXEnter的触发判定

    public Role Target;

    public Weapon CurWeapon { get; set; }
    public Stack<AnimState> Cmds = new Stack<AnimState>();
    private Dictionary<int, AnimState> States = new Dictionary<int, AnimState>();
    public Dictionary<int, Assault> Assaults = new Dictionary<int, Assault>();//talents
    public Assault CurAssault;
    public QF.Res.ResLoader Loader;

    public Hero hero;//是否有必要??
    public bool IsPause;
    public bool IsDead;
    public bool OnTactic//是否正在执行决策(决策打断或正常结束时,需要新的策略)
    {
        get { return Cmds.Count > 0; }//最后的动作一开始,就可以进行下一个决策??
    }

    public Dictionary<Role, float> Hates = new Dictionary<Role, float>();//此人的仇恨表
    public List<Role> HatedBy = new List<Role>();//仇恨此人的列表

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
        

        Loader = QF.Res.ResLoader.Allocate();
        //use preload async in manager to ensure sync here ??
        GameObject model = Loader.LoadSync<GameObject>(Data.Model);
        Controller = GameObject.Instantiate(model).GetComponent<RoleController>();
        Controller.role = this;
        Controller.Init();
        Init();
    }

    //初始化属性
    //获取武器??
    //初始化状态
    //LoadScene->EnterLevel->Logic.InitHeroes->Role.Init
    public virtual void Init()
    {
        InitAttributes();
        InitWeapon();
        InitAssault();

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
        Status = PushState(0);
        //CurWeapon.GetCombos();
    }

    public void InitAssault()
    {
        Assaults.Clear();
        
        foreach(int id in CurWeapon.Data.Actions.Values)
        {
            InitAssault(id);
        }   

    }

    public void InitAssault(int id)
    {
        AssaultExtension assault = new AssaultExtension(id, this);
        Assaults[id] = assault;
    }

    public AssaultExtension GetAssaultById(int id)
    {
        Assault ast;
        if (!Assaults.TryGetValue(id, out ast))
        {
            Debug.LogError("has no assault: " + id);
        }
        return (AssaultExtension)ast;
    }

    private List<int> tpids = new List<int>();
    public Assault SelectAssault()
    {
        tpids = Assaults.Keys.ToList();
        int id = Random.Range(0, Assaults.Count);
        AssaultExtension ast = GetAssaultById(tpids[id]);
        CurAssault = ast;
        return ast;
    }

    public void UseAssault(Assault ast, Role target)
    {
        Cmds.Clear();
        ast.Start(target);
        //CurAssault = ast;
    }

    //同样使用状态模式来更换武器、服装??
    //WeaponState
    //ClothState
    public void SetWeapon(Weapon wp)
    {
        CurWeapon = wp;//create instance by name

    }

    //always no null??
    public AnimStateExtension GetAnimStateById(int id)
    {
        AnimState st;
        if (!States.TryGetValue(id, out st))
        {
            st = new AnimStateExtension(this, id);
            States[id] = st;
        }
        return (AnimStateExtension)st;
    }

    public AnimStateExtension PushState(int id)
    {
        AnimStateExtension st = GetAnimStateById(id);
        Cmds.Push(st);
        return st;
    }

//role驱动行为树更合适，行为树仅提供决策，大部分逻辑仍在role中完成??
    public virtual void Update(float dt)
    {
        if(IsDead )
            return;

        StateUpdate();
        CurWeapon.Update(dt);
        BehaviorDesigner.Runtime.BehaviorManager.instance.Tick(Controller.tree);//决策放在逻辑帧的最后??
        //应该在Tick中call update，而不是update call tick??
    }

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

        if (Cmds.Count > 0)
        {
            AnimState next = Cmds.Peek();
            if (Status.CanTransit(next) == 2)//无需完整播放当前动作片段
            {
                SetState(Cmds.Pop());
                return;
            }
        }

        //不需要完整播放完的动画（idle，walk）设置其framecount为1??
        //can transit=2, 可以去掉break?
        if (Status.CurFrame >= Status.FrameCount)// || Status.IsBreak)
        {
            //if cmds 皆不可用（或没有），自动转向idle??
            if (Cmds.Count == 0)//motion除外?
            {
                PushState(0);//Idle可合并入walk(vertical =0)??
            }

            while (Cmds.Count > 0)
            {
                //有无必要lock??
                AnimState next = Cmds.Pop();
                if (Status.CanTransit(next) > 0)
                {
                    SetState(next);
                    break;
                }
            }
        }

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
            int type = DataManager.Instance.WeaponImpacts[wp.Data.Category][(int)atk.State];
            OnStatusBreak(type);
        }    
        return force;
    }

    //受击硬直处理,push 即可，不再需要break??
    public virtual void OnStatusBreak(int impact)//受击需要分层状态机??
    {
        //打断之后,立即clear cmd,但是gen tact应在break state结束时进行?
        Cmds.Clear();//打断一个,整个策略放弃??
        Steady = Data.Steady;//重置韧性
        //如果硬直类似亚特大的带有交互性质，考虑用TimeLine??
        //暂不考虑自身状态对最终硬直的影响??
        PushState(impact);
    }

    //当前动作被打断（比如受攻击）
    //比较当前动作与state的优先级
    //默认attack可以打断walk, Walk不可打断HitReaction??
    //与transit 0,1,2的关联??
    //不应包含push
    public virtual bool OnStateBreak(ANIMATIONSTATE state, bool forceBreak = false)
    {
        bool can = true;
        if(can)
        {
            //OnStateBreak();
            PushState((int)state);
        }
        return can;
    }

    //被击中    
    public virtual void OnMasoch(Weapon wp)
    {
        //Pop HUD "How Dare You!"
        float lost  = CalculateLostHP(wp.ForceDict);
        HP -= (int)lost;
        if(HP <=0)
        {
            End(wp.Owner);
            return;
        }
            

        CalculateImpact(wp);//韧性计算

        //回魔计算
        float mpGain = Attributes.MPDmgRecovery;
        Remedy(RoleAttribute.MP, mpGain, this);

        //耐力扣除(仅限防御状态??)

        //更新仇恨值
        //Hates[wp.Owner] += lost;
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
        return lostHP;
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

    //仇恨来源:初见\伤害(anst or weapon 固有+lostHP)\第三方辅助\
    //仇恨清除,仇恨转移:
    //4、“目标更换事件”触发条件一：一旦怪物的仇恨表中开始有数据存在（即不为空），就会触发一次“目标更换事件”；
    //5、“目标更换事件”触发条件二：如果怪物或NPC仇恨列表中“当前战斗目标”和“当前最大仇恨值目标”不同，且“当前第一、第二仇恨值比值”>110%，则会触发“目标更换事件”；
    //6、“目标更换事件”触发条件三：一旦怪物或NPC仇恨列表中有一个目标仇恨值达到仇恨上限，即65535时，则会触发“目标更换事件”（注意：该事件会在“极限衰减”过程之前完成）；
    //7、“目标更换事件”触发条件四：怪物或NPC仇恨列表中，“当前战斗目标”的仇恨值发生衰减或清除事件（不管是因为什么原因），都会触发“目标更换事件”；
    //8、“目标更换事件”触发条件五：怪物或NPC在响应“呼救”和“召集”技能或者怪物在受到“愤怒嘲讽”技能时，会立刻触发一次“目标更换事件”（可以通过让技能携带一个具有“目标更换事件”触发功能的脚本来实现）。

    //默认攻击目标(最近), assault中默认找最仇恨??
    public virtual Role FindClosest(ref float sqrDist)
    {
        sqrDist = float.MaxValue;
        Role near = null;
        foreach(Role role in Logic.Instance.GetAliveEnemies(Faction))
        {
            float sqr = SquarePlanarDist(role);
            if (sqr < sqrDist)
            {
                near = role;
                sqrDist = sqr;
            }
        }
        Target = near;
        return near; 
    }

    //碰撞检定优先于朝向检定优先于距离检定
    //Dist为最短距离,非两圆心距离
    public ResultType IsTargetWithinRange(Role target, float MaxRange, float MinRange, float Angle)//可用于技能攻击长度范围,以及角度范围
    {
        float dist = SquarePlanarDist(target);//质心距离
        float arriveDist = (Data.Radius + MaxRange + target.Data.Radius) * (Data.Radius + MaxRange + target.Data.Radius);

        Vector2 from = RoleUtil.To2DPlanarUnit(Forward);
        Vector2 to = new Vector2(target.Position.x - Position.x, target.Position.z - Position.z);//招式角度范围,与nav无关
        float angle = Vector2.SignedAngle(from, to);

        if (dist < MinRange * MinRange)//圆心距离
            return ResultType.TOONEAR;
        else if (angle > Angle * 0.5f)
            return ResultType.LEFTSIDE;
        else if (angle < -Angle * 0.5f)
            return ResultType.RIGHTSIDE;
        else if (dist > arriveDist)
            return ResultType.TOOFAR;
        else
            return ResultType.SUCCESS;
    }

    public ResultType MoveToTarget(Vector3 pos)
    {
        return Controller.MoveToPosByNav(pos);// later check path
    }

    public void WalkBack()//check if back is blocked??
    {


    }

    public void WalkTurnLeft()
    {

    }

    public void WalkTurnRight()
    {

    }

    public void StartNav()
    {
        Controller.StartNav();
    }

    public void StopNav()
    {
        Controller.StopNav();
    }

    //注意避免过头转身??过犹不及
    //tgtDir由nav给出,路线原因,并不一定是dest-position??
    //前往某地不可作为前往某物的子方法,二者有根本的区别.
    public ResultType MoveToPosition(Vector3 dest)
    {
        Vector3 targetDir = Controller.GetDirToPosbyNav(dest, false);
        if (targetDir == Vector3.zero)
            return ResultType.FAILURE;

        Vector3 todest = RoleUtil.To3DPlan(dest - Position);
        float dist = Vector3.Dot(todest, todest);

        Vector2 from = new Vector2(Forward.x, Forward.z);
        float angle = Vector2.SignedAngle(from, targetDir);

        //出口优先
        if (dist <= Const.ErrorArrive * Const.ErrorArrive)
            return ResultType.SUCCESS;
        else if (angle > Const.ErrorAngle)
            return ResultType.LEFTSIDE;
        else if (angle < -Const.ErrorAngle)
            return ResultType.RIGHTSIDE;
        else
            return ResultType.TOOFAR;
    }

    private float SquarePlanarDist(Role target)
    {
        if (target == null)
        {
            Debug.Log("null target for planar dist");
            return 0f;
        }

        Vector3 todest = RoleUtil.To3DPlan(target.Position - Position);
        float dist = Vector3.Dot(todest, todest);
        return dist;
    }

    public virtual void End(Role Murder)
    {
        Logic.Instance.RoleDead(this);
        Controller.tree.enabled = false;
        Debug.LogError("Died: " + Data.ID);
        PushState(4);
    }

}
