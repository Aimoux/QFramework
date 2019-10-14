using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using QF;
using Common;
using GameData;

public class BattleSceneLoader : MonoSingleton<BattleSceneLoader>
{



    public void LoadLevel(BATTLEMODE mode, string scene, UnityAction onLoad)
    {


        //QF.LevelLoader

    }




///// <summary>
///// 战斗场景加载器
///// </summary>
//class BattleSceneLoader : BehaviourSingleton<BattleSceneLoader>
//{

//    // Use this for initialization
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }
//    void OnDestroy()
//    {
//        ClearCache();
//        ClearRoleCache(true);
//    }
//    private UISceneLoading LoadingUI = null;

//    private int total = 1;
//    private int current = 0;
//    private List<Common.Hero> roles = new List<Common.Hero>();
//    // Dictionary<string, int> PreloadRoles = new Dictionary<string, int>();//被替代
//    Dictionary<string, IntStr> PreloadUnits = new Dictionary<string, IntStr>();
//    Dictionary<string, int> PreloadFXs = new Dictionary<string, int>();

//    private static Dictionary<int, List<int>> hardcodeSummonResource = new Dictionary<int, List<int>>{

//        {47,new List<int>{130}},
//        {43,new List<int>{128}},
//        {1913, new List<int> {1910, 128} },//公会Boss亡灵审判官召唤物 YuHaizhi 20190819
//	};

//    private static Dictionary<string, List<int>> hardcodeRoleResource = new Dictionary<string, List<int>>{

//        {"M148",new List<int>{415}},
//        {"C100007",new List<int>{396}},
//        {"C100005",new List<int>{323}},
//        {"C059",new List<int>{313,375}}
//    };
//    private static Dictionary<int, List<int>> hardcodeTalentResource = new Dictionary<int, List<int>> {

//        {14921,new List<int>{248}},
//        {11631,new List<int>{270}},
//        {12591,new List<int>{289}},
//        {12771,new List<int>{291,293}},
//        {15091,new List<int>{291,293}},
//        {13441,new List<int>{309}},
//        {14001,new List<int>{316}},
//        {14271,new List<int>{318,319,320}},
//        {15191,new List<int>{334}},
//        {13581,new List<int>{375}},
//        {13601,new List<int>{375}}
//    };


//    public static GameObject CacheRoot = null;
//    public static GameObject RolesCacheRoot = null;
//    public void LoadLevel(string name, List<Common.Hero> heroes, GameData.LevelData level, UnityAction onLoad, E_BattleMode mode = E_BattleMode.Campaign, int param = 0)
//    {
//        DebugStatisticsUtil.Instance.LogBattleLoading(true);

//        this.roles.Clear();
//        ClearCache();
//        GameObject o = (GameObject)Instantiate(ResourcesLoader.Load("UI/prefab/UISceneLoading"));
//        if (!Tutorial.Instance.IsFinish(Tutorial.Tuto_5V5))
//        {
//            o.SetActive(false);
//        }
//        this.LoadingUI = o.GetComponent<UISceneLoading>();
//        LevelLoader.OnProgress += this.LoadingUI.OnProgress;
//        //显示加载UI
//        //TODO:增加加载背景及进度条，或者在进度条增加游戏小提示
//        List<string> bundles = new List<string>();
//        //根据当前玩家的数据，生成必须的资源队列
//        if (heroes != null)
//        {
//            foreach (Common.Hero hero in heroes.ToArray())
//            {
//                bundles.Add(string.Format("Units/C{0:D3}", hero.tid));
//                this.roles.Add(hero);
//                if (hardcodeSummonResource.ContainsKey(hero.tid))
//                {
//                    foreach (int summer in hardcodeSummonResource[hero.tid])
//                    {
//                        this.roles.Add(new Common.Hero { tid = summer });
//                    }
//                }
//            }
//        }

//        if (level != null)
//        {
//            Dictionary<int, int> monsterNum = new Dictionary<int, int>();
//            Dictionary<int, int> roundMonsterNum = new Dictionary<int, int>();
//            for (int round = 1; round <= level.Rounds; round++)
//            {
//                roundMonsterNum.Clear();
//                GameData.CombatConfigData battle;
//                if (mode == E_BattleMode.Temple)
//                {
//                    battle = LogicManager.TransfromBattleData(DataManager.Instance.TempleMonsterDataGroups[param]);//拿战斗数据
//                }
//                else
//                {
//                    battle = DataNewManager.Instance.CombatConfigs.Value[level.LevelID][round];
//                }

//                for (int i = 1; i <= 5; i++)
//                {
//                    int id = (int)typeof(GameData.CombatConfigData).GetProperty(string.Format("Monster{0}ID", i)).GetValue(battle, null);
//                    if (id > 0)
//                    {
//                        if (roundMonsterNum.ContainsKey(id))
//                        {
//                            roundMonsterNum[id]++;
//                        }
//                        else
//                        {
//                            roundMonsterNum[id] = 1;
//                        }

//                        if (monsterNum.ContainsKey(id) && monsterNum[id] >= roundMonsterNum[id])
//                        {
//                            continue;
//                        }

//                        if (monsterNum.ContainsKey(id))
//                        {
//                            monsterNum[id]++;
//                        }
//                        else
//                        {
//                            monsterNum[id] = 1;
//                        }

//                        this.roles.Add(new Common.Hero { tid = id });
//                        string modelName = DataManager.Instance.Roles[id].Model;
//                        if (DataManager.Instance.Models.ContainsKey(modelName))
//                        {
//                            modelName = DataManager.Instance.Models[modelName].Resource;
//                        }
//                        bundles.Add(string.Format("Units/{0}", modelName));

//                        //预加载公会Boss召唤物 YuHaizhi 20190819
//                        if (hardcodeSummonResource.ContainsKey(id))
//                        {
//                            foreach (int summer in hardcodeSummonResource[id])
//                            {
//                                if (roles.Any(e => e.tid == summer))
//                                    continue;

//                                this.roles.Add(new Common.Hero { tid = summer });
//                                string summerName = DataManager.Instance.Roles[summer].Model;
//                                if (DataManager.Instance.Models.ContainsKey(summerName))
//                                {
//                                    modelName = DataManager.Instance.Models[summerName].Resource;
//                                }
//                                bundles.Add(string.Format("Units/{0}", summerName));
//                            }
//                        }


//                    }
//                }
//            }
//        }
//        bundles = bundles.Distinct().ToList();

//        //启动加载过程
//        //StartCoroutine(PreloadAssets("Battle", bundles, onLoad));
//        StartCoroutine(LoadingScene("Battle", onLoad));
//    }
//    public void LoadUnrealLevel(string name, List<Common.Hero> heroes, GameData.LevelData level, UnityAction onLoad)
//    {
//        DebugStatisticsUtil.Instance.LogBattleLoading(true);
//        this.roles.Clear();
//        ClearRoleCache();
//        List<string> bundles = new List<string>();
//        //根据当前玩家的数据，生成必须的资源队列
//        if (heroes != null)
//        {
//            foreach (Common.Hero hero in heroes.ToArray())
//            {
//                bundles.Add(string.Format("Units/C{0:D3}", hero.tid));
//                this.roles.Add(hero);
//                if (hardcodeSummonResource.ContainsKey(hero.tid))
//                {
//                    foreach (int summer in hardcodeSummonResource[hero.tid])
//                    {
//                        this.roles.Add(new Common.Hero { tid = summer });
//                    }
//                }
//            }
//        }
//        //启动加载过程
//        StartCoroutine(LoadingUnrealScene(onLoad));
//    }

//    private IEnumerator LoadingScene(string scene, UnityAction onLoad)
//    {
//        // 战斗回放再试一次特别处理 http://jira.ggdev.co/browse/HERO-2888
//        if (!Logic.Instance.IsTryAgain)
//        {
//            UIManager.Instance.SaveOpenedWindow();
//        }
//        else
//        {
//            Debug.Log("Don't SaveOpenedWindow because TryAgain!");
//        }

//        //PreloadRoles.Clear();
//        PreloadUnits.Clear();
//        PreloadFXs.Clear();
//        List<int> hardcorefx = new List<int>();
//        Dictionary<int, bool> heroTrans = new Dictionary<int, bool>(); //英雄变身预加载
//        foreach (var hero in this.roles.ToArray())
//        {
//            var id = hero.tid;
//            GameData.RoleData role = RoleSkinSwitcher.Instance.GetCurrentSkinRole(hero); //若拥有神器，返回的RoleData.Model已被替换为ModelSwitch
//            if (role == null)
//                continue;

//            string modelName = role.Model;
//            if (DataManager.Instance.Models.ContainsKey(modelName))
//            {
//                modelName = DataManager.Instance.Models[modelName].Resource;//modelName被替换为不含ModelSwitch信息的resource（prefab名字），若不替换则找不到资源，Resource.Load()失败
//            }

//            string aliasName = Role.IsMirac(hero) ? role.Model : "";
//            AddPreloadUnit(modelName, aliasName);

//            if (hardcodeRoleResource.ContainsKey(role.Model))//M148、C100007、C100005、C059
//            {
//                hardcorefx.AddRange(hardcodeRoleResource[role.Model]);
//            }
//            //英雄变身预加载
//            if (role.RoleType == "Hero" && role.PeriodGroup != 0 && !heroTrans.ContainsKey(role.PeriodGroup))
//            {
//                heroTrans.Add(role.PeriodGroup, hero.currSkinId > 0);
//            }

//            foreach (GameData.TalentGroupData tg in DataManager.Instance.TalentGroups[id].Values)
//            {
//                foreach (GameData.TalentData talent in DataManager.Instance.Talents[tg.TalentGroupID].Values)
//                {
//                    AddPreloadFX(talent.CastingEffect); // 施法特效（绑定在Role身上--只是播放下，不需要更新）RoleJoint.AddEffect--RoleController.AddEffect---GameObjectManager.Instance.CreateEffect
//                    AddPreloadFX(talent.HitEffect);     // 命中特效（绑定在Role身上--只是播放下，不需要更新）RoleJoint.AddEffect--RoleController.AddEffect---GameObjectManager.Instance.CreateEffect
//                    AddPreloadFX(talent.DirectEffect);  // 直接特效（位置，方向特效--需要EffectJoint来控制，需要更新）Logic.PlayEffect----new EffectJoint(GameObjectManager.Instance.CreateEffect)----Logic.Instance.Scene.AddJoint
//                    AddPreloadFX(talent.FlyResource);   // 飞行特效（位置，方向特效--需要EffectJoint来控制，需要更新）new EffectJoint(effect, EffectType.Cast)---this.Scene.AddJoint(actor)
//                    AddPreloadFX(talent.BounceEffect);  // 弹跳特效（位置，方向特效--需要EffectJoint来控制，需要更新）new EffectJoint(this, EffectType.Bounce)---Logic.Instance.Scene.AddJoint(effect);

//                    if (hardcodeTalentResource.ContainsKey(talent.ID))
//                    {
//                        hardcorefx.AddRange(hardcodeTalentResource[talent.ID]);
//                    }

//                    if (talent.AbilityIDs != null)
//                    {
//                        foreach (int abilityID in talent.AbilityIDs)
//                        {
//                            GameData.AbilityData ability_info = DataManager.Instance.Abilities[abilityID];
//                            AddPreloadFX(ability_info.Effect);
//                            AddPreloadFX(ability_info.HitEffect);
//                        }
//                    }
//                }
//            }
//        }

//        foreach (int eid in hardcorefx)
//        {
//            string fxurl = DataManager.Instance.Effects[eid];
//            if (PreloadFXs.ContainsKey(fxurl))
//            {
//                continue;
//            }

//            AddPreloadFX(fxurl);
//        }
//        ////英雄变身预加载
//        foreach (int trans in heroTrans.Keys)
//        {
//            if (DataManager.Instance.Transforms.ContainsKey(trans))
//            {
//                var transfroms = DataManager.Instance.Transforms[trans];
//                foreach (TransformData item in transfroms.Values)
//                {
//                    if ((!heroTrans[trans] && item.Model.Contains("skin")) || (heroTrans[trans] && !item.Model.Contains("skin")))
//                    {
//                        continue;
//                    }
//                    if (!DataManager.Instance.Models.ContainsKey(item.Model))
//                    {
//                        continue;
//                    }
//                    else
//                    {
//                        if (DataManager.Instance.Models[item.Model].Resource != item.Model)
//                            continue;
//                    }
//                    if (!PreloadUnits.ContainsKey(item.Model))
//                    {
//                        AddPreloadUnit(item.Model);
//                    }
//                }
//            }
//        }

//        //怪物变身预加载(22章公会boss) YuHaizhi 20190819
//        foreach (Common.Hero monster in roles)
//        {
//            RoleData data = DataManager.Instance.Roles[monster.tid];
//            if (data.RoleType == "Monster" && DataManager.Instance.Transforms.ContainsKey(data.PeriodGroup))
//            {
//                foreach (var transdata in DataManager.Instance.Transforms[data.PeriodGroup])
//                {
//                    ModelData mondata = DataManager.Instance.Models[transdata.Value.Model];
//                    if (!PreloadUnits.ContainsKey(mondata.Resource))
//                        AddPreloadUnit(mondata.Resource, transdata.Value.Model);//资源名，模型名
//                }
//            }
//        }

//        // AddPreloadRole("S003");
//        AddPreloadUnit("S003");
//        Preload("FX/", "fx_combat_drop_money", 5);
//        Preload("UI/prefab/", "UIFloatingBar", 15);
//        Preload("UI/prefab/", "UIPopupText", 25);
//        // float total = PreloadRoles.Count + PreloadFXs.Count;
//        float total = PreloadUnits.Count + PreloadFXs.Count;

//        float curload = 0.0f;
//        //foreach (KeyValuePair<string, int> kv in PreloadRoles)
//        //{
//        //    Preload("Units/", kv.Key, kv.Value);
//        //    curload += 1f;

//        //    LevelLoader.Instance.NotifyProgress(curload / total * 0.8f);
//        //    if (curload % 4 == 1)
//        //    { 
//        //        yield return new WaitForEndOfFrame();
//        //    }
//        //}

//        foreach (KeyValuePair<string, IntStr> kv in PreloadUnits)
//        {
//            Preload("Units/", kv.Key, kv.Value.count, kv.Value.aliasName);
//            curload += 1f;

//            LevelLoader.Instance.NotifyProgress(curload / total * 0.8f);
//            if (curload % 4 == 1)
//            {
//                yield return new WaitForEndOfFrame();
//            }
//        }

//        Preload("FX/", DataManager.Instance.Effects[2], 1);
//        foreach (KeyValuePair<string, int> kv in PreloadFXs)
//        {
//            Preload("FX/", kv.Key, kv.Value * 5);
//            curload += 1f;
//            LevelLoader.Instance.NotifyProgress(curload / total * 0.8f);
//            if (curload % 8 == 1)
//            {
//                yield return new WaitForEndOfFrame();
//            }
//        }

//        // 应该是回放的时候不重新load关卡，由于战斗UI的扩展太乱了，现在不好做扩展了
//        if (Application.loadedLevelName == "Battle" && Logic.Instance.IsTryAgain && Logic.Instance.IsLiveMode)
//        {
//            GameObject o = GameObject.Find("UISceneLoading(Clone)");
//            if (o != null)
//            {
//                Destroy(o);
//            }

//            onLoad();
//        }
//        else
//        {
//            LevelLoader.LoadLevel(scene, onLoad);
//        }

//    }

//    private IEnumerator LoadingUnrealScene(UnityAction onLoad)
//    {
//        // 战斗回放再试一次特别处理 http://jira.ggdev.co/browse/HERO-2888
//        if (!Logic.Instance.IsTryAgain)
//        {
//            UIManager.Instance.SaveOpenedWindow();
//        }
//        else
//        {
//            Debug.Log("Don't SaveOpenedWindow because TryAgain!");
//        }
//        PreloadUnits.Clear();
//        PreloadFXs.Clear();
//        foreach (var hero in roles.ToArray())
//        {
//            var id = hero.tid;
//            GameData.RoleData role = RoleSkinSwitcher.Instance.GetCurrentSkinRole(hero, true); //若拥有神器，返回的RoleData.Model已被替换为ModelSwitch
//            if (role == null)
//                continue;

//            string modelName = role.Model;
//            if (DataManager.Instance.Models.ContainsKey(modelName))
//            {
//                modelName = DataManager.Instance.Models[modelName].Resource;//modelName被替换为不含ModelSwitch信息的resource（prefab名字），若不替换则找不到资源，Resource.Load()失败
//            }

//            //string aliasName = Role.IsMirac(hero) ? role.Model : "";
//            AddPreloadUnit(modelName, modelName);

//            foreach (GameData.UnrealTalentGroupData tg in DataManager.Instance.UnrealTalentGroups[id].Values)
//            {
//                if (tg.Unlock <= 1)
//                {
//                    foreach (GameData.UnrealTalentData talent in DataManager.Instance.UnrealTalents[tg.TalentGroupID].Values)
//                    {
//                        AddPreloadFX(talent.CastingEffect); // 施法特效（绑定在Role身上--只是播放下，不需要更新）RoleJoint.AddEffect--RoleController.AddEffect---GameObjectManager.Instance.CreateEffect
//                        AddPreloadFX(talent.HitEffect);     // 命中特效（绑定在Role身上--只是播放下，不需要更新）RoleJoint.AddEffect--RoleController.AddEffect---GameObjectManager.Instance.CreateEffect
//                        AddPreloadFX(talent.DirectEffect);  // 直接特效（位置，方向特效--需要EffectJoint来控制，需要更新）Logic.PlayEffect----new EffectJoint(GameObjectManager.Instance.CreateEffect)----Logic.Instance.Scene.AddJoint
//                        AddPreloadFX(talent.FlyResource);   // 飞行特效（位置，方向特效--需要EffectJoint来控制，需要更新）new EffectJoint(effect, EffectType.Cast)---this.Scene.AddJoint(actor)
//                        AddPreloadFX(talent.BounceEffect);  // 弹跳特效（位置，方向特效--需要EffectJoint来控制，需要更新）new EffectJoint(this, EffectType.Bounce)---Logic.Instance.Scene.AddJoint(effect);

//                        if (talent.AbilityIDs != null)
//                        {
//                            foreach (int abilityID in talent.AbilityIDs)
//                            {
//                                GameData.AbilityData ability_info = DataManager.Instance.Abilities[abilityID];
//                                AddPreloadFX(ability_info.Effect);
//                                AddPreloadFX(ability_info.HitEffect);
//                            }
//                        }
//                    }
//                }
//            }
//        }
//        Preload("FX/", "fx_direct_C100107_ult", 9);
//        Preload("FX/", "fx_combat_drop_money", 5);
//        yield return null;
//        Preload("UI/prefab/", "UIFloatingBar", 15);
//        yield return null;
//        Preload("UI/prefab/", "UIPopupText", 25);
//        yield return null;
//        float curload = 0f;
//        foreach (KeyValuePair<string, IntStr> kv in PreloadUnits)
//        {
//            PreloadRole("Units/", kv.Key, kv.Value.count, kv.Value.aliasName);
//            curload += 1f;
//            yield return null;
//        }
//        foreach (KeyValuePair<string, int> kv in PreloadFXs)
//        {
//            Preload("FX/", kv.Key, kv.Value * 5);
//            curload += 1f;
//            if (curload % 4 == 1)
//            {
//                yield return null;
//            }
//        }
//        if (onLoad != null)
//        {
//            onLoad();
//        }
//    }

//    void AddPreloadUnit(string role, string aliasName = "")
//    {
//        if (string.IsNullOrEmpty(role))
//        {
//            return;
//        }
//        if (!PreloadUnits.ContainsKey(role))
//        {
//            PreloadUnits.Add(role, new IntStr(aliasName));//count=0
//        }
//        PreloadUnits[role].count++;

//    }

//    void AddPreloadFX(string fx)
//    {
//        if (string.IsNullOrEmpty(fx))
//        {
//            return;
//        }
//        if (!PreloadFXs.ContainsKey(fx))
//        {
//            PreloadFXs[fx] = 0;
//        }
//        PreloadFXs[fx]++;
//    }

//    /// <summary>
//    /// 战斗场景资源预加载
//    /// </summary>
//    /// <param name="path"></param>
//    /// <param name="name"></param>
//    /// <param name="count"></param>
//    void Preload(string path, string name, int count, string aliasName = "")//增加参数解决重复加载的问题
//    {
//        if (string.IsNullOrEmpty(name))
//        {
//            return;
//        }

//        if (CacheRoot == null)
//        {
//            CacheRoot = new GameObject();
//            CacheRoot.name = "BattlePreloads";
//            DontDestroyOnLoad(CacheRoot);
//        }

//        if (GameUtil.NeedLowQuality())
//        {
//            return;
//        }

//        GameObject[] gos = new GameObject[count];
//        for (int i = 0; i < count; i++)
//        {
//            gos[i] = ObjectPoolManager.Take(path, name, aliasName);//add aliasName para
//            if (gos[i] != null)
//            {
//                // 每次Take---spwan后的GameObject放在CacheRoot节点下管理
//                gos[i].transform.SetParent(BattleSceneLoader.CacheRoot.transform, false);
//            }
//        }
//        for (int i = 0; i < count; i++)
//        {
//            ObjectPoolManager.Unspawn(gos[i]);
//        }
//    }

//    public static void ClearCache()
//    {
//        if (CacheRoot != null)
//        {
//            Destroy(CacheRoot);
//            CacheRoot = null;
//        }
//    }

//    #region ChickenDinner资源加载相关

//    /// <summary>
//    /// 战斗场景英雄资源加载
//    /// </summary>
//    /// <param name="path"></param>
//    /// <param name="name"></param>
//    /// <param name="count"></param>
//    public void PreloadRole(string path, string name, int count, string aliasName = "")//增加参数解决重复加载的问题
//    {
//        if (string.IsNullOrEmpty(name))
//        {
//            return;
//        }

//        if (RolesCacheRoot == null)
//        {
//            RolesCacheRoot = new GameObject();
//            RolesCacheRoot.name = "BattlePreloadRole";
//            DontDestroyOnLoad(RolesCacheRoot);
//        }

//        if (GameUtil.NeedLowQuality())
//        {
//            return;
//        }

//        GameObject[] gos = new GameObject[count];
//        for (int i = 0; i < count; i++)
//        {
//            gos[i] = ObjectPoolManager.TakeRole(path, name, aliasName);//add aliasName para
//            if (gos[i] != null)
//            {
//                // 每次Take---spwan后的GameObject放在CacheRoot节点下管理
//                gos[i].transform.SetParent(BattleSceneLoader.RolesCacheRoot.transform, false);
//            }
//        }
//        for (int i = 0; i < count; i++)
//        {
//            ObjectPoolManager.UnspawnRole(gos[i]);
//        }
//    }
//    public static void ClearRoleCache(bool isExitUnreal = false)
//    {
//        ClearCache();
//        if (RolesCacheRoot != null)
//        {
//            if (isExitUnreal)
//            {
//                Destroy(RolesCacheRoot);
//                RolesCacheRoot = null;
//            }
//            else
//            {
//                for (int i = 0; i < RolesCacheRoot.transform.childCount; i++)
//                {
//                    if (RolesCacheRoot.transform.GetChild(i) != null && RolesCacheRoot.transform.GetChild(i).gameObject.activeSelf)
//                    {
//                        RolesCacheRoot.transform.GetChild(i).gameObject.SetActive(false);
//                    }
//                }
//            }
//        }
//    }
//    #endregion

//}









}
