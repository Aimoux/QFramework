using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using Common;
using System.Linq;

//��ɫ���࣬�̳�Q��Entity??
//ս����ʱ���ݣ��߼���ע������ֲ����??
public class Role
{
    public RoleAttributeData Attributes { get; set; }//��ʼ����Ŀ�����ֵ���Ǽ�ʱ��ֵ
    public RoleAttributeData BaseAttributes { get; set; }//NPC������Ҷ�����?

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

    public WeaponData weapon { get; set; }
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

    //drop out stack
    private Stack<AnimState> Cmds = new Stack<AnimState>();
    private AnimState IdleSt;

    public Role()
    {


    }

    public Role(Animator animator)
    {
        this.anim = animator;
        this.Status = new IdleState(this);//���ڹ��췽���У���ѡ��ʼ״̬
        IdleSt = Status;
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

    //ͬ��ʹ��״̬ģʽ��������������װ??
    //WeaponState
    //ClothState


    //�������߼�����������������λ�ڴ˴�??
    public AnimState Status { get; private set; }
    //״̬���ģʽ
    private bool m_bRunBegin = false;//OnXXEnter�Ĵ����ж�

    //��������
    public void PushState(AnimState animState)
    {
        Cmds.Push(animState);
    }

    // �趨״̬������״̬��ջ��ת���ж�?? �����ж�??
    public void SetState(AnimState animState)
    {
        Debug.Log(anim.name + " SetState:" + animState.State.ToString());
        m_bRunBegin = false;

        anim.SetInteger(CommonAnim.StateID, (int)animState.State);
        // ֪ͨǰһ��State�Y��
        if (Status != null)
            Status.OnStateExit();

        // �趨
        Status = animState;
    }

    public delegate void EnableSensor();//�¼���ָ��֡������⡣ģ���ϵ�triggerʼ�չ�ѡ�������ô˴����õ�boolֵ??
    public delegate void DisableSnesor();

    //ÿ��Tick�����ã������δ���??
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

        if (Status.FrameCount <= 0)
        {
            //if cmds �Բ����ã���û�У����Զ�ת��idle??
            if (Cmds.Count == 0)
            {
                Cmds.Push(IdleSt);
            }

            while (Cmds.Count > 0)
            {
                //���ޱ�Ҫlock??
                AnimState next = Cmds.Pop();
                if (CanTrans(next))
                {
                    SetState(next);
                    Cmds.Clear();//��Ч��ֻ��һ��??
                    break;
                }
            }
        }

    }

    //���߱��ҩ����������õ�layer�Լ�AvatarMask
    //�ҩ��������UpBody�㣬�������Head��
    //Ϊ������ȸ��ӣ�anim.SetLayerWeight(lyId, wgt), lerp��ʵ���Ͽ����ڱ༭����ֱ��ָ��Up��Head��weightΪ1??

    //����״̬��??״̬ģʽ??
    //Idle, Move(?)������״̬��ת������Ҫ�ȴ�
    //��Idle������״̬��ת�������Ҫ�ȴ�
    //��������anystate �� has exit time������˴�
    public bool CanTrans(AnimState next)
    {
        switch (Status.State)//��ǰ
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

    //��ʼ������
    //��ȡ����??
    //��ʼ��״̬
    public virtual void Init()
    {


        InitAttributes();
    }

    //��ʼ������
    protected void InitAttributes()
    {
        //base
        if (BaseAttributes == null)
        {
            this.InitBaseAttributes();
            this.InitStarAddition();
            this.InitEquipAddition();
            //����
            //this.InitRunesAttribAddition();
            this.BaseAttributes = this.Attributes.Clone();
        }
        else
        {
            this.Attributes = this.BaseAttributes.Clone();
        }
        InitEquipAttributes();


    }

    //װ������
    protected void InitEquipAttributes()
    {



    }


    public virtual void OnSadism(Weapon wp, Role Masoch)//�򵽱���
    {
        //Pop HUD "I Got You!"
        AddSelfExite();



    }

    //����ʱ����������
    public virtual void CalculateForce(WeaponData data, Dictionary<DamageType, float> forceDict)
    {
        int strDmg = DataManager.Instance.WeaponAdds[data.AddStrLv][this.Strength].StrengthAdd;//Դ�������ĸ����˺�ֵ
        int dexDmg = DataManager.Instance.WeaponAdds[data.AddDexLv][this.Dexterity].DexterityAdd;//Դ������
        int mtlDmg = DataManager.Instance.WeaponAdds[data.AddMntLv][this.Mental].MentalAdd;//Դ����־

        if (forceDict[DamageType.BLUNT] > 0)//����������ʽ���ۻ��������������ݼӳ�
        {
            forceDict[DamageType.BLUNT] += (strDmg + dexDmg);
        }
        if (forceDict[DamageType.PIERCE] > 0)//�̻�
        {
            forceDict[DamageType.PIERCE] += (strDmg + dexDmg);
        }
        if (forceDict[DamageType.SLASH] > 0)//ն��
        {
            forceDict[DamageType.SLASH] += (strDmg + dexDmg);
        }
        if (forceDict[DamageType.ELECTRIC] > 0)//���������־�ӳ�
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

    protected virtual int AddSelfExite()
    {

        return 0;
    }

    //������    
    public virtual void OnMasoch(Weapon wp)
    {
        //Pop HUD "How Dare You!"
        CalculateInjury(wp.ForceDict);

        //�����۳�

        //���Լ���

        //���³��ֵ
        HatredDict[wp.Owner] += 10;
    }

    //��������̬Ӱ�죺����/����
    //�������˺��ֵ�Ӱ�죬����������Ч��Ӱ��
    //�ܹ��������⼼��Ӱ��
    protected virtual void CalculateInjury(Dictionary<DamageType, float> forceDict)
    {
        //����ֵ���˺���ʽ
        //��ʼ�˺�
        foreach (var kv in forceDict)
        {
            if (kv.Value > 0)
            {
                int type = (int)kv.Key;



            }
        }

        float lostHP = 0f;

        //����������̬��Ӱ�켰������۳�����������exte��


        //����Ч���Ĵ���



    }

    private Dictionary<Role, int> HatredDict = new Dictionary<Role, int>();

    //����Ϊ������
    //�������ܹ������ϸ�Ŀ�궪ʧ
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


}
