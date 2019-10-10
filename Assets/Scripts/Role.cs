using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using Common;

//角色基类，继承Q的Entity??
//战斗即时数据，逻辑层注意与表现层分离??
public class Role
{
    public RoleAttributeData Attributes { get; set; }
    //NPC读表，玩家读配置?
    public int Level { get; set; }

    public int HP { get; set; }
    //实时HP
    public int Stamina { get; set; }
    //实时耐力
    public int Stimulation { get; set; }
    //实时兴奋
    public int Strength { get; set; }
    //降智打击及buff
    public int Dexterity { get; set; }

    public int Mental { get; set; }

    public int Steady { get; set; }
    //韧性（打断）
    public Vector3 Position { get; set; }

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

    public Role()
    {
        

    }

    public Role(Animator animator)
    {
        this.anim = animator;
        this.Status = new IdleState(this);//至于构造方法中，可选初始状态
        IdleSt = Status;
    }

    public Role(RoleAttributeData data)//NPC、玩家公用
    {
        this.Attributes = data;
        //        this.Level = this.Attributes.Level;
        //        this.HP = this.Attributes.HP;
        //        this.Stamina = this.Attributes.Stamina;
        //        this.Stimulation = this.Attributes.Stimulation;
        //        this.Strength = this.Attributes.Strength;
        //        this.Dexterity = this.Attributes.Dexterity;
        //        this.Mental = this.Attributes.Mental;

    }

    // public Role(bool om)//??
    // {
    // Attributes = new RoleAttributeData();
    // Attributes.Level = Player.Instance.Level;
    //Attributes.HP = Player.Instance.CfgHP;
    //Attributes.Stamina = Player.Instance.CfgStamina;
    // Attributes.Stimulation = Player.Instance.CfgStimulation;
    //Attributes.Strength = Player.Instance.CfgStrength;
    // Attributes.Dexterity = Player.Instance.CfgDexterity;
    // Attributes.Mental = Player.Instance.CfgMental;
    // }

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
            if(Cmds.Count ==0)
            {
                Cmds.Push(IdleSt);
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

    public void OnSadism(Role Masoch)//打到别人
    {
        AddSelfExite();
        float dmg = CalculateForce();
        if(Masoch !=null)//过耦合??
        {
            Masoch.OnMasoch(dmg, this);
        }
        //Pop HUD "I Got You!"

        //命中后武器增加特效??

        //另一种方法，在攻击开始帧，把武器的伤害数量与类型传递给sensor??


    }

    public float WeaponForce//武器中调用??
    {
        get
        {
            return CalculateForce();
        }
    }

    private float CalculateForce()//
    {
        //str dex fix
        //buff fix
        //weapon level

        return 0f;
    }

    private int AddSelfExite()
    {

        return 0;
    }

    //单次攻击多种伤害属性混合??附魔、充电
    public void OnMasoch(float dmg, Role Sadist)//被击中
    {
        //Pop HUD "How Dare You!"
        float loss = CalcuteInjury(dmg);


    }

    //Dictionary<int, float> weaponforce;
    //左剑右枪 仍然需要weapon 类的存在??
    //挥砍->投掷 武器切换 curweapon 的缺陷，同时挥出两把武器。 先投掷出的武器比挥砍命中的时间更晚

    private float CalcuteInjury(float dmg)
    {

        return 0f;
    }
}
