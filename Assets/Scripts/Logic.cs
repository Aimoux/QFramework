using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using QF;
using Common;
using GameData;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;


public class Logic : MonoSingleton<Logic>
{
    public IBattleScene Scene;
    public BATTLEMODE BattleMode;

    public Dictionary<int, List<Role>> Surviors = new Dictionary<int, List<Role>>();//阵营,角色
    public Dictionary<int, List<Role>> Deads = new Dictionary<int, List<Role>>();
    public Dictionary<int, List<Role>> AllRoles = new Dictionary<int, List<Role>>();

    public void LoadLevel(BATTLEMODE mode, string scene, UnityAction onLoad)
    {

        BattleSceneLoader.Instance.LoadLevel(mode, scene, onLoad);

    }

    //Tick
    public void Tick(float passTime)
    {

        //英雄的行为树Tick
        int roleNum = this.Roles.Count;
        for (int i = 0; i < roleNum; i++)
        {
            BTRoleData element = this.Roles[i];
            if (!element.IsPause)
            {
                //if (element.anim.GetInteger("AtkST") != -1)
                //Debug.Log("Tick Start: " + element.obj.name);
                BehaviorManager.instance.Tick(element.tree);
            }
            else
            {
                element.tree.PauseWhenDisabled = true;
                element.tree.DisableBehavior(true);
            }
            roleNum = this.Roles.Count;//element的单个Tick内，可改变Roles[]
        }

        //特效类
        int castCount = this.CastEffects.Count;
        for (int i = 0; i < castCount; i++)
        {
            Behavior element = this.CastEffects[i];
            //if (element == null) Debug.LogError("null cast effect: " + element.name);
            BehaviorManager.instance.Tick(element);
            castCount = this.CastEffects.Count;//element的单个Tick内，可改变CastEffects[]
        }

        // 召唤物
        foreach (BTRoleData element in this.Summons)
        {
            if (!element.IsPause) BehaviorManager.instance.Tick(element.tree);
        }


    }

    public void InitRoles(Dictionary<int, List<Hero>> roles)
    {
        foreach(int side in roles.Keys)
        {
            foreach(Hero hero in roles[side])
            {
                Role role = Role.Create(hero);
                role.Faction = side;
                role.FactOrg = side;


                BehaviorTree tree = Enemies[i].GetComponent<BehaviorTree>();
                SharedBTRoleData role = (SharedBTRoleData)tree.GetVariable("BTCaster");
                if (role.Value == null) role.Value = new BTRoleData();
                role.Value.Index = i;
                role.Value.Side = RoleSide.Enemy;
                role.Value.AntiSide = -(int)role.Value.Side;
                AddRole(role.Value);

            }




        }


    }

    //role创建独立于role model(RoleController)??
    public void InitAllies(List<Hero> allies)
    {


    }

    public void InitEnemies(List<Hero> enemies)
    {



    }

    public void InitBattle()
    {
        


    }

    public List<Behavior> CastEffects = new List<Behavior>();

    void Start()
    {
        AliveCount.Add(RoleSide.Player, 0);
        AliveCount.Add(RoleSide.Enemy, 0);

        DeadCount.Add(RoleSide.Player, 0);
        DeadCount.Add(RoleSide.Enemy, 0);

        //利用Tag，名称来初始化role 列表？？
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Bot");
        if (Enemies == null) Debug.LogError("find no enemy!!!");
        for (int i = 0; i < Enemies.Length; i++)
        {
            BehaviorTree tree = Enemies[i].GetComponent<BehaviorTree>();
            SharedBTRoleData role = (SharedBTRoleData)tree.GetVariable("BTCaster");
            if (role.Value == null) role.Value = new BTRoleData();
            role.Value.Index = i;
            role.Value.Side = RoleSide.Enemy;
            role.Value.AntiSide = -(int)role.Value.Side;
            AddRole(role.Value);
        }

        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        if (Players == null) Debug.LogError("find no player!!!");
        for (int i = 0; i < Players.Length; i++)
        {
            BehaviorTree tree = Players[i].GetComponent<BehaviorTree>();
            SharedBTRoleData role = (SharedBTRoleData)tree.GetVariable("BTCaster");
            if (role.Value == null) Debug.LogError("null player roledata");
            role.Value.Index = Roles.Count;
            role.Value.Side = RoleSide.Player;
            role.Value.AntiSide = -(int)role.Value.Side;
            AddRole(role.Value);
        }

    }

    public void onTimer(double passTime)
    {
       
    }

    public IEnumerable<BTRoleData> GetSurvivors(RoleSide side)
    {
        BTRoleData[] units = new BTRoleData[this.AliveRoles[side].Count];
        this.AliveRoles[side].CopyTo(units);

        for (int i = units.Length - 1; i >= 0; i--)
        {
            BTRoleData role = units[i];
            yield return role;
        }
    }

    public void AddRole(BTRoleData role)
    {
        this.Roles.Add(role);
        role.Index = this.Roles.Count - 1;

        if (role.RoleST == (int)RoleState.Death)
        {
            this.DeadCount[role.Side]++;
        }
        else
        {
            this.AliveRoles[role.Side].Add(role);
            this.AliveRoles[RoleSide.Both].Add(role);
            this.AliveCount[role.Side]++;
        }

    }

    public void RoleDead(BTRoleData role)
    {
        this.AliveRoles[role.Side].Remove(role);
        this.AliveRoles[RoleSide.Both].Remove(role);
        DeadRoles.Add(role);
        DeadCount[role.Side]++;
        AliveCount[role.Side]--;

    }


    public bool VertifyBattleResult(RoleSide side)
    {
        if (AliveRoles[side].Count == 0)//limitTime
            return true;


        return false;
    }

}


//public enum TotemTriggerType
//{
//    ENTER_BATTLE,
//    Ultimate,
//    TalentA,
//    TalentB,
//    TalentC,
//    Passive,
//    NormalAttack,
//    ActiveTalent = 10,
//    Poisoming = 11 //毒系触发
//}
////图腾属性名称类型
//public enum TotemAddSkillEffect
//{
//    Poison_Injury_Per_Second = 100,// 毒系图腾 ： 每秒毒伤
//    Reduce_Response = 101, //毒系图腾 ： 降低回复效果
//}
//public enum E_BattleMode
//{
//    /// <summary>
//    /// 普通战斗--PVE
//    /// </summary>
//    Campaign,
//    /// <summary>
//    /// 挑战Duel--PVP
//    /// </summary>
//    Duel,
//    /// <summary>
//    /// 竞技场--PVP
//    /// </summary>
//    Arena,
//    /// <summary>
//    /// 远征--PVE
//    /// </summary>
//    Crusade,
//    /// <summary>
//    /// 远征挑战模式--PVE
//    /// </summary>
//    CrusadeChallenge,
//    /// <summary>
//    /// 秘宝龙穴--PVP
//    /// </summary>
//    Treasure,
//    /// <summary>
//    /// 时空裂缝--PVE
//    /// </summary>
//    TimeRift,
//    /// <summary>
//    /// 英雄演武堂--PVE
//    /// </summary>
//    HeroTrial,
//    /// <summary>
//    /// 外域模式--PVE
//    /// </summary>
//    Outland,
//    /// <summary>
//    /// 公会副本
//    /// </summary>
//    GuildInstance,
//    /// <summary>
//    /// 王者竞技场--PVP
//    /// </summary>
//    GrandArena,
//    /// <summary>
//    /// 远古神庙--PVE
//    /// </summary>
//    Temple,
//    /// <summary>
//    /// 巅峰挑战--PVP
//    /// </summary>
//    GrandChallenge,
//    /// <summary>
//    /// 第一战
//    /// </summary>
//    FirstBattle,
//    /// <summary>
//    /// 工会战
//    /// </summary>
//    GuildChampion,
//    /// <summary>
//    /// 争霸战(直播，非直播两种方式)
//    /// </summary>
//    PersonChampion,
//    /// <summary>
//    /// 战争营地，大厅防守阵形
//    /// </summary>
//    WarCampHallSetTeam,
//    /// <summary>
//    /// 战争营地, 大厅的掠夺战斗
//    /// </summary>
//    HallOfWar,
//    /// <summary>
//    /// 新英雄战斗测试，测试人员用
//    /// </summary>
//    BattleTest,
//    /// <summary>
//    /// 诸神争霸
//    /// </summary>
//    WarOfGods,
//    /// <summary>
//    /// 对决Alpha专用
//    /// </summary>
//    DuelAlpha,
//    /// <summary>
//    /// 世界Boss
//    /// </summary>
//    WorldBoss,
//    /// <summary>
//    /// 世界 Boss 自动战斗
//    /// </summary>
//    WorldBossAuto,
//    /// <summary>
//    /// 东征先遣队
//    /// </summary>
//    CrusadeRaid,
//    /// <summary>
//    /// 东征挑战模式先遣队
//    /// </summary>
//    CrusadeChallengeRaid,
//    /// <summary>
//    /// 勇者乱斗
//    /// </summary>
//    BraveMelee,
//    /// <summary>
//    /// 外传
//    /// </summary>
//    Story,
//    /// <summary>
//    /// 虚幻
//    /// </summary>
//    Unreal
//}

///// 战斗管理器，单例，管理所有战斗数据及逻辑
//public class Logic : Singleton<Logic>//战斗数据的基本判断
//{
//    // 战斗状态（貌似不用了OnEnterFame）
//    const int BATTLE_STAGE_INIT = -1;
//    const int BATTLE_STAGE_WATCHING = 0;
//    const int BATTLE_STAGE_FIGHTING = 1;
//    const int BATTLE_STAGE_FINISHED = 2;
//    const int BATTLE_STAGE_PLAY_ANIMATION_BEFORE_START = 3;
//    // 貌似也不用了（OnEnterFame）
//    const float FIGHTING_TIMEOUT = 60f * 3f;
//    const float NEW_PVE_FIGHTING_TIMEOUT = 60 * 1.5f;
//    const int INTERVAL_SAVE_TEMP_BATTLE = 10;    // -- 临时保存战斗结果的时间间隔，10s
//    // 一波战斗时限90秒
//    const float defaultTimeLimit = 90;
//    // 战斗类型
//    public E_BattleMode BattleMode;
//    // 是否为回放
//    public bool IsReplayMode;
//    // 回放中再试一次标记
//    public bool IsTryAgain;
//    // 是否是boss关
//    public bool IsBossLevel = false;
//    // 是否是pvp竞技

//    // 是否为实时战斗播放
//    public bool IsLiveMode;
//    // 是否为PVP类战斗
//    public bool IsArenaMode
//    {
//        get
//        {
//            if (this.BattleMode == E_BattleMode.Arena ||
//                this.BattleMode == E_BattleMode.GrandArena ||
//                this.BattleMode == E_BattleMode.Duel ||
//                this.BattleMode == E_BattleMode.GuildChampion ||
//                this.BattleMode == E_BattleMode.PersonChampion ||
//                this.BattleMode == E_BattleMode.WarOfGods ||
//                this.BattleMode == E_BattleMode.BraveMelee ||
//                this.BattleMode == E_BattleMode.WorldBossAuto)
//                return true;
//            return false;
//        }
//    }
//    // 未使用？？
//    public bool GMMode;
//    // 回放记录（普通竞技场，对决，龙穴）
//    public Server.PVPRecord PvpReplayRecord;
//    // 回放记录（王者竞技场）
//    public Server.AdvArenaRoundBattleRecord TopPvpReplayRecord;
//    // 回放记录(争霸赛)
//    public Server.PCSPlusBattleReplay PersonalChampionRecord;
//    // 回放记录(战队营地)
//    public Server.HallOfWarQueryReplayReply HallOfWarRecord;
//    // 回放记录(诸神战)
//    public Server.WarOfGodsQueryReplayReply WarOfGodsRecord;
//    // 回放记录(勇者乱斗)
//    public Server.BraveMeleeQueryReplayReply BraveMeleeRecord;

//    // 战斗关卡数据
//    public LevelData LevelData;
//    // 战斗每一波战斗数据
//    public CombatConfigData CombatConfig;
//    // 关卡怒气增益系数
//    public float MPBonus;
//    // 回合数
//    public int Round;
//    // 战斗矩形区域
//    public Rect BattleRect;
//    // 前进速度系数
//    public float MarchingSpeed = 1.75f;
//    // 是否正在运行
//    public bool Running = false;
//    // 战斗引擎状态
//    public bool Enabled;
//    // 帧计数
//    public int doneFrameCount;
//    // 战斗倒计时时间（90s）
//    public double LimitTime;
//    // 战斗金币掉落奖励
//    public int GoldCount = 0;
//    // 战斗宝箱掉落奖励统计
//    public int LootCount = 0;

//    // 神庙需要的额外数据
//    public int monsterIndex;
//    public int ownerUserId;

//    // 世界Boss额外数据
//    public int worldBossTeamId;
//    public int worldBossBuffCount;

//    // 暂停级别
//    public int PauseLevel;
//    // 释放大招引起屏幕暂停的阵营（没有用？？？）
//    public RoleSide PauseSide;
//    // 战斗结果
//    public Common.BATTLE_RESULT BattleResult;
//    // 战斗评星
//    public int ResultStars;

//    // 己方英雄数据列表保存
//    public List<Common.Hero> HeroList;
//    // 双方英雄角色对象列表
//    public List<Role> Roles = new List<Role>();
//    // 玩家操作数据记录（回放会用到）
//    public List<OpRecord> OpRecords = new List<OpRecord>();

//    // 英雄存活数（每一方的存活）
//    public Dictionary<RoleSide, int> AliveCount = new Dictionary<RoleSide, int>();
//    // 英雄死亡数（每一方的死亡）
//    public Dictionary<RoleSide, int> DeadCount = new Dictionary<RoleSide, int>();

//    // 存活角色对象保存
//    public Dictionary<RoleSide, List<Role>> AliveRoles = new Dictionary<RoleSide, List<Role>>()
//    {
//        { RoleSide.Player , new List<Role>() },
//        { RoleSide.Enemy , new List<Role>() },
//        { RoleSide.Both , new List<Role>() }
//    };
//    // 召唤物
//    public List<Role> Summons = new List<Role>();
//    // 死亡角色对象保存
//    public List<Role> DeadRoles = new List<Role>();
//    // 
//    public List<Element> CastEffects = new List<Element>();
//    // 怪物上阵最大限制
//    const int MAX_POSITION = 5;
//    // 战斗状态
//    private int _battleState = -1;
//    // 关卡是否结束
//    public bool LevelEnded;
//    // 战斗结束
//    public bool BattleEnd;
//    // 战斗中伤害值记录
//    public BattleStatistic Statistic = new BattleStatistic();
//    // 战斗场景指针
//    public IBattleScene Scene { get; set; }
//    // 已经无效（废弃）
//    public float beginTime = 0;
//    // 已经无效（废弃）
//    private float lastSaveTempBattleTime = 0;
//    // 当前直播到第几帧
//    public int TargetDoneFrameCount = 0;
//    //ChickenDinner 战斗资源是否加载完成
//    public bool isLoaded = false;

//    public Vector3 ForwardDirection
//    {
//        get
//        {
//            if (this.Scene != null)
//            {
//                return this.Scene.ForwardDirection;
//            }
//            return Vector3.one;
//        }
//    }
//    // 前进角度
//    public Vector3 ForwardAngle
//    {
//        get
//        {
//            if (this.Scene != null)
//            {
//                return this.Scene.ForwardAngles;
//            }
//            return Vector3.one;
//        }
//    }
//    // 这个是3d中使用的，位置节点
//    public Vector3[] BattlePoints
//    {
//        get
//        {
//            if (this.Scene != null)
//            {
//                return this.Scene.BattlePoints;
//            }
//            return new Vector3[3];
//        }
//    }
//    // 挑战中的防守方id（不应该放在战斗逻辑中）    
//    public int DefenderID;
//    // 龙穴类型id（不应该放在战斗逻辑中）
//    public int TreasureTypeID;
//    // 怪物总数（没有使用过？？）
//    int totalMonster = 0;
//    //  手动操作索引（大招）
//    int NextOperationIdx;

//    public bool Supplied;
//    // 己方英雄id列表
//    List<int> HeroIDList = new List<int>();

//    // 死亡墓碑列表
//    public List<Vector2> DeathPositions = new List<Vector2>();
//    public List<EffectJoint> DeathPositionEffectJoints = new List<EffectJoint>();

//    // 对战双方的图腾等级
//    public int playerTotemLevel = 0;
//    public int enemyTotemLevel = 0;

//    // 对战双方的图腾类型
//    public int playerTotemType = 0;
//    public int enemyTotemType = 0;
//    //ChickenDinner 出生位置
//    public int offSetX = 0;
//    /// <summary>
//    /// 类构造函数
//    /// </summary>
//    public Logic()
//    {

//    }

//    /// <summary>
//    /// 角色单位列表添加
//    /// </summary>
//    /// <param name="role"></param>
//    /// <param name="promisor"></param>
//    /// <param name="position"></param>
//    /// <param name="init_action"></param>
//    public void AddRole(Role role, Role promisor = null, Vector2 position = new Vector2(), int init_action = 0)
//    {
//        this.Roles.Add(role);
//        role.Index = this.Roles.Count;
//        role.Config.Promisor = promisor;
//        if (role.IsDead)
//        {
//            this.DeadCount[role.Side]++;
//        }
//        else
//        {
//            this.AliveRoles[role.Side].Add(role);
//            this.AliveRoles[RoleSide.Both].Add(role);
//            this.AliveCount[role.Side]++;
//        }
//        if (promisor == null)
//        {
//            role.PreviousPosition = role.Position;
//            role.GlobalIdx = this.Roles.Count - 1;
//        }
//        else
//        {
//            promisor.SummonList.Add(role);
//            role.Config.IsDemon = true;
//            //role.InitAttributes();//符文  提升召唤物属性添加  
//            role.Position = position;
//            role.PreviousPosition = role.Position;
//            role.Born(1, init_action);
//            if (role.Joint != null)
//                role.Joint.OnEnterFrame(0);
//        }
//    }


//    /// <summary>
//    /// 己方英雄单位信息配置
//    /// </summary>
//    /// <param name="config"></param>
//    /// <param name="tid"></param>
//    /// <param name="is_bot"></param>
//    public void InitConfigPredictInfo(RoleConfig config, int tid, bool is_bot)
//    {
//        if (DataManager.Instance.GlobalConfig.EnableMaxAttStrategy)
//        {
//            RoleData uinfo = DataManager.Instance.Roles[tid];
//            if (uinfo.RoleType == "Monster")
//            {
//                config.PredictQuality = is_bot;
//                config.PredictTalent = is_bot;
//                config.PredictMaxQuality = false;
//            }
//            else
//            {
//                config.PredictQuality = false;
//                config.PredictTalent = false;
//                config.PredictMaxQuality = true;
//            }
//        }
//        else
//        {
//            config.PredictQuality = is_bot;
//            config.PredictTalent = is_bot;
//            config.PredictMaxQuality = false;
//        }
//    }

//    /// <summary>
//    /// 替补
//    /// </summary>
//    /// <param name="substitute"></param>
//    /// <param name="isBot"></param>
//    /// <param name="side"></param>
//    public void SetSubstitute(HeroDataEx substitute, bool isBot, RoleSide side)
//    {
//        // 诸神战替补逻辑
//        if (BattleMode != E_BattleMode.WarOfGods)
//            return; ;

//        if (substitute == null || substitute.@base == null || substitute.extra == null || substitute.extra.HP == 0)
//            return;

//        var config = new RoleConfig { HPFactor = DataManager.Instance.RoleConfigs[(int)substitute.@base.level].HPFactorArena };
//        InitConfigPredictInfo(config, substitute.@base.tid, isBot);
//        Role role = Role.Create(substitute.@base, side, config, substitute.extra);

//        if (side == RoleSide.Player)
//            role.Position = new Vector2
//            {
//                x = Const.InitialPosition[2].x - BattleRect.xMax * 0.5f,
//                y = Const.InitialPosition[2].y
//            };
//        else
//            role.Position = new Vector2
//            {
//                x = BattleRect.xMax * 0.5f - Const.InitialPosition[2].x,
//                y = Const.InitialPosition[2].y
//            };
//        HeroIDList.Add(substitute.@base.tid);
//        SubtituteRoles.Add(role);
//    }

//    /// <summary>
//    /// pvp类战斗调用：初始化己方英雄(initMyHeroes、initEnemyHeoes)
//    /// </summary>
//    /// <param name="heroes"></param>
//    /// <param name="isBot"></param>
//    /// <param name="inputExtra"></param>
//    public void initMyHeroes(List<Common.Hero> heroes, bool isBot, Dictionary<int, Common.HeroExtra> inputExtra)
//    {
//        //SetSubstitute(ref heroes, isBot, inputExtra, RoleSide.Player);
//        for (int i = 0; i < heroes.Count; i++)
//        {
//            Common.Hero hero = heroes[i];
//            RoleConfig config = new RoleConfig();
//            config.HPFactor = (float)DataManager.Instance.RoleConfigs[(int)hero.level].HPFactorArena;
//            InitConfigPredictInfo(config, (int)hero.tid, isBot);
//            Common.HeroExtra extra = null;
//            if (inputExtra != null)
//            {
//                extra = inputExtra[hero.tid];
//                if (extra == null || extra.HP == 0)
//                {
//                    continue;
//                }
//            }
//            Role role = Role.Create(hero, RoleSide.Player, config, extra);
//            // Crusde,CrusadeChallenge
//            if (this.BattleMode == E_BattleMode.Crusade || this.BattleMode == E_BattleMode.CrusadeChallenge || this.BattleMode == E_BattleMode.Treasure || this.BattleMode == E_BattleMode.HallOfWar ||
//                this.BattleMode == E_BattleMode.GrandChallenge || this.BattleMode == E_BattleMode.FirstBattle || this.BattleMode == E_BattleMode.DuelAlpha)
//            {
//                role.activeTalentReadyToGo = false;
//            }
//            //Init hero hire data here
//            this.AddRole(role);
//            // 设置role avatar的位置
//            role.Position = new Vector2();
//            role.Position.x = Const.InitialPosition[i].x - this.BattleRect.xMax * 0.5f;
//            role.Position.y = Const.InitialPosition[i].y;

//            this.HeroIDList.Add(hero.tid);
//        }
//    }
//    /// <summary>
//    /// pvp类战斗调用：初始化敌方英雄(initMyHeroes、initEnemyHeoes)
//    /// </summary>
//    /// <param name="enemys"></param>
//    /// <param name="isBot"></param>
//    /// <param name="inputExtra"></param>
//    public void initEnemyHeoes(List<Common.Hero> enemys, bool isBot, Dictionary<int, Common.HeroExtra> inputExtra)
//    {
//        //SetSubstitute(ref enemys, isBot, inputExtra, RoleSide.Enemy);
//        for (int i = 0; i < enemys.Count; i++)
//        {
//            Common.Hero hero_enermy = enemys[i];
//            RoleConfig config_enermy = new RoleConfig();
//            config_enermy.HPFactor = (float)DataManager.Instance.RoleConfigs[(int)hero_enermy.level].HPFactorArena;
//            InitConfigPredictInfo(config_enermy, (int)hero_enermy.tid, isBot);
//            Common.HeroExtra extra = null;
//            if (inputExtra != null)
//            {
//                extra = inputExtra[hero_enermy.tid];
//                if (extra == null || extra.HP == 0)
//                {
//                    continue;
//                }
//            }
//            Role role = Role.Create(hero_enermy, RoleSide.Enemy, config_enermy, extra);
//            this.AddRole(role);
//            // 设置role avatar的位置
//            role.Position = new Vector2();
//            role.Position.x = this.BattleRect.xMax * 0.5f - Const.InitialPosition[i].x;
//            role.Position.y = Const.InitialPosition[i].y;
//        }
//    }
//    /// <summary>
//    /// ChickenDinner类战斗调用：初始化己方英雄(initMyHeroes、initEnemyHeoes)
//    /// </summary>
//    /// <param name="heroes"></param>
//    /// <param name="isBot"></param>
//    /// <param name="inputExtra"></param>
//    public void initUnrealMyHeroes(List<Common.Hero> heroes, Dictionary<int, Common.HeroExtra> inputExtra)
//    {
//        //SetSubstitute(ref heroes, isBot, inputExtra, RoleSide.Player);
//        for (int i = 0; i < heroes.Count; i++)
//        {
//            Common.Hero hero = heroes[i];
//            RoleConfig config = new RoleConfig();
//            //config.HPFactor = (float)DataManager.Instance.RoleConfigs[(int)hero.level].HPFactorArena;
//            InitConfigPredictInfo(config, (int)hero.tid, false);
//            Common.HeroExtra extra = null;
//            if (inputExtra != null)
//            {
//                extra = inputExtra[hero.tid];
//                if (extra == null || extra.HP == 0)
//                {
//                    continue;
//                }
//            }
//            Vector2 birthPos = new Vector2(Const.InitialUnrealPosition[hero.favoriteState].x * 85,
//                Const.InitialUnrealPosition[hero.favoriteState].y * 85);
//            Role role = Role.CreateUnreal(hero, RoleSide.Player, config, extra, birthPos);
//            //Init hero hire data here
//            this.AddRole(role);
//            // 设置role avatar的位置
//            role.Position = birthPos;
//            this.HeroIDList.Add(hero.tid);
//        }
//    }

//    /// <summary>
//    /// ChickenDinner类战斗调用：初始化敌方英雄(initMyHeroes、initEnemyHeoes)
//    /// </summary>
//    /// <param name="enemys"></param>
//    /// <param name="isBot"></param>
//    /// <param name="inputExtra"></param>
//    public void initUnrealEnemyHeoes(List<Common.Hero> enemys, Dictionary<int, Common.HeroExtra> inputExtra)
//    {
//        for (int i = 0; i < enemys.Count; i++)
//        {
//            Common.Hero hero_enermy = enemys[i];
//            RoleConfig config_enermy = new RoleConfig();
//            config_enermy.HPFactor = (float)DataManager.Instance.RoleConfigs[(int)hero_enermy.level].HPFactorArena;
//            InitConfigPredictInfo(config_enermy, (int)hero_enermy.tid, false);
//            Common.HeroExtra extra = null;
//            if (inputExtra != null)
//            {
//                extra = inputExtra[hero_enermy.tid];
//                if (extra == null || extra.HP == 0)
//                {
//                    continue;
//                }
//            }
//            Vector2 birthPos = new Vector2(Const.InitialUnrealPosition[hero_enermy.favoriteState].x * -85 + 85,
//                Const.InitialUnrealPosition[hero_enermy.favoriteState].y * 85);
//            Role role = Role.CreateUnreal(hero_enermy, RoleSide.Enemy, config_enermy, extra, birthPos);
//            role.MonIdx = hero_enermy.favoriteState + 1;
//            this.AddRole(role);
//            // 设置role avatar的位置
//            role.Position = birthPos;
//        }
//    }

//    /// <summary>
//    /// 生成怪物单位信息配置
//    /// </summary>
//    /// <param name="isboss"></param>
//    /// <param name="index"></param>
//    /// <returns></returns>
//    private RoleConfig CreateConfig(bool isboss, int index)
//    {
//        RoleConfig config = new RoleConfig();
//        config.IsBoss = false;
//        config.IsMonster = true;
//        config.Scale = 1;
//        config.HPFactor = (this.CombatConfig.MonsterHPPct > 0 ? this.CombatConfig.MonsterHPPct : DataManager.Numerics[10].Param2) / (float)DataManager.Numerics[10].Param2;
//        config.DmgPerSecFactor = (this.CombatConfig.MonsterDPSPct > 0 ? this.CombatConfig.MonsterDPSPct : DataManager.Numerics[10].Param2) / (float)DataManager.Numerics[10].Param2;
//        config.Money = (int)typeof(CombatConfigData).GetProperty(string.Format("MoneyReward{0}", index)).GetValue(this.CombatConfig, null);
//        config.PredictQuality = true;
//        config.PredictTalent = true;
//        config.PredictMaxQuality = false;
//        if (isboss)
//        {
//            config.IsBoss = true;
//            config.Scale = config.Scale * (this.CombatConfig.BOSSSIZEPct > 0 ? this.CombatConfig.BOSSSIZEPct : DataManager.Numerics[7].Param) / DataManager.Numerics[10].Param2;
//            config.HPFactor = config.HPFactor * (this.CombatConfig.BossHPPct > 0 ? this.CombatConfig.BossHPPct : DataManager.Numerics[10].Param2) / DataManager.Numerics[10].Param2;
//            config.DmgPerSecFactor = config.DmgPerSecFactor * (this.CombatConfig.BossDPSPct > 0 ? this.CombatConfig.BossDPSPct : DataManager.Numerics[10].Param2) / DataManager.Numerics[10].Param2;
//        }

//        return config;
//    }
//    /// <summary>
//    /// pve类战斗调用：初始化己方英雄（InitPlayerHeroes、InitMonsters）
//    /// </summary>
//    /// <param name="heroList"></param>
//    /// <param name="isbot"></param>
//    public void InitPlayerHeroes(List<Common.Hero> heroList, bool isbot)
//    {
//        //初始化玩家英雄数据
//        this.HeroList = heroList;
//        //this.HeroList.Sort(new HeroAttackRangeComparer());
//        QuickSort.Sort(HeroList, new HeroAttackRangeComparer());
//        RoleConfig config = new RoleConfig();
//        config.PredictQuality = DataManager.Instance.GlobalConfig.EnableMaxAttStrategy ? false : isbot;
//        config.PredictTalent = DataManager.Instance.GlobalConfig.EnableMaxAttStrategy ? false : isbot;
//        config.PredictMaxQuality = DataManager.Instance.GlobalConfig.EnableMaxAttStrategy ? true : false;
//        foreach (Common.Hero hero in this.HeroList)
//        {
//            Role role = Role.Create(hero, RoleSide.Player, config, null);
//            this.AddRole(role);
//            this.HeroIDList.Add(hero.tid);
//            //TODO:Init Role hire data here
//            role.activeTalentReadyToGo = isbot;
//        }
//    }
//    /// <summary>
//    /// pve类战斗调用：怪物初始化（InitPlayerHeroes、InitMonsters）
//    /// </summary>
//    /// <param name="ls"></param>
//    /// <param name="boss"></param>
//    private void InitMonsters(Dictionary<int, Common.HeroExtra> ls, int boss)
//    {
//        Common.HeroExtra extra = null;
//        for (int i = 0, curIndex = 0; i < MAX_POSITION; i++)
//        {
//            extra = ((ls != null && ls.ContainsKey(i)) ? ls[i] : null);
//            int id = (int)typeof(CombatConfigData).GetProperty(string.Format("Monster{0}ID", i + 1)).GetValue(this.CombatConfig, null);
//            if (id <= 0)
//                continue;

//            totalMonster++;
//            if (extra != null && extra.HP == 0)
//                continue;

//            int lv = (int)typeof(CombatConfigData).GetProperty(string.Format("Level{0}", i + 1)).GetValue(this.CombatConfig, null);
//            int st = (int)typeof(CombatConfigData).GetProperty(string.Format("Stars{0}", i + 1)).GetValue(this.CombatConfig, null);
//            RoleConfig cfg = CreateConfig((i + 1) == boss, i + 1);
//            RoleData roleData = DataManager.Instance.Roles[id];

//            Common.Hero hero = new Common.Hero();
//            hero.tid = id;
//            hero.level = lv;
//            hero.stars = st;
//            if (roleData.CurrentSkin > 0)
//            {
//                hero.currSkinId = roleData.CurrentSkin;
//            }

//            //神器关卡boss为满级状态 YuHaizhi 20190514
//            if (CombatConfig.LevelType == "mirac" && i + 1 == boss && DataManager.Instance.Roles[id].CanMirac)
//            {
//                hero = CreateMiracBoss(hero, CombatConfig);
//            }

//            Role ro = Role.Create(hero, RoleSide.Enemy, cfg, extra);
//            ro.MonIdx = i + 1;

//            this.AddRole(ro);
//            ro.MP = (int)typeof(CombatConfigData).GetProperty(string.Format("MP{0}", ro.MonIdx)).GetValue(this.CombatConfig, null);
//            if (ro.HP > 0)
//            {
//                curIndex++;
//            }
//            //boss战测试
//            foreach (int k in this.CombatConfig.BossPosition)//设定是否为boss，再根据是否为boss战来设定大小
//            {
//                if (curIndex == k)
//                {
//                    ro.IsBoss = true;
//                }
//            }
//            //boss战测试
//            Vector2 position = new Vector2();
//            position.x = this.BattleRect.xMax * 0.5f - Const.InitialPosition[curIndex - 1].x;
//            position.y = Const.InitialPosition[curIndex - 1].y;
//            ro.Position = position;
//            ro.Joint.Controller.SetPosition(ro.Position);
//        }
//    }

//    //神器关卡boss的特殊处理 YuHaizhi 20190514
//    private Common.Hero CreateMiracBoss(Common.Hero hero, CombatConfigData data)
//    {
//        hero.legend = new HeroLegend();
//        hero.legend.status = 3;
//        if (data.RoundID == 2)
//        {
//            hero.legend.str = DataManager.Instance.GlobalConfig.MiracTransit1 + 1;
//        }
//        else if (data.RoundID == 3)
//        {
//            hero.legend.str = DataManager.Instance.GlobalConfig.MiracTransit3 + 1;
//        }
//        return hero;
//    }

//    /// <summary>
//    /// 获取场上活着的索引到的英雄
//    /// </summary>
//    /// <param name="index"></param>
//    /// <param name="side"></param>
//    /// <returns></returns>
//    public Role GetRole(int index, RoleSide side)
//    {
//        if (this.AliveRoles[side].Count >= index)
//        {
//            return this.AliveRoles[side][index - 1];
//        }
//        return null;
//    }
//    /// <summary>
//    /// 获取某阵营场上活着的英雄
//    /// </summary>
//    /// <param name="index"></param>
//    /// <param name="side"></param>
//    /// <returns></returns>
//    public IEnumerable<Element> GetSurvivors(RoleSide side, bool excludeDemon = false)
//    {
//        Role[] units = new Role[this.AliveRoles[side].Count];
//        this.AliveRoles[side].CopyTo(units);
//        for (int i = units.Length - 1; i >= 0; i--)
//        {
//            Role role = units[i];
//            if (excludeDemon && role.Config.IsDemon)
//            {
//                continue;
//            }
//            if (role.IsUnrealBoss)
//            {
//                continue;
//            }
//            yield return role;
//        }
//    }


//    public List<Role> GetRoles(RoleSide side, bool excludeDemon = false)
//    {
//        List<Role> roleList = new List<Role>();
//        foreach (Role role in this.AliveRoles[side])
//        {
//            if (excludeDemon && role.Config.IsDemon)
//            {
//                continue;
//            }

//            roleList.Add(role);
//        }

//        return roleList;
//    }


//    /// <summary>
//    /// 召唤（召唤兽）
//    /// </summary>
//    /// <param name="id"></param>
//    /// <param name="reverse"></param>
//    /// <param name="owner"></param>
//    /// <returns></returns>
//    public Role Summon(int id, bool reverse, Role owner)
//    {
//        SummonData data = DataManager.Instance.Summons[id];
//        Role summon = new Role(data, reverse, owner);
//        this.Summons.Add(summon);
//        return summon;
//    }
//    /// <summary>
//    /// 获得召唤兽
//    /// </summary>
//    /// <returns></returns>
//    public IEnumerable<Role> GetSommons()
//    {
//        foreach (Role role in this.Summons)
//        {
//            yield return role;
//        }
//    }

//    // 这个函数已经无效，不被调用了（OnEnterFame）
//    private bool checkDoBattleAction()
//    {
//        while (this.OpRecords.Count > this.NextOperationIdx)
//        {
//            OpRecord record = this.OpRecords[this.NextOperationIdx];
//            if (record.Tick == this.doneFrameCount)
//            {
//                Role role = this.Roles[record.RoleIdx];
//                if (role != null)
//                {
//                    if (record.OpCode == 0)
//                        return true;
//                }
//            }
//            else
//                this.NextOperationIdx++;
//        }
//        return false;
//    }
//    // 这个函数已经无效，不被调用了（OnEnterFame）
//    private void saveTempBattle()
//    {
//        string logs = Logger.GetLogs(true);
//        System.IO.File.WriteAllText("Logs/" + DateTime.Now.ToLongTimeString() + ".log", logs);
//    }
//    // 这个函数已经无效，不被调用了
//    public void OnEnterFame(float dt)
//    {
//        if (!this.Running)
//            return;

//        beginTime += dt;
//        if (this._battleState == BATTLE_STAGE_FIGHTING)
//        {
//            float _fight_time = FIGHTING_TIMEOUT;
//            if (this.BattleMode == E_BattleMode.Campaign)
//            {
//                _fight_time = NEW_PVE_FIGHTING_TIMEOUT;
//            }
//            else if (IsReplayMode)
//            {
//                //_fight_time = ReportReplay.getTotalTime();
//            }

//            if (beginTime >= _fight_time)
//            {
//                if (IsReplayMode || this.checkDoBattleAction())
//                {
//                    // -- 时间到了，自动结束战斗
//                    this._battleState = BATTLE_STAGE_FINISHED;
//                    this.Running = false;
//                    BATTLE_RESULT result = BATTLE_RESULT.BATTLE_RESULT_TIMEOUT;
//                    this.EndBattle(result);

//                }
//                else
//                {

//                    this._battleState = BATTLE_STAGE_FINISHED;
//                    this.Running = false;
//                    // -- 清除战斗特效
//                    //todo

//                    this.EndBattle(BATTLE_RESULT.BATTLE_RESULT_DEFEAT);
//                }
//                return;
//            }

//            // -- pvp战斗开始后，要自动保存结果
//            if (this.BattleMode == E_BattleMode.Arena)
//            {
//                int notCostTimeSec = (int)Math.Floor(beginTime);
//                if (notCostTimeSec % INTERVAL_SAVE_TEMP_BATTLE == 0 && notCostTimeSec > lastSaveTempBattleTime)
//                {
//                    // -- 保存临时结果
//                    lastSaveTempBattleTime = notCostTimeSec;
//                    this.saveTempBattle();
//                }
//            }

//            //	Logic.Instance.Scene.setTimeOutText(  beginTime) ;
//        }
//        else if (this._battleState == BATTLE_STAGE_PLAY_ANIMATION_BEFORE_START)
//        {
//            //TODO:
//            //
//        }
//        else
//        {
//            //	Running = false;
//        }
//    }

//    /// <summary>
//    /// 处理大招的微操作
//    /// </summary>
//    void ProcessOP()
//    {
//        while (this.OpRecords.Count > this.NextOperationIdx)
//        {
//            OpRecord record = this.OpRecords[this.NextOperationIdx];
//            if (record.Tick == this.doneFrameCount)
//            {
//                Role role = this.Roles[record.RoleIdx];
//                if (role != null)
//                {
//                    if (record.OpCode == 0)
//                        role.UseActiveTalent();
//                }
//                this.NextOperationIdx++;
//            }
//            else
//                break;
//        }
//    }

//    /// <summary>
//    /// 处理元件逻辑
//    /// </summary>
//    /// <param name="dt"></param>
//    void ProcessEntities(double dt)
//    {
//        // 角色实体类
//        int roleNum = this.Roles.Count;
//        for (int i = 0; i < roleNum; i++)
//        {
//            Role element = this.Roles[i];
//            if (!element.IsPause)
//            {
//#if BATTLE_LOG
//                if (Global.BattleLog)
//                    Logic.log("{0} Process >>>>>>", element.FullName);
//#endif
//                element.OnEnterFrame(dt);
//            }
//            else
//            {
//                if (element.Joint.Controller != null)
//                    element.Joint.Controller.SetAnimationSpeed(0);
//            }
//            roleNum = this.Roles.Count;
//        }

//        // 技能实体类Effect
//        int CastCount = this.CastEffects.Count;
//        for (int i = 0; i < CastCount; i++)
//        {
//            Element element = this.CastEffects[i];
//            if (!element.IsPause)
//                element.OnEnterFrame(dt);
//            CastCount = this.CastEffects.Count;
//        }
//        this.CastEffects.RemoveAll(new Predicate<Element>((x) => { return x.Running == false; }));

//        // 召唤物
//        foreach (Role element in this.Summons)
//        {
//            if (!element.IsPause)
//                element.OnEnterFrame(dt);
//        }
//        this.Summons.RemoveAll(new Predicate<Role>((x) => { return x.Running == false; }));
//    }


//    /// <summary>
//    ///  重置场上英雄角色数据
//    /// </summary>
//    private void ResetRoles()
//    {
//        Role[] roles = this.Roles.ToArray();
//        this.Roles.Clear();
//        this.AliveRoles[RoleSide.Both].Clear();
//        this.AliveRoles[RoleSide.Player].Clear();
//        this.AliveRoles[RoleSide.Enemy].Clear();
//        this.AliveCount[RoleSide.Enemy] = 0;
//        this.AliveCount[RoleSide.Player] = 0;
//        this.DeadCount[RoleSide.Enemy] = 0;
//        this.DeadCount[RoleSide.Player] = 0;
//        this.DeadRoles.Clear();

//        foreach (Role role in roles)
//        {
//            if (role.Side == RoleSide.Player)
//                this.AddRole(role);
//        }
//    }

//    /// <summary>
//    ///  重置场上英雄传奇效果
//    /// </summary>
//    public void ResetLegendRoles()
//    {
//        Role[] roles = this.Roles.ToArray();
//        foreach (Role role in roles)
//        {
//            if (role.Side == RoleSide.Player)
//            {
//                if (role.Hero != null && Role.IsLegend(role.Hero))
//                {
//                    role.Joint.Controller.AddEffect(DataManager.Instance.Effects[92], -1, EffectType.Normal);
//                }
//            }
//        }
//    }
//    /// <summary>
//    /// 初始化己方英雄出生位置？？？？
//    /// </summary>
//    public void InitPlayerAliveRoles()
//    {
//        int i = 0;
//        foreach (Role role in this.AliveRoles[RoleSide.Player])
//        {
//            Vector2 position = new Vector2();
//            position.x = Const.InitialPosition[i].x - this.BattleRect.xMax * 0.5f;//从战场最左侧外出生
//            position.y = Const.InitialPosition[i].y;
//            i++;
//            role.Joint.Disable = false;
//            role.Position = position;//在战斗中的开始位置
//        };
//    }
//    /// <summary>
//    /// 初始化进入战斗的关卡数据
//    /// </summary>
//    public void InitBattleCore()
//    {
//        //重置关卡数据
//        this.Enabled = true;
//        this.doneFrameCount = 0;
//        //this.fixedFrameTime = 0.125f;

//        this.HeroIDList.Clear();
//        this.Statistic.Clear();
//        this.Roles.Clear();
//        this.OpRecords.Clear();
//        this.NextOperationIdx = 0;
//        this.IsReplayMode = false;
//        this.IsLiveMode = false;
//        this.TargetDoneFrameCount = 0;
//        this.IsTryAgain = false;
//        this.IsBossLevel = false;
//        this.TreasureTypeID = 0;
//        this.LevelEnded = false;
//        this.BattleRect = new Rect(0, DataManager.Numerics[78].Param, DataManager.Numerics[79].Param, DataManager.Numerics[79].Param2);
//        this.CastEffects.Clear();
//        this.Summons.Clear();
//        ResetRoles();
//        this.totalMonster = 0;
//        this.PauseLevel = 0;
//        this.PauseSide = RoleSide.None;
//        this.Supplied = false;
//        this.Running = true;
//#if !SERVER
//        this.Running = (this.BattleMode != E_BattleMode.Unreal);
//#endif
//        this.BattleEnd = false;
//        //this.accDeltaTime = fixedFrameTime;
//        this.LimitTime = (this.BattleMode != E_BattleMode.GuildInstance || this.LimitTime <= 0) ? defaultTimeLimit : this.LimitTime;
//        this.GoldCount = 0;
//        this.LootCount = 0;

//        this.playerTotemType = 0;
//        this.playerTotemLevel = 0;
//        this.enemyTotemType = 0;
//        this.enemyTotemLevel = 0;
//#if BATTLE_LOG
//        if (Global.BattleLog)
//        {
//            Logic.log("ResetLevel");
//        }
//#endif
//    }
//    /// <summary>
//    /// 战斗初始化(pve战斗)
//    /// </summary>
//    /// <param name="combat"></param>
//    /// <param name="extra_list"></param>
//    public void InitBattle(CombatConfigData combat, Dictionary<int, Common.HeroExtra> ls = null)
//    {
//        //初始化战斗数据
//        this.CastEffects.Clear();
//        this.Summons.Clear();
//        this.DeathPositions.Clear();
//        this.DeathPositionEffectJoints.Clear();
//        this.DeadRoles.Clear();

//        ResetRoles();
//        this.totalMonster = 0;
//        this.PauseLevel = 0;
//        this.PauseSide = RoleSide.None;
//        this.Supplied = false;
//        this.Running = true;
//        this.BattleEnd = false;
//        //this.accDeltaTime = fixedFrameTime;
//        this.LimitTime = (this.BattleMode != E_BattleMode.GuildInstance || this.LimitTime <= 0) ? defaultTimeLimit : this.LimitTime;

//        this.CombatConfig = combat;
//        this.Round = combat.RoundID > 0 ? combat.RoundID : 1;

//        InitMonsters(ls, this.CombatConfig.BossPosition.Count > 0 ? this.CombatConfig.BossPosition[0] : 0);
//        InitPlayerAliveRoles();
//        foreach (Role role in this.Roles)
//        {
//            role.Init();
//        }
//        Level level = new Level(this.LevelData.LevelID, this.Round);
//        level.Config();

//        if (this.Scene != null)
//        {
//            this.Scene.OnBattleStart("pve", combat);
//        }
//    }
//    /// <summary>
//    /// 战斗初始化(pvp战斗)
//    /// </summary>
//    public void InitPvpBattle(CombatConfigData combat)
//    {
//        this.DeathPositions.Clear();
//        this.DeathPositionEffectJoints.Clear();
//        this.DeadRoles.Clear();
//        if (this.Scene != null)
//        {
//            this.Scene.OnBattleStart("pvp", combat);
//        }
//    }

//    public void Rest(bool bEnemyRest = false)
//    {
//        foreach (Role role in this.GetSurvivors(bEnemyRest ? RoleSide.Enemy : RoleSide.Player))
//        {
//            float factor = 1f;
//            if (this.LevelData.MpsRestraint > 0)
//            {
//                factor = this.LevelData.MpsRestraint / (float)DataManager.Numerics[10].Param2;
//            }
//            role.Rest(factor);
//            role.ClearAbilities();
//        }
//        if (!bEnemyRest)
//        {
//            this.Supplied = true;
//        }
//    }

//    /// <summary>
//    /// 结束战斗
//    /// </summary>
//    /// <param name="result"></param>
//    public void EndBattle(Common.BATTLE_RESULT result)
//    {
//        //客户端打印Log
//        //string allLogs = Logger.GetLogs();
//        //string logname = "ClientLog" + ".log";
//        //File.WriteAllText("./" + logname, allLogs);

//#if !SERVER
//        DebugStatisticsUtil.Instance.LogFpsCheckbattle(false);
//#endif

//        if (this.BattleMode == E_BattleMode.Treasure && result == BATTLE_RESULT.BATTLE_RESULT_DEFEAT)
//        {
//            this.Rest(true);
//        }
//#if !SERVER
//        Time.timeScale = 1.0f;
//#endif
//        this.Enabled = false;
//        this.Running = false;
//        this.BattleEnd = false;
//        this.LevelEnded = true;
//        this.BattleResult = result;

//        BattleParam.WinReason winReson = result == BATTLE_RESULT.BATTLE_RESULT_VICTORY ? BattleParam.WinReason.Normal : BattleParam.WinReason.None;
//        // 王者竞技场+公会战+争霸战（战斗结果判断--超时、存活、伤害）
//        if (this.BattleMode == E_BattleMode.GrandArena ||
//            this.BattleMode == E_BattleMode.GuildChampion ||
//            this.BattleMode == E_BattleMode.PersonChampion ||
//            this.BattleMode == E_BattleMode.WarOfGods ||
//            this.BattleMode == E_BattleMode.BraveMelee
//            )
//        {
//            BATTLE_RESULT _result = result;
//            // 超时处理
//            if (result == BATTLE_RESULT.BATTLE_RESULT_TIMEOUT)
//            {
//                // 己方剩余英雄数量较少
//                int playerDemonCount = 0;
//                int enemyDemonCount = 0;
//                foreach (Role role in this.AliveRoles[RoleSide.Player])
//                {
//                    if (role.Config.IsDemon)
//                    {
//                        playerDemonCount++;
//                    }
//                }

//                foreach (Role role in this.AliveRoles[RoleSide.Enemy])
//                {
//                    if (role.Config.IsDemon)
//                    {
//                        enemyDemonCount++;
//                    }
//                }

//                var playerAlives = AliveCount[RoleSide.Player] - playerDemonCount;
//                var enemyAlives = AliveCount[RoleSide.Enemy] - enemyDemonCount;

//                // 诸神战考虑替补
//                if (BattleMode == E_BattleMode.WarOfGods)
//                {
//                    playerAlives += SubtituteRoles.FindAll(sub => sub.Side == RoleSide.Player).Count;
//                    enemyAlives += SubtituteRoles.FindAll(sub => sub.Side == RoleSide.Enemy).Count;
//                }

//                if (playerAlives < enemyAlives)
//                    _result = BATTLE_RESULT.BATTLE_RESULT_DEFEAT;
//                // 己方剩余英雄数量较多
//                else if (playerAlives > enemyAlives)
//                {
//                    _result = BATTLE_RESULT.BATTLE_RESULT_VICTORY;
//                    winReson = BattleParam.WinReason.AliveWin;
//                }
//                else
//                {
//                    // 剩余人数相同时，总伤害较多者获胜
//                    float playerDamage = Statistic.GetSideTotalDamage(RoleSide.Player);
//                    float enemyDamage = Statistic.GetSideTotalDamage(RoleSide.Enemy);
//                    if (playerDamage < enemyDamage)
//                        _result = BATTLE_RESULT.BATTLE_RESULT_DEFEAT;
//                    else
//                    {
//                        _result = BATTLE_RESULT.BATTLE_RESULT_VICTORY;
//                        winReson = BattleParam.WinReason.DamageWin;
//                    }
//                }
//            }
//            this.BattleResult = _result;
//        }

//        if (Logic.Instance.Scene != null)
//        {
//            if (this.IsReplayMode)
//            {
//                if (result != Common.BATTLE_RESULT.BATTLE_RESULT_CANCELED)
//                {
//                    // 战斗回放结果显示（TODO:）
//                    foreach (Role role in this.GetSurvivors(RoleSide.Enemy))
//                    {
//                        if (role.Joint != null)
//                            role.Joint.Waiting();
//                    }

//#if !SERVER
//                    // 直播战斗的处理
//                    if (this.IsLiveMode == true)
//                    {
//                        if (this.BattleMode == E_BattleMode.GuildChampion)
//                        {
//                            // 公会战直播
//                            if (UIBattleGuildChampion.instance != null)
//                                UIBattleGuildChampion.instance.StartCoroutine(DelayInvoker.Invoke(UIBattleGuildChampion.instance.EndBattle, 2f));
//                        }
//                        else if (this.BattleMode == E_BattleMode.PersonChampion)
//                        {
//                            // 争霸战直播（本场结束--显示1s结果）
//                            Server.PCSPlusBattleReplay replayInfo = PCSBuffer.BattleLive.Instance.LiveBattleReplayList[PCSBuffer.BattleLive.Instance.LiveBattleInfo.battleIndex - 1];
//                            GameObject resultUI = (GameObject)MonoBehaviour.Instantiate(ResourcesLoader.Load("UI/prefab/UIPCSLiveBattleResult"));
//                            resultUI.GetComponent<UIPCSLiveBattleResult>().InitUIData(replayInfo);
//                        }

//                        return;
//                    }

//                    // 非直播战斗的处理
//                    if (BattleMode == E_BattleMode.WorldBoss || BattleMode == E_BattleMode.WorldBossAuto)
//                    {
//                        GameObject go = GameObject.Instantiate(ResourcesLoader.Load("UI/prefab/UIWorldBossEnd")) as GameObject;
//                        go.GetComponent<Canvas>().planeDistance = 29;
//                    }
//                    else
//                    {
//                        GameObject o =
//                            (GameObject)MonoBehaviour.Instantiate(ResourcesLoader.Load("UI/prefab/UIArenaRecord"));
//                        if (BattleMode == E_BattleMode.GrandArena)
//                        {
//                            // 王者竞技场
//                            if (Logic.Instance.TopPvpReplayRecord != null)
//                            {
//                                //o.GetComponent<UIArenaRecord>().setRecordData(Logic.Instance.TopPvpReplayRecord, true);
//                                o.GetComponent<UIArenaRecord>()
//                                    .DelaySetRecordData(2f, Logic.Instance.TopPvpReplayRecord, true);
//                            }
//                        }
//                        else if (BattleMode == E_BattleMode.PersonChampion)
//                        {
//                            // 争霸战
//                            if (Logic.Instance.PersonalChampionRecord != null)
//                            {
//                                //o.GetComponent<UIArenaRecord>().setRecordData(Logic.Instance.TopPvpReplayRecord, true);
//                                o.GetComponent<UIArenaRecord>()
//                                    .DelaySetRecordData(2f, Logic.Instance.PersonalChampionRecord, true);
//                            }
//                        }
//                        else if (BattleMode == E_BattleMode.HallOfWar)
//                        {
//                            // 战斗营地
//                            if (Instance.HallOfWarRecord != null)
//                            {
//                                //o.GetComponent<UIArenaRecord>().setRecordData(Logic.Instance.TopPvpReplayRecord, true);
//                                o.GetComponent<UIArenaRecord>().DelaySetRecordData(2f, Instance.HallOfWarRecord, true);
//                            }
//                        }
//                        else if (BattleMode == E_BattleMode.WarOfGods)
//                        {
//                            // 诸神战
//                            if (Instance.WarOfGodsRecord != null)
//                            {
//                                o.GetComponent<UIArenaRecord>().DelaySetRecordData(2f, Instance.WarOfGodsRecord, true);
//                            }
//                        }
//                        else
//                        {
//                            // 普通竞技场
//                            if (Logic.Instance.PvpReplayRecord != null)
//                            {
//                                //o.GetComponent<UIArenaRecord>().setRecordData(Logic.Instance.PvpReplayRecord, true);
//                                o.GetComponent<UIArenaRecord>()
//                                    .DelaySetRecordData(2f, Logic.Instance.PvpReplayRecord, true);
//                            }

//                        }
//                    }
//#endif
//                }

//                // 战斗回放直接return
//                return;
//            }

//            /*string allLogs = Logger.GetLogs();
//            string logname = "YYYYYYUUUUUU1111" + ".log";
//            File.WriteAllText("./" + logname, allLogs);*/
//            //
//            if (result == Common.BATTLE_RESULT.BATTLE_RESULT_VICTORY)
//            {
//                //TODO:事件处理
//                if (this.BattleMode != E_BattleMode.Unreal)
//                {
//                    this.Scene.CollectLoots(3f);
//                }
//            }
//            else if (result == Common.BATTLE_RESULT.BATTLE_RESULT_DEFEAT)
//            {
//                //TODO:事件处理
//                foreach (Role role in this.GetSurvivors(RoleSide.Enemy))
//                {
//                    if (role.Joint != null)
//                        role.Joint.Waiting();
//                }
//                //注册关卡退出事件
//            }
//            else if (result == Common.BATTLE_RESULT.BATTLE_RESULT_CANCELED)
//            {
//                //TODO:事件处理
//            }
//            else if (result == Common.BATTLE_RESULT.BATTLE_RESULT_TIMEOUT)
//            {
//                if (this.BattleMode == E_BattleMode.Crusade || this.BattleMode == E_BattleMode.CrusadeChallenge || this.BattleMode == E_BattleMode.Treasure || this.BattleMode == E_BattleMode.GrandChallenge || this.BattleMode == E_BattleMode.FirstBattle)
//                {
//                    //TODO:事件处理

//                }
//                else if (this.BattleMode == E_BattleMode.GuildInstance || this.BattleMode == E_BattleMode.Outland || this.BattleMode == E_BattleMode.Temple)
//                {
//                }
//                else
//                {//this.BattleExitProcess中,已处理退出战斗消息 _ 弹出重复UI
//                    //                    BattleParam param = new BattleParam(this.LevelData.LevelID, this.BattleMode, this.HeroIDList, this.BattleMode == E_BattleMode.Campaign, false);
//                    //                    param.LoseReson = "timeout";
//                    //                    this.Scene.BattleResultProcess(param, DataManager.Numerics[3].Arg);
//                }
//            }
//#if SERVER
//#else
//            if (!this.IsReplayMode)
//            {
//                // 刷新神龟斗士次数(胜利/失败/超时 都计算次数)
//                bool isIn = false;
//                switch (this.BattleMode)
//                {
//                    case E_BattleMode.TimeRift: 	// 时空裂缝 pve
//                        isIn = this.HeroIDList.Contains(
//                            TaskBuffer.GetHeroBattleHeroID(
//                                TaskBuffer.heroBattleTypeDict[TaskBuffer.HEROBATTLE_TYPE.ADVCHAPTER]));
//                        if (isIn)
//                        {
//                            //避免跟普通每日任务重复累加
//                            TaskBuffer.SetTimesFre(1, isIn);
//                        }
//                        break;
//                    case E_BattleMode.HeroTrial: 	// 英雄演武堂 pve
//                        isIn = this.HeroIDList.Contains(
//                            TaskBuffer.GetHeroBattleHeroID(
//                                TaskBuffer.heroBattleTypeDict[TaskBuffer.HEROBATTLE_TYPE.ADVCHAPTER]));
//                        if (isIn)
//                        {
//                            //避免跟普通每日任务重复累加
//                            TaskBuffer.SetArmFre(1, isIn);
//                        }
//                        break;
//                    case E_BattleMode.Outland: 	// 外域之门 pve
//                        isIn = this.HeroIDList.Contains(
//                            TaskBuffer.GetHeroBattleHeroID(
//                                TaskBuffer.heroBattleTypeDict[TaskBuffer.HEROBATTLE_TYPE.OUTLAND]));
//                        if (isIn)
//                        {
//                            TaskBuffer.SetOutlandFre(1, isIn);
//                        }
//                        break;
//                    case E_BattleMode.Crusade:
//                    case E_BattleMode.CrusadeChallenge: 	// 远征 pvp
//                        isIn = this.HeroIDList.Contains(
//                            TaskBuffer.GetHeroBattleHeroID(
//                                TaskBuffer.heroBattleTypeDict[TaskBuffer.HEROBATTLE_TYPE.CRUSADESTAGE]));
//                        if (isIn)
//                        {
//                            TaskBuffer.SetCompleteCrusadeStageFre(1, isIn);
//                        }
//                        break;

//                    case E_BattleMode.GrandChallenge: 	// 巅峰挑战
//                        isIn = this.HeroIDList.Contains(
//                            TaskBuffer.GetHeroBattleHeroID(
//                                TaskBuffer.heroBattleTypeDict[TaskBuffer.HEROBATTLE_TYPE.GRANDCHALLENGE]));
//                        if (isIn)
//                        {
//                            TaskBuffer.SetCompleteGrandChallengeStagePre(1, isIn);
//                        }
//                        break;
//                    case E_BattleMode.Treasure: 	// 秘宝龙穴
//                        isIn = this.HeroIDList.Contains(
//                            TaskBuffer.GetHeroBattleHeroID(
//                                TaskBuffer.heroBattleTypeDict[TaskBuffer.HEROBATTLE_TYPE.EXCAVATE]));
//                        if (isIn)
//                        {
//                            TaskBuffer.SetExcavateBattleFre(1, isIn);
//                        }
//                        break;
//                    case E_BattleMode.WarOfGods: 	// 诸神争霸
//                        int heroID = TaskBuffer.GetHeroBattleHeroID(TaskBuffer.heroBattleTypeDict[TaskBuffer.HEROBATTLE_TYPE.WAROFGODS]);
//                        if (WarOfGodsManager.Instance.Data.StartWarData != null)
//                        {
//                            if (WarOfGodsManager.Instance.Data.StartWarData.attackerUserId ==
//                                Player.Instance.data.userid)
//                            {
//                                isIn =
//                                    WarOfGodsManager.Instance.Data.StartWarData.attackerHeroes.Any(
//                                        hero => hero.@base.tid == heroID);
//                            }
//                            else
//                            {
//                                isIn =
//                                     WarOfGodsManager.Instance.Data.StartWarData.oppoHeroes.Any(
//                                         hero => hero.@base.tid == heroID);
//                            }
//                        }
//                        if (isIn)
//                        {
//                            TaskBuffer.SetWarOfGodsfre(1, isIn);
//                        }
//                        break;
//                    case E_BattleMode.HallOfWar: 	// 战队营地
//                        isIn = this.HeroIDList.Contains(
//                            TaskBuffer.GetHeroBattleHeroID(
//                                TaskBuffer.heroBattleTypeDict[TaskBuffer.HEROBATTLE_TYPE.CAMPRAID]));
//                        if (isIn)
//                        {
//                            TaskBuffer.SetCampRaidfre(1, isIn);
//                        }
//                        break;
//                }
//            }
//            //发送关卡退出消息,找地方注册监听

//            // 王者竞技场的特殊处理，added 20150805
//            if (this.BattleMode == E_BattleMode.GrandArena)
//            {
//                UITopArean2.Instance.AddBattleResult(this.BattleResult);
//                int res = UITopArean2.Instance.GetBattleResult();
//                if (res == 0)
//                {
//                    BattleParam param = new BattleParam(this.LevelData.LevelID, this.BattleMode, this.HeroIDList, false,
//                        this.BattleResult == BATTLE_RESULT.BATTLE_RESULT_VICTORY, winReson);
//                    this.Scene.BattleResultProcess(param, DataManager.Numerics[3].Arg);
//                }
//                else
//                {
//                    BattleParam param = new BattleParam(this.LevelData.LevelID, this.BattleMode, this.HeroIDList, false,
//                        this.BattleResult == BATTLE_RESULT.BATTLE_RESULT_VICTORY, winReson);
//                    this.Scene.BattleResultProcess(param, DataManager.Numerics[3].Arg);

//                    Client.ClientMessage msg = CreateCombatMsg(result);
//                    NetworkManager.Instance.SendProtobufMessage(msg);
//                }

//            }
//            else if (BattleMode == E_BattleMode.WarOfGods)//诸神
//            {
//                UIBattle.Instance.WarOfGodsRegion.BattlOver();
//                WarOfGodsManager.Instance.WarOfGodsEndBattleReq(returnData =>
//                {
//                    var param = new BattleParam(LevelData.LevelID, BattleMode, HeroIDList, false,
//                        BattleResult == BATTLE_RESULT.BATTLE_RESULT_VICTORY, winReson);
//                    Scene.BattleResultProcess(param, DataManager.Numerics[3].Arg);
//                });
//            }
//            else if (BattleMode == E_BattleMode.BraveMelee)//勇者乱斗
//            {
//                UIBattle.Instance.BaraveMeleeRegion.BattlOver();
//                bool isWin = false;
//                if (BraveMeleeManager.Instance.Data.StartWarData.attackerUserId == Player.Instance.data.userid)
//                {
//                    isWin = BraveMeleeManager.Instance.Data.StartWarData.isWin == 1;
//                }
//                else
//                {
//                    isWin = BraveMeleeManager.Instance.Data.StartWarData.isWin == 2;
//                }

//                BraveMeleeManager.Instance.BraveMeleeEndBattleReq(BraveMeleeManager.Instance.Data.StartWarData.checkid, returnData =>
//                {
//                    BraveMeleeManager.Instance.Data.BEndBattle = true;
//                    var param = new BattleParam(LevelData.LevelID, BattleMode, HeroIDList, false, isWin, winReson);
//                    Scene.BattleResultProcess(param, 1.5f);
//                });
//            }
//            else if (BattleMode == E_BattleMode.Unreal)//ChickenDinner 战斗结束逻辑处理
//            {
//                if (!UnrealTutorailManager.Instance.isNewPlayer)
//                {
//                    UnrealDataManager.Instance.EndBattleReq();
//                }
//            }
//            else
//            {
//                // 刷新关卡信息
//                if (result == BATTLE_RESULT.BATTLE_RESULT_VICTORY && !this.IsReplayMode)
//                {
//                    LevelBuffer.CompleteLevel(this.LevelData.LevelID, this.ResultStars, this.BattleMode);

//                    TaskBuffer.triggerFarmStageTask(this.LevelData.LevelID);
//                    //更新任务追踪面板
//                    TaskBuffer.triggerTrackTask("CompleteStage", 1, this.LevelData.LevelID);

//                    // 刷新每日任务次数
//                    switch (this.BattleMode)
//                    {
//                        case E_BattleMode.Campaign: 	// 关卡
//                            if (LevelBuffer.currentSelectLevel >= LevelBuffer.EliteLevelStarIndex && LevelBuffer.currentSelectLevel <= 20000)
//                                Player.Instance.SetEliteRecord(LevelBuffer.currentSelectLevel, 1);

//                            //补充神器关卡次数 YuHaizhi 20190128
//                            else if (LevelBuffer.IsMiracLevel(LevelBuffer.currentSelectLevel))
//                                Player.Instance.SetMiracRecord(LevelBuffer.currentSelectLevel, 1);
//                            break;
//                        case E_BattleMode.TimeRift: 	// 时空裂缝 pve
//                            TaskBuffer.SetTimesFre(1);
//                            //更新任务追踪面板
//                            TaskBuffer.triggerTrackTask("FarmChapter102");
//                            break;
//                        case E_BattleMode.HeroTrial: 	// 英雄演武堂 pve
//                            TaskBuffer.SetArmFre(1);
//                            //更新任务追踪面板
//                            TaskBuffer.triggerTrackTask("FarmChapter103");
//                            break;
//                        case E_BattleMode.Outland: 		// 外域之门 pve
//                            TaskBuffer.SetOutlandFre(1);
//                            //更新任务追踪面板
//                            TaskBuffer.triggerTrackTask("FarmChapter105");
//                            break;
//                        case E_BattleMode.Crusade:		// 远征 pvp
//                            TaskBuffer.SetCompleteCrusadeStageFre(1);
//                            //更新任务追踪面板
//                            TaskBuffer.triggerTrackTask("CompleteCrusadeStage");
//                            break;
//                        case E_BattleMode.GrandChallenge:		// 巅峰挑战
//                            TaskBuffer.SetCompleteGrandChallengeStagePre(1);
//                            break;
//                        default:
//                            break;
//                    }
//                }

//                Client.ClientMessage msg = CreateCombatMsg(result);
//                if (this.BattleMode == E_BattleMode.GuildInstance)
//                {
//                    GuildBuffer.InstanceEndReq(msg, () =>
//                    {
//                        Logic.Instance.BattleExitProcess(Common.BATTLE_RESULT.BATTLE_RESULT_VICTORY, false);
//                    });
//                }
//                else if (this.BattleMode == E_BattleMode.FirstBattle)
//                {
//                    BattleParam param = new BattleParam(this.LevelData.LevelID, this.BattleMode, this.HeroIDList, this.BattleMode == E_BattleMode.Campaign, false);
//                    param.Victory = true;
//                    this.Scene.BattleResultProcess(param, DataManager.Numerics[3].Arg);
//                }
//                else if (this.BattleMode == E_BattleMode.HallOfWar)
//                {
//                    WarCampManager.Instance.EndWarRequest(msg, returnData =>
//                    {
//                        returnData.result = returnData.isWin == 1 ? BATTLE_RESULT.BATTLE_RESULT_VICTORY : BATTLE_RESULT.BATTLE_RESULT_DEFEAT;
//                        BattleExitProcess(returnData.result, false);
//                    });
//                }
//                else if (this.BattleMode == E_BattleMode.BattleTest)
//                {
//                    BattleParam param = new BattleParam(this.LevelData.LevelID, this.BattleMode, this.HeroIDList, this.BattleMode == E_BattleMode.Campaign, false);
//                    param.Victory = true;
//                    this.Scene.BattleResultProcess(param, DataManager.Numerics[3].Arg);
//                }
//                else
//                {
//                    NetworkManager.Instance.SendProtobufMessage(msg);
//                }
//            }
//#endif
//        }
//    }
//    /// <summary>
//    /// 战斗tick
//    /// </summary>
//    /// <param name="passTime"></param>
//    public void onTimer(double passTime)
//    {
//        if (!this.Running)
//            return;

//        // 执行微操做记录
//        this.ProcessOP();
//        // 执行实体逻辑+表现更新
//        this.ProcessEntities(passTime);
//        this.doneFrameCount++;
//#if BATTLE_LOG
//		if (Global.BattleLog)
//			Logic.log(this.ToString());
//#endif
//        ProcessSubstitute(RoleSide.Player);
//        ProcessSubstitute(RoleSide.Enemy);

//        // 战斗是否结束的处理逻辑
//        this.VerifyBattleResult(passTime);
//    }

//    public List<Role> SubtituteRoles = new List<Role>();
//    private Dictionary<RoleSide, int> _substitutDic = new Dictionary<RoleSide, int>
//    {
//        {RoleSide.Player, 0},
//        {RoleSide.Enemy, 0}
//    };

//    public void ClearSubtituteInfo()
//    {
//        SubtituteRoles.Clear();
//        _substitutDic = new Dictionary<RoleSide, int>
//        {
//            {RoleSide.Player, 0},
//            {RoleSide.Enemy, 0}
//        };
//    }

//    /// <summary>
//    /// 替补
//    /// </summary>
//    void ProcessSubstitute(RoleSide side)
//    {
//        if (BattleMode != E_BattleMode.WarOfGods)
//            return;
//        if (!_substitutDic.ContainsKey(side))
//            return;

//        if (AliveRoles[side].FindAll(r => r.IsHero).Count >= 5)
//            return;

//        if (_substitutDic[side]++ != 0)
//            return;
//        var role = SubtituteRoles.Find(sub => sub.Side == side);
//        if (role == null)
//            return;
//        role.Init();
//        role.SetPosition(role.Position);
//        AddRole(role);
//        SubtituteRoles.Remove(role);
//    }

//    /// <summary>
//    /// 战斗结果处理
//    /// </summary>
//    /// <param name="dt"></param>
//    void VerifyBattleResult(double dt)
//    {
//        this.LimitTime = this.LimitTime - dt;
//        //战斗结束条件判断
//        //没有敌人了
//        BATTLE_RESULT result = BATTLE_RESULT.BATTLE_RESULT_DEFEAT;
//        bool resume = true;
//        // 己方剩余英雄数量较少
//        int playerDemonCount = 0;
//        int enemyDemonCount = 0;
//        foreach (Role role in this.AliveRoles[RoleSide.Player])
//        {
//            if (role.Config.IsDemon)
//            {
//                playerDemonCount++;
//            }
//            if (this.BattleMode == E_BattleMode.Unreal && role.IsUnrealBoss)
//            {
//                //水晶
//                playerDemonCount++;
//            }
//        }
//        foreach (Role role in this.AliveRoles[RoleSide.Enemy])
//        {
//            if (role.Config.IsDemon)
//            {
//                enemyDemonCount++;
//            }
//        }
//        if (AliveCount[RoleSide.Enemy] - enemyDemonCount <= 0)
//        {
//            this.BattleEnd = true;
//        }
//        else if (this.LimitTime <= 0)
//        {
//            this.BattleEnd = true;
//            // 超时算是：同归于尽
//            if (this.BattleMode == E_BattleMode.Crusade || this.BattleMode == E_BattleMode.CrusadeChallenge || this.BattleMode == E_BattleMode.Treasure || this.BattleMode == E_BattleMode.HallOfWar || this.BattleMode == E_BattleMode.GrandChallenge)
//                foreach (Role role in this.Roles)
//                {
//                    role.ExParamI3 = 1;         // 避免死后复活
//                    role.End(null);
//                }
//            resume = false;
//            result = BATTLE_RESULT.BATTLE_RESULT_TIMEOUT;
//        }
//        else if (this.AliveCount[RoleSide.Player] - playerDemonCount <= 0 || (this.BattleMode == E_BattleMode.FirstBattle && this.AliveCount[RoleSide.Player] == 1))
//        {
//            this.BattleEnd = true;
//            if (this.BattleMode == E_BattleMode.Unreal)
//            {
//                //处理水晶被攻击
//                Role unrealBoss = null;
//                foreach (Role role in this.AliveRoles[RoleSide.Player])
//                {
//                    if (role.IsUnrealBoss)
//                    {
//                        unrealBoss = role;
//                        break;
//                    }
//                }
//#if !SERVER
//                if (unrealBoss != null)
//                {
//                    EffectJoint joint = new EffectJoint("fx_direct_C100103_atk4");
//                    if (joint.HaveGameObject)
//                    {
//                        joint.Controller.SetLayer(1);
//                        joint.Controller.SetPosition(new Vector3(unrealBoss.Position.x, unrealBoss.Position.y, 0));
//                        joint.Controller.Direction = 1.0f;
//                        joint.Controller.SetScale(new Vector2(1, 1));
//                    }
//                    Logic.Instance.Scene.AddJoint(joint);
//                }
//#endif
//            }
//        }
//        if (this.BattleEnd)
//        {
//            TalentDelegate.Instance.ClearAllRegister();
//            this.ResultStars = 0;
//            this.Resume(resume);
//            this.Statistic.OnRoundEnd();
//            //foreach (Role role in this.Roles)
//            for (int i = this.Roles.Count - 1; i >= 0; i--)
//            {
//                if (!this.Roles[i].Config.IsDemon)
//                {
//                    this.Roles[i].ClearAbilities();

//                    // fix:11/29/2016清理场上timer类型effect
//                    RoleExtension roleEx = this.Roles[i] as RoleExtension;
//                    if (roleEx != null)
//                    {
//                        EffectJoint joint = roleEx.effectJoint;
//                        if (joint != null)
//                        {
//                            joint.Stop();
//                            roleEx.effectJoint = null;
//                        }
//                        EffectJoint joint2 = roleEx.effectJoint2;
//                        if (joint2 != null)
//                        {
//                            joint2.Stop();
//                            roleEx.effectJoint2 = null;
//                        }
//                    }
//                }
//            }

//            this.Roles.RemoveAll(delegate (Role role)
//            {
//                return role.Config.IsDemon;
//            });

//            if (AliveCount[RoleSide.Enemy] - enemyDemonCount <= 0)
//            {
//                this.BattleEnd = false;
//                this.Running = false;

//                if (this.Round < this.LevelData.Rounds)
//                {
//                    // 公会副本，时间到了即截止
//                    if (BattleMode == E_BattleMode.GuildInstance && LimitTime <= 0)
//                        Complete();
//                    else
//                        Proceed();
//                }
//                else
//                {
//                    Complete();
//                }
//                if (Logic.Instance.Scene != null)
//                {
//                    foreach (Role role in this.Roles)
//                    {
//                        if (role.Side == RoleSide.Enemy && role.Joint != null)
//                            role.Joint.OnDeath();
//                    }
//                }
//            }
//            else if (this.BattleMode == E_BattleMode.FirstBattle && this.AliveCount[RoleSide.Player] == 1)
//            {
//                this.EndBattle(result);
//            }
//            else
//            {
//                this.EndBattle(result);
//            }
//        }
//    }
//    /// <summary>
//    /// 退出战斗消息处理
//    /// </summary>
//    /// <param name="result"></param>
//    public void BattleExitProcess(Common.BATTLE_RESULT result, bool error)
//    {
//        if (this.Scene == null)
//            return;

//        if (result == Common.BATTLE_RESULT.BATTLE_RESULT_VICTORY)
//        {
//            if (error)
//            {
//                // 服务器战斗胜利验证失败
//                BattleParam param = new BattleParam(this.LevelData.LevelID, this.BattleMode, this.HeroIDList, this.BattleMode == E_BattleMode.Campaign, false);
//                param.LoseReson = "fail";
//                this.Scene.BattleResultProcess(param, DataManager.Numerics[3].Arg);
//            }
//            else
//            {
//                //TODO:播放胜利音效
//                BattleParam param = new BattleParam(this.LevelData.LevelID, this.BattleMode, this.HeroIDList, this.BattleMode == E_BattleMode.Campaign, true);
//                param.IsReplay = this.IsReplayMode;
//                this.Scene.BattleResultProcess(param, DataManager.Numerics[10].Arg);//战斗胜利
//            }
//        }
//        else if (result == Common.BATTLE_RESULT.BATTLE_RESULT_DEFEAT)
//        {
//            BattleParam param = new BattleParam(this.LevelData.LevelID, this.BattleMode, this.HeroIDList, this.BattleMode == E_BattleMode.Campaign, false);
//            param.LoseReson = "fail";
//            this.Scene.BattleResultProcess(param, DataManager.Numerics[3].Arg);
//            //TODO:修复经验获取
//        }
//        else if (result == BATTLE_RESULT.BATTLE_RESULT_TIMEOUT)
//        {
//            BattleParam param = new BattleParam(this.LevelData.LevelID, this.BattleMode, this.HeroIDList, this.BattleMode == E_BattleMode.Campaign, false);
//            param.LoseReson = "timeout";
//            this.Scene.BattleResultProcess(param, DataManager.Numerics[3].Arg);
//        }
//    }

//    private void Proceed()
//    {
//        // 清理召唤物
//        foreach (Role role in this.GetSurvivors(RoleSide.Player))
//        {
//            if (role.Config.IsDemon)
//                role.End(null);
//        }
//        foreach (Role role in this.GetSurvivors(RoleSide.Enemy))
//        {
//            if (role.Config.IsDemon)
//                role.End(null);
//        }
//        if (Logic.Instance.Scene != null)
//        {
//            if (this.Scene != null)
//                this.Scene.ShowNextButton();

//            foreach (Role role in this.GetSurvivors(RoleSide.Player))
//            {
//                if (role.Joint != null)
//                {
//                    role.Joint.Waiting();
//                    ((RoleExtension)role).isCastingManualSkill = false;
//                }

//            }
//        }
//        else
//        {
//            if (!this.Supplied)
//                this.Rest(false);

//            CombatConfigData combatcfg = DataNewManager.Instance.CombatConfigs.Value[this.LevelData.LevelID][this.Round + 1];
//            this.InitBattle(combatcfg);
//        }
//    }
//    /// <summary>
//    /// 结束战斗
//    /// </summary>
//    private void Complete()
//    {
//        int deadNum = 0;
//        foreach (Role role in this.Roles)
//        {
//            if (role.Side == RoleSide.Player && role.IsHero && role.Config.IsDemon != true && role.IsDead)
//                deadNum++;
//        }
//        // 战斗结果的评星（简单计算）
//        this.ResultStars = Mathf.Max(1, 3 - deadNum);
//#if !SERVER
//        if (this.BattleMode == E_BattleMode.Crusade && null != CrusadeBuffer.crusadeReply &&
//            CrusadeBuffer.crusadeReply.openPanel.info.mode == Server.CrusadeInfo.CRUSADE_MODE.CRUSADE_MODE_NORMAL)
//            this.Rest(false);

//        if (this.BattleMode == E_BattleMode.CrusadeChallenge && null != CrusadeChallengeBuffer.Instance.data &&
//            CrusadeChallengeBuffer.Instance.data.openPanel.info.mode == Server.CrusadeChallengeInfo.CRUSADE_MODE.CRUSADE_MODE_NORMAL)
//            this.Rest(false);
//#else
//        if (this.BattleMode == E_BattleMode.Crusade)
//            this.Rest(false);

//		if (this.BattleMode == E_BattleMode.CrusadeChallenge)
//			this.Rest(false);
//#endif

//        if (this.BattleMode == E_BattleMode.Treasure)
//            this.Rest(false);

//        this.EndBattle(Common.BATTLE_RESULT.BATTLE_RESULT_VICTORY);
//        if (Logic.Instance.Scene != null)
//        {
//            this.LevelEnded = true;
//            foreach (Role role in this.GetSurvivors(RoleSide.Player))
//            {
//                role.ClearAbilities();
//                role.InitAttributes();
//                if (role.Joint != null)
//                    role.Joint.Cheer();
//            }
//        }
//    }

//    public void GetCurrentHeroesData(List<HeroData> myroles, List<HeroData> enemies, List<HeroData> hires)
//    {
//        foreach (Role role in this.Roles)
//        {
//            if (!role.Config.IsDemon)
//            {
//                HeroData hero = new HeroData();
//                hero.tid = role.ID;
//                HeroExtra extraData = new HeroExtra();
//                extraData.HP = (int)Math.Ceiling(role.HP * Const.PercentageRatio / role.Attributes.HP);
//                extraData.MP = (int)Math.Ceiling(role.MP * Const.PercentageRatio / role.Attributes.MP);
//                if (role.ExtraData != null)
//                {
//                    extraData.ExtraData = role.ExtraData.ExtraData;
//                }
//                if (role.ExParamI3 != 0)
//                {
//                    extraData.ExtraData = role.ExParamI3;
//                }
//                hero.extra = extraData;

//                if (role.Side == RoleSide.Player)
//                {
//                    if (myroles != null)
//                    {
//                        myroles.Add(hero);
//                    }
//                    if (hires != null && role.IsHired)
//                        hires.Add(hero);
//                }
//                else if (role.Side == RoleSide.Enemy)
//                {
//                    if (enemies != null)
//                    {
//                        enemies.Add(hero);
//                    }
//                }
//            }
//        }
//    }


//    public void GetCurrentHeroesDataDic(Dictionary<int, HeroData> myroles, Dictionary<int, HeroData> enemies, Dictionary<int, HeroData> hires)
//    {
//        foreach (Role role in this.Roles)
//        {
//            if (!role.Config.IsDemon)
//            {
//                HeroData hero = new HeroData();
//                hero.tid = role.ID;
//                HeroExtra extraData = new HeroExtra();
//                extraData.HP = (int)Math.Ceiling(role.HP * Const.PercentageRatio / role.Attributes.HP);
//                extraData.MP = (int)Math.Ceiling(role.MP * Const.PercentageRatio / role.Attributes.MP);
//                if (role.ExtraData != null)
//                {
//                    extraData.ExtraData = role.ExtraData.ExtraData;
//                }
//                if (role.ExParamI3 != 0)
//                {
//                    extraData.ExtraData = role.ExParamI3;
//                }
//                hero.extra = extraData;

//                if (role.Side == RoleSide.Player)
//                {
//                    if (myroles != null)
//                    {
//                        myroles.Add(hero.tid, hero);
//                    }
//                    if (hires != null && role.IsHired)
//                        hires.Add(hero.tid, hero);
//                }
//                else if (role.Side == RoleSide.Enemy)
//                {
//                    if (enemies != null)
//                    {
//                        enemies.Add(hero.tid, hero);
//                    }
//                }
//            }
//        }
//    }


//    /// <summary>
//    /// 填充战斗结束上行消息
//    /// </summary>
//    /// <param name="result"></param>
//    /// <returns></returns>
//    public Client.ClientMessage CreateCombatMsg(Common.BATTLE_RESULT result)
//    {
//        Client.ClientMessage msg = new Client.ClientMessage();
//#if !SERVER
//        switch (this.BattleMode)
//        {
//            case E_BattleMode.Arena:
//                {
//                    msg.ladder = new Ladder();
//                    msg.ladder.endBattle = new LadderEndBattle();
//                    msg.ladder.endBattle.result = result;
//                }
//                break;
//            case E_BattleMode.GrandArena:
//                {
//                    msg.advArena = new Client.AdvArena();
//                    msg.advArena.endBattle = new AdvArenaEndBattle();
//                    msg.advArena.endBattle.result.AddRange(UITopArean2.Instance.battleResults);
//                }
//                break;
//            case E_BattleMode.Duel:
//            case E_BattleMode.DuelAlpha:
//                {
//                    msg.playerChallenge = new PlayerChallenge();
//                    msg.playerChallenge.endBattle = new ChallengeEndBattle();
//                    msg.playerChallenge.endBattle.say = new ChatSay();
//                    msg.playerChallenge.endBattle.result = result;

//                    msg.playerChallenge.endBattle.say.contentType = 3;
//                    msg.playerChallenge.endBattle.say.channel = CHAT_CHANNEL.CHAT_CHANNEL_PERSONAL;
//                    msg.playerChallenge.endBattle.say.content = string.Format(UICommon.Instance.GetChatColorString() + "{0}>", LocalizationManager.Instance.GetStringOrg("KEY.5353")) + " <> " + string.Format("<link|challenge|{0}>", LocalizationManager.Instance.GetStringOrg("KEY.5354"));
//                    msg.playerChallenge.endBattle.say.targetid = DefenderID;
//                    msg.playerChallenge.endBattle.oppoUserid = DefenderID;
//                    msg.playerChallenge.endBattle.say.accessory = new ChatAccessory();
//                    msg.playerChallenge.endBattle.say.accessory.type = ChatAccessory.ACCESSORY_TYPE.ACCESSORY_TYPE_BINARY;
//                    msg.playerChallenge.endBattle.say.accessory.binary = System.Text.Encoding.Default.GetBytes("type:challengereplay,");

//                    List<uint> opList = new List<uint>();
//                    foreach (OpRecord op in this.OpRecords)
//                    {
//                        uint value = 0;
//                        switch (op.OpCode)
//                        {
//                            case 0:
//                                value = (uint)MathUtil.makeBits(12, op.Tick, 4, op.RoleIdx, 3, op.OpCode);
//                                break;
//                            case 4:
//                                break;
//                        }
//                        opList.Add(value);
//                    }
//                    msg.playerChallenge.endBattle.operations.AddRange(opList);
//                }
//                break;
//            case E_BattleMode.Crusade:
//                {
//                    msg.crusade = new Crusade();
//                    msg.crusade.endBattle = new CrusadeEndBattle();
//                    msg.crusade.endBattle.stageid = -2 - LevelData.LevelID; // -2 - (-3, -4, -5) = 1, 2, 3
//                    msg.crusade.endBattle.result = result;
//                    msg.crusade.endBattle.selfTotemType = TotemDataBuffer.Instance.useType;
//                    msg.crusade.endBattle.oppoTotemType = TotemDataBuffer.Instance.oppoType;

//                    List<HeroData> self = new List<HeroData>();
//                    List<HeroData> oppo = new List<HeroData>();
//                    List<HeroData> merc = new List<HeroData>();
//                    this.GetCurrentHeroesData(self, oppo, merc);
//                    foreach (var v in self)
//                    {
//                        CrusadeHero hero = new CrusadeHero();
//                        hero.customData = v.extra.ExtraData;
//                        hero.heroid = v.tid;
//                        hero.hpPerc = v.extra.HP;
//                        hero.mpPerc = v.extra.MP;
//                        msg.crusade.endBattle.selfHeroes.Add(hero);
//                    }

//                    foreach (var v in merc)
//                    {
//                        CrusadeHero hero = new CrusadeHero();
//                        hero.customData = v.extra.ExtraData;
//                        hero.heroid = v.tid;
//                        hero.hpPerc = v.extra.HP;
//                        hero.mpPerc = v.extra.MP;
//                        msg.crusade.endBattle.selfHeroes.Add(hero);
//                    }
//                    foreach (var v in oppo)
//                    {
//                        CrusadeHero hero = new CrusadeHero();
//                        hero.customData = v.extra.ExtraData;
//                        hero.heroid = v.tid;
//                        hero.hpPerc = v.extra.HP;
//                        hero.mpPerc = v.extra.MP;
//                        msg.crusade.endBattle.oppoHeroes.Add(hero);
//                    }
//                    List<int> list = new List<int>();
//                    foreach (OpRecord op in this.OpRecords)
//                    {
//                        int value = 0;
//                        switch (op.OpCode)
//                        {
//                            case 0:
//                                value = MathUtil.makeBits(12, op.Tick, 4, op.RoleIdx, 3, op.OpCode);
//                                break;

//                            case 4:
//                                break;
//                        }
//                        list.Add(value);
//                    }
//                    msg.crusade.endBattle.operations.AddRange(list);

//                    Debug.Log("<<<<<<<<<<<<<<<<<<<<<<<<Crusade End Data>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");

//                }
//                break;

//            case E_BattleMode.CrusadeChallenge:
//                {
//                    msg.crusadeChallenge = new CrusadeChallenge();
//                    msg.crusadeChallenge.endBattle = new CrusadeChallengeEndBattle();
//                    msg.crusadeChallenge.endBattle.stageid = -2 - LevelData.LevelID; // -2 - (-3, -4, -5) = 1, 2, 3
//                    msg.crusadeChallenge.endBattle.result = result;

//                    List<HeroData> self = new List<HeroData>();
//                    List<HeroData> oppo = new List<HeroData>();
//                    List<HeroData> merc = new List<HeroData>();
//                    this.GetCurrentHeroesData(self, oppo, merc);
//                    foreach (var v in self)
//                    {
//                        CrusadeHero hero = new CrusadeHero();
//                        hero.customData = v.extra.ExtraData;
//                        hero.heroid = v.tid;
//                        hero.hpPerc = v.extra.HP;
//                        hero.mpPerc = v.extra.MP;
//                        msg.crusadeChallenge.endBattle.selfHeroes.Add(hero);
//                    }

//                    foreach (var v in merc)
//                    {
//                        CrusadeHero hero = new CrusadeHero();
//                        hero.customData = v.extra.ExtraData;
//                        hero.heroid = v.tid;
//                        hero.hpPerc = v.extra.HP;
//                        hero.mpPerc = v.extra.MP;
//                        msg.crusadeChallenge.endBattle.selfHeroes.Add(hero);
//                    }
//                    foreach (var v in oppo)
//                    {
//                        CrusadeHero hero = new CrusadeHero();
//                        hero.customData = v.extra.ExtraData;
//                        hero.heroid = v.tid;
//                        hero.hpPerc = v.extra.HP;
//                        hero.mpPerc = v.extra.MP;
//                        msg.crusadeChallenge.endBattle.oppoHeroes.Add(hero);
//                    }
//                    List<int> list = new List<int>();
//                    foreach (OpRecord op in this.OpRecords)
//                    {
//                        int value = 0;
//                        switch (op.OpCode)
//                        {
//                            case 0:
//                                value = MathUtil.makeBits(12, op.Tick, 4, op.RoleIdx, 3, op.OpCode);
//                                break;

//                            case 4:
//                                break;
//                        }
//                        list.Add(value);
//                    }
//                    msg.crusadeChallenge.endBattle.operations.AddRange(list);
//                    msg.crusadeChallenge.endBattle.dbuffType = CrusadeChallengeBuffer.Instance.debufferType;
//                    msg.crusadeChallenge.endBattle.selfTotemType = TotemDataBuffer.Instance.useType;
//                    msg.crusadeChallenge.endBattle.oppoTotemType = TotemDataBuffer.Instance.oppoType;
//                    Debug.Log("<<<<<<<<<<<<<<<<<<<<<<<<Crusade End Data>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");

//                }
//                break;

//            case E_BattleMode.GuildInstance:
//                {
//                    msg.guild = new Client.Guild();
//                    msg.guild.instanceEnd = new Client.GuildInstanceEnd();
//                    msg.guild.instanceEnd.result = result;
//                    msg.guild.instanceEnd.totemType = TotemDataBuffer.Instance.useType;
//                    // 同一章中所有已通过关卡的总比重
//                    float pastWeight = 0;
//                    float totalWeight = 0;
//                    // 同一关中通过的比重
//                    float levelPastWeight = 0;
//                    float levelTotalWeight = 0;

//                    // 存放与正在打的关卡同一章中的所有关卡
//                    Dictionary<int, Dictionary<int, CombatConfigData>> tmpComConfigs = new Dictionary<int, Dictionary<int, CombatConfigData>>();
//                    foreach (int key in DataManager.Instance.Levels.Keys)
//                    {
//                        if (DataManager.Instance.Levels[key].SectionID != LevelData.SectionID)
//                            continue;
//                        tmpComConfigs.Add(key, DataNewManager.Instance.CombatConfigs.Value[key]);
//                    }

//                    CombatConfigData tmpConfig = null;
//                    foreach (int key in tmpComConfigs.Keys)
//                    {
//                        foreach (int _key in tmpComConfigs[key].Keys)
//                        {
//                            // 每一波怪物
//                            tmpConfig = tmpComConfigs[key][_key];
//                            // 小于当前关卡id的为已经通过的关卡
//                            if (key < CombatConfig.LevelID)
//                            {
//                                pastWeight += tmpConfig.RaidWaveWeight;

//                            }
//                            else if (key == CombatConfig.LevelID)
//                            {
//                                // 当前关卡中已经通过的波次
//                                if (CombatConfig.RoundID > tmpConfig.RoundID)
//                                {
//                                    pastWeight += tmpConfig.RaidWaveWeight;
//                                    levelPastWeight += tmpConfig.RaidWaveWeight;
//                                }
//                                levelTotalWeight += tmpConfig.RaidWaveWeight;
//                            }
//                            totalWeight += tmpConfig.RaidWaveWeight;
//                        }
//                    }

//                    float totalDamage = 0;
//                    float currentBleed = 0;
//                    float totalBleed = 0;
//                    int roleCount = Roles.Count;

//                    for (int _i = 0; _i < MAX_POSITION; _i++)
//                    {
//                        msg.guild.instanceEnd.hpinfo.Add(0);
//                        msg.guild.instanceEnd.extraData.Add(0);
//                    }

//                    for (int i = 0; i < roleCount; i++)
//                    {
//                        if (Roles[i].Config.IsMonster)
//                        {
//                            currentBleed += Roles[i].HP;
//                            totalBleed += Roles[i].MaxHP;
//                            msg.guild.instanceEnd.hpinfo[Roles[i].MonIdx - 1] = (int)Math.Ceiling(Roles[i].HP / Roles[i].MaxHP * 10000);
//                            //msg.guild.instanceEnd.hpinfo.Add((int)Math.Floor(Roles[i].HP / Roles[i].MaxHP * 10000));
//                            msg.guild.instanceEnd.extraData[Roles[i].MonIdx - 1] = Roles[i].ExParamI3;
//                        }
//                        // 获取伤害
//                        totalDamage += Roles[i].IsHero ? Statistic.GetHeroDamage(Roles[i].ID) : 0;
//                    }

//                    float tmpWeight = (1f - currentBleed / totalBleed) * CombatConfig.RaidWaveWeight;
//                    pastWeight += tmpWeight;
//                    levelPastWeight += tmpWeight;


//                    // 总进度
//                    msg.guild.instanceEnd.progress = (int)Math.Floor(pastWeight / totalWeight * 10000);
//                    // 本关卡的进度
//                    msg.guild.instanceEnd.stageProgress = (int)Math.Floor(levelPastWeight / levelTotalWeight * 10000);

//                    msg.guild.instanceEnd.wave = Round;
//                    msg.guild.instanceEnd.heroes.AddRange(this.HeroIDList);
//                    msg.guild.instanceEnd.damage = (int)Math.Floor(totalDamage + 0.5f);

//                    List<int> opList = new List<int>();
//                    foreach (OpRecord op in this.OpRecords)
//                    {
//                        int value = 0;
//                        switch (op.OpCode)
//                        {
//                            case 0:
//                                value = MathUtil.makeBits(12, op.Tick, 4, op.RoleIdx, 3, op.OpCode);
//                                break;
//                            case 4:
//                                break;
//                        }
//                        opList.Add(value);
//                    }
//                    msg.guild.instanceEnd.operations.AddRange(opList);
//                    msg.guild.instanceEnd.totemType = TotemDataBuffer.Instance.useType;

//                }
//                break;
//            case E_BattleMode.Treasure:
//                {
//                    msg.mine = new Mine();
//                    msg.mine.endBattle = new MineEndBattle();
//                    msg.mine.endBattle.result = result;
//                    msg.mine.endBattle.totemType = TotemDataBuffer.Instance.useType;
//                    List<HeroData> _self = new List<HeroData>();
//                    List<HeroData> _oppo = new List<HeroData>();
//                    List<HeroData> _merc = new List<HeroData>();
//                    this.GetCurrentHeroesData(_self, _oppo, _merc);
//                    foreach (var v in _self)
//                    {
//                        MineHero hero = new MineHero();
//                        hero.customData = v.extra.ExtraData;
//                        hero.heroid = v.tid;
//                        hero.hpPerc = v.extra.HP;
//                        hero.mpPerc = v.extra.MP;
//                        msg.mine.endBattle.selfHeroes.Add(hero);
//                    }
//                    foreach (var v in _merc)
//                    {
//                        MineHero hero = new MineHero();
//                        hero.customData = v.extra.ExtraData;
//                        hero.heroid = v.tid;
//                        hero.hpPerc = v.extra.HP;
//                        hero.mpPerc = v.extra.MP;
//                        msg.mine.endBattle.selfHeroes.Add(hero);
//                    }

//                    foreach (var v in _oppo)
//                    {
//                        MineHero hero = new MineHero();
//                        hero.customData = v.extra.ExtraData;
//                        hero.heroid = v.tid;
//                        hero.hpPerc = v.extra.HP;
//                        hero.mpPerc = v.extra.MP;
//                        msg.mine.endBattle.oppoHeroes.Add(hero);
//                    }

//                    List<int> list = new List<int>();
//                    foreach (OpRecord op in this.OpRecords)
//                    {
//                        int value = 0;
//                        switch (op.OpCode)
//                        {
//                            case 0:
//                                value = MathUtil.makeBits(12, op.Tick, 4, op.RoleIdx, 3, op.OpCode);
//                                break;

//                            case 4:
//                                break;
//                        }
//                        list.Add(value);
//                    }
//                    msg.mine.endBattle.operations.AddRange(list);
//                    msg.mine.endBattle.typeid = TreasureTypeID;

//                }
//                break;

//            case E_BattleMode.HallOfWar:
//                {
//                    var list = new List<int>();
//                    foreach (var op in this.OpRecords)
//                    {
//                        int value = 0;
//                        switch (op.OpCode)
//                        {
//                            case 0:
//                                value = MathUtil.makeBits(12, op.Tick, 4, op.RoleIdx, 3, op.OpCode);
//                                break;

//                            case 4:
//                                break;
//                        }
//                        list.Add(value);
//                    }
//                    msg.hallOfWar = new HallOfWar { endBattle = new HallOfWarEndBattle() };
//                    msg.hallOfWar.endBattle.operations.AddRange(list);
//                    msg.hallOfWar.endBattle.recordId = WarCampManager.Instance.GetBuildingInfo<WCBuildingHall>().RecordId;
//                }
//                break;

//            case E_BattleMode.Temple:
//                {
//                    msg.temple = new Temple();
//                    msg.temple.endBattle = new TempleEndBattle();
//                    msg.temple.endBattle.uid = ownerUserId;
//                    msg.temple.endBattle.totemType = TotemDataBuffer.Instance.useType;
//                    List<HeroData> _oppo = new List<HeroData>();
//                    this.GetCurrentHeroesData(null, _oppo, null);
//                    foreach (HeroData data in _oppo)
//                    {
//                        msg.temple.endBattle.hpInfo.Add(data.extra.HP);
//                    }

//                    msg.temple.endBattle.index = monsterIndex;
//                    List<int> list = new List<int>();
//                    foreach (OpRecord op in this.OpRecords)
//                    {
//                        int value = 0;
//                        switch (op.OpCode)
//                        {
//                            case 0:
//                                value = MathUtil.makeBits(12, op.Tick, 4, op.RoleIdx, 3, op.OpCode);
//                                break;
//                            case 4:
//                                break;
//                        }
//                        list.Add(value);
//                    }
//                    msg.temple.endBattle.operation.AddRange(list);
//                    msg.temple.endBattle.result = result;
//                }
//                break;
//            case E_BattleMode.GrandChallenge:
//                {
//                    List<HeroData> myheros = new List<HeroData>();
//                    this.GetCurrentHeroesData(myheros, null, null);

//                    List<uint> recs = new List<uint>();
//                    foreach (OpRecord op in this.OpRecords)
//                    {
//                        int value = 0;
//                        switch (op.OpCode)
//                        {
//                            case 0:
//                                value = MathUtil.makeBits(12, op.Tick, 4, op.RoleIdx, 3, op.OpCode);
//                                break;

//                            case 4:
//                                break;
//                        }
//                        recs.Add((uint)value);
//                    }
//                    msg.grandChallenge = new Client.GrandChallenge();
//                    msg.grandChallenge.endBattle = new Client.GrandChallengeEndBattle();
//                    msg.grandChallenge.endBattle.result = result;
//                    msg.grandChallenge.endBattle.selfHeroes.AddRange(myheros);
//                    msg.grandChallenge.endBattle.totemType = TotemDataBuffer.Instance.useType;
//                    if (GrandChallengeBuffer.Instance.hireHeroId > 0)
//                    {
//                        msg.grandChallenge.endBattle.isHire = (uint)GrandChallengeBuffer.GCDate.Instance.openPanel.hireHeroInfo.hero.@base.tid;
//                    }
//                    msg.grandChallenge.endBattle.operations.AddRange(recs);
//                }
//                break;
//            case E_BattleMode.WorldBossAuto:
//                break;
//            case E_BattleMode.WorldBoss:
//                {
//                    List<int> list = new List<int>();
//                    foreach (OpRecord op in this.OpRecords)
//                    {
//                        int value = 0;
//                        switch (op.OpCode)
//                        {
//                            case 0:
//                                value = MathUtil.makeBits(12, op.Tick, 4, op.RoleIdx, 3, op.OpCode);
//                                break;

//                            case 4:
//                                break;
//                        }
//                        list.Add(value);
//                    }

//                    float totalDamage = Statistic.GetSideTotalDamage(RoleSide.Player);

//                    Role Boss = this.AliveRoles[RoleSide.Enemy][0];
//                    float hpRetio = (1 - Boss.HP / DataManager.Instance.GlobalConfig.WorldBossHPRatio) * Const.PercentageRatio;
//                    int HpLoss = (int)Math.Floor(Logic.Instance.worldBossBuffCount * Const.PercentageRatio + hpRetio + 0.5);
//                    int HpInfo = 999999999 - HpLoss;

//                    msg.worldBoss =
//                        new Client.WorldBoss
//                        {
//                            endBattle =
//                                new Client.WorldBossEndBattle { teamId = this.worldBossTeamId, hpinfo = HpInfo, damage = (int)Math.Floor(totalDamage + 0.5f), result = result }
//                        };
//                    msg.worldBoss.endBattle.operations.AddRange(list);

//                }
//                break;
//            default:
//                {
//                    msg.exitStage = new Client.ExitStage();
//                    msg.exitStage.result = result;
//                    msg.exitStage.totemType = TotemDataBuffer.Instance.useType;
//                    //if (Common.BATTLE_RESULT.BATTLE_RESULT_VICTORY == result)
//                    //{
//                    msg.exitStage.heroes.AddRange(this.HeroIDList);
//                    msg.exitStage.stars = this.ResultStars;
//                    List<int> list = new List<int>();
//                    foreach (OpRecord op in this.OpRecords)
//                    {
//                        int value = 0;
//                        switch (op.OpCode)
//                        {
//                            case 0:
//                                value = MathUtil.makeBits(12, op.Tick, 4, op.RoleIdx, 3, op.OpCode);
//                                break;

//                            case 4:
//                                break;
//                        }
//                        list.Add(value);
//                    }
//                    msg.exitStage.operations.AddRange(list);
//                    List<HeroData> selfHero = new List<HeroData>();
//                    this.GetCurrentHeroesData(selfHero, null, null);
//                    //}
//                    msg.exitStage.unitBattleData.Clear();

//                    //完成此关卡时更新任务进度 YuHaizhi 20190226
//                    for (int type = 0; type < TaskBuffer.TaskTypes; type++)
//                    {
//                        Server.UserTask legendTask_ = TaskBuffer.getWorkingHeroTask(0, type);
//                        if (legendTask_ != null)
//                        {
//                            if (type < 2 && legendTask_.id > 1 || type >= 2)//传奇、破碎神器的第一步为合装备，完美神器为副本
//                            {
//                                LegendTaskData lData = DataManager.Instance.LegendTasks[legendTask_.line][legendTask_.id];
//                                int tid_ = lData.HeroID;
//                                bool isRightStage = false;
//                                foreach (int sId in lData.TaskProgressID)
//                                {
//                                    if (sId == Logic.Instance.LevelData.LevelID)
//                                    {
//                                        isRightStage = true;
//                                        break;
//                                    }
//                                }

//                                if (isRightStage && HeroIDList.Contains(tid_))
//                                {
//                                    UnitBattleData uData = new UnitBattleData();
//                                    uData.unitHid = tid_;
//                                    uData.unitMaxDamage = (int)Logic.Instance.Statistic.GetHeroDamage(tid_);//FarmStageDamage

//                                    foreach (Role role in this.Roles)
//                                    {
//                                        if (role.Side == RoleSide.Player && role.ID == tid_)
//                                        {
//                                            uData.unitKillList.AddRange(role.DeathNote);//FarmStageKill
//                                        }
//                                    }

//                                    msg.exitStage.unitBattleData.Add(uData);
//                                    break;//按传奇->神器的优先级
//                                }

//                            }
//                        }



//                    }


//                }
//                break;
//        }
//#endif
//        return msg;
//    }


//    public void CastTalent(Role role)
//    {
//        OpRecord record = new OpRecord()
//        {
//            OpCode = 0,
//            Round = this.Round,
//            Tick = this.doneFrameCount,
//            RoleIdx = role.GlobalIdx
//        };
//        Logic.Instance.OpRecords.Add(record);
//    }

//    /// <summary>
//    /// 添加位置特效（子弹实体）
//    /// </summary>
//    /// <param name="effect"></param>
//    public void AddEffect(Effect effect)
//    {
//        if (effect != null)
//        {
//            this.CastEffects.Add(effect);
//            if (effect.Type == EffectType.Cast)
//            {
//                effect.LastHitPosition = new Vector2(effect.Position.x, effect.Position.y);
//                if (this.Scene != null)
//                {
//                    EffectJoint actor = new EffectJoint(effect, EffectType.Cast);
//                    this.Scene.AddJoint(actor);
//                }
//            }
//        }
//    }

//    public void Resume(bool force = false)
//    {
//        if (this.PauseLevel == 0 && force)
//        {
//            this.PauseSide = RoleSide.None;
//            return;
//        }

//        this.PauseLevel = force ? 0 : (--this.PauseLevel);

//        if (this.PauseLevel != 0)
//        {
//            return;
//        }
//        this.PauseSide = RoleSide.None;
//        // 恢复所有英雄动作
//        this.ResumeAllAnimation();
//        // 恢复屏幕颜色
//        if (this.Scene != null)
//        {
//            this.Scene.PlayEffect(ScreenEffect.Freeze, 0, 0f);
//        }
//    }

//    /// <summary>
//    /// 冻结所有英雄动作
//    /// </summary>
//    /// <param name="needMask"></param>
//    public void SuspendAllAnimation(bool needMask = true)
//    {
//        foreach (Element role in this.Roles)
//        {
//            role.Suspend(needMask);
//        }

//        foreach (Element summ in this.Summons)
//        {
//            summ.Suspend(needMask);
//        }

//        foreach (Element cast in this.CastEffects)
//        {
//            cast.Suspend(needMask);
//        }
//    }

//    public void ResumeAllAnimation()
//    {
//        foreach (Element element in this.Roles)
//        {
//            element.Resume();
//        }
//        foreach (Element element in this.Summons)
//        {
//            element.Resume();
//        }
//        foreach (Element element in this.CastEffects)
//        {
//            element.Resume();
//        }
//    }

//    public bool isElementInSelfHeroes(int tid)
//    {
//        foreach (Common.Hero h_ in HeroList)
//        {
//            if (h_.tid == tid)
//            {
//                return true;
//            }
//        }

//        return false;
//    }

//    public bool isHeroAliveById(int tid)
//    {
//        bool flag = false;
//        foreach (Role role in this.AliveRoles[RoleSide.Both])
//        {
//            if (role.Data.ID == tid)
//            {
//                flag = true;
//                break;
//            }
//        }
//        return flag;
//    }

//    public override string ToString()
//    {
//#if SERVER
//#if BATTLE_LOG
//        string s = string.Format("============================================================\nTick {0}\n", this.doneFrameCount);
//        foreach (Role role in this.Roles)
//        {
//            s = s + role.ToString();
//        }
//        if(this.CastEffects.Count>0)
//            s += "CAST_EFFECT:\n";
//        foreach (Element element in this.CastEffects)
//        {
//            s = s + element.ToString();
//        }
//        return s;
//#endif
//#else
//        return "Logic";
//        //客户端打印log
//        /*string s = string.Format("============================================================\nTick {0}\n", this.doneFrameCount);
//        foreach (Role role in this.Roles)
//        {
//            s = s + role.ToString();
//        }
//        if (this.CastEffects.Count > 0)
//            s += "CAST_EFFECT:\n";
//        foreach (Element element in this.CastEffects)
//        {
//            s = s + element.ToString();
//        }
//        return s;*/
//#endif
//        return "";
//    }

//    public static void log(string log)
//    {
//#if SERVER
//        Logger.LogCombat(log);
//#else
//        Logger.Log(log);
//#endif
//    }

//    public static void log(string format, params object[] args)
//    {
//        log(string.Format(format, args));
//    }

//    public void LoadLevel(string name, List<Common.Hero> heroes, LevelData level, UnityEngine.Events.UnityAction onLoad, E_BattleMode mode = E_BattleMode.Campaign, int param = 0)
//    {
//#if SERVER
//        onLoad();
//#else
//        BattleSceneLoader.Instance.LoadLevel(name, heroes, level, onLoad, mode, param);
//#endif
//    }
//    public void LoadUnrealLevel(string name, List<Common.Hero> heroes, LevelData level, UnityEngine.Events.UnityAction onLoad)
//    {
//#if SERVER
//        onLoad();
//#else
//        BattleSceneLoader.Instance.LoadUnrealLevel(name, heroes, level, onLoad);
//#endif
//    }

//    /// <summary>
//    /// 播放位置方向特效（DirectEffect）例如船长的：洪水技能，一个水柱
//    /// 非绑定在Role角色身上的特效
//    /// 需要创建EffectJoint来控制（更新位置、方向等）
//    /// </summary>
//    /// <param name="effect"></param>
//    /// <param name="pos"></param>
//    /// <param name="scale"></param>
//    /// <param name="height"></param>
//    /// <param name="z"></param>
//    public static void PlayEffect(string effect, Vector2 pos, Vector2 scale, float height, int z)
//    {
//        if (Logic.Instance.Scene != null)
//        {
//            EffectJoint joint = new EffectJoint(effect);
//            if (joint.HaveGameObject)
//            {
//                joint.Controller.SetLayer(z);
//                joint.Controller.SetPosition(new Vector3(pos.x, pos.y, height));
//                joint.Controller.Direction = scale.x > 0 ? 1.0f : -1.0f;
//                joint.Controller.SetScale(scale);
//            }
//            Logic.Instance.Scene.AddJoint(joint);
//        }

//    }
//    public static void PlayEffect(int effect, Vector2 pos, Vector2 scale, float height, int z)
//    {
//        if (DataManager.Instance.Effects.ContainsKey(effect))
//        {
//            PlayEffect(DataManager.Instance.Effects[effect], pos, scale, height, z);
//        }
//    }

//    public void SetLimitTime()
//    {
//        this.LimitTime = defaultTimeLimit;
//    }

//    public static int getStageBossData(int stageId)
//    {
//        if (!DataManager.Instance.Levels.ContainsKey(stageId))
//        {
//            return 0;
//        }
//        GameData.LevelData data = DataManager.Instance.Levels[stageId];
//        int chapterId = data.SectionID;

//        Dictionary<int, GameData.CombatConfigData> dict = DataNewManager.Instance.CombatConfigs.Value[stageId];
//        GameData.CombatConfigData battleData = null;

//        int key = 0;
//        foreach (var v in dict)
//        {
//            if (battleData == null || v.Key > key)
//            {
//                battleData = v.Value;
//            }
//        }

//        if (battleData != null)
//        {
//            if (battleData.BossPosition.Find(delegate (int ID) { return ID == 1; }) == 1)
//            {
//                return battleData.Monster1ID;
//            }
//            if (battleData.BossPosition.Find(delegate (int ID) { return ID == 2; }) == 2)
//            {
//                return battleData.Monster2ID;
//            }
//            if (battleData.BossPosition.Find(delegate (int ID) { return ID == 3; }) == 3)
//            {
//                return battleData.Monster3ID;
//            }
//            if (battleData.BossPosition.Find(delegate (int ID) { return ID == 4; }) == 4)
//            {
//                return battleData.Monster4ID;
//            }
//            if (battleData.BossPosition.Find(delegate (int ID) { return ID == 5; }) == 5)
//            {
//                return battleData.Monster5ID;
//            }
//        }

//        return 0;
//    }


//    public bool IsOutOfBattleArea(Vector2 pos)
//    {
//        return pos.x < -this.BattleRect.xMax / 2 || pos.x > this.BattleRect.xMax / 2 || pos.y < this.BattleRect.yMin || pos.y > this.BattleRect.yMax;
//    }


//    public void TriggerTotemByActiveTalent(Role from)
//    {
//        TotemLevelData totem = null;
//        if (from.Side == RoleSide.Player)
//        {
//            if (playerTotemType == 0 || playerTotemLevel == 0)
//            {
//                return;
//            }
//            totem = DataManager.Instance.TotemLevelData.Where(data => (data.Value.Type == playerTotemType && data.Value.Level == playerTotemLevel)).Select(data => data.Value).First();
//        }

//        if (from.Side == RoleSide.Enemy)
//        {
//            if (enemyTotemType == 0 || enemyTotemLevel == 0)
//            {
//                return;
//            }
//            totem = DataManager.Instance.TotemLevelData.Where(data => (data.Value.Type == enemyTotemType && data.Value.Level == enemyTotemLevel)).Select(data => data.Value).First();
//        }


//        if (totem == null)
//        {
//            return;
//        }

//        string conditonFaction = string.Empty;
//        bool factionIdentical = false; // 派系吻合
//        foreach (string triggerFaction in totem.Condition)
//        {
//            string[] condition = triggerFaction.Split(':');
//            conditonFaction = condition[1];
//            if (from.Data.HeroTags.Contains(conditonFaction))
//            {
//                factionIdentical = true;
//                break;
//            }
//        }

//        // 派系不吻合
//        if (!factionIdentical)
//        {
//            return;
//        }

//        string totemEffect = Const.GetTotemEffect(conditonFaction);

//        // 找出大招触发的属性加成
//        Dictionary<int, int> addAttrDict = new Dictionary<int, int>();
//        Type type = typeof(TotemLevelData);

//        for (int i = 1; i <= 3; i++)
//        {
//            PropertyInfo propertyType = type.GetProperty("AddAttTrigger" + i);
//            if (propertyType != null)
//            {
//                object triggerType = propertyType.GetValue(totem, null);
//                if (Convert.ToInt32(triggerType) == (int)TotemTriggerType.Ultimate)
//                {
//                    propertyType = type.GetProperty("AddAttType" + i);
//                    object attrType = propertyType.GetValue(totem, null);
//                    if (attrType != null)
//                    {
//                        PropertyInfo propertyAddAttrNum = type.GetProperty("AddAttrNum" + i);
//                        if (propertyAddAttrNum != null)
//                        {
//                            object addAttrNum = propertyAddAttrNum.GetValue(totem, null);
//                            addAttrDict.Add(Convert.ToInt32(attrType), Convert.ToInt32(addAttrNum));
//                        }
//                    }
//                }
//            }
//        }

//        if (addAttrDict.Count > 0)
//        {
//            List<Role> roles = AliveRoles[from.Side];
//            foreach (Role role in roles)
//            {
//                if (role.Config != null && role.Config.IsDemon)
//                {
//                    continue;
//                }
//                AbilityData abilityData = DataManager.Instance.Abilities[totem.InitiativeBuff];
//                abilityData.Effect = totemEffect;
//                Type AbilityType = typeof(AbilityData);
//                foreach (int attr in addAttrDict.Keys)
//                {
//                    RoleAttribute attribute = (RoleAttribute)attr;
//                    string attrString = attribute.ToString();
//                    PropertyInfo propertyType = AbilityType.GetProperty(attrString);
//                    if (propertyType != null)
//                    {
//                        propertyType.SetValue(abilityData, addAttrDict[attr], null);
//                    }
//                }
//                role.AddAbility(abilityData, from);
//            }
//        }
//    }
//    //火焰图腾技能  
//    public void TriggerFireTotemByActiveTalent(Talent talent, Role from, Role target)
//    {
//        TotemLevelData totem = null;
//        if (from != null && target != null && from.Side == target.Side) //判断阵营是否相同
//            return;

//        totem = GetTotemLevelData(from);
//        if (totem == null)
//            return;
//        string conditonFaction = string.Empty;
//        bool factionIdentical = false; // 派系吻合
//        var group = !DataManager.Instance.TalentGroups.ContainsKey(from.Data.ID) ?
//               null : DataManager.Instance.TalentGroups[from.Data.ID].Where(data => (data.Value.TalentGroupID == talent.Data.TalentGroupID)).Select(data => data.Value).First();
//        if (group != null && group.TalentTags.Count == 0 && group.ParentID > 0) //为了解决技能伤害是通过子技能实现的
//            group = DataManager.Instance.TalentGroups[from.Data.ID].Where(data => (data.Value.TalentGroupID == group.ParentID)).Select(data => data.Value).First();
//        foreach (string triggerFaction in totem.Condition)
//        {
//            string[] condition = triggerFaction.Split(':');
//            conditonFaction = condition[1];
//            if (group != null && group.TalentTags.Contains(conditonFaction)) //技能是火焰系的
//            {
//                factionIdentical = true;
//                break;
//            }
//        }

//        // 派系不吻合
//        if (!factionIdentical)
//        {
//            return;
//        }

//        // 火焰技能触发的属性加成
//        Dictionary<int, int> addAttrDict = ChooseFireTriggerAdd(totem, TotemTriggerType.ActiveTalent);
//        if (addAttrDict.Count > 0)
//        {
//            if (!target.Abilities.Any(ability => ability.AbilityData.ID == totem.InitiativeBuff))
//            {
//                target.FireTotemCount = 0; //初始化叠加数
//                target.FireTotemTime = (float)this.LimitTime + DataManager.Instance.DNumerics[85].Arg;
//            }
//            //新需求 火焰图腾叠层数时加冷却时间 2019-10-9
//            if (target.FireTotemTime - (float)this.LimitTime >= DataManager.Instance.DNumerics[85].Arg)
//            {
//                AbilityData abilityData = DataManager.Instance.Abilities[totem.InitiativeBuff].Clone();
//                Type AbilityType = typeof(AbilityData);
//                target.FireTotemCount++;
//                target.FireTotemTime = (float)this.LimitTime;
//                target.FireTotemCount = Mathf.Min(target.FireTotemCount, DataManager.Instance.DNumerics[85].Param);
//                foreach (int attr in addAttrDict.Keys)
//                {
//                    RoleAttribute attribute = (RoleAttribute)attr;
//                    string attrString = attribute.ToString();
//                    PropertyInfo propertyType = AbilityType.GetProperty(attrString);
//                    if (propertyType != null)
//                    {
//                        propertyType.SetValue(abilityData, addAttrDict[attr] * target.FireTotemCount, null);
//                    }
//                }
//                abilityData.Effect = string.Format("fx_buff_fenghuang_{0}", target.FireTotemCount);
//                target.AddAbility(abilityData, from);
//            }
//        }
//    }
//    //毒系图腾技能
//    public bool TriggerPoisoningTotemByActiveTalent(Role from, AbilityData ability)
//    {
//        TotemLevelData totem = null;
//        if (from == null)
//            return false;
//        if (from != null && ((RoleSide)((int)from.Side * -1)) == RoleSide.Player)
//        {
//            if (playerTotemType == 0 || playerTotemLevel == 0)
//            {
//                return false;
//            }
//            totem = DataManager.Instance.TotemLevelData.Where(data => (data.Value.Type == playerTotemType && data.Value.Level == playerTotemLevel)).Select(data => data.Value).First();
//        }
//        if (from != null && ((RoleSide)((int)from.Side * -1)) == RoleSide.Enemy)
//        {
//            if (enemyTotemType == 0 || enemyTotemLevel == 0)
//            {
//                return false;
//            }
//            totem = DataManager.Instance.TotemLevelData.Where(data => (data.Value.Type == enemyTotemType && data.Value.Level == enemyTotemLevel)).Select(data => data.Value).First();
//        }
//        if (totem == null)
//            return false;
//        string conditonFaction = string.Empty;
//        bool factionIdentical = false; // 派系吻合
//        foreach (string triggerFaction in totem.Condition)
//        {
//            string[] condition = triggerFaction.Split(':');
//            conditonFaction = condition[1];
//            if (ability != null && ability.AbilityTags != null && ability.AbilityTags.Contains(conditonFaction)) //技能是火焰系的
//            {
//                factionIdentical = true;
//                break;
//            }
//        }
//        // 派系不吻合
//        if (!factionIdentical)
//            return false;
//        // 技能触发的属性加成
//        Dictionary<int, int> addAttrDict = ChooseFireTriggerAdd(totem, TotemTriggerType.Poisoming);
//        if (addAttrDict.Count > 0)
//        {
//            from.PoisoningTotemInjury = addAttrDict[100];
//            from.PoisoningTotemReduce_Remedy = (float)addAttrDict[101] / 100;
//            return true;
//        }
//        return false;
//    }
//    /// <summary>
//    /// 获取 target 阵营的图腾数据
//    /// </summary>
//    /// <param name="target"></param>
//    /// <returns></returns>
//    public TotemLevelData GetTotemLevelData(Role target)
//    {
//        TotemLevelData totem = null;
//        if (target != null && target.Side == RoleSide.Player)
//        {
//            if (playerTotemType == 0 || playerTotemLevel == 0)
//            {
//                return null;
//            }
//            totem = DataManager.Instance.TotemLevelData.Where(data => (data.Value.Type == playerTotemType && data.Value.Level == playerTotemLevel)).Select(data => data.Value).First();
//        }
//        if (target != null && target.Side == RoleSide.Enemy)
//        {
//            if (enemyTotemType == 0 || enemyTotemLevel == 0)
//            {
//                return null;
//            }
//            totem = DataManager.Instance.TotemLevelData.Where(data => (data.Value.Type == enemyTotemType && data.Value.Level == enemyTotemLevel)).Select(data => data.Value).First();
//        }
//        if (totem == null)
//            return null;
//        else
//            return totem;
//    }

//    /// <summary>
//    /// 找出由 _type 触发的属性加成
//    /// </summary>
//    /// <param name="totem"></param>
//    /// <param name="_type">触发条件</param>
//    /// <returns></returns>
//    public Dictionary<int, int> ChooseFireTriggerAdd(TotemLevelData totem, TotemTriggerType _type)
//    {
//        Dictionary<int, int> addAttrDict = new Dictionary<int, int>();
//        Type type = typeof(TotemLevelData);

//        for (int i = 1; i <= 3; i++)
//        {
//            PropertyInfo propertyType = type.GetProperty("AddAttTrigger" + i);
//            if (propertyType != null)
//            {
//                object triggerType = propertyType.GetValue(totem, null);
//                if (Convert.ToInt32(triggerType) == (int)_type)
//                {
//                    propertyType = type.GetProperty("AddAttType" + i);
//                    object attrType = propertyType.GetValue(totem, null);
//                    if (attrType != null)
//                    {
//                        PropertyInfo propertyAddAttrNum = type.GetProperty("AddAttrNum" + i);
//                        if (propertyAddAttrNum != null)
//                        {
//                            object addAttrNum = propertyAddAttrNum.GetValue(totem, null);
//                            addAttrDict.Add(Convert.ToInt32(attrType), Convert.ToInt32(addAttrNum));
//                        }
//                    }
//                }
//            }
//        }
//        return addAttrDict;
//    }

//}
