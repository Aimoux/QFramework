using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QF;
using GameData;

public class CombatManager : Singleton<CombatManager>
{
    //����
    //��������̽��������Ϣ�����������ܻ�����

    public delegate void OnHitEvent();//������
    public OnHitEvent OnHitEventHandler;


    public delegate void OnAttackEvent();
    public OnAttackEvent OnAttackEventHandler;


    public void ooop()
    {
        //Sadism and Masochism
        if (OnHitEventHandler != null)
            OnHitEventHandler();

        if (OnAttackEventHandler != null)
            OnAttackEventHandler();
    }
    
    protected CombatManager()
    {

    }

}
