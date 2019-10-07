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

    public WeaponData weapon{ get; set; }
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

    public Role()
    {
      

    }

    public Role (Animator animator)
    {
        this.anim = animator;
        this.Status = new IdleState(this);//至于构造方法中，可选初始状态
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

    //动画机逻辑处理（攻击、条件）位于此处??
    public  AnimState Status {get; private set;}
    //状态设计模式
    private bool m_bRunBegin = false;//OnXXEnter的触发判定

    // 设定状态
    public void SetState(AnimState animState)
    {
        Debug.Log (anim.name +" SetState:"+ animState.State.ToString());
        m_bRunBegin = false;

        // 载入场景
        if (anim != null)
        {
            anim.SetInteger(CommonAnim.StateID, (int)animState.State); 
        }
        else
            Debug.LogError("null animator");

        // 通知前一个State結束
        if (Status != null)
            Status.OnStateExit();

        // 设定
        Status = animState;  
    }

    //每个Tick被调用
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
    }
}
