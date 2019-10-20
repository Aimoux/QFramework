using System;
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

    public enum ResultType//多用途,寻路以及可释放技能
    {
        Success =0,//寻路
        Failure =1,
        SameTick = 2,
        Cooldown = 3,
        Stun = 6,
        Disable = 7,
        Silence = 8,
        NoTarget = 9,
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
        public List<int> gears = new List<int>();
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

