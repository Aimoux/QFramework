using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QF;
using GameData;

public class CombatManager : Singleton<CombatManager>
{
    //单例
    //接收武器探测器的消息（攻击方、受击方）

    public delegate void OnHitEvent();//被击中
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
