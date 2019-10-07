//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;
//using UnityEngine.AI;
//
//public class UnityMethods : MonoBehaviour
//{
//    public static UnityMethods Instance;
//    public float constNum = 100f;
//    public GameObject UIRoot;
//    public GameObject PlayerBar;
//    public GameObject EnemyBar;
//
//    private void Awake()
//    {
//        Instance = this;
//        PlayerBar = Resources.Load("Prefabs/PlayerBar") as GameObject;
//        EnemyBar = Resources.Load("Prefabs/EnemyBar") as GameObject;
//
//    }
//
//    private void Start()
//    {
//        UIRoot = GameObject.Find("UI Root");       
//       
//        if (UIRoot == null || PlayerBar == null || EnemyBar == null)
//            Debug.LogError("null UI elements: " + this.gameObject.name);
//
//    }
//
//
//    //重载亦可？？
//    public void SetRoleAnimation(Animator anim, string name, AnimType type, object ret)
//    {
//        if (anim == null)
//            Debug.LogError("null anim try to set animation: " + this.gameObject.name);
//
//        switch (type)
//        {
//            case AnimType.Int:
//                anim.SetInteger(name, (int)ret);
//                break;
//
//            case AnimType.Float:
//                anim.SetFloat(name, (float)ret);
//                break;
//
//            case AnimType.Bool:
//                anim.SetBool(name, (bool)ret);
//                break;
//
//            case AnimType.Trigger:
//                anim.SetTrigger(name);
//                break;
//        }
//
//    }
//
//    //初始化血条
//    public void InitHPBar(BTRoleData role)
//    {
//        GameObject HPBar = null;
//
//        if (role.Side == RoleSide.Player)
//            HPBar = NGUITools.AddChild(UIRoot, PlayerBar);
//        else
//            HPBar = NGUITools.AddChild(UIRoot, EnemyBar);
//
//        HPBar.GetComponent<UIFollowTarget>().SetTarget(role.obj.transform.Find("HPPos"));
//        role.pop = HPBar.transform.Find("Text").GetComponent<HUDText>();
//        role.bar = HPBar.GetComponent<UIProgressBar>();
//        //Debug.Log("HpPos parent is: " + role.gameObject.name);
//
//    }
//
//    public void PopText(BTRoleData role, string text)
//    {
//        if (role.pop == null)
//            return;
//
//        if (role.Side == RoleSide.Player && role.pop != null)
//            role.pop.Add(text, Color.green, 1f);
//        else
//            role.pop.Add(text, Color.red, 1f);
//    }
//
//    public void UpdateHPBar(BTRoleData role, float value)
//    {
//        value = value > 1f ? 1f : value;
//        value = value < 0f ? 0f : value;
//
//        role.bar.value = value;
//
//    }
//
//    public void DestroyHPBar(BTRoleData role)
//    {
//        GameObject.DestroyImmediate(role.bar.gameObject);
//
//    }
//
//    public Type GetType(string name)
//    {
//        Type type = Type.GetType(name);
//        return type;
//
//    }
//
//    public void StartNav(BTRoleData role, Vector3 pos, float vertical, NavMeshPath path)
//    {
//        SetRoleAnimation(role.anim, "Vertical", AnimType.Float, vertical);//每帧调用不合适？？
//        SetRoleAnimation(role.anim, "CanAttack", AnimType.Bool, false);//查询当前anim state 判定？？
//
//        role.nav.enabled = true;
//        role.nav.isStopped = false;
//        role.nav.updatePosition = true;
//        role.nav.updateRotation = false;    
//        
//    }
//
//
//    /// <summary>
//    /// 风险：start时路径存在，行进update过程中，路径不通？？
//    /// </summary>
//    /// <param name="role"></param>
//    /// <param name="pos"></param>
//    /// <param name="vertical"></param>
//    /// <param name="path"></param>
//    public ResultType MoveToTargetByNav(BTRoleData role, Vector3 pos, NavMeshPath path)
//    {
//        if (!NavMesh.CalculatePath(role.obj.transform.position, p
//os, NavMesh.AllAreas, path))
//            return ResultType.Failed;
//
//        if (path.status != NavMeshPathStatus.PathComplete)
//            return ResultType.Failed;
//
//        role.nav.SetDestination(pos);
//        role.nav.speed = (role.anim.deltaPosition / Time.deltaTime).magnitude;
//        role.nav.nextPosition = role.obj.transform.position;//Animator Apply root motion
//        Vector3 targetFWD = role.nav.desiredVelocity;
//        targetFWD = new Vector3(targetFWD.x, 0f, targetFWD.z).normalized;
//        role.obj.transform.forward = Vector3.Slerp(role.obj.transform.forward, targetFWD, 0.8f);
//        //nav.Warp(transform.position);//此语句导致desiredVelocity=0？？   
//
//        role.Position = role.obj.transform.position;
//
//        return ResultType.Success;
//
//        //官方案例方案
//        //nav.speed = (anim.deltaPosition / Time.deltaTime).magnitude;
//        ////使此句有意义，需改变ApplyRootMotion？？
//        //transform.position = nav.nextPosition;
//        ////取消RigidBody件？？
//        //transform.forward = anim.deltaRotation * transform.forward;
//        ////瞬移nav，解决卡地形问题
//        //nav.Warp(transform.position);
//
//    }
//
//    //施法期间，保持朝向目标
//    public void UpdateRote(BTRoleData Caster, BTRoleData Target)
//    {
//        Vector3 targetFWD = Target.obj.transform.position - Caster.obj.transform.position;
//        targetFWD = new Vector3(targetFWD.x, 0f, targetFWD.z).normalized;
//        Caster.obj.transform.forward = Vector3.Slerp(Caster.obj.transform.forward, targetFWD, 0.8f);
//    }
//
//    public void EndNav(BTRoleData role)
//    {
//        role.nav.isStopped = true;
//    }
//
//    public float ComputeDist(BTRoleData caster, BTRoleData target)
//    {
//        float dist = (caster.obj.transform.position - target.obj.transform.position).magnitude;
//        return dist;
//    }
//
//    public string TypeToName(int type)
//    {
//        string name = null;
//
//        switch (type)
//        {
//            case 0:
//                name = "None";
//                break;
//
//            case 1:
//                name = "Strength";
//                break;
//
//            case 2:
//                name = "Intelligence";
//                break;
//
//            case 3:
//                name = "Agility";
//                break;
//
//            case 4:
//                name = "HP";
//                break;
//
//            case 5:
//                name = "MP";
//                break;
//
//            case 6:
//                name = "AttackDamage";
//                break;
//
//            case 7:
//                name = "AbilityPower";
//                break;
//        }
//
//        return name;
//    }
//
//
//}
