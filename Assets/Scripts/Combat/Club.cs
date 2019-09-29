using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class Club : MonoBehaviour, IWeapon
{
    public bool isActive { get; private set; }//collider ȫ�� enable??
    public int Id;
    private WeaponData data;
    private Collider colid;
    private IWeapon weapon;

    public void Init(int id)
    {
        //datamanager.contain()
        Id = id;
        data = new GameData.WeaponData(id);//������Ϊ���ֵ�??
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

    //���罣�������˺����㷽����ͬ��
    //����һ��������Ӧ�ϲ�Ϊһ���ࡰ������������㼶�����ʣ�
    //���������������� public IWeapon weapon; weapon = CreateInstaceByType. Ȼ��weapon.OnHitTarget()��
    public float OnHitTarget()
    {
        return 0f;
    }

}
