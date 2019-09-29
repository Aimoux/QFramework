using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class Club : MonoBehaviour, IWeapon
{
    public bool isActive { get; private set; }//collider 全程 enable??
    public int Id;
    private WeaponData data;
    private Collider colid;
    private IWeapon weapon;

    public void Init(int id)
    {
        //datamanager.contain()
        Id = id;
        data = new GameData.WeaponData(id);//待修正为查字典??
        //weapon = ??

    }

    void Start()
    {
        colid = GetComponent<Collider>();
        if (colid == null)
            Debug.LogError("null colider: " + this.name);

        colid.isTrigger = true;
    }

    void OnTriggerEnter()
    {
        OnHitTarget();
        weapon.OnHitTarget();//??
    }

    //假如剑、棍的伤害计算方法相同。
    //方案一：剑、棍应合并为一个类“近”，（抽象层级不合适）
    //方案二：定义属性 public IWeapon weapon; weapon = CreateInstaceByType. 然后weapon.OnHitTarget()。
    public float OnHitTarget()
    {
        return 0f;
    }

}
