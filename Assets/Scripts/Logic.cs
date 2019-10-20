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

    public Dictionary<int, List<Role>> Surviors = new Dictionary<int, List<Role>>();//��Ӫ,��ɫ
    public Dictionary<int, List<Role>> Deads = new Dictionary<int, List<Role>>();
    public List<Role> AllRoles = new List<Role>();
    public List<Behavior> CastEffects = new List<Behavior>();

    public void LoadLevel(BATTLEMODE mode, string scene, UnityAction onLoad)
    {
        BattleSceneLoader.Instance.LoadLevel(mode, scene, onLoad);
    }

    public void InitBattle()
    {
        AllRoles.Clear();
        Surviors.Clear();
        Deads.Clear();
        CastEffects.Clear();
    }

    //read from player saves, not monster config
    public void CreatePlayerRoles(int side, List<Hero> savedHeroes)
    {
        foreach (Hero hero in savedHeroes)
        {
            Role role = Role.Create(hero);
            role.Faction = side;
            role.FactOrg = side;

            //born position??
            AddRole(role);
        }
    }

    public void AddRole(Role role)
    {
        AllRoles.Add(role);
        role.Index = AllRoles.Count - 1;

        if (role.Status.State == ANIMATIONSTATE.DEATH)//die at born??
        {
            Deads[role.FactOrg].Add(role);
        }
        else
        {
            if (!Surviors.ContainsKey(role.FactOrg))
                Surviors[role.FactOrg] = new List<Role> { role };
            else if (!Surviors[role.FactOrg].Contains(role))
                Surviors[role.FactOrg].Add(role);
        }

        //测试专用
        role.Position += Vector3.forward * role.Index * 5;

    }

    public void RoleDead(Role role)
    {
        if (Surviors.ContainsKey(role.FactOrg) && Surviors[role.FactOrg].Contains(role))
            Surviors[role.FactOrg].Remove(role);

        if (!Deads.ContainsKey(role.FactOrg))
            Deads[role.FactOrg] = new List<Role> { role };
        else if (!Deads[role.FactOrg].Contains(role))
            Deads[role.FactOrg].Add(role);
    }

    //read config data
    public void CreateEnemiesByLevelData(int Level, int side = 1)
    {
        //first round only for now
        LevelMonsterData data = DataManager.Instance.LevelMonsters[Level][1];

        foreach (int mid in data.Monsters)
        {
            CreateMonsterByID(mid, side);
        }


    }

    private void CreateMonsterByID(int mid, int side)
    {
        MonsterConfigData data = DataManager.Instance.MonsterConfigs[mid];
        Hero hero = new Hero(data.ID, data.Level, data.Weapons);
        Role role = Role.Create(hero);
        role.Faction = side;//??
        role.FactOrg = side;
        AddRole(role);
    }


    //stay same level with player
    public void CreateMatchedEnemies(List<int> enemies)
    {


    }

    public IEnumerable<Role> GetAliveAllies(int side)
    {
        if (!Surviors.ContainsKey(side))
            yield return null;

        foreach (Role role in Surviors[side].ToArray())
            yield return role;
    }
    
    public IEnumerable<Role> GetAliveEnemies(int side)//pass self side
    {
        // if (!Surviors.ContainsKey(side))
        //     yield return null;

        // foreach (Role role in Surviors[side].ToArray())
        //     yield return role;
        
        foreach(int faction in Surviors.Keys)
        {
            if(faction == side)
                continue;
            
            foreach(Role role in Surviors[faction])
                yield return role;

        }


    }

    //Tick
    public void Tick(float passTime)
    {
        int roleNum = AllRoles.Count;
        for (int i = 0; i < roleNum; i++)
        {
            Role role = AllRoles[i];
            if (!role.IsPause)
            {
                //Debug.Log("Tick Start: " + element.obj.name);
                role.Update(passTime);
                //BehaviorManager.instance.Tick(role.Controller.tree);// call role.onenterframe in task to drive
            }
            //else
            //{
            //    role.tree.PauseWhenDisabled = true;
            //    role.tree.DisableBehavior(true);
            //}
            roleNum = AllRoles.Count;//may change within one tick(summon)
        }

        int castCount = this.CastEffects.Count;
        for (int i = 0; i < castCount; i++)
        {
            //if (element == null) Debug.LogError("null cast effect: " + element.name);
            Behavior element = this.CastEffects[i];
            BehaviorManager.instance.Tick(element);
            castCount = this.CastEffects.Count;//within one tick, summon new effect possible
        }

        // summon
        //foreach (BTRoleData element in this.Summons)
        //{
        //    if (!element.IsPause)
        //        BehaviorManager.instance.Tick(element.tree);
        //}
    }

    //no other side member exist
    public bool VertifyBattleResult(int side)
    {
        foreach (int fact in Surviors.Keys)
        {
            if (fact == side)
                continue;

            if (Surviors[fact] != null && Surviors[fact].Count > 0)
                return false;
        }
        return true;

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
//    Poisoming = 11 //��ϵ����
//}
////ͼ��������������
//public enum TotemAddSkillEffect
//{
//    Poison_Injury_Per_Second = 100,// ��ϵͼ�� �� ÿ�붾��
//    Reduce_Response = 101, //��ϵͼ�� �� ���ͻظ�Ч��
//}
//public enum E_BattleMode
//{
//    /// <summary>
//    /// ��ͨս��--PVE
//    /// </summary>
//    Campaign,
//    /// <summary>
//    /// ��սDuel--PVP
//    /// </summary>
//    Duel,
//    /// <summary>
//    /// ������--PVP
//    /// </summary>
//    Arena,
//    /// <summary>
//    /// Զ��--PVE
//    /// </summary>
//    Crusade,
//    /// <summary>
//    /// Զ����սģʽ--PVE
//    /// </summary>
//    CrusadeChallenge,
//    /// <summary>
//    /// �ر���Ѩ--PVP
//    /// </summary>
//    Treasure,
//    /// <summary>
//    /// ʱ���ѷ�--PVE
//    /// </summary>
//    TimeRift,
//    /// <summary>
//    /// Ӣ��������--PVE
//    /// </summary>
//    HeroTrial,
//    /// <summary>
//    /// ����ģʽ--PVE
//    /// </summary>
//    Outland,
//    /// <summary>
//    /// ���ḱ��
//    /// </summary>
//    GuildInstance,
//    /// <summary>
//    /// ���߾�����--PVP
//    /// </summary>
//    GrandArena,
//    /// <summary>
//    /// Զ������--PVE
//    /// </summary>
//    Temple,
//    /// <summary>
//    /// �۷���ս--PVP
//    /// </summary>
//    GrandChallenge,
//    /// <summary>
//    /// ��һս
//    /// </summary>
//    FirstBattle,
//    /// <summary>
//    /// ����ս
//    /// </summary>
//    GuildChampion,
//    /// <summary>
//    /// ����ս(ֱ������ֱ�����ַ�ʽ)
//    /// </summary>
//    PersonChampion,
//    /// <summary>
//    /// ս��Ӫ�أ�������������
//    /// </summary>
//    WarCampHallSetTeam,
//    /// <summary>
//    /// ս��Ӫ��, �������Ӷ�ս��
//    /// </summary>
//    HallOfWar,
//    /// <summary>
//    /// ��Ӣ��ս�����ԣ�������Ա��
//    /// </summary>
//    BattleTest,
//    /// <summary>
//    /// ��������
//    /// </summary>
//    WarOfGods,
//    /// <summary>
//    /// �Ծ�Alphaר��
//    /// </summary>
//    DuelAlpha,
//    /// <summary>
//    /// ����Boss
//    /// </summary>
//    WorldBoss,
//    /// <summary>
//    /// ���� Boss �Զ�ս��
//    /// </summary>
//    WorldBossAuto,
//    /// <summary>
//    /// ������ǲ��
//    /// </summary>
//    CrusadeRaid,
//    /// <summary>
//    /// ������սģʽ��ǲ��
//    /// </summary>
//    CrusadeChallengeRaid,
//    /// <summary>
//    /// �����Ҷ�
//    /// </summary>
//    BraveMelee,
//    /// <summary>
//    /// �⴫
//    /// </summary>
//    Story,
//    /// <summary>
//    /// ���
//    /// </summary>
//    Unreal
//}

///// ս������������������������ս�����ݼ��߼�
//public class Logic : Singleton<Logic>//ս�����ݵĻ����ж�
//{
//    // ս��״̬��ò�Ʋ�����OnEnterFame��
//    const int BATTLE_STAGE_INIT = -1;
//    const int BATTLE_STAGE_WATCHING = 0;
//    const int BATTLE_STAGE_FIGHTING = 1;
//    const int BATTLE_STAGE_FINISHED = 2;
//    const int BATTLE_STAGE_PLAY_ANIMATION_BEFORE_START = 3;
//    // ò��Ҳ�����ˣ�OnEnterFame��
//    const float FIGHTING_TIMEOUT = 60f * 3f;
//    const float NEW_PVE_FIGHTING_TIMEOUT = 60 * 1.5f;
//    const int INTERVAL_SAVE_TEMP_BATTLE = 10;    // -- ��ʱ����ս�������ʱ����10s
//    // һ��ս��ʱ��90��
//    const float defaultTimeLimit = 90;
//    // ս������
//    public E_BattleMode BattleMode;
//    // �Ƿ�Ϊ�ط�
//    public bool IsReplayMode;
//    // �ط�������һ�α��
//    public bool IsTryAgain;
//    // �Ƿ���boss��
//    public bool IsBossLevel = false;
//    // �Ƿ���pvp����

//    // �Ƿ�Ϊʵʱս������
//    public bool IsLiveMode;
//    // �Ƿ�ΪPVP��ս��
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
//    // δʹ�ã���
//    public bool GMMode;
//    // �طż�¼����ͨ���������Ծ�����Ѩ��
//    public Server.PVPRecord PvpReplayRecord;
//    // �طż�¼�����߾�������
//    public Server.AdvArenaRoundBattleRecord TopPvpReplayRecord;
//    // �طż�¼(������)
//    public Server.PCSPlusBattleReplay PersonalChampionRecord;
//    // �طż�¼(ս��Ӫ��)
//    public Server.HallOfWarQueryReplayReply HallOfWarRecord;
//    // �طż�¼(����ս)
//    public Server.WarOfGodsQueryReplayReply WarOfGodsRecord;
//    // �طż�¼(�����Ҷ�)
//    public Server.BraveMeleeQueryReplayReply BraveMeleeRecord;

//    // ս���ؿ�����
//    public LevelData LevelData;
//    // ս��ÿһ��ս������
//    public CombatConfigData CombatConfig;
//    // �ؿ�ŭ������ϵ��
//    public float MPBonus;
//    // �غ���
//    public int Round;
//    // ս����������
//    public Rect BattleRect;
//    // ǰ���ٶ�ϵ��
//    public float MarchingSpeed = 1.75f;
//    // �Ƿ���������
//    public bool Running = false;
//    // ս������״̬
//    public bool Enabled;
//    // ֡����
//    public int doneFrameCount;
//    // ս������ʱʱ�䣨90s��
//    public double LimitTime;
//    // ս����ҵ��佱��
//    public int GoldCount = 0;
//    // ս��������佱��ͳ��
//    public int LootCount = 0;

//    // ������Ҫ�Ķ�������
//    public int monsterIndex;
//    public int ownerUserId;

//    // ����Boss��������
//    public int worldBossTeamId;
//    public int worldBossBuffCount;

//    // ��ͣ����
//    public int PauseLevel;
//    // �ͷŴ���������Ļ��ͣ����Ӫ��û���ã�������
//    public RoleSide PauseSide;
//    // ս�����
//    public Common.BATTLE_RESULT BattleResult;
//    // ս������
//    public int ResultStars;

//    // ����Ӣ�������б���
//    public List<Common.Hero> HeroList;
//    // ˫��Ӣ�۽�ɫ�����б�
//    public List<Role> Roles = new List<Role>();
//    // ��Ҳ������ݼ�¼���طŻ��õ���
//    public List<OpRecord> OpRecords = new List<OpRecord>();

//    // Ӣ�۴������ÿһ���Ĵ�
//    public Dictionary<RoleSide, int> AliveCount = new Dictionary<RoleSide, int>();
//    // Ӣ����������ÿһ����������
//    public Dictionary<RoleSide, int> DeadCount = new Dictionary<RoleSide, int>();

//    // ����ɫ���󱣴�
//    public Dictionary<RoleSide, List<Role>> AliveRoles = new Dictionary<RoleSide, List<Role>>()
//    {
//        { RoleSide.Player , new List<Role>() },
//        { RoleSide.Enemy , new List<Role>() },
//        { RoleSide.Both , new List<Role>() }
//    };
//    // �ٻ���
//    public List<Role> Summons = new List<Role>();
//    // ������ɫ���󱣴�
//    public List<Role> DeadRoles = new List<Role>();
//    // 
//    public List<Element> CastEffects = new List<Element>();
//    // ���������������
//    const int MAX_POSITION = 5;
//    // ս��״̬
//    private int _battleState = -1;
//    // �ؿ��Ƿ����
//    public bool LevelEnded;
//    // ս������
//    public bool BattleEnd;
//    // ս�����˺�ֵ��¼
//    public BattleStatistic Statistic = new BattleStatistic();
//    // ս������ָ��
//    public IBattleScene Scene { get; set; }
//    // �Ѿ���Ч��������
//    public float beginTime = 0;
//    // �Ѿ���Ч��������
//    private float lastSaveTempBattleTime = 0;
//    // ��ǰֱ�����ڼ�֡
//    public int TargetDoneFrameCount = 0;
//    //ChickenDinner ս����Դ�Ƿ�������
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
//    // ǰ���Ƕ�
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
//    // �����3d��ʹ�õģ�λ�ýڵ�
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
//    // ��ս�еķ��ط�id����Ӧ�÷���ս���߼��У�    
//    public int DefenderID;
//    // ��Ѩ����id����Ӧ�÷���ս���߼��У�
//    public int TreasureTypeID;
//    // ����������û��ʹ�ù�������
//    int totalMonster = 0;
//    //  �ֶ��������������У�
//    int NextOperationIdx;

//    public bool Supplied;
//    // ����Ӣ��id�б�
//    List<int> HeroIDList = new List<int>();

//    // ����Ĺ���б�
//    public List<Vector2> DeathPositions = new List<Vector2>();
//    public List<EffectJoint> DeathPositionEffectJoints = new List<EffectJoint>();

//    // ��ս˫����ͼ�ڵȼ�
//    public int playerTotemLevel = 0;
//    public int enemyTotemLevel = 0;

//    // ��ս˫����ͼ������
//    public int playerTotemType = 0;
//    public int enemyTotemType = 0;
//    //ChickenDinner ����λ��
//    public int offSetX = 0;
//    /// <summary>
//    /// �๹�캯��
//    /// </summary>
//    public Logic()
//    {

//    }

//    /// <summary>
//    /// ��ɫ��λ�б����
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
//            //role.InitAttributes();//����  �����ٻ����������  
//            role.Position = position;
//            role.PreviousPosition = role.Position;
//            role.Born(1, init_action);
//            if (role.Joint != null)
//                role.Joint.OnEnterFrame(0);
//        }
//    }


//    /// <summary>
//    /// ����Ӣ�۵�λ��Ϣ����
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
//    /// �油
//    /// </summary>
//    /// <param name="substitute"></param>
//    /// <param name="isBot"></param>
//    /// <param name="side"></param>
//    public void SetSubstitute(HeroDataEx substitute, bool isBot, RoleSide side)
//    {
//        // ����ս�油�߼�
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
//    /// pvp��ս�����ã���ʼ������Ӣ��(initMyHeroes��initEnemyHeoes)
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
//            // ����role avatar��λ��
//            role.Position = new Vector2();
//            role.Position.x = Const.InitialPosition[i].x - this.BattleRect.xMax * 0.5f;
//            role.Position.y = Const.InitialPosition[i].y;

//            this.HeroIDList.Add(hero.tid);
//        }
//    }
//    /// <summary>
//    /// pvp��ս�����ã���ʼ���з�Ӣ��(initMyHeroes��initEnemyHeoes)
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
//            // ����role avatar��λ��
//            role.Position = new Vector2();
//            role.Position.x = this.BattleRect.xMax * 0.5f - Const.InitialPosition[i].x;
//            role.Position.y = Const.InitialPosition[i].y;
//        }
//    }
//    /// <summary>
//    /// ChickenDinner��ս�����ã���ʼ������Ӣ��(initMyHeroes��initEnemyHeoes)
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
//            // ����role avatar��λ��
//            role.Position = birthPos;
//            this.HeroIDList.Add(hero.tid);
//        }
//    }

//    /// <summary>
//    /// ChickenDinner��ս�����ã���ʼ���з�Ӣ��(initMyHeroes��initEnemyHeoes)
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
//            // ����role avatar��λ��
//            role.Position = birthPos;
//        }
//    }

//    /// <summary>
//    /// ���ɹ��ﵥλ��Ϣ����
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
//    /// pve��ս�����ã���ʼ������Ӣ�ۣ�InitPlayerHeroes��InitMonsters��
//    /// </summary>
//    /// <param name="heroList"></param>
//    /// <param name="isbot"></param>
//    public void InitPlayerHeroes(List<Common.Hero> heroList, bool isbot)
//    {
//        //��ʼ�����Ӣ������
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
//    /// pve��ս�����ã������ʼ����InitPlayerHeroes��InitMonsters��
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

//            //�����ؿ�bossΪ����״̬ YuHaizhi 20190514
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
//            //bossս����
//            foreach (int k in this.CombatConfig.BossPosition)//�趨�Ƿ�Ϊboss���ٸ����Ƿ�Ϊbossս���趨��С
//            {
//                if (curIndex == k)
//                {
//                    ro.IsBoss = true;
//                }
//            }
//            //bossս����
//            Vector2 position = new Vector2();
//            position.x = this.BattleRect.xMax * 0.5f - Const.InitialPosition[curIndex - 1].x;
//            position.y = Const.InitialPosition[curIndex - 1].y;
//            ro.Position = position;
//            ro.Joint.Controller.SetPosition(ro.Position);
//        }
//    }

//    //�����ؿ�boss�����⴦�� YuHaizhi 20190514
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
//    /// ��ȡ���ϻ��ŵ���������Ӣ��
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
//    /// ��ȡĳ��Ӫ���ϻ��ŵ�Ӣ��
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
//    /// �ٻ����ٻ��ޣ�
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
//    /// ����ٻ���
//    /// </summary>
//    /// <returns></returns>
//    public IEnumerable<Role> GetSommons()
//    {
//        foreach (Role role in this.Summons)
//        {
//            yield return role;
//        }
//    }

//    // ��������Ѿ���Ч�����������ˣ�OnEnterFame��
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
//    // ��������Ѿ���Ч�����������ˣ�OnEnterFame��
//    private void saveTempBattle()
//    {
//        string logs = Logger.GetLogs(true);
//        System.IO.File.WriteAllText("Logs/" + DateTime.Now.ToLongTimeString() + ".log", logs);
//    }
//    // ��������Ѿ���Ч������������
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
//                    // -- ʱ�䵽�ˣ��Զ�����ս��
//                    this._battleState = BATTLE_STAGE_FINISHED;
//                    this.Running = false;
//                    BATTLE_RESULT result = BATTLE_RESULT.BATTLE_RESULT_TIMEOUT;
//                    this.EndBattle(result);

//                }
//                else
//                {

//                    this._battleState = BATTLE_STAGE_FINISHED;
//                    this.Running = false;
//                    // -- ���ս����Ч
//                    //todo

//                    this.EndBattle(BATTLE_RESULT.BATTLE_RESULT_DEFEAT);
//                }
//                return;
//            }

//            // -- pvpս����ʼ��Ҫ�Զ�������
//            if (this.BattleMode == E_BattleMode.Arena)
//            {
//                int notCostTimeSec = (int)Math.Floor(beginTime);
//                if (notCostTimeSec % INTERVAL_SAVE_TEMP_BATTLE == 0 && notCostTimeSec > lastSaveTempBattleTime)
//                {
//                    // -- ������ʱ���
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
//    /// ������е�΢����
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
//    /// ����Ԫ���߼�
//    /// </summary>
//    /// <param name="dt"></param>
//    void ProcessEntities(double dt)
//    {
//        // ��ɫʵ����
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

//        // ����ʵ����Effect
//        int CastCount = this.CastEffects.Count;
//        for (int i = 0; i < CastCount; i++)
//        {
//            Element element = this.CastEffects[i];
//            if (!element.IsPause)
//                element.OnEnterFrame(dt);
//            CastCount = this.CastEffects.Count;
//        }
//        this.CastEffects.RemoveAll(new Predicate<Element>((x) => { return x.Running == false; }));

//        // �ٻ���
//        foreach (Role element in this.Summons)
//        {
//            if (!element.IsPause)
//                element.OnEnterFrame(dt);
//        }
//        this.Summons.RemoveAll(new Predicate<Role>((x) => { return x.Running == false; }));
//    }


//    /// <summary>
//    ///  ���ó���Ӣ�۽�ɫ����
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
//    ///  ���ó���Ӣ�۴���Ч��
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
//    /// ��ʼ������Ӣ�۳���λ�ã�������
//    /// </summary>
//    public void InitPlayerAliveRoles()
//    {
//        int i = 0;
//        foreach (Role role in this.AliveRoles[RoleSide.Player])
//        {
//            Vector2 position = new Vector2();
//            position.x = Const.InitialPosition[i].x - this.BattleRect.xMax * 0.5f;//��ս������������
//            position.y = Const.InitialPosition[i].y;
//            i++;
//            role.Joint.Disable = false;
//            role.Position = position;//��ս���еĿ�ʼλ��
//        };
//    }
//    /// <summary>
//    /// ��ʼ������ս���Ĺؿ�����
//    /// </summary>
//    public void InitBattleCore()
//    {
//        //���ùؿ�����
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
//    /// ս����ʼ��(pveս��)
//    /// </summary>
//    /// <param name="combat"></param>
//    /// <param name="extra_list"></param>
//    public void InitBattle(CombatConfigData combat, Dictionary<int, Common.HeroExtra> ls = null)
//    {
//        //��ʼ��ս������
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
//    /// ս����ʼ��(pvpս��)
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
//    /// ����ս��
//    /// </summary>
//    /// <param name="result"></param>
//    public void EndBattle(Common.BATTLE_RESULT result)
//    {
//        //�ͻ��˴�ӡLog
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
//        // ���߾�����+����ս+����ս��ս������ж�--��ʱ�����˺���
//        if (this.BattleMode == E_BattleMode.GrandArena ||
//            this.BattleMode == E_BattleMode.GuildChampion ||
//            this.BattleMode == E_BattleMode.PersonChampion ||
//            this.BattleMode == E_BattleMode.WarOfGods ||
//            this.BattleMode == E_BattleMode.BraveMelee
//            )
//        {
//            BATTLE_RESULT _result = result;
//            // ��ʱ����
//            if (result == BATTLE_RESULT.BATTLE_RESULT_TIMEOUT)
//            {
//                // ����ʣ��Ӣ����������
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

//                // ����ս�����油
//                if (BattleMode == E_BattleMode.WarOfGods)
//                {
//                    playerAlives += SubtituteRoles.FindAll(sub => sub.Side == RoleSide.Player).Count;
//                    enemyAlives += SubtituteRoles.FindAll(sub => sub.Side == RoleSide.Enemy).Count;
//                }

//                if (playerAlives < enemyAlives)
//                    _result = BATTLE_RESULT.BATTLE_RESULT_DEFEAT;
//                // ����ʣ��Ӣ�������϶�
//                else if (playerAlives > enemyAlives)
//                {
//                    _result = BATTLE_RESULT.BATTLE_RESULT_VICTORY;
//                    winReson = BattleParam.WinReason.AliveWin;
//                }
//                else
//                {
//                    // ʣ��������ͬʱ�����˺��϶��߻�ʤ
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
//                    // ս���طŽ����ʾ��TODO:��
//                    foreach (Role role in this.GetSurvivors(RoleSide.Enemy))
//                    {
//                        if (role.Joint != null)
//                            role.Joint.Waiting();
//                    }

//#if !SERVER
//                    // ֱ��ս���Ĵ���
//                    if (this.IsLiveMode == true)
//                    {
//                        if (this.BattleMode == E_BattleMode.GuildChampion)
//                        {
//                            // ����սֱ��
//                            if (UIBattleGuildChampion.instance != null)
//                                UIBattleGuildChampion.instance.StartCoroutine(DelayInvoker.Invoke(UIBattleGuildChampion.instance.EndBattle, 2f));
//                        }
//                        else if (this.BattleMode == E_BattleMode.PersonChampion)
//                        {
//                            // ����սֱ������������--��ʾ1s�����
//                            Server.PCSPlusBattleReplay replayInfo = PCSBuffer.BattleLive.Instance.LiveBattleReplayList[PCSBuffer.BattleLive.Instance.LiveBattleInfo.battleIndex - 1];
//                            GameObject resultUI = (GameObject)MonoBehaviour.Instantiate(ResourcesLoader.Load("UI/prefab/UIPCSLiveBattleResult"));
//                            resultUI.GetComponent<UIPCSLiveBattleResult>().InitUIData(replayInfo);
//                        }

//                        return;
//                    }

//                    // ��ֱ��ս���Ĵ���
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
//                            // ���߾�����
//                            if (Logic.Instance.TopPvpReplayRecord != null)
//                            {
//                                //o.GetComponent<UIArenaRecord>().setRecordData(Logic.Instance.TopPvpReplayRecord, true);
//                                o.GetComponent<UIArenaRecord>()
//                                    .DelaySetRecordData(2f, Logic.Instance.TopPvpReplayRecord, true);
//                            }
//                        }
//                        else if (BattleMode == E_BattleMode.PersonChampion)
//                        {
//                            // ����ս
//                            if (Logic.Instance.PersonalChampionRecord != null)
//                            {
//                                //o.GetComponent<UIArenaRecord>().setRecordData(Logic.Instance.TopPvpReplayRecord, true);
//                                o.GetComponent<UIArenaRecord>()
//                                    .DelaySetRecordData(2f, Logic.Instance.PersonalChampionRecord, true);
//                            }
//                        }
//                        else if (BattleMode == E_BattleMode.HallOfWar)
//                        {
//                            // ս��Ӫ��
//                            if (Instance.HallOfWarRecord != null)
//                            {
//                                //o.GetComponent<UIArenaRecord>().setRecordData(Logic.Instance.TopPvpReplayRecord, true);
//                                o.GetComponent<UIArenaRecord>().DelaySetRecordData(2f, Instance.HallOfWarRecord, true);
//                            }
//                        }
//                        else if (BattleMode == E_BattleMode.WarOfGods)
//                        {
//                            // ����ս
//                            if (Instance.WarOfGodsRecord != null)
//                            {
//                                o.GetComponent<UIArenaRecord>().DelaySetRecordData(2f, Instance.WarOfGodsRecord, true);
//                            }
//                        }
//                        else
//                        {
//                            // ��ͨ������
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

//                // ս���ط�ֱ��return
//                return;
//            }

//            /*string allLogs = Logger.GetLogs();
//            string logname = "YYYYYYUUUUUU1111" + ".log";
//            File.WriteAllText("./" + logname, allLogs);*/
//            //
//            if (result == Common.BATTLE_RESULT.BATTLE_RESULT_VICTORY)
//            {
//                //TODO:�¼�����
//                if (this.BattleMode != E_BattleMode.Unreal)
//                {
//                    this.Scene.CollectLoots(3f);
//                }
//            }
//            else if (result == Common.BATTLE_RESULT.BATTLE_RESULT_DEFEAT)
//            {
//                //TODO:�¼�����
//                foreach (Role role in this.GetSurvivors(RoleSide.Enemy))
//                {
//                    if (role.Joint != null)
//                        role.Joint.Waiting();
//                }
//                //ע��ؿ��˳��¼�
//            }
//            else if (result == Common.BATTLE_RESULT.BATTLE_RESULT_CANCELED)
//            {
//                //TODO:�¼�����
//            }
//            else if (result == Common.BATTLE_RESULT.BATTLE_RESULT_TIMEOUT)
//            {
//                if (this.BattleMode == E_BattleMode.Crusade || this.BattleMode == E_BattleMode.CrusadeChallenge || this.BattleMode == E_BattleMode.Treasure || this.BattleMode == E_BattleMode.GrandChallenge || this.BattleMode == E_BattleMode.FirstBattle)
//                {
//                    //TODO:�¼�����

//                }
//                else if (this.BattleMode == E_BattleMode.GuildInstance || this.BattleMode == E_BattleMode.Outland || this.BattleMode == E_BattleMode.Temple)
//                {
//                }
//                else
//                {//this.BattleExitProcess��,�Ѵ����˳�ս����Ϣ _ �����ظ�UI
//                    //                    BattleParam param = new BattleParam(this.LevelData.LevelID, this.BattleMode, this.HeroIDList, this.BattleMode == E_BattleMode.Campaign, false);
//                    //                    param.LoseReson = "timeout";
//                    //                    this.Scene.BattleResultProcess(param, DataManager.Numerics[3].Arg);
//                }
//            }
//#if SERVER
//#else
//            if (!this.IsReplayMode)
//            {
//                // ˢ����궷ʿ����(ʤ��/ʧ��/��ʱ ���������)
//                bool isIn = false;
//                switch (this.BattleMode)
//                {
//                    case E_BattleMode.TimeRift: 	// ʱ���ѷ� pve
//                        isIn = this.HeroIDList.Contains(
//                            TaskBuffer.GetHeroBattleHeroID(
//                                TaskBuffer.heroBattleTypeDict[TaskBuffer.HEROBATTLE_TYPE.ADVCHAPTER]));
//                        if (isIn)
//                        {
//                            //�������ͨÿ�������ظ��ۼ�
//                            TaskBuffer.SetTimesFre(1, isIn);
//                        }
//                        break;
//                    case E_BattleMode.HeroTrial: 	// Ӣ�������� pve
//                        isIn = this.HeroIDList.Contains(
//                            TaskBuffer.GetHeroBattleHeroID(
//                                TaskBuffer.heroBattleTypeDict[TaskBuffer.HEROBATTLE_TYPE.ADVCHAPTER]));
//                        if (isIn)
//                        {
//                            //�������ͨÿ�������ظ��ۼ�
//                            TaskBuffer.SetArmFre(1, isIn);
//                        }
//                        break;
//                    case E_BattleMode.Outland: 	// ����֮�� pve
//                        isIn = this.HeroIDList.Contains(
//                            TaskBuffer.GetHeroBattleHeroID(
//                                TaskBuffer.heroBattleTypeDict[TaskBuffer.HEROBATTLE_TYPE.OUTLAND]));
//                        if (isIn)
//                        {
//                            TaskBuffer.SetOutlandFre(1, isIn);
//                        }
//                        break;
//                    case E_BattleMode.Crusade:
//                    case E_BattleMode.CrusadeChallenge: 	// Զ�� pvp
//                        isIn = this.HeroIDList.Contains(
//                            TaskBuffer.GetHeroBattleHeroID(
//                                TaskBuffer.heroBattleTypeDict[TaskBuffer.HEROBATTLE_TYPE.CRUSADESTAGE]));
//                        if (isIn)
//                        {
//                            TaskBuffer.SetCompleteCrusadeStageFre(1, isIn);
//                        }
//                        break;

//                    case E_BattleMode.GrandChallenge: 	// �۷���ս
//                        isIn = this.HeroIDList.Contains(
//                            TaskBuffer.GetHeroBattleHeroID(
//                                TaskBuffer.heroBattleTypeDict[TaskBuffer.HEROBATTLE_TYPE.GRANDCHALLENGE]));
//                        if (isIn)
//                        {
//                            TaskBuffer.SetCompleteGrandChallengeStagePre(1, isIn);
//                        }
//                        break;
//                    case E_BattleMode.Treasure: 	// �ر���Ѩ
//                        isIn = this.HeroIDList.Contains(
//                            TaskBuffer.GetHeroBattleHeroID(
//                                TaskBuffer.heroBattleTypeDict[TaskBuffer.HEROBATTLE_TYPE.EXCAVATE]));
//                        if (isIn)
//                        {
//                            TaskBuffer.SetExcavateBattleFre(1, isIn);
//                        }
//                        break;
//                    case E_BattleMode.WarOfGods: 	// ��������
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
//                    case E_BattleMode.HallOfWar: 	// ս��Ӫ��
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
//            //���͹ؿ��˳���Ϣ,�ҵط�ע�����

//            // ���߾����������⴦��added 20150805
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
//            else if (BattleMode == E_BattleMode.WarOfGods)//����
//            {
//                UIBattle.Instance.WarOfGodsRegion.BattlOver();
//                WarOfGodsManager.Instance.WarOfGodsEndBattleReq(returnData =>
//                {
//                    var param = new BattleParam(LevelData.LevelID, BattleMode, HeroIDList, false,
//                        BattleResult == BATTLE_RESULT.BATTLE_RESULT_VICTORY, winReson);
//                    Scene.BattleResultProcess(param, DataManager.Numerics[3].Arg);
//                });
//            }
//            else if (BattleMode == E_BattleMode.BraveMelee)//�����Ҷ�
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
//            else if (BattleMode == E_BattleMode.Unreal)//ChickenDinner ս�������߼�����
//            {
//                if (!UnrealTutorailManager.Instance.isNewPlayer)
//                {
//                    UnrealDataManager.Instance.EndBattleReq();
//                }
//            }
//            else
//            {
//                // ˢ�¹ؿ���Ϣ
//                if (result == BATTLE_RESULT.BATTLE_RESULT_VICTORY && !this.IsReplayMode)
//                {
//                    LevelBuffer.CompleteLevel(this.LevelData.LevelID, this.ResultStars, this.BattleMode);

//                    TaskBuffer.triggerFarmStageTask(this.LevelData.LevelID);
//                    //��������׷�����
//                    TaskBuffer.triggerTrackTask("CompleteStage", 1, this.LevelData.LevelID);

//                    // ˢ��ÿ���������
//                    switch (this.BattleMode)
//                    {
//                        case E_BattleMode.Campaign: 	// �ؿ�
//                            if (LevelBuffer.currentSelectLevel >= LevelBuffer.EliteLevelStarIndex && LevelBuffer.currentSelectLevel <= 20000)
//                                Player.Instance.SetEliteRecord(LevelBuffer.currentSelectLevel, 1);

//                            //���������ؿ����� YuHaizhi 20190128
//                            else if (LevelBuffer.IsMiracLevel(LevelBuffer.currentSelectLevel))
//                                Player.Instance.SetMiracRecord(LevelBuffer.currentSelectLevel, 1);
//                            break;
//                        case E_BattleMode.TimeRift: 	// ʱ���ѷ� pve
//                            TaskBuffer.SetTimesFre(1);
//                            //��������׷�����
//                            TaskBuffer.triggerTrackTask("FarmChapter102");
//                            break;
//                        case E_BattleMode.HeroTrial: 	// Ӣ�������� pve
//                            TaskBuffer.SetArmFre(1);
//                            //��������׷�����
//                            TaskBuffer.triggerTrackTask("FarmChapter103");
//                            break;
//                        case E_BattleMode.Outland: 		// ����֮�� pve
//                            TaskBuffer.SetOutlandFre(1);
//                            //��������׷�����
//                            TaskBuffer.triggerTrackTask("FarmChapter105");
//                            break;
//                        case E_BattleMode.Crusade:		// Զ�� pvp
//                            TaskBuffer.SetCompleteCrusadeStageFre(1);
//                            //��������׷�����
//                            TaskBuffer.triggerTrackTask("CompleteCrusadeStage");
//                            break;
//                        case E_BattleMode.GrandChallenge:		// �۷���ս
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
//    /// ս��tick
//    /// </summary>
//    /// <param name="passTime"></param>
//    public void onTimer(double passTime)
//    {
//        if (!this.Running)
//            return;

//        // ִ��΢������¼
//        this.ProcessOP();
//        // ִ��ʵ���߼�+���ָ���
//        this.ProcessEntities(passTime);
//        this.doneFrameCount++;
//#if BATTLE_LOG
//		if (Global.BattleLog)
//			Logic.log(this.ToString());
//#endif
//        ProcessSubstitute(RoleSide.Player);
//        ProcessSubstitute(RoleSide.Enemy);

//        // ս���Ƿ�����Ĵ����߼�
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
//    /// �油
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
//    /// ս���������
//    /// </summary>
//    /// <param name="dt"></param>
//    void VerifyBattleResult(double dt)
//    {
//        this.LimitTime = this.LimitTime - dt;
//        //ս�����������ж�
//        //û�е�����
//        BATTLE_RESULT result = BATTLE_RESULT.BATTLE_RESULT_DEFEAT;
//        bool resume = true;
//        // ����ʣ��Ӣ����������
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
//                //ˮ��
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
//            // ��ʱ���ǣ�ͬ���ھ�
//            if (this.BattleMode == E_BattleMode.Crusade || this.BattleMode == E_BattleMode.CrusadeChallenge || this.BattleMode == E_BattleMode.Treasure || this.BattleMode == E_BattleMode.HallOfWar || this.BattleMode == E_BattleMode.GrandChallenge)
//                foreach (Role role in this.Roles)
//                {
//                    role.ExParamI3 = 1;         // �������󸴻�
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
//                //����ˮ��������
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

//                    // fix:11/29/2016������timer����effect
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
//                    // ���ḱ����ʱ�䵽�˼���ֹ
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
//    /// �˳�ս����Ϣ����
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
//                // ������ս��ʤ����֤ʧ��
//                BattleParam param = new BattleParam(this.LevelData.LevelID, this.BattleMode, this.HeroIDList, this.BattleMode == E_BattleMode.Campaign, false);
//                param.LoseReson = "fail";
//                this.Scene.BattleResultProcess(param, DataManager.Numerics[3].Arg);
//            }
//            else
//            {
//                //TODO:����ʤ����Ч
//                BattleParam param = new BattleParam(this.LevelData.LevelID, this.BattleMode, this.HeroIDList, this.BattleMode == E_BattleMode.Campaign, true);
//                param.IsReplay = this.IsReplayMode;
//                this.Scene.BattleResultProcess(param, DataManager.Numerics[10].Arg);//ս��ʤ��
//            }
//        }
//        else if (result == Common.BATTLE_RESULT.BATTLE_RESULT_DEFEAT)
//        {
//            BattleParam param = new BattleParam(this.LevelData.LevelID, this.BattleMode, this.HeroIDList, this.BattleMode == E_BattleMode.Campaign, false);
//            param.LoseReson = "fail";
//            this.Scene.BattleResultProcess(param, DataManager.Numerics[3].Arg);
//            //TODO:�޸������ȡ
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
//        // �����ٻ���
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
//    /// ����ս��
//    /// </summary>
//    private void Complete()
//    {
//        int deadNum = 0;
//        foreach (Role role in this.Roles)
//        {
//            if (role.Side == RoleSide.Player && role.IsHero && role.Config.IsDemon != true && role.IsDead)
//                deadNum++;
//        }
//        // ս����������ǣ��򵥼��㣩
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
//    /// ���ս������������Ϣ
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
//                    // ͬһ����������ͨ���ؿ����ܱ���
//                    float pastWeight = 0;
//                    float totalWeight = 0;
//                    // ͬһ����ͨ���ı���
//                    float levelPastWeight = 0;
//                    float levelTotalWeight = 0;

//                    // ��������ڴ�Ĺؿ�ͬһ���е����йؿ�
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
//                            // ÿһ������
//                            tmpConfig = tmpComConfigs[key][_key];
//                            // С�ڵ�ǰ�ؿ�id��Ϊ�Ѿ�ͨ���Ĺؿ�
//                            if (key < CombatConfig.LevelID)
//                            {
//                                pastWeight += tmpConfig.RaidWaveWeight;

//                            }
//                            else if (key == CombatConfig.LevelID)
//                            {
//                                // ��ǰ�ؿ����Ѿ�ͨ���Ĳ���
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
//                        // ��ȡ�˺�
//                        totalDamage += Roles[i].IsHero ? Statistic.GetHeroDamage(Roles[i].ID) : 0;
//                    }

//                    float tmpWeight = (1f - currentBleed / totalBleed) * CombatConfig.RaidWaveWeight;
//                    pastWeight += tmpWeight;
//                    levelPastWeight += tmpWeight;


//                    // �ܽ���
//                    msg.guild.instanceEnd.progress = (int)Math.Floor(pastWeight / totalWeight * 10000);
//                    // ���ؿ��Ľ���
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

//                    //��ɴ˹ؿ�ʱ����������� YuHaizhi 20190226
//                    for (int type = 0; type < TaskBuffer.TaskTypes; type++)
//                    {
//                        Server.UserTask legendTask_ = TaskBuffer.getWorkingHeroTask(0, type);
//                        if (legendTask_ != null)
//                        {
//                            if (type < 2 && legendTask_.id > 1 || type >= 2)//���桢���������ĵ�һ��Ϊ��װ������������Ϊ����
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
//                                    break;//������->���������ȼ�
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
//    /// ���λ����Ч���ӵ�ʵ�壩
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
//        // �ָ�����Ӣ�۶���
//        this.ResumeAllAnimation();
//        // �ָ���Ļ��ɫ
//        if (this.Scene != null)
//        {
//            this.Scene.PlayEffect(ScreenEffect.Freeze, 0, 0f);
//        }
//    }

//    /// <summary>
//    /// ��������Ӣ�۶���
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
//        //�ͻ��˴�ӡlog
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
//    /// ����λ�÷�����Ч��DirectEffect�����紬���ģ���ˮ���ܣ�һ��ˮ��
//    /// �ǰ���Role��ɫ���ϵ���Ч
//    /// ��Ҫ����EffectJoint�����ƣ�����λ�á�����ȣ�
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
//        bool factionIdentical = false; // ��ϵ�Ǻ�
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

//        // ��ϵ���Ǻ�
//        if (!factionIdentical)
//        {
//            return;
//        }

//        string totemEffect = Const.GetTotemEffect(conditonFaction);

//        // �ҳ����д��������Լӳ�
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
//    //����ͼ�ڼ���  
//    public void TriggerFireTotemByActiveTalent(Talent talent, Role from, Role target)
//    {
//        TotemLevelData totem = null;
//        if (from != null && target != null && from.Side == target.Side) //�ж���Ӫ�Ƿ���ͬ
//            return;

//        totem = GetTotemLevelData(from);
//        if (totem == null)
//            return;
//        string conditonFaction = string.Empty;
//        bool factionIdentical = false; // ��ϵ�Ǻ�
//        var group = !DataManager.Instance.TalentGroups.ContainsKey(from.Data.ID) ?
//               null : DataManager.Instance.TalentGroups[from.Data.ID].Where(data => (data.Value.TalentGroupID == talent.Data.TalentGroupID)).Select(data => data.Value).First();
//        if (group != null && group.TalentTags.Count == 0 && group.ParentID > 0) //Ϊ�˽�������˺���ͨ���Ӽ���ʵ�ֵ�
//            group = DataManager.Instance.TalentGroups[from.Data.ID].Where(data => (data.Value.TalentGroupID == group.ParentID)).Select(data => data.Value).First();
//        foreach (string triggerFaction in totem.Condition)
//        {
//            string[] condition = triggerFaction.Split(':');
//            conditonFaction = condition[1];
//            if (group != null && group.TalentTags.Contains(conditonFaction)) //�����ǻ���ϵ��
//            {
//                factionIdentical = true;
//                break;
//            }
//        }

//        // ��ϵ���Ǻ�
//        if (!factionIdentical)
//        {
//            return;
//        }

//        // ���漼�ܴ��������Լӳ�
//        Dictionary<int, int> addAttrDict = ChooseFireTriggerAdd(totem, TotemTriggerType.ActiveTalent);
//        if (addAttrDict.Count > 0)
//        {
//            if (!target.Abilities.Any(ability => ability.AbilityData.ID == totem.InitiativeBuff))
//            {
//                target.FireTotemCount = 0; //��ʼ��������
//                target.FireTotemTime = (float)this.LimitTime + DataManager.Instance.DNumerics[85].Arg;
//            }
//            //������ ����ͼ�ڵ�����ʱ����ȴʱ�� 2019-10-9
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
//    //��ϵͼ�ڼ���
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
//        bool factionIdentical = false; // ��ϵ�Ǻ�
//        foreach (string triggerFaction in totem.Condition)
//        {
//            string[] condition = triggerFaction.Split(':');
//            conditonFaction = condition[1];
//            if (ability != null && ability.AbilityTags != null && ability.AbilityTags.Contains(conditonFaction)) //�����ǻ���ϵ��
//            {
//                factionIdentical = true;
//                break;
//            }
//        }
//        // ��ϵ���Ǻ�
//        if (!factionIdentical)
//            return false;
//        // ���ܴ��������Լӳ�
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
//    /// ��ȡ target ��Ӫ��ͼ������
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
//    /// �ҳ��� _type ���������Լӳ�
//    /// </summary>
//    /// <param name="totem"></param>
//    /// <param name="_type">��������</param>
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
