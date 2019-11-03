using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using Common;
using QF;

public class Weapon
{
    public WeaponData Data;
    public WeaponAttributeData Attrs;
    public Role Owner;
    public List<List<AnimState>> ComboLists = new List<List<AnimState>>();

    public Enchantium curEnchant;

    private Dictionary<DamageType, float> BaseForceDict = new Dictionary<DamageType, float>
    {
        { DamageType.BLUNT, 0f},
        { DamageType.ELECTRIC, 0f},
        { DamageType.FIRE, 0f},
        { DamageType.ICE, 0f},
        { DamageType.MAGIC, 0f},
        { DamageType.PIERCE, 0f},
        { DamageType.SLASH, 0f}

    };

    private Dictionary<DamageType, float> _ForceDict = new Dictionary<DamageType, float>();
    public Dictionary<DamageType, float> ForceDict
    {
        get
        {
            _ForceDict.Clear();
            foreach (var kv in BaseForceDict)
                _ForceDict.Add(kv.Key, kv.Value * Owner.Status.Data.AttackDamageRatio);

            Owner.CalculateForce(this.Data, _ForceDict);//�Ƿ���ѭ��??
            return _ForceDict;
        }
    }

    public Dictionary<RoleAttributeType, float> AttrDict;//�����˺� ����ʱ�� vs ������Ч ������

    public Weapon(int id, int level, Role owner)
    {
        Data = DataManager.Instance.Weapons[id];
        Attrs = DataManager.Instance.WeaponAttributes[id][level];

        BaseForceDict[DamageType.BLUNT] = Attrs.DamageBlunt;
        BaseForceDict[DamageType.ELECTRIC] = Attrs.DamageElectric;
        BaseForceDict[DamageType.FIRE] = Attrs.DamageFire;
        BaseForceDict[DamageType.ICE] = Attrs.DamageIce;
        BaseForceDict[DamageType.MAGIC] = Attrs.DamageMagic;
        BaseForceDict[DamageType.PIERCE] = Attrs.DamagePierce;
        BaseForceDict[DamageType.SLASH] = Attrs.DamageSlash;

        this.Owner = owner;

    }

    ~Weapon()
    {
        this.OnDestroy();
    }

    //avail combos for each weapon
    public void GetCombos()
    {
        AnimState IdleSt = new AnimStateExtension(Owner, 0);
        for (int i = 1; i <= Data.Combos.Count; i++)
        {
            string[] combos = Data.Combos[i].Split('-');//�Ƴ�ͷβ�ķ��š�/
            foreach (string combo in combos)
            {
                List<AnimState> ComboCmd = new List<AnimState>();
                string fixedcomb = combo.Replace("\"", "");
                fixedcomb = fixedcomb.Replace("/","");
                string[] states = fixedcomb.Split('+');      

                if(states.Length >1)//����ר��??
                    continue;

                foreach (string state in states)
                {
                    int id = System.Convert.ToInt32(state);
                    AnimState anst = new AnimStateExtension(Owner, id);
                    ComboCmd.Add(anst);
                }
                ComboCmd.Add(IdleSt);//combo������Idle��Ϊ����??
                ComboCmd.Reverse();
                ComboLists.Add(ComboCmd);//˳��ߵ���??
            }
        }

    }

    public AnimState CreateAnimStByName(string state)
    {
        return null;
    }

    public virtual void Enchant(Enchantium encho)//Ψһ��ħ�����߻�ȡ��ǰ��
    {
        Dechant();
        curEnchant = encho;
        EnchantEffect(encho);//change materil or shader??
        BaseForceDict[encho.type] += encho.exdmg;
    }

    public virtual void EnchantEffect(Enchantium encho)
    {


    }


    public virtual void Dechant()
    {
        if (curEnchant == null)
            return;

        ResoreEffect();
        float dmg = BaseForceDict[curEnchant.type];
        dmg -= curEnchant.exdmg;
        BaseForceDict[curEnchant.type] = dmg > 0 ? dmg : 0f;

        curEnchant = null;
    }

    public virtual void ResoreEffect()// or enchanteffect(null)??
    {


    }


    //��������֮����������ӳɼ�����ʵ�˺�??
    //buffϵͳ����ʱ����������ֱ�������˺����ı��˺�����
    //weapon ����hc�� talent
    //���ι������ܴ򵽶���,�ʲ���ΪWeapon����Masoch����??
    public virtual void OnHit(Role Masoch)//������Ч
    {



    }

    //weapon manager ��ί�л�ֱ�ӵ���??
    public virtual void Update(float deltaTime)//�������ж�, �Ƿ��б�Ҫ??
    {
        if(curEnchant != null)
        {
            curEnchant.duration -= deltaTime;
            if (curEnchant.duration <= 0)
                Dechant();
        }


    }

    public virtual void OnDestroy()//Ͷ����?? ������??
    {
        WeaponManager.Instance.Weapons.Remove(this);

    }

    public class Enchantium
    {
        public DamageType type;
        public float exdmg;
        public float duration;
        public string effect;

        public Enchantium(DamageType type, float dmg, float lifetime)
        {
            this.type = type;
            this.exdmg = dmg;
            this.duration = lifetime;

        }

    }
}
