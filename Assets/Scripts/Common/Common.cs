using System;
using System.Linq;
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
        STORY =0,//剧情模式
        HENTAI =1,//HX
        CAMPAIGN = 2,//关卡
        CUSTOMIZED =3,//自定义
    }

    public enum ANIMATIONTYPE
    {
        IDLE =0,
        MOTION =1,//移动
        ATTACK =2,//攻击
        HITREACTION =2,//受击


    }

    public enum ANIMATIONSTATE
    {
        //状态是否必须与动画机clip一一对应??
        //是否足够资源单双手切换??
        #region Motion
        IDLE = 0,//两种idle，一种为只有一帧长度，另一种帧数作为common cd，transit同普攻??
        WALKFORWARD = 1011,
        WALKBACK = 1012,
        WALKLEFT = 1013,
        WALKRIGHT = 1014,
        WALKTURNLEFT = 1015,
        WALKTURNRIGHT = 1016,
        RUNFORWARD = 1021,
        RUNBACK = 1022,
        RUNLEFT = 1023,
        RUNRIGHT = 1024,
        RUNTURNLEFT = 1025,
        RUNTURNRIGHT = 1026,
        ROLLFORWARD = 1031,//翻滚
        ROLLBACK = 1032,
        ROLLLEFT = 1033,
        ROLLRIGHT = 1034,
        JUMP = 104,//跳跃??
        FALL = 105,//坠落
        #endregion

        #region Attack
        SINGLEATK1 = 2011,//基础轻击
        SINGLEATK2 = 2012,//基础重击
        SINGLEATK3 = 2013,
        SINGLEATK4 = 2014,
        SINGLEATK5 = 2015,
        TWOCOMBO1PART1 = 20211,//二连击一段
        TWOCOMBO1PART2 = 20212,//二连击二段
        TWOCOMBO2PART1 = 20221,
        TWOCOMBO2PART2 = 20222,
        TWOCOMBO3PART1 = 20231,
        TWOCOMBO3PART2 = 20232,
        THREECOMBO1PART1 = 20311,//三连击一段
        THREECOMBO1PART2 = 20312,//三连击二段
        THREECOMBO1PART3 = 20313,//三连击三段
        THREECOMBO2PART1 = 20321,
        THREECOMBO2PART2 = 20322,
        THREECOMBO2PART3 = 20323,
        KICKATK1 = 2041,
        KICKATK2 = 2042,
        JUMPATK1 = 2051,
        JUMPATK2 = 2052,

        #endregion

        #region HitReaction
        BREAKLITE = 301,//轻击
        BREAKHEAVY = 302,//重击，击退?
        KNOCKDOWN = 303,//击倒
        KNOCKUP = 304,//击飞??
                      //特技以及中招动作均由TimeLine指定，所以此类状态可归到paralysis??
                      //CHARMED =4, //被魅惑
                      //DISGUST =4,//被恶心到
        #endregion

        DEATH = 4,
        BLOCK = 4,//防御
    }

    public enum AssaultType
    {
        SINGLEATK1 = 1,//基础轻击
        SINGLEATK2 = 2,//基础重击
        SINGLEATK3 = 3,
        SINGLEATK4 = 4,
        SINGLEATK5 = 5,
        TWOCOMBO1 = 6,//二连击一段
        TWOCOMBO2 = 7,
        TWOCOMBO3 = 8,
        THREECOMBO1 = 9,//三连击一段
        THREECOMBO2 = 10,

        KICKATK1 = 2041,
        KICKATK2 = 2042,
        JUMPATK1 = 2051,
        JUMPATK2 = 2052,


    }

    //由一个或多个AnimState组成
    //执行中\中断\完成
    //将目标或自身状态作为决策参数(可选)
    //考虑用行为树配置(适用于可组合\替换\拆解,不适用于共有的基础模块)
    //理清Tactic\AnimState\Talent的区别与联系
    public class Tactic// tact as interface, combo继承tact??
    {
        public int Combo;//??random combo?? kneel for mercy?? faker joker??
        public int ComboRange;//??走近几步接突刺(前跃斩)
        public Vector3 Destination;//only useful when Combo =0? 撤退需要转身?战术撤退不必转身?
        public int Action;// for none-combat tact: run to heal, kneel
                          //所有可能的连击组合 weapon data中配置此处的子集key,combo broken??
                          //push命令以combo为单位, 不同combo之间存在 common cd??


    }


    //武器冲击力类型，影响受击方的硬直
    // public enum ImpactType
    // {
    //     NONE =0,
    //     LITE =1,//轻武器奇普攻
    //     HEAVY =2,//轻武器重击，重武器偶普攻
    //     KNOCKBACK =3,//重武器奇数普攻
    //     KNOCKDOWN =4,//常规突刺
    //     KNOCKHEAVY =5,//下砸
    //     KNOCKUP =6,//上挑

    // }


    public enum PERSONALITY
    {
        COWARD =0, //懦弱
        BOLD = 1,//勇敢



    }

    public enum ResultType//多用途,寻路以及可释放技能
    {
        SUCCESS =0,//寻路
        FAILURE =1,
        LEFTSIDE =2,
        RIGHTSIDE =3,
        TOOFAR = 4,
        TOONEAR = 5,//模型碰到一起,与技能距离判定共用此结果??
        RUNNING = 6,
        NOTARGET = 7,
        //SAMETICK = 999,
        //COOLDOWN = 999,

        //STUN = 999,
        //DISABLE = 999,
        //SILENCE = 999,

        //Untargetable = 10,

        //OutOfScreen = 14,
        //TalentCastring = 15,
        //HasAbility = 16,
        //CantUseInInstance = 17,
        //MindChain = 18,
        //OnlyNormalAttack = 19,
        //UnHeal = 20,
        //UnMove = 21

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

    //武器的特殊动作效果目标:群嘲\随机嘲\最远嘲
    public enum TargetType
    {
        None = 0,
        Self = 1,
        Target = 2,
        Random = 3,
        All = 4,
        DeadBody = 5,
        Nearest = 10,
        Farthest = 11,
        Weakest = 12,
        MaxHP = 13,
        MinHP = 14,
        MaxMP = 15,
        MinMP = 16,
        MaxStrength = 17,
        MaxAgility = 18,
        MaxIntelligence = 19,
        MaxAttackDamage = 20,
        MaxAbilityPower = 21,
        NearestSummon = 101
    }

    //确有必要??
    [Serializable]
    public class Hero
    {
        public int ID;
        public int Level;
        public List<int> gears = new List<int>();//已装备物品,背包物品位于Player
        public int Exp;//是否固定与玩家相等,或是必须带动升级??

        public Dictionary<int, int> Weapons = new Dictionary<int, int>();//id and lv

        public Hero(int id, int lv, Dictionary<int, int> wps)
        {
            ID =id;
            Level =lv;

            if (wps == null)
                Debug.LogError("null wps");
            else
                Weapons = wps;
        }

    }

    //装备
    public class Item
    {
        public int ID;
        public int Amount;
    }

    public class Const :Singleton<Const>
    {
        public const string StateID = "StateID";
        public const string Vertical = "Vertical";
        public const string Jump ="Jump";
        public const string OnGround = "OnGround";
        public const string AttackLite = "AttackLite";
        public const string AttackHeavy = "AttackHeavy";

        public const string MotionLayer = "MotionLayer";
        public const string CombatLayer = "CombatLayer";

        public const string SharedSelf ="SharedSelf";
        public const string SharedTarget = "SharedTarget";
        public const float DeltaFrame = 0.033333f;//1/30 30Hz帧率
        public const float ZeroFloat = 0.0000001f;
        public const float DefaultWeaponRange = 1f;
        public const float CollisionRadius = 1f;//质心距离小于此数值认为发生碰撞
        public const float ErrorAngle = 10f;//当前朝向与目标方向夹角小于此数值,可认为已正面之
        public const float ErrorArrive = 0.5f;//与某点距离小于此数值,可认为已到达
        public const float MaxRoteAngDelta = 5f;//单帧旋转最大角度

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

        protected Const()
        {
        }

        

    }

    public class RoleUtil
    {

        public static Vector3 To3DPlan(Vector3 offset)
        {
            Vector3 off = new Vector3(offset.x, 0f,offset.z);
            return off;
        }

        public static Vector3 To3DPlanUnit(Vector3 direction)
        {
            Vector3 dir = new Vector3(direction.x, 0f, direction.z).normalized;
            return dir;
        }

        public static Vector2 To2DPlanar(Vector3 Offset)
        {
            Vector2 off = new Vector2(Offset.x, Offset.z);
            return off;
        }

        public static Vector2 To2DPlanarUnit(Vector3 direction)
        {
            Vector2 dir = new Vector2(direction.x, direction.z).normalized;
            return dir;
        }
    }
   

}

