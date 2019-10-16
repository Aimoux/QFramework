using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GSM : MonoBehaviour
{
    //gsm绑定于Battle.prefab上面
    //gsm 与 logic 的分工: logic 管理战斗,但并非mono,目的是战斗逻辑与场景原件解耦??
    //战斗场景原件的获取由gsm在预制体中绑定??
    void Awake()
    {
        // 加载战斗场景UI
        //GameObject battleSceneUI = Instantiate(ResourcesLoader.Load("UI/prefab/BattleSceneUI")) as GameObject;
        //this.BattleUI = battleSceneUI.GetComponentInChildren<UIBattle>();
        //this.BattleUI.mainCamera = this.mainCamera;
        //battleSceneUI.transform.SetParent(this.transform, false);

        //// 默认场景地图
        //if (!Tutorial.Instance.IsFinish(Tutorial.Tuto_5V5))
        //{
        //    this.Background.sprite = ResourcesLoader.Load<Sprite>("StageBg/level_46");
        //}
        //else if (this.Background != null)
        //{
        //    DataNewManager.Instance.CombatConfigs.SafeUse((value) => {
        //        this.Background.sprite = ResourcesLoader.Load<Sprite>(value[1][1].BackgroundPic);
        //    });
        //    // this.Background.transform.localScale = new Vector3(0.01f, 0.01f, 1f);
        //}

        //// For IPhoneX
        //if ((float)Display.main.systemWidth / Display.main.systemHeight > 2f)
        //{
        //    mainCamera.GetComponent<FixedWidthCamera>().width = 11f;
        //    Background.transform.localScale = Vector3.one * 1.1f;
        //}
    }

    void Start()
    {
        //Logic.Instance.Scene = this;
        //this.PrepareData();
        //PauseType = 0;
        //StartCoroutine(DelayPlayMusic());
    }

    void OnDestroy()
    {
        //MessagePublisher.Instance.Unsubscribe<Server.TempleReply>(this.OnExitTempleReply);

    }

    /// <summary>
    /// 游戏主循环：每帧更新
    /// 更新实体的表现
    /// </summary>
    void Update()
    {
        //if (PauseType != 0)
        //{
        //    return;
        //}

        //float passTime = Time.deltaTime;
        //foreach (IJoint joint in Joints.ToArray())
        //{
        //    if (joint.Element != null && joint.Element.IsJointPause)
        //        continue;
        //    if (joint.IsRunning)
        //    {
        //        joint.OnEnterFrame(passTime);
        //    }
        //}

        //// 清理不运行的joint
        //Joints.RemoveAll(j => j.IsRunning == false);

    }


    private void FixedUpdate()
    {

        Logic.Instance.Tick(Time.fixedDeltaTime);

    }



    //核心流程
//    Logic.Instance.Scene = this;
//  public void PauseBattle(PauseType type)
//    {
//        this.PauseType |= (int)type;
//        Logic.Instance.SuspendAllAnimation(false);
//    }

//    public void ResumeBattle(PauseType type)
//    {
//        this.PauseType ^= (int)type;
//        Logic.Instance.ResumeAllAnimation();
//    }
//  this.BattleUI.InitBattleUI(Logic.Instance.BattleMode, battle.RoundID);
//   bool battlemode = this.AutoBattleMode;
//    CombatConfigData combatcfg = DataNewManager.Instance.CombatConfigs.Value[Logic.Instance.LevelData.LevelID][Logic.Instance.Round + 1];
//    Logic.Instance.InitBattle(combatcfg);
//if (!newhero) { 	// 不获得新英雄就按以前的走，有新英雄就播放新英雄动画
//			Logic.Instance.BattleExitProcess(Logic.Instance.BattleResult, reply.result != Server.ExitStageReply.EXIT_STAGE_RESULT.EXIT_STAGE_RESULT_KNOWN);
//		}






}
