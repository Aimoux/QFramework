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

    public Enchantium curEnchant;

    private Dictionary<DamageType, float> _forceDict = new Dictionary<DamageType, float>
    {
        { DamageType.BLUNT, 0f},
        { DamageType.ELECTRIC, 0f},
        { DamageType.FIRE, 0f},
        { DamageType.ICE, 0f},
        { DamageType.MAGIC, 0f},
        { DamageType.PIERCE, 0f},
        { DamageType.SLASH, 0f}

    };


    public Dictionary<DamageType, float> ForceDict//伤害类型、伤害数值
    {
        get
        {
            CalculateForce();
            return this.ForceDict;

        }
    }

    public Dictionary<RoleAttributeType, float> AttrDict;//属性伤害 持续时间 vs 概率生效 永久性

    public Weapon(int id, int level, Role owner)
    {
        Data = DataManager.Instance.Weapons[id];
        if (!DataManager.Instance.Weapons.TryGetValue(id, out Data))
            Debug.LogError("wrong weapon id: " + id);

        Attrs = DataManager.Instance.WeaponAttributes[id][level];

        _forceDict[DamageType.BLUNT] = Attrs.DamageBlunt;
        _forceDict[DamageType.ELECTRIC] = Attrs.DamageElectric;
        _forceDict[DamageType.FIRE] = Attrs.DamageFire;
        _forceDict[DamageType.ICE] = Attrs.DamageIce;
        _forceDict[DamageType.MAGIC] = Attrs.DamageIce;
        _forceDict[DamageType.PIERCE] = Attrs.DamagePierce;
        _forceDict[DamageType.SLASH] = Attrs.DamageSlash;

        this.Owner = owner;

    }

    public virtual void Enchant(Enchantium encho)//唯一附魔，后者会取代前者
    {
        Dechant();
        curEnchant = encho;
        EnchantEffect(encho);//change materil or shader??
        _forceDict[encho.type] += encho.exdmg;
    }

    public virtual void EnchantEffect(Enchantium encho)
    {


    }


    public virtual void Dechant()
    {
        if (curEnchant == null)
            return;

        ResoreEffect();
        float dmg = _forceDict[curEnchant.type];
        dmg -= curEnchant.exdmg;
        _forceDict[curEnchant.type] = dmg > 0 ? dmg : 0f;

        curEnchant = null;
    }

    public virtual void ResoreEffect()// or enchanteffect(null)??
    {


    }


    public virtual void CalculateForce()
    {




    }

    //武器到手之后根据力量加成计算真实伤害??
    //buff系统，暂时增加力量、直接增加伤害、改变伤害类型
    //weapon 类似hc的 talent
    public virtual void OnHit()//命中特效
    {


    }

    //weapon manager 中委托或直接调用??
    public virtual void Update(float deltaTime)//持续性判定, 是否有必要??
    {
        if(curEnchant != null)
        {
            curEnchant.duration -= deltaTime;
            if (curEnchant.duration <= 0)
                Dechant();
        }


    }

    public virtual void OnDestroy()//投掷类?? 被换下??
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
