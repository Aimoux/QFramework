using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QF;

//职责：初始化、Tick Tree?? logic Manager?? GSM??
public class GamingManager : MonoSingleton <GamingManager> {

    public Transform CameraPos;
    public Transform PlayerModel;
    public Transform PlayerRoot;
    public Animator PlayerAnim;

    public override void OnSingletonInit()
    {
        base.OnSingletonInit();
    }
    public override void Dispose()
    {
        base.Dispose();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    void FixedUpdate()
    {
        //Tick tree??

    }

}
