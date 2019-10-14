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
///// ս������������
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
//    // Dictionary<string, int> PreloadRoles = new Dictionary<string, int>();//�����
//    Dictionary<string, IntStr> PreloadUnits = new Dictionary<string, IntStr>();
//    Dictionary<string, int> PreloadFXs = new Dictionary<string, int>();

//    private static Dictionary<int, List<int>> hardcodeSummonResource = new Dictionary<int, List<int>>{

//        {47,new List<int>{130}},
//        {43,new List<int>{128}},
//        {1913, new List<int> {1910, 128} },//����Boss�������й��ٻ��� YuHaizhi 20190819
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
//        //��ʾ����UI
//        //TODO:���Ӽ��ر������������������ڽ�����������ϷС��ʾ
//        List<string> bundles = new List<string>();
//        //���ݵ�ǰ��ҵ����ݣ����ɱ������Դ����
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
//                    battle = LogicManager.TransfromBattleData(DataManager.Instance.TempleMonsterDataGroups[param]);//��ս������
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

//                        //Ԥ���ع���Boss�ٻ��� YuHaizhi 20190819
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

//        //�������ع���
//        //StartCoroutine(PreloadAssets("Battle", bundles, onLoad));
//        StartCoroutine(LoadingScene("Battle", onLoad));
//    }
//    public void LoadUnrealLevel(string name, List<Common.Hero> heroes, GameData.LevelData level, UnityAction onLoad)
//    {
//        DebugStatisticsUtil.Instance.LogBattleLoading(true);
//        this.roles.Clear();
//        ClearRoleCache();
//        List<string> bundles = new List<string>();
//        //���ݵ�ǰ��ҵ����ݣ����ɱ������Դ����
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
//        //�������ع���
//        StartCoroutine(LoadingUnrealScene(onLoad));
//    }

//    private IEnumerator LoadingScene(string scene, UnityAction onLoad)
//    {
//        // ս���ط�����һ���ر��� http://jira.ggdev.co/browse/HERO-2888
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
//        Dictionary<int, bool> heroTrans = new Dictionary<int, bool>(); //Ӣ�۱���Ԥ����
//        foreach (var hero in this.roles.ToArray())
//        {
//            var id = hero.tid;
//            GameData.RoleData role = RoleSkinSwitcher.Instance.GetCurrentSkinRole(hero); //��ӵ�����������ص�RoleData.Model�ѱ��滻ΪModelSwitch
//            if (role == null)
//                continue;

//            string modelName = role.Model;
//            if (DataManager.Instance.Models.ContainsKey(modelName))
//            {
//                modelName = DataManager.Instance.Models[modelName].Resource;//modelName���滻Ϊ����ModelSwitch��Ϣ��resource��prefab���֣��������滻���Ҳ�����Դ��Resource.Load()ʧ��
//            }

//            string aliasName = Role.IsMirac(hero) ? role.Model : "";
//            AddPreloadUnit(modelName, aliasName);

//            if (hardcodeRoleResource.ContainsKey(role.Model))//M148��C100007��C100005��C059
//            {
//                hardcorefx.AddRange(hardcodeRoleResource[role.Model]);
//            }
//            //Ӣ�۱���Ԥ����
//            if (role.RoleType == "Hero" && role.PeriodGroup != 0 && !heroTrans.ContainsKey(role.PeriodGroup))
//            {
//                heroTrans.Add(role.PeriodGroup, hero.currSkinId > 0);
//            }

//            foreach (GameData.TalentGroupData tg in DataManager.Instance.TalentGroups[id].Values)
//            {
//                foreach (GameData.TalentData talent in DataManager.Instance.Talents[tg.TalentGroupID].Values)
//                {
//                    AddPreloadFX(talent.CastingEffect); // ʩ����Ч������Role����--ֻ�ǲ����£�����Ҫ���£�RoleJoint.AddEffect--RoleController.AddEffect---GameObjectManager.Instance.CreateEffect
//                    AddPreloadFX(talent.HitEffect);     // ������Ч������Role����--ֻ�ǲ����£�����Ҫ���£�RoleJoint.AddEffect--RoleController.AddEffect---GameObjectManager.Instance.CreateEffect
//                    AddPreloadFX(talent.DirectEffect);  // ֱ����Ч��λ�ã�������Ч--��ҪEffectJoint�����ƣ���Ҫ���£�Logic.PlayEffect----new EffectJoint(GameObjectManager.Instance.CreateEffect)----Logic.Instance.Scene.AddJoint
//                    AddPreloadFX(talent.FlyResource);   // ������Ч��λ�ã�������Ч--��ҪEffectJoint�����ƣ���Ҫ���£�new EffectJoint(effect, EffectType.Cast)---this.Scene.AddJoint(actor)
//                    AddPreloadFX(talent.BounceEffect);  // ������Ч��λ�ã�������Ч--��ҪEffectJoint�����ƣ���Ҫ���£�new EffectJoint(this, EffectType.Bounce)---Logic.Instance.Scene.AddJoint(effect);

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
//        ////Ӣ�۱���Ԥ����
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

//        //�������Ԥ����(22�¹���boss) YuHaizhi 20190819
//        foreach (Common.Hero monster in roles)
//        {
//            RoleData data = DataManager.Instance.Roles[monster.tid];
//            if (data.RoleType == "Monster" && DataManager.Instance.Transforms.ContainsKey(data.PeriodGroup))
//            {
//                foreach (var transdata in DataManager.Instance.Transforms[data.PeriodGroup])
//                {
//                    ModelData mondata = DataManager.Instance.Models[transdata.Value.Model];
//                    if (!PreloadUnits.ContainsKey(mondata.Resource))
//                        AddPreloadUnit(mondata.Resource, transdata.Value.Model);//��Դ����ģ����
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

//        // Ӧ���ǻطŵ�ʱ������load�ؿ�������ս��UI����չ̫���ˣ����ڲ�������չ��
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
//        // ս���ط�����һ���ر��� http://jira.ggdev.co/browse/HERO-2888
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
//            GameData.RoleData role = RoleSkinSwitcher.Instance.GetCurrentSkinRole(hero, true); //��ӵ�����������ص�RoleData.Model�ѱ��滻ΪModelSwitch
//            if (role == null)
//                continue;

//            string modelName = role.Model;
//            if (DataManager.Instance.Models.ContainsKey(modelName))
//            {
//                modelName = DataManager.Instance.Models[modelName].Resource;//modelName���滻Ϊ����ModelSwitch��Ϣ��resource��prefab���֣��������滻���Ҳ�����Դ��Resource.Load()ʧ��
//            }

//            //string aliasName = Role.IsMirac(hero) ? role.Model : "";
//            AddPreloadUnit(modelName, modelName);

//            foreach (GameData.UnrealTalentGroupData tg in DataManager.Instance.UnrealTalentGroups[id].Values)
//            {
//                if (tg.Unlock <= 1)
//                {
//                    foreach (GameData.UnrealTalentData talent in DataManager.Instance.UnrealTalents[tg.TalentGroupID].Values)
//                    {
//                        AddPreloadFX(talent.CastingEffect); // ʩ����Ч������Role����--ֻ�ǲ����£�����Ҫ���£�RoleJoint.AddEffect--RoleController.AddEffect---GameObjectManager.Instance.CreateEffect
//                        AddPreloadFX(talent.HitEffect);     // ������Ч������Role����--ֻ�ǲ����£�����Ҫ���£�RoleJoint.AddEffect--RoleController.AddEffect---GameObjectManager.Instance.CreateEffect
//                        AddPreloadFX(talent.DirectEffect);  // ֱ����Ч��λ�ã�������Ч--��ҪEffectJoint�����ƣ���Ҫ���£�Logic.PlayEffect----new EffectJoint(GameObjectManager.Instance.CreateEffect)----Logic.Instance.Scene.AddJoint
//                        AddPreloadFX(talent.FlyResource);   // ������Ч��λ�ã�������Ч--��ҪEffectJoint�����ƣ���Ҫ���£�new EffectJoint(effect, EffectType.Cast)---this.Scene.AddJoint(actor)
//                        AddPreloadFX(talent.BounceEffect);  // ������Ч��λ�ã�������Ч--��ҪEffectJoint�����ƣ���Ҫ���£�new EffectJoint(this, EffectType.Bounce)---Logic.Instance.Scene.AddJoint(effect);

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
//    /// ս��������ԴԤ����
//    /// </summary>
//    /// <param name="path"></param>
//    /// <param name="name"></param>
//    /// <param name="count"></param>
//    void Preload(string path, string name, int count, string aliasName = "")//���Ӳ�������ظ����ص�����
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
//                // ÿ��Take---spwan���GameObject����CacheRoot�ڵ��¹���
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

//    #region ChickenDinner��Դ�������

//    /// <summary>
//    /// ս������Ӣ����Դ����
//    /// </summary>
//    /// <param name="path"></param>
//    /// <param name="name"></param>
//    /// <param name="count"></param>
//    public void PreloadRole(string path, string name, int count, string aliasName = "")//���Ӳ�������ظ����ص�����
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
//                // ÿ��Take---spwan���GameObject����CacheRoot�ڵ��¹���
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
