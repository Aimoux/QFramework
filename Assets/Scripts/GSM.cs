using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GSM : MonoBehaviour
{
    //gsm����Battle.prefab����
    //gsm �� logic �ķֹ�: logic ����ս��,������mono,Ŀ����ս���߼��볡��ԭ������??
    //ս������ԭ���Ļ�ȡ��gsm��Ԥ�����а�??
    void Awake()
    {
        // ����ս������UI
        //GameObject battleSceneUI = Instantiate(ResourcesLoader.Load("UI/prefab/BattleSceneUI")) as GameObject;
        //this.BattleUI = battleSceneUI.GetComponentInChildren<UIBattle>();
        //this.BattleUI.mainCamera = this.mainCamera;
        //battleSceneUI.transform.SetParent(this.transform, false);

        //// Ĭ�ϳ�����ͼ
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
    /// ��Ϸ��ѭ����ÿ֡����
    /// ����ʵ��ı���
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

        //// �������е�joint
        //Joints.RemoveAll(j => j.IsRunning == false);

    }


    private void FixedUpdate()
    {

        Logic.Instance.Tick(Time.fixedDeltaTime);

    }



    //��������
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
//if (!newhero) { 	// �������Ӣ�۾Ͱ���ǰ���ߣ�����Ӣ�۾Ͳ�����Ӣ�۶���
//			Logic.Instance.BattleExitProcess(Logic.Instance.BattleResult, reply.result != Server.ExitStageReply.EXIT_STAGE_RESULT.EXIT_STAGE_RESULT_KNOWN);
//		}






}
