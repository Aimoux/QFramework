using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QF;

//储存存档，序列化，操控按键配置
public class ConfigControll :Singleton<ConfigControll>
{
    [Header("===key settings===")]
    public string keyUp = "w";
    public string keyDown = "s";
    public string keyLeft = "a";
    public string keyRight = "d";
    public string keyRun = "caps lock";
    public string keyJump = "space";
    public string keyOperate = "e";
    public string keyFire = "f";

//    [Header("===Variables===")]
//    public float drUp;
//    public float drRight;
//    private float drUpVelocity;
//    private float drRightVelocity;
//    private float targetDrUp;
//    private float targetDrRight;
//    public bool retainAnim = false;
//    public float drMag;
//    public Vector3 drVec;
//    [Header("===ActionBool===")]
//    public bool run = false;
//    public bool jump = false;
//    public bool lastJump = false;
//    public bool openFire = false;
//    public bool operate = false;

    protected ConfigControll()
    {
    }


}
