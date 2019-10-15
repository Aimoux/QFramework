using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using QF;
using UnityEngine.AI;

namespace Common
{
    public enum BATTLEMODE
    {
        AREAN =0,//战斗
        STORY =1,//剧情模式
        HENTAI =2,
    }

    public enum ANIMATIONSTATE
    {
        //状态是否必须与动画机clip一一对应??
        //是否足够资源单双手切换??
        IDLE =0,
        WALK =1,
        RUN =2,
        ROLL=5,//翻滚
        JUMP = 4,//跳跃??
        DEATH=4,
        BLOCK =4,//防御
        STAGGER=4,//摇晃（被击中， avatar mask??上半身）可通过武器表中动态赋值??
        KNOCKDOWN =4,//被击倒、击飞（none root motion）
        PARALYSIS = 4,//被特技控制
        //特技以及中招动作均由TimeLine指定，所以此类状态可归到paralysis??
        //CHARMED =4, //被魅惑
        //DISGUST =4,//被恶心到
        ATTACKLITE =3,//站立轻击
        ATTACKHEAVY =4,//站立重击
        ATTACKSPECIAL =4,//战技
        ATTACKSPRINT =4,//奔跑攻击
        ULTIMATE1 =4,//特技， npc仅此一个
        ULTIMATE2 =4,//
        ULTIMATE3 =4,//暂定主角最多三个
    }

    public enum PERSONALITY
    {
        COWARD =0, //懦弱
        BOLD = 1,//勇敢



    }

    public enum ResultType
    {
        Success =0,//寻路
        Failure =1,

    }

    public enum RoleAttributeType//永久降智打击， 非永久则需要buff系统
    {
        HP =0,
        STAMINA =1,
        STRENGTH =2,
        DEXTERITY =3,
    }

    public enum DamageType//用于抗性计算
    {
        SLASH = 0,
        PIERCE =1,
        BLUNT =2,
        MAGIC =3,
        FIRE =4,
        ICE =5,
        ELECTRIC =6,

    }

    //确有必要??
    public class Hero
    {
        public int ID;
        public int Level;
        public List<int> gears;


    }

    //装备
    public class Item
    {
        public int ID;
        public int Amount;
    }


    public class CommonAnim :Singleton<CommonAnim>
    {
        public const string StateID = "StateID";
        public const string Vertical = "Vertical";
        public const string Jump ="Jump";
        public const string OnGround = "OnGround";
        public const string AttackLite = "AttackLite";
        public const string AttackHeavy = "AttackHeavy";

        public const string MotionLayer = "MotionLayer";
        public const string CombatLayer = "CombatLayer";

        //private int idVertical = Animator.StringToHash("Vertical");
        //    private int idJump = Animator.StringToHash("Jump");
        //    private int idGround = Animator.StringToHash("OnGround");
        //    private int idFire = Animator.StringToHash("Fire");
        //    private int idFireAngle = Animator.StringToHash("FireAngle");
        //    private int idFlipped = Animator.StringToHash("Flipped");
        //    private int idOperate = Animator.StringToHash("Operate");
        //    private string combatLayerName = "Combat";
        //    private string motionLayerName = "Locomotive";
        //    private string hitedLayerName = "Hited";
        //    private string jumpState = "Jump";
        //    private string healState = "HealOther";
        //    private string hitedState = "Hited";
        //    private int idCombatLayer;
        //    private int idMotionLayer;
        //    private int idHitedLayer;

        protected CommonAnim()
        {
        }

//        public void StartNav(Animator anim, Vector3 pos, float vertical, NavMeshPath path)
//        {
//            SetRoleAnimation(role.anim, "Vertical", AnimType.Float, vertical);//每帧调用不合适？？
//            SetRoleAnimation(role.anim, "CanAttack", AnimType.Bool, false);//查询当前anim state 判定？？
//
//            role.nav.enabled = true;
//            role.nav.isStopped = false;
//            role.nav.updatePosition = true;
//            role.nav.updateRotation = false;    
//
//        }

        // 风险：start时路径存在，行进update过程中，路径不通？？
//        public ResultType MoveToTargetByNav(BTRoleData role, Vector3 pos, NavMeshPath path)
//        {
//            if (!NavMesh.CalculatePath(role.obj.transform.position, pos, NavMesh.AllAreas, path))
//                return ResultType.Failed;
//
//            if (path.status != NavMeshPathStatus.PathComplete)
//                return ResultType.Failed;
//
//            role.nav.SetDestination(pos);
//            role.nav.speed = (role.anim.deltaPosition / Time.deltaTime).magnitude;
//            role.nav.nextPosition = role.obj.transform.position;//Animator Apply root motion
//            Vector3 targetFWD = role.nav.desiredVelocity;
//            targetFWD = new Vector3(targetFWD.x, 0f, targetFWD.z).normalized;
//            role.obj.transform.forward = Vector3.Slerp(role.obj.transform.forward, targetFWD, 0.8f);
//            //nav.Warp(transform.position);//此语句导致desiredVelocity=0？？   
//
//            role.Position = role.obj.transform.position;
//
//            return ResultType.Success;
//
//            //官方案例方案
//            //nav.speed = (anim.deltaPosition / Time.deltaTime).magnitude;
//            ////使此句有意义，需改变ApplyRootMotion？？
//            //transform.position = nav.nextPosition;
//            ////取消RigidBody件？？
//            //transform.forward = anim.deltaRotation * transform.forward;
//            ////瞬移nav，解决卡地形问题
//            //nav.Warp(transform.position);
//
//        }
//
//        //施法期间，保持朝向目标
//        public void UpdateRote(BTRoleData Caster, BTRoleData Target)
//        {
//            Vector3 targetFWD = Target.obj.transform.position - Caster.obj.transform.position;
//            targetFWD = new Vector3(targetFWD.x, 0f, targetFWD.z).normalized;
//            Caster.obj.transform.forward = Vector3.Slerp(Caster.obj.transform.forward, targetFWD, 0.8f);
//        }
//
//        public void EndNav(BTRoleData role)
//        {
//            role.nav.isStopped = true;
//        }
//
//        public float ComputeDist(BTRoleData caster, BTRoleData target)
//        {
//            float dist = (caster.obj.transform.position - target.obj.transform.position).magnitude;
//            return dist;
//        }

    }

   

}

