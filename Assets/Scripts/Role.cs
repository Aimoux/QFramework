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

    //NPC������Ҷ�����?
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

    //��ʼ����Ŀ�����ֵ���Ǽ�ʱ��ֵ:����+װ��
    public RoleAttributeData Attributes
    {
        get;
        set;


    }

    public RoleController Controller;

    public int Index { get; set; }//����ΨһID,tick˳��??
    public int FactOrg { get; set; }//ԭ����
    public int Faction { get; set; }//��̬����
    public int HP { get; set; }
    //ʵʱHP
    public int Stamina { get; set; }
    //ʵʱ����
    public int MP { get; set; }
    //ʵʱ�˷�
    public int Strength { get; set; }
    //���Ǵ����buff
    public int Dexterity { get; set; }

    public int Mental { get; set; }

    public int Steady { get; set; }
    //���ԣ���ϣ�
    public Vector3 Position { get; set; }
    public Vector3 Forward { get; set; }

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

    public Hero hero;//�Ƿ��б�Ҫ??
    public bool IsPause;

    //��ȡ�浵(����װ��)
    public static Role Create(Hero hero)
    {
        return new Role(hero);
    }

    public Role(Hero hero)
    {
        this.hero = hero;
        ID = hero.ID;
        Level = hero.Level;
        GameObject model = new GameObject();//load instantiate
        Controller = model.GetComponent<RoleController>();
        Controller.tree.SetVariableValue("sdrole", this);//const//??
        Init();
    }

    public static Role Create(int id, int lv)
    {
        return new Role(id, lv);
    }

    public Role(int id, int lv)
    {
        this.ID = id;
        this.Level = lv;
    }

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
                Cmds.Push(IdleSt);//Idle�ɺϲ���walk(vertical =0)??
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
    //LoadScene->EnterLevel->Logic.InitHeroes->Role.Init



    public virtual void Init()
    {


        InitAttributes();
    }

    //��ʼ������
    protected void InitAttributes()
    {
        //base
        Attributes = BaseAttributes.Clone();
        InitEquipAttributes();

    }

    //װ������
    protected void InitEquipAttributes()
    {



    }


    public virtual void OnSadism(Weapon wp, Role Masoch)//�򵽱���
    {
        //Pop HUD "I Got You!"

        //���л���,��������,�ɵ���??
        float mpGain = wp.Data.MPAtkRecovery + Attributes.MPAtkRecovery;
        Remedy(RoleAttribute.MP, mpGain, this);
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

    //������    
    public virtual void OnMasoch(Weapon wp)
    {
        //Pop HUD "How Dare You!"
        CalculateLostHP(wp.ForceDict);

        //��ħ����
        float mpGain = Attributes.MPDmgRecovery;
        Remedy(RoleAttribute.MP, mpGain, this);

        //�����۳�

        //���Լ���

        //���³��ֵ
        HatredDict[wp.Owner] += 10;
    }

    //��������̬Ӱ�죺����/����
    //�������˺��ֵ�Ӱ�죬����������Ч��Ӱ��
    //�ܹ��������⼼��Ӱ��
    //��������HP??
    protected virtual float CalculateLostHP(Dictionary<DamageType, float> forceDict)
    {
        //���������ǻ���
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

        //�����������ǿ���,������������25%(���)
        //������this������,�������,�涨��˳��,�����ױ��
        float[] emtForces = new float[] { forceDict[DamageType.FIRE], forceDict[DamageType.ICE],
            forceDict[DamageType.ELECTRIC], forceDict[DamageType.MAGIC]};
        float[] emtResists = new float[] {Attributes.FireResistance, Attributes.IceResistance,
        Attributes.ElectricResistance, Attributes.MagicResistance };

        for (int i = 0; i < emtForces.Length; i++)
        {
            lostHP += emtForces[i] * 0.75f * (1f - emtResists[i]);
        }


        //����������̬��Ӱ�켰������۳�����������exte��
        //����Ч���Ĵ���


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

    //if(idle or move) ->FindTarget ->Move(Rotate) ->Atk {atk1, atk2, atk3}{gen random avail comb sets and push}
    //if(out range) ->Find->Move()->...cycle
    //can not rotate in atk motion(other than attached in anim clip)

    //behaviour pattern gen and select( supported by sufficient anim combo)

    //virtual behavior
    //override behav for each npc??

    //Atk Combo ids
    public ANIMATIONSTATE[] GenTactic()
    {
        //ÿ��������combo��ͬ,��ȡ��weapon����??
        ANIMATIONSTATE[] LightCombo = new ANIMATIONSTATE[]
        { ANIMATIONSTATE.ATTACKLITE, ANIMATIONSTATE.ATTACKLITE };

        ANIMATIONSTATE[] HybridCombo = new ANIMATIONSTATE[]
        {
            ANIMATIONSTATE.ATTACKLITE, ANIMATIONSTATE.ATTACKHEAVY
        };


        return LightCombo;
    }


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

        return null;
    }



}
