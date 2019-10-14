using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Common;
using GameData;

//战斗的入口
public static class LogicManager 
{

    //button事件可直接调用此处
    public static void EnterLevel(BATTLEMODE mode, string scene)
    {

        UnityAction onLoad = delegate ()
        {
            Logic.Instance.BattleMode = mode;
            Logic.Instance.InitAllies(new List<Role>());
            Logic.Instance.InitEnemies(new List<Role>());
            Logic.Instance.InitBattle();
        };

        Logic.Instance.LoadLevel(mode, scene, onLoad);
    }

    

    //


















    /// pve类:关卡战斗入口,需要传入 LevelData , List<Hero> ， isbot（是否为电脑）
    //public static void EnterLevel(E_BattleMode battleMode, LevelData level, List<Common.Hero> heroes, bool isbot,
    //    int roundId = 1,
    //    List<uint> operations = null,
    //                              int playerTotemType = 0,
    //                              int playerTotemLevel = 0,
    //                              int enemyTotemType = 0,
    //                              int enemyTotemLevel = 0)
    //{
    //    UnityEngine.Events.UnityAction action = delegate ()
    //    {
    //        Logic logic = Logic.Instance;
    //        logic.BattleMode = battleMode;
    //        logic.InitBattleCore();
    //        logic.LevelData = level;
    //        logic.OpRecords.Clear();
    //        if (operations != null)
    //        {
    //            foreach (int i in operations)
    //            {
    //                logic.OpRecords.Add(ParseOpRecord(i, 12, 4, 3));
    //            }
    //        }
    //        logic.MPBonus = level.MPBonus;
    //        // 对战双方图腾等级
    //        logic.playerTotemType = playerTotemType;
    //        logic.playerTotemLevel = playerTotemLevel;
    //        logic.enemyTotemType = enemyTotemType;
    //        logic.enemyTotemLevel = enemyTotemLevel;

    //        //初始化英雄，会加载第一次   
    //        logic.InitPlayerHeroes(heroes, isbot);
    //        //初始化场景包括刷怪
    //        CombatConfigData data = DataNewManager.Instance.CombatConfigs.Value[level.LevelID][roundId];//拿战斗数据
    //        logic.InitBattle(data);
    //    };
    //    Logic.Instance.LoadLevel(level.LevelResource, heroes, level, action);
    //}

    //第一次战斗
//    public static void Enter5V5(List<Common.Hero> heroes, List<Common.Hero> enemys, Dictionary<int, Common.HeroExtra> myHeroData, Dictionary<int, Common.HeroExtra> enemysHeroData)
//    {
//        LevelData level = DataManager.Instance.Levels[-2];
//#if !SERVER
//        LevelBuffer.currentSelectLevel = -2;
//#endif
//        Util.Random.Srand(80);
//        UnityEngine.Events.UnityAction action = delegate ()
//        {
//            Logic logic = Logic.Instance;
//            logic.BattleMode = E_BattleMode.FirstBattle;
//            logic.InitBattleCore();
//            logic.LevelData = level;
//            logic.HeroList = heroes;
//            logic.Round = 1;
//            logic.MPBonus = 1;
//            // 对战双方图腾等级
//            logic.playerTotemLevel = 0;
//            logic.enemyTotemLevel = 0;

//            QuickSort.Sort(heroes, new HeroAttackRangeComparer());
//            QuickSort.Sort(enemys, new HeroAttackRangeComparer());
//            logic.initMyHeroes(heroes, false, myHeroData);
//            logic.initEnemyHeoes(enemys, false, enemysHeroData);
//            foreach (Role role in logic.Roles)
//            {
//                role.Init();
//            }
//            CombatConfigData data = DataNewManager.Instance.CombatConfigs.Value[-2][1];//拿战斗数据
//            //初始化场景
//            logic.InitPvpBattle(data);
//        };
//        List<Common.Hero> roles = new List<Common.Hero>();
//        roles.AddRange(heroes);
//        roles.AddRange(enemys);
//        Logic.Instance.LoadLevel(level.LevelResource, roles, level, action);
//    }








}
