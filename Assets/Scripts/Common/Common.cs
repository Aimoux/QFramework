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
        MOTION =0,//移动
        ATTACK =1,//攻击
        HITREACTION =2,//受击


    }

    public enum ANIMATIONSTATE
    {
        //状态是否必须与动画机clip一一对应??
        //是否足够资源单双手切换??
        #region Motion
        IDLE =0,
        WALKFORWARD =101,
        WALKBACK =1011,
        WALKLEFT =1012,
        WALKRIGHT =1013,
        RUNFORWARD =102,
        RUNBACK = 1021,
        RUNLEFT = 1022,
        RUNRIGHT = 1023,
        ROLLFORWARD=103,//翻滚
        ROLLBACK =1031,
        ROLLLEFT = 1032,
        ROLLRIGHT = 1033,
        JUMP = 104,//跳跃??
        FALL = 105,//坠落
        #endregion

        #region Attack
        ATTACKLITE =201,//站立轻击 有无必要设置 attack lite 1??
        ATTACKLITE2 = 2011,//站立轻击偶数
        ATTACKLITESPRINT = 2012,//奔跑轻击
        ATTACKLITEROLL = 2013,//翻滚轻击
        ATTACKHEAVY =202,//站立重击
        ATTACKHEAVY2 = 2021,//站立重击偶数
        ATTACKHEAVYSPRINT = 2022,//奔跑重击
        ATTACKHEAVYROLL = 2023,//翻滚重击 
        ATTACKSPECIAL =203,//战技
        ATTACKSPRINT =204,//奔跑攻击
        ULTIMATE1 =205,//特技， npc仅此一个
        ULTIMATE2 =206,//
        ULTIMATE3 =207,//暂定主角最多三个
        #endregion

        #region HitReaction
        BREAKLITE = 301,//轻击
        BREAKHEAVY =302,//重击，击退?
        KNOCKDOWN = 303,//击倒
        KNOCKUP = 304,//击飞??
        //特技以及中招动作均由TimeLine指定，所以此类状态可归到paralysis??
        //CHARMED =4, //被魅惑
        //DISGUST =4,//被恶心到
        #endregion
        
        DEATH=4,
        BLOCK =4,//防御
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
        public Dictionary<int, List<ANIMATIONSTATE>> Combos = new Dictionary<int, List<ANIMATIONSTATE>>
        {
            { 0, new List<ANIMATIONSTATE>{ ANIMATIONSTATE.ATTACKLITE} },//轻
            { 1, new List<ANIMATIONSTATE>{ ANIMATIONSTATE.ATTACKHEAVY} },//重
            { 2, new List<ANIMATIONSTATE>{ ANIMATIONSTATE.ATTACKLITE, ANIMATIONSTATE.ATTACKLITE} },//轻+轻
            { 3, new List<ANIMATIONSTATE>{ ANIMATIONSTATE.ATTACKLITE, ANIMATIONSTATE.ATTACKHEAVY} },//轻+重
            { 4, new List<ANIMATIONSTATE>{ ANIMATIONSTATE.ATTACKLITE, ANIMATIONSTATE.ATTACKHEAVY, ANIMATIONSTATE.ATTACKLITE} },//轻+重+轻
            { 5, new List<ANIMATIONSTATE>{ ANIMATIONSTATE.ROLLFORWARD, ANIMATIONSTATE.ATTACKLITE} },//滚+轻
            { 6, new List<ANIMATIONSTATE>{ ANIMATIONSTATE.ROLLFORWARD, ANIMATIONSTATE.ATTACKHEAVY} },//滚+重
            { 7, new List<ANIMATIONSTATE>{ ANIMATIONSTATE.FALL, ANIMATIONSTATE.ATTACKLITE} },//落+轻

        };

        public static Tactic Generate(int situation)
        {



            return new Tactic(situation);
        }

        public Tactic(int st)
        {

        }

        public void GenTactic()
        {
            Stack<ANIMATIONSTATE> cmds = new Stack<ANIMATIONSTATE>();
            ANIMATIONSTATE[] Walks = new ANIMATIONSTATE[] { ANIMATIONSTATE.WALKFORWARD, ANIMATIONSTATE.WALKBACK,
            ANIMATIONSTATE.WALKLEFT, ANIMATIONSTATE.WALKRIGHT};

            int dir = UnityEngine.Random.Range(0, 4);
            cmds.Push(Walks[dir]);

            int act = UnityEngine.Random.Range(0, Combos.Count);
            foreach (ANIMATIONSTATE state in Combos[act])
                cmds.Push(state);


        }


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
        SAMETICK = 2,
        COOLDOWN = 3,
        RUNNING = 4,
        STUN = 6,
        DISABLE = 7,
        SILENCE = 8,
        NOTARGET = 9,
        Untargetable = 10,
        TooFar = 12,
        TooNear = 13,//确有必要??
        OutOfScreen = 14,
        TalentCastring = 15,
        HasAbility = 16,
        CantUseInInstance = 17,
        MindChain = 18,
        OnlyNormalAttack = 19,
        UnHeal = 20,
        UnMove = 21

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

   

}

