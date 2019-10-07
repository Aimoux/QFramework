using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using Common;

//��ɫ���࣬�̳�Q��Entity??
//ս����ʱ���ݣ��߼���ע������ֲ����??
public class Role
{
    public RoleAttributeData Attributes { get; set; }
    //NPC������Ҷ�����?
    public int Level { get; set; }

    public int HP { get; set; }
    //ʵʱHP
    public int Stamina { get; set; }
    //ʵʱ����
    public int Stimulation { get; set; }
    //ʵʱ�˷�
    public int Strength { get; set; }
    //���Ǵ����buff
    public int Dexterity { get; set; }

    public int Mental { get; set; }

    public int Steady { get; set; }
    //���ԣ���ϣ�
    public Vector3 Position { get; set; }

    public WeaponData weapon{ get; set; }
    //����ģʽ������������ʹ�ýӿ�??
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
        this.Status = new IdleState(this);//���ڹ��췽���У���ѡ��ʼ״̬
    }

    public Role(RoleAttributeData data)//NPC����ҹ���
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

    //�������߼�����������������λ�ڴ˴�??
    public  AnimState Status {get; private set;}
    //״̬���ģʽ
    private bool m_bRunBegin = false;//OnXXEnter�Ĵ����ж�

    // �趨״̬
    public void SetState(AnimState animState)
    {
        Debug.Log (anim.name +" SetState:"+ animState.State.ToString());
        m_bRunBegin = false;

        // ���볡��
        if (anim != null)
        {
            anim.SetInteger(CommonAnim.StateID, (int)animState.State); 
        }
        else
            Debug.LogError("null animator");

        // ֪ͨǰһ��State�Y��
        if (Status != null)
            Status.OnStateExit();

        // �趨
        Status = animState;  
    }

    //ÿ��Tick������
    public void StateUpdate()
    {
        // ֪ͨ�µ�State�_ʼ
        if (Status != null && m_bRunBegin == false)
        {
            Status.OnStateEnter();
            m_bRunBegin = true;
        }

        if (Status != null)
            Status.OnStateUpdate();
    }
}
