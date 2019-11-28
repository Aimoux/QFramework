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
        HITREACTION =3,//受击
        DEATH =4,//gg
        STUNT =5,//特技

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
        STUNTATK1 = 2061,//特技1 TimeLine, 帧数机制?Idle替代?
        STUNTATK2 = 2062,

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
        BLOCK = 5,//防御
    }

    public enum AssaultType
    {
        SINGLEATK1 = 1,//基础轻击
        SINGLEATK2 = 2,//基础重击
        SINGLEATK3 = 3,
        SINGLEATK4 = 4,
        SINGLEATK5 = 5,
        TWOCOMBO1 = 6,//二连击
        TWOCOMBO2 = 7,
        TWOCOMBO3 = 8,
        THREECOMBO1 = 9,//三连击
        THREECOMBO2 = 10,

        KICKATK1 = 2041,
        KICKATK2 = 2042,
        JUMPATK1 = 2051,
        JUMPATK2 = 2052,


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
        MANALOW =8,
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
        TARGET = 0,//原目标(最近)
        MAXHATRED =1,//仇恨值
        SELF = 2,
        NONE = 3,
        Random = 4,
        All = 5,
        DeadBody = 6,
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
        public const string Caster = "Caster";
        public const string Target = "Target";
        public const string Role = "Role";
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
        public const float InitialHate = 10f;//初始仇恨

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


public class MathUtil
{
    public static int bits(int num, int start, int count)
    {
        int remain = num >> start;
        int mask = (int)Mathf.Pow(2, count) - 1;
        return remain & mask;
    }

    public static int makeBits(params int[] values)
    {
        int num_args = values.Length;

        int value = 0;
        int b0 = 0;

        for (int i = num_args; i > 1; i = i - 2)
        {
            int b = values[i - 2];
            int v = values[i - 1];

            int v2 = bits(v, 0, b);
            if (v != v2)
            {
                Debug.LogError("makebits value out of bit range : " + v);
                return 0;
            }

            v = v << b0;
            value = value + v;
            b0 = b0 + b;
        }

        return value;
    }

    public static System.Collections.Generic.List<int> splitbits(int value, params int[] values)
    {
        System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
        int num_args = values.Length;
        int b0 = 0;
        for (int i = num_args; i > 1; i = i - 1)
        {
            int b = values[i - 1];
            list.Add(bits(value, b0, b));
            b0 = b0 + b;
        }
        list.Reverse();
        return list;
    }

    public static bool FGreat(float numA, float numB)
    {
        return Math.Abs(numA - numB) < 0.00015f || numA > numB;
    }
}

//public class DeepCopy
//{
//    public static T Clone<T>(T source)
//    {
//        if (!typeof(T).IsSerializable)
//        {
//            throw new ArgumentException("The type must be serializable.", "source");
//        }

//        // Don't serialize a null object, simply return the default for that object
//        if (object.ReferenceEquals(source, null))
//        {
//            return default(T);
//        }
//#if SERVER
//        string data = Newtonsoft.Json.JsonConvert.SerializeObject(source);
//        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data);
//#else
//        string data = fastJSON.JSON.ToJSON(source);
//        return fastJSON.JSON.ToObject<T>(data);
//#endif
//    }

//    /// <summary>
//    /// 将Source的属性值赋值给Target
//    /// </summary>
//    /// <param name="source">Source.</param>
//    /// <param name="target">Target.</param>
//    /// <typeparam name="T">The 1st type parameter.</typeparam>
//    /// <typeparam name="U">The 2nd type parameter.</typeparam>
//    public static void CloneValue<T, U>(T source, ref U target)
//    {

//        //		string data = fastJSON.JSON.ToJSON(source);
//        //		target =  fastJSON.JSON.ToObject<U>(data);


//        Type sourceType = source.GetType();
//        Type targetType = target.GetType();
//        PropertyInfo[] PropertyList = targetType.GetProperties();

//        foreach (PropertyInfo targetPI in PropertyList)
//        {
//            string name = targetPI.Name;
//            PropertyInfo sourcePI = sourceType.GetProperty(name);
//            if (sourcePI != null)
//            {
//                object value = sourcePI.GetValue(source, null);
//                targetPI.SetValue(target, value, null);
//            }
//        }
//    }
//}


//public class DelayInvoker : MonoBehaviour
//{
//    public static IEnumerator Invoke(Action action, float delaySeconds)
//    {
//        yield return new WaitForSeconds(delaySeconds);
//        action();
//    }
//}

//public class DebugStatisticsUtil : BehaviourSingleton<DebugStatisticsUtil>
//{
//    void Update()
//    {
//        // fps 计算
//        if (fpsCheckbattleBegin > 0)
//        {
//            if (Time.timeScale > 0 && Time.deltaTime > 0.0f)
//            {
//                fpsCheckbattleAccum += (Time.timeScale / Time.deltaTime);
//                ++fpsCheckbattleFrames;
//            }
//        }

//        // 
//        if (fpsCheckUIBegin > 0)
//        {
//            // 开始了战斗, 停止
//            if (this.battleBegin > 0f)
//            {
//                // 重置
//                fpsCheckUIBegin = 0;
//                fpsCheckUIAccum = 0.0f;
//                fpsCheckUIFrames = 0;
//            }
//            else
//            {

//                if (Time.deltaTime > 0.0f)
//                {
//                    fpsCheckUITimeLeft -= Time.deltaTime;
//                    fpsCheckUIAccum += (Time.timeScale / Time.deltaTime);
//                    ++fpsCheckUIFrames;
//                }


//                if (fpsCheckUITimeLeft <= 0.0f) // 到达更新间隔
//                {
//                    LogFpsCheckUI(false);
//                    // 重置
//                    fpsCheckUIBegin = 0;
//                    fpsCheckUIAccum = 0.0f;
//                    fpsCheckUIFrames = 0;
//                }
//            }
//        }
//    }

//    // 1. 网速监控: 逻辑服务器的话，给出平均返回时间即可 (因为丢包就会失败，然后会增加等待，等于会包含在内)
//    private long sendBegin = 0;
//    private List<string> networkList = new List<string>(); // 暂时不用, 采用 pixel 方式发到网站
//    private List<int> networkListTime = new List<int>(); // 只记录时间

//    public void LogNetworkMessage(bool isSend = true, string serverRunTime = "", string reason = "")
//    {
//#if !SERVER
//        try
//        {
//            if (UpComingBuffer.GetInstance() == null || UpComingBuffer.GetInstance().UpComingEventList == null ||
//                UpComingBuffer.GetInstance().UpComingEventList.Count <= 0)
//            {
//                return; // 未进入游戏内, 先不记录
//            }

//            if (isSend)
//            {
//                this.sendBegin = Local.GetMilliseconds();
//                return;
//            }

//            if (sendBegin == 0)
//            {
//                return; // 非请自来
//            }

//            long nowTime = Local.GetMilliseconds();

//            //networkList.Add(string.Format("-NETWORKSPEED - {0}_{1}_{2}", Config.UserId, reason, nowTime - this.sendBegin));
//            int runTime_ = 0;
//            if (serverRunTime.Length > 0)
//            {
//                runTime_ = int.Parse(serverRunTime);
//            }

//            if (runTime_ < 800)
//            {
//                networkListTime.Add((int)(nowTime - this.sendBegin - 35 - 15)); // sub 1 frame + decode time
//            }

//            // 累计到上限次数,追加给网络协议
//            if (networkListTime.Count > 4)
//            {
//                int avgNum = (int)(networkListTime.Sum() / networkListTime.Count * 1f);

//                Debug.Log(string.Format("LogNetworkMessage^^^^^^^^^^^^^^^^^^^^^^^_TIME: {0}", avgNum));
//                TrackUtil.Instance.LogPixelUnlimit(UICommon.Pixel_GameServerNetworkTime + "_" + avgNum + "_" + Config.Address);

//                networkList.Clear();
//                networkListTime.Clear();
//            }

//            sendBegin = 0;
//        }
//        catch (Exception e)
//        {
//            Debug.Log("DebugStatisticsUtil LogNetworkMessage exception: " + e.ToString());
//        }
//#endif
//    }

//    // 2. fps监控（战斗内和战斗外）
//    // 战斗外触发式检测, 在特定场景或者开窗口后 并且检测间隔要5分钟, 每次检测 6s, 如果被战斗或者切换焦点打断, 则重新开始
//    public float fpsCheckUIUpdateInterval = 6.0f; // fps更新间隔

//    private float fpsCheckUITimeLeft = 0.0f; // 当前更新间隔的计时
//    private float fpsCheckUIAccum = 0.0f; // fps累积
//    private int fpsCheckUIFrames = 0; // 帧数累积
//    private long fpsCheckUIBegin = 0;
//    private List<string> fpsCheckUIList = new List<string>(); // 暂时不用, 采用 pixel 方式发到网站
//    private List<int> fpsCheckUITimeList = new List<int>(); // 只记录时间
//    private int fpsCheckUITimeListMax = 0; // 统计最大上限

//    public void LogFpsCheckUI(bool isBegin = true, string reason = "")
//    {
//#if !SERVER
//        try
//        {
//            if (isBegin)
//            {
//                if (fpsCheckUITimeLeft > 0)
//                {
//                    return; // 还在检测中
//                }

//                // 随机几率开始检测
//                var random = new System.Random();
//                var num = random.Next(1, 100);
//                if (num > 90)
//                {
//                    return;
//                }

//                this.fpsCheckUIBegin = Local.GetMilliseconds();
//                fpsCheckUITimeLeft = fpsCheckUIUpdateInterval;
//                fpsCheckUIAccum = 0.0f;
//                fpsCheckUIFrames = 0;
//                return;
//            }

//            if (fpsCheckUIBegin == 0)
//            {
//                return; // 非请自来
//            }

//            float fps = fpsCheckUIAccum / fpsCheckUIFrames;
//            if (fps > 3)
//            {
//                fpsCheckUITimeList.Add((int)Math.Ceiling(fps));
//            }

//            // 累计到上限次数,追加给网络协议
//            if (fpsCheckUITimeList.Count > fpsCheckUITimeListMax)
//            {
//                int avgNum = (int)(fpsCheckUITimeList.Sum() / fpsCheckUITimeList.Count * 1f);

//                string pixelStr_ = UICommon.Pixel_UISceneFPS + "_" + avgNum;
//                if (reason != "")
//                {
//                    pixelStr_ += "_" + reason;
//                }

//                Debug.Log(string.Format("LogFpsCheckUI^^^^^^^^^^^^^^^^^^^^^^^_FPS: {0}", avgNum));
//                TrackUtil.Instance.LogPixelUnlimit(pixelStr_);

//                fpsCheckUIList.Clear();
//                fpsCheckUITimeList.Clear();

//                fpsCheckUITimeListMax++;
//                if (fpsCheckUITimeListMax > 3)
//                {
//                    fpsCheckUITimeListMax = 3;
//                }
//            }

//            fpsCheckUIBegin = 0;
//            fpsCheckUIAccum = 0.0f;
//            fpsCheckUIFrames = 0;
//        }
//        catch (Exception e)
//        {
//            Debug.Log("DebugStatisticsUtil LogFpsCheckUI exception: " + e.ToString());
//        }
//#endif
//    }

//    // 战斗中触发式检测, 检测整个战斗生命
//    private float fpsCheckbattleAccum = 0.0f; // fps累积

//    private int fpsCheckbattleFrames = 0; // 帧数累积
//    private long fpsCheckbattleBegin = 0;
//    private List<string> fpsCheckbattleList = new List<string>(); // 暂时不用, 采用 pixel 方式发到网站
//    private List<int> fpsCheckbattleTimeList = new List<int>(); // 只记录时间

//    public void LogFpsCheckbattle(bool isBegin = true, string reason = "")
//    {
//#if !SERVER
//        try
//        {
//            if (isBegin)
//            {
//                this.fpsCheckbattleBegin = Local.GetMilliseconds();
//                fpsCheckbattleAccum = 0.0f;
//                fpsCheckbattleFrames = 0;
//                return;
//            }

//            if (fpsCheckbattleBegin == 0)
//            {
//                return; // 非请自来
//            }

//            //battleLoadingList.Add(string.Format("-NETWORKSPEED - {0}_{1}_{2}", Config.UserId, reason, nowTime - this.sendBegin));
//            float fps = fpsCheckbattleAccum / fpsCheckbattleFrames;
//            fpsCheckbattleTimeList.Add((int)Math.Ceiling(fps));

//            // 累计到上限次数,追加给网络协议
//            if (fpsCheckbattleTimeList.Count > 2)
//            {
//                int avgNum = (int)(fpsCheckbattleTimeList.Sum() / fpsCheckbattleTimeList.Count * 1f);

//                string pixelStr_ = UICommon.Pixel_BattleSceneFPS + "_" + avgNum;
//                if (reason != "")
//                {
//                    pixelStr_ += "_" + reason;
//                }

//                Debug.Log(string.Format("LogFpsCheckbattle^^^^^^^^^^^^^^^^^^^^^^^_FPS: {0}", avgNum));
//                TrackUtil.Instance.LogPixelUnlimit(pixelStr_);

//                fpsCheckbattleList.Clear();
//                fpsCheckbattleTimeList.Clear();
//            }

//            fpsCheckbattleBegin = 0;
//            fpsCheckbattleAccum = 0.0f;
//            fpsCheckbattleFrames = 0;
//        }
//        catch (Exception e)
//        {
//            Debug.Log("DebugStatisticsUtil LogFpsCheckbattle exception: " + e.ToString());
//        }
//#endif
//    }


//    // 3. 以及战斗loading时间
//    private long battleBegin = 0;

//    private List<string> battleLoadingList = new List<string>(); // 暂时不用, 采用 pixel 方式发到网站
//    private List<int> battleLoadingListTime = new List<int>(); // 只记录时间

//    public void LogBattleLoading(bool isBegin = true, string reason = "")
//    {
//#if !SERVER
//        try
//        {
//            if (isBegin)
//            {
//                this.battleBegin = Local.GetMilliseconds();
//                return;
//            }

//            if (battleBegin == 0)
//            {
//                return; // 非请自来
//            }

//            long nowTime = Local.GetMilliseconds();

//            //battleLoadingList.Add(string.Format("-NETWORKSPEED - {0}_{1}_{2}", Config.UserId, reason, nowTime - this.sendBegin));
//            battleLoadingListTime.Add((int)(nowTime - this.battleBegin));

//            // 累计到上限次数,追加给网络协议
//            if (battleLoadingListTime.Count > 3)
//            {
//                int avgNum = (int)(battleLoadingListTime.Sum() / battleLoadingListTime.Count * 1f);

//                string pixelStr_ = UICommon.Pixel_BattleLoadingTime + "_" + avgNum;
//                if (reason != "")
//                {
//                    pixelStr_ += "_" + reason;
//                }

//                Debug.Log(string.Format("LogBattleLoading^^^^^^^^^^^^^^^^^^^^^^^_TIME: {0}", avgNum));
//                TrackUtil.Instance.LogPixelUnlimit(pixelStr_);

//                battleLoadingList.Clear();
//                battleLoadingListTime.Clear();
//            }

//            battleBegin = 0;
//        }
//        catch (Exception e)
//        {
//            Debug.Log("DebugStatisticsUtil LogBattleLoading exception: " + e.ToString());
//        }
//#endif
//    }

//    // 4. 网站登录需要时间统计
//    private long webBegin = 0;

//    public void LogWebLoginTime(bool isBegin = true, string reason = "")
//    {
//#if !SERVER
//        try
//        {
//            if (isBegin)
//            {
//                this.webBegin = Local.GetMilliseconds();
//                return;
//            }

//            if (webBegin == 0)
//            {
//                return; // 非请自来
//            }

//            long nowTime = Local.GetMilliseconds();

//            //battleLoadingList.Add(string.Format("-NETWORKSPEED - {0}_{1}_{2}", Config.UserId, reason, nowTime - this.sendBegin));

//            int avgNum = (int)(nowTime - this.webBegin);

//            string pixelStr_ = UICommon.Pixel_WebLoginTime + "_" + avgNum;
//            if (reason != "")
//            {
//                pixelStr_ += "_" + reason;
//            }

//            Debug.Log(string.Format("LogWebLoginTime^^^^^^^^^^^^^^^^^^^^^^^_TIME: {0}", avgNum));
//            TrackUtil.Instance.LogPixelUnlimit(pixelStr_);

//            webBegin = 0;
//        }
//        catch (Exception e)
//        {
//            Debug.Log("DebugStatisticsUtil LogWebLoginTime exception: " + e.ToString());
//        }
//#endif
//    }

//    // 5. 内存占用
//    public void LogTotalMemory()
//    {
//#if !SERVER
//        try
//        {
//            int totalMemory = SystemInfo.systemMemorySize;
//            string pixelStr_ = UICommon.Pixel_TotalMemory + "_" + totalMemory;
//            Debug.Log(string.Format("LogToalMemory^^^^^^^^^^^^^^^^^^^^^^^_TIME: {0}", totalMemory));
//            TrackUtil.Instance.LogPixelUnlimit(pixelStr_);
//        }
//        catch (Exception e)
//        {
//            Debug.Log("DebugStatisticsUtil LogToalMemory exception: " + e.ToString());
//        }
//#endif
//    }

//    // 6. 机型类型

//    // 7. 连接 proxy 的时间
//    public void LogProxyConnectTime(int time = 0, string reason = "")
//    {
//#if !SERVER
//        try
//        {
//            string pixelStr_ = UICommon.Pixel_ProxyConnectTime + "_" + time + "_" + Config.Address;
//            if (reason != "")
//            {
//                pixelStr_ += "_" + reason;
//            }
//            Debug.Log(string.Format("LogProxyConnectTime^^^^^^^^^^^^^^^^^^^^^^^_TIME: {0}", time));
//            TrackUtil.Instance.LogPixelUnlimit(pixelStr_);
//        }
//        catch (Exception e)
//        {
//            Debug.Log("DebugStatisticsUtil LogProxyConnectTime exception: " + e.ToString());
//        }
//#endif
//    }


//    private DebugStatisticsUtil()
//    {
//    }

//    private void SaveToDebugList()
//    {
//    }
//}


//public class QuickSort
//{

//    public static void Sort<T>(List<T> list, IComparer<T> comparer)
//    {
//        int start = 0;
//        int end = list.Count - 1;

//        Sort(list, start, end, comparer);
//    }

//    public static void Sort<T>(List<T> list, int start, int end, IComparer<T> comparer)
//    {
//        while (start < end)
//        {
//            int i, j = 0;

//            if (comparer.Compare(list[end], list[start]) < 0)
//            {
//                swap(list, start, end);
//            }

//            if (end - start == 1)
//            {
//                break;
//            }

//            i = (start + end) / 2;


//            if (comparer.Compare(list[i], list[start]) < 0)
//            {
//                swap(list, i, start);
//            }
//            else
//            {

//                if (comparer.Compare(list[end], list[i]) < 0)
//                {
//                    swap(list, i, end);
//                }
//            }

//            if (end - start == 2)
//            {
//                break;
//            }

//            T Pivot = list[i];

//            swap(list, i, end - 1);

//            i = start; j = end - 1;
//            for (; ; )
//            {
//                while (comparer.Compare(list[++i], Pivot) < 0)
//                {
//                    if (i > end)
//                    {
//                    }
//                }
//                while (comparer.Compare(Pivot, list[--j]) < 0)
//                {
//                    if (j < start)
//                    {

//                    }
//                }

//                if (j < i)
//                {
//                    break;
//                }

//                swap(list, i, j);
//            }

//            swap(list, end - 1, i);

//            if (i - start < end - i)
//            {
//                j = start;
//                i = i - 1;
//                start = i + 2;
//            }
//            else
//            {
//                j = i + 1;
//                i = end;
//                end = j - 2;
//            }

//            Sort(list, j, i, comparer);
//        }
//    }


//    public static void swap<T>(List<T> list, int a, int b)
//    {
//        T temp = list[a];
//        list[a] = list[b];
//        list[b] = temp;
//    }

//}

//#if !SERVER
//public class UIUtil : MonoBehaviour
//{
//    public static void ClearContainer(Transform container)
//    {
//        // 清空
//        for (int i = 0; i < container.childCount; i++)
//        {
//            GameObject obj = container.GetChild(i).gameObject;
//            Destroy(obj);
//        }
//    }
//}
//#endif

//#if !SERVER
//public class GameUtil : MonoBehaviour
//{
//    public static bool IsGameRestarting = false;
//    private static long _systemTotalMem = 0;
//    private static long _systemAvailableMem = 0;
//    private static long _lastAvailMemGetTime = 0;

//    public static long GetSystemTotalMem()
//    {
//        if (_systemTotalMem == 0)
//        {
//            _systemTotalMem = PlatformTools.HGGetSystemTotalMem();
//        }

//        return _systemTotalMem;
//    }

//    public static long GetSystemAvailableMem()
//    {
//        long ct = DateTime.UtcNow.Ticks / 10000000;
//        if (ct > _lastAvailMemGetTime + 10)
//        {
//            _lastAvailMemGetTime = ct;
//            _systemAvailableMem = PlatformTools.HGGetSystemAvailableMem();
//        }

//        return _systemAvailableMem;
//    }

//    public static string FormatFileSize(long number)
//    {
//        float result = number;
//        string suffix = "B";
//        if (result > 900)
//        {
//            suffix = "KB";
//            result = result / 1024;
//        }
//        if (result > 900)
//        {
//            suffix = "MB";
//            result = result / 1024;
//        }
//        if (result > 900)
//        {
//            suffix = "GB";
//            result = result / 1024;
//        }
//        if (result > 900)
//        {
//            suffix = "TB";
//            result = result / 1024;
//        }
//        if (result > 900)
//        {
//            suffix = "PB";
//            result = result / 1024;
//        }

//        var value = result.ToString(result < 100 ? "#0.00" : "#0");
//        return value + suffix;
//    }

//    public static string GenerateUuid()
//    {
//        return GetMd5(SystemInfo.deviceUniqueIdentifier.ToString());
//    }

//    public static string GetMd5(string str)
//    {
//        MD5 md5 = System.Security.Cryptography.MD5.Create();
//        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(str);
//        byte[] hashBytes = md5.ComputeHash(inputBytes);

//        // Convert the byte array to hexadecimal string
//        StringBuilder sb = new StringBuilder();
//        for (int i = 0; i < hashBytes.Length; i++)
//        {
//            sb.Append(hashBytes[i].ToString("X2"));
//        }
//        return sb.ToString();
//    }

//    /**
//    * UpdateFinish
//    * ConnectServerError
//    * LoginServerFailed
//    * ServerChanged
//    * ServerDisconnectError
//    * LanguageChanged
//    * AccountLinked
//    * TimezoneChanged
//    */
//    public static void RestartGame(bool needKillApp = false, string reason = "")
//    {
//        IsGameRestarting = true;
//        Debug.Log("================ Restart Game for [" + reason + "], kill app: " + needKillApp.ToString() + " =============");

//        PlayerPrefs.SetString("RestartReason", reason);
//        PlayerPrefs.Save();

//        bool isRestarted = false;
//        if (needKillApp)
//        {
//#if (!UNITY_EDITOR && UNITY_ANDROID)
//            isRestarted = true;
//            string msg = LocalizationManager.Instance.GetString("KEY.6497");
//            PlatformTools.HGPopupRestartGameMessage(msg,LocalizationManager.Instance.GetString("KEY.6496"));
//#endif
//        }

//        if (!isRestarted)
//        {
//            GateKeeper.ClearGameVersion();
//            LoadingManager.Clear();
//            LevelLoader.Clear();
//            ChatBuffer.Clear();

//            NetworkManager.Instance.Reset();
//            LevelBuffer.ClearStaticVariable();
//            ShopBuffer.ClearStaticVariable();
//            UIManager.Instance.clearUICache();

//            Player.Instance.ClearServerList();
//            GuildBuffer.ClearStaticVariable();
//            FacebookManager.Instance.clearCache();
//            Player.Instance.LoginInfo = null;
//            Player.Instance.IsInBraveMelee = false;

//            SingletonStack.DestoryLoadingSceneSingleton();

//            TrackUtil.Instance.LogPixelAndRestartGame();
//        }
//    }

//    public static void QuitGame()
//    {
//        TrackUtil.Instance.LogPixelAndQuit(UICommon.Pixel_QuitGame);
//        //Application.Quit();
//    }

//    public static void GCCollect()
//    {
//        Resources.UnloadUnusedAssets();
//        System.GC.Collect();
//        //PlatformTools.HGGCCollect();
//    }

//    public static bool NeedLowQuality()
//    {
//        return false;
//#if UNITY_IPHONE
//#if UNITY_5
//        UnityEngine.iOS.DeviceGeneration iOSGen = UnityEngine.iOS.Device.generation;
//        if (iOSGen == UnityEngine.iOS.DeviceGeneration.iPhone3G ||
//            iOSGen == UnityEngine.iOS.DeviceGeneration.iPhone3GS ||
//            iOSGen == UnityEngine.iOS.DeviceGeneration.iPhone4 ||
//            iOSGen == UnityEngine.iOS.DeviceGeneration.iPhone4S ||
//            iOSGen == UnityEngine.iOS.DeviceGeneration.iPad1Gen ||
//            iOSGen == UnityEngine.iOS.DeviceGeneration.iPad2Gen ||
//            iOSGen == UnityEngine.iOS.DeviceGeneration.iPadMini1Gen ||
//            iOSGen == UnityEngine.iOS.DeviceGeneration.iPadMini2Gen ||
//            iOSGen == UnityEngine.iOS.DeviceGeneration.iPodTouch1Gen ||
//            iOSGen == UnityEngine.iOS.DeviceGeneration.iPodTouch2Gen ||
//            iOSGen == UnityEngine.iOS.DeviceGeneration.iPodTouch3Gen ||
//            iOSGen == UnityEngine.iOS.DeviceGeneration.iPodTouch4Gen ||
//            iOSGen == UnityEngine.iOS.DeviceGeneration.iPodTouch5Gen
//            )
//#else
//        var iOSGen = iPhone.generation;
//        if (iOSGen == iPhoneGeneration.iPhone3G ||
//            iOSGen == iPhoneGeneration.iPhone3GS ||
//            iOSGen == iPhoneGeneration.iPhone4 ||
//            iOSGen == iPhoneGeneration.iPhone4S ||
//            iOSGen == iPhoneGeneration.iPad1Gen ||
//            iOSGen == iPhoneGeneration.iPad2Gen ||
//            iOSGen == iPhoneGeneration.iPadMini1Gen ||
//            iOSGen == iPhoneGeneration.iPadMini2Gen ||
//            iOSGen == iPhoneGeneration.iPodTouch1Gen ||
//            iOSGen == iPhoneGeneration.iPodTouch2Gen ||
//            iOSGen == iPhoneGeneration.iPodTouch3Gen ||
//            iOSGen == iPhoneGeneration.iPodTouch4Gen ||
//            iOSGen == iPhoneGeneration.iPodTouch5Gen
//            )
//#endif
//        {
//            return true;
//        }
//#endif
//        return false;
//    }
//}


//public class TrackUtil : BehaviourSingleton<TrackUtil>
//{
//    /*
//    * GS: game start
//    * LS: Login Start
//    * LF: Login Finished
//    * US: Update Start
//    * UF1: Update Finish and connect to server (there is no files need update)
//    * UF2: Update Finish and need to Restart (there is some files need update)
//    * SLS: Server login success
//    * SLF: Server login failed
//    * EMS: enter main scene
//    */
//    private string _csVersion = "";

//    // -- uuid for each device to append to pixel url for track before login
//    private string _uuid = "";

//    public Dictionary<string, int> sendPixelTimes = new Dictionary<string, int>();

//    private TrackUtil()
//    {
//        ParseCsVersion();
//    }

//    private string ParseCsVersion()
//    {
//        if (!string.IsNullOrEmpty(GateKeeper.CS_DLL_VERSION))
//        {
//            string[] sarray = GateKeeper.CS_DLL_VERSION.Split('_');
//            if (sarray.Length > 2)
//            {
//                _csVersion = sarray[2].Replace("v", "");
//                Debug.Log("Cs version parse as : " + _csVersion);
//            }
//        }
//        return _csVersion;
//    }

//    public void LogPixelAndQuit(string type)
//    {
//        LogPixel(type);
//        InvokeRepeating("DoExitGame", 1, 0);
//    }

//    private void DoExitGame()
//    {
//        Debug.Log("================Quit Game after 1s=============");
//        Application.Quit();
//    }

//    public void LogPixel(string type)
//    {
//        // 防止重复发送
//        if (!this.sendPixelTimes.ContainsKey(type))
//        {
//            this.sendPixelTimes[type] = 0;
//        }
//        if (this.sendPixelTimes[type] > 1)  // 最多发两次，预防没发送成功
//        {
//            return;
//        }
//        this.sendPixelTimes[type]++;

//        int startNums = PlayerPrefs.GetInt("StartNums", 0);
//        if (startNums <= 50)
//        {
//            StartCoroutine(PostRequest(type, startNums));
//        }
//    }

//    public void LogPixelUnlimit(string type)
//    {
//        int startNums = PlayerPrefs.GetInt("StartNums", 0);
//        StartCoroutine(PostRequest(type, startNums));
//    }

//    IEnumerator PostRequest(string type, int startNums)
//    {
//        string uidStr = "";
//        if (type == UICommon.Pixel_LoginStart)
//        {
//            string restartReason = PlayerPrefs.GetString("RestartReason", "");
//            if (!string.IsNullOrEmpty(restartReason))
//            {
//                uidStr += "&restartReason=" + restartReason;
//                PlayerPrefs.SetString("RestartReason", "");
//                PlayerPrefs.Save();
//            }

//            int totalMemory = SystemInfo.systemMemorySize;
//            uidStr += "&totalMemory=" + totalMemory;
//        }

//        if (Application.platform == RuntimePlatform.Android)
//        {
//            uidStr += "&os=2";
//        }
//        else
//        {
//            uidStr += "&os=1";
//        }
//        uidStr += "&type=" + type;

//        if (GateKeeper.Is50mClient)
//        {
//            uidStr += "&is50mClient=1";
//        }
//        else
//        {
//            uidStr += "&is50mClient=0";
//        }

//        if (GateKeeper.APP_HD_VALUE == 1)
//        {
//            uidStr += "&hd=1";
//        }
//        else
//        {
//            uidStr += "&hd=0";
//        }

//        if (GateKeeper.IsNewbie)
//        {
//            uidStr += "&newbie=1";
//        }

//        if (GateKeeper.IsAmazonApp)
//        {
//            uidStr += "&isAmazon=1";
//        }
//        else
//        {
//            uidStr += "&isAmazon=0";
//        }

//        if (string.IsNullOrEmpty(_csVersion))
//        {
//            _csVersion = ParseCsVersion();
//        }
//        uidStr += "&version=" + _csVersion;
//        uidStr += "&appVersion=" + GateKeeper.AppVersion;
//        uidStr += "&obbVersion=" + GateKeeper.ObbVersion;
//        uidStr += "&channel=" + GateKeeper.Channel;

//        if (_uuid == "")
//        {
//            if (GateKeeper.PixelUUID != "")
//            {
//                _uuid = GateKeeper.PixelUUID;
//            }
//            else
//            {
//                _uuid = GameUtil.GenerateUuid();
//            }
//        }
//        uidStr += "&uuid=" + _uuid;
//        uidStr += "&user_id=" + Config.UserId;

//        if (GateKeeper.IsWithCG)
//        {
//            uidStr += "&sn=-1";
//        }
//        else
//        {
//            uidStr += "&sn=" + startNums;
//        }

//        string url = "http://" + GateKeeper.SITE_DOMAIN + "/pixel.jpg?client=u3d" + uidStr;
//        Debug.Log("Log Pixel: " + url);

//        bool useWWW = true;
//        if (useWWW)
//        {
//            WWW www;
//            www = new WWW(url);
//            float begin = Time.realtimeSinceStartup;
//            while (!www.isDone && Time.realtimeSinceStartup - begin < 2.0f)
//            {
//                yield return new WaitForEndOfFrame();
//            }
//            if (!www.isDone)
//            {
//                Debug.Log("Log Pixel: (retry)" + url);
//                www = new WWW(url + "&retry");
//                yield return www;
//                if (!string.IsNullOrEmpty(www.error))
//                {
//                    throw new Exception(www.error);
//                }
//            }
//            Debug.Log("Log Pixel: (ret)" + type + ":" + www.text.Length);
//        }
//        else
//        {
//            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
//            request.Timeout = 1000 * 1;
//            request.Method = "GET";
//            request.BeginGetResponse(new AsyncCallback(ReadCallback), request);

//            Debug.Log("Start OpenReadAsync: " + type);
//            yield return request;
//        }
//    }

//    private void ReadCallback(IAsyncResult ar)
//    {
//        Debug.Log("ReadCallback response: " + ar.ToString());
//    }

//    public void LogPixelAndRestartGame()
//    {
//        LogPixel(UICommon.Pixel_RestartGame);
//        StartCoroutine(DoLoadLoadingScene());
//    }

//    private IEnumerator DoLoadLoadingScene()
//    {
//        // 如果后台下载已经开始，需要等下载解压都完成才能重新加载loading界面
//        if (LoadingManager.s_needUpdate/* && GateKeeper.ConfirmBgDownload*/)
//        {
//            Debug.Log("Need wait partial download finish before restart");
//            /*UIPatchProgress.Show(true, null);
//            while (!UIPatchProgress.PatchDone)
//            {
//                yield return new WaitForEndOfFrame();
//            }*/
//            yield return new WaitForEndOfFrame();
//            Debug.Log("Partial Download and Patch Done, Restart Loading");
//            Application.LoadLevel(0);
//        }
//        else
//        {
//            Debug.Log("No background download running, wait for a while before restart");
//            yield return new WaitForSeconds(0.2f);
//            Application.LoadLevel(0);
//        }
//    }
//}

//#endif

public class PlayerPrefsKey
{
    public const string UpdateFileNums = "UpdateFileNums";
    public const string LocalDeviceId = "LocalDeviceId";
    public const string Localuuid = "Localuuid";
    public const string TimeForLunch = "TimeForLunch";
    public const string TimeForSupper = "TimeForSupper";
    public const string NightTreat = "NightTreat";
    public const string MorningShopReloaded = "MorningShopReloaded";
    public const string ArenaRewarded = "ArenaRewarded";
    public const string Showfps = "Showfps";
    public const string BGMMute = "BGMMute";
    public const string HeroIdCachae = "HeroIdCachae";
    public const string RuneDeckLocalName = "RuneDeckLocalName";
    public const string UpcomingRefreshDate = "UpcomingRefreshDate";
    public const string StartNums = "StartNums";
    public const string UpdateNums = "UpdateNums";
    public const string InstallDate = "InstallDate";
    public const string RestartReason = "RestartReason";
    public const string UseSystemLanguage = "UseSystemLanguage";
    public const string Language = "Language";
    public const string SoundMute = "SoundMute";
    public const string RunePageIndex = "RunePageIndex";
    public const string Setting = "Setting";
    public const string NextCanMatchTime = "NextCanMatchTime";
    public const string GiftPackage = "GiftPackage";
    public const string BattleCount = "BattleCount";
    public const string UnrealTutorailNewPlayer = "UnrealTutorailNewPlayer";
}