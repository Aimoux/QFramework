//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityStandardAssets.Characters.ThirdPerson;
//using MorePPEffects;
//using UnityEngine.Playables;
//public class EventManager : MonoBehaviour {
//
//	#region Parameter
//    [Header("===Event Parameter===")]
//	public static EventManager _instance;
//    public PortalGun ptlGun;
//    public PlayableDirector director;
//    [Header ("===DeviceName===")]
//	public GameObject[] playerGo;
//	public GameObject curtPlayer;
//    public GameObject boss;
//    public GameObject firstPortal;
//    public GameObject secondPortal;
//    [Header("===Doors===")]
//    public GameObject DoorToSection2;
//    public GameObject DoorToLaserCube;
//    [Header ("===Operators===")]
//    public GameObject interactOperator;
//    public GameObject doorOperator;
//    public GameObject bracketOperator;
//    public GameObject laserOperator;
//    public GameObject UFO;
//    public GameObject Pod;
//    public GameObject riseFallBracket;
//    public GameObject healer;
//    public GameObject deserter;
//    public GameObject harbinger;
//    [Header("===SectionDevices")]
//    public GameObject Section1;
//    public GameObject Section2;
//    public GameObject Section3;
//    public GameObject Section4;
//    public GameObject SectionBoss;
//    public GameObject SectionHalf;
//    public GameObject Exterior;
//    public GameObject bots;
//    public GameObject[] bossWallCage;
//    public GameObject upLight;
//    public GameObject leftLight;
//    public GameObject rightLight;
//    public int curSection;
//    [Header("===Bool Switch===")]
//    public bool rise = false;
//	public bool isOpenGate = false;
//    public float riseSpeed = 10f;
//	public float timeReset=2;
//	public float timeStay=1;
//	public  ThirdPersonUserControl moveScrpt ;
//	public int playerIndex=0;
//    public bool ghostMode = false;
//    public bool victory = false;
//    public bool mapShown = true;
//    public float ghostTime = 30f;
//    [Header("===UI Settings===")]
//    public GameObject bossUI;
//    public GameObject girlUI;
//    public GameObject menuUI;
//    public GameObject mapUI;
//    public GameObject hintUI;
//    public RectTransform progressUI;
//    public TweenPosition tweenMap;
//    public string keyMenu = "escape";
//    public string keyMap = "m";
//
//   
//    #endregion
//    void Awake() //在awake中声明单例，确保即使本脚本所附的GameObject不是isActive状态，也能生成单例？
//	{
//		_instance = this;
//        ptlGun = GetComponent<PortalGun>();
//    }
//		// Use this for initialization
//	void Start ()
//    {
//       
//        if (DoorToSection2 == null || DoorToLaserCube == null ||ptlGun==null || bossWallCage==null
//            ||firstPortal==null ||secondPortal ==null ||bossUI==null||menuUI==null ||Exterior==null
//            ||girlUI==null||hintUI==null ||tweenMap ==null||laserOperator==null||progressUI==null
//            ||deserter==null||healer==null||harbinger==null||upLight==null||leftLight==null||rightLight==null
//            ||bots==null||director==null)
//            Debug.LogError("null component:"+this.name);
//        
//	}
//	
//	// Update is called once per frame
//	void Update () 
//	{
//		if(Input.GetKeyDown (keyMenu )&& menuUI.activeSelf==false)
//        {
//            EnableMenuUI();
//        }
//        if (Input.GetKeyDown(keyMap)) ToggleMap();
//        if (Input.GetKeyDown(KeyCode.P)) Time.timeScale = 0;
//        if (Input.GetKeyDown(KeyCode.R)) Time.timeScale = 1;
//
//    }
//
//    private void FixedUpdate()
//    {
//        if (Input.GetKey(KeyCode.U)) Debug.Log("U pressed in fixedtime.");
//    }
//
//    #region UI Management
//
//    public void EnableMenuUI()
//    {
//        Cursor.visible = false;
//        menuUI.SetActive(true);
//        menuUI.GetComponentInChildren<MenuManager>().EnableMenuCursor();
//        CursorManager._instance.ToggleMenuOn();
//        ptlGun.SetOverUITrue();
//        curtPlayer.GetComponent<GrilGunController>().canFire = false;
//        curtPlayer.GetComponent<GrilGunController>().DisableCombatSensor();
//        DisableCombatUI();
//
//
//    }
//    public void DisableMenuUI()
//    {
//        menuUI.SetActive(false);
//        menuUI.GetComponentInChildren<MenuManager>().DisableMenuCursor();
//        CursorManager._instance.ToggleMenuOn();
//        ptlGun.SetOverUIFalse();
//        if(ghostMode==false)
//        {
//            curtPlayer.GetComponent<GrilGunController>().canFire = true;
//            curtPlayer.GetComponent<GrilGunController>().EnableCombatSensor();
//        }       
//        EnableCombatUI();
//       
//    }
//
//    public void EnableCombatUI()
//    {        
//        girlUI.SetActive(true);
//        if (curSection == 5 && victory == false)  bossUI.SetActive(true);
//    }
//
//    public void DisableCombatUI()
//    {
//        bossUI.SetActive(false);
//        girlUI.SetActive(false);
//    }
//
//    public void PopHint()
//    {
//        hintUI.SetActive(true);
//
//    }
//
//    public void HideHint()
//    {
//        hintUI.SetActive(false);
//    }
//
//    public void ToggleMap()
//    {
//        if(mapShown )
//        {
//            tweenMap.PlayReverse();
//        }
//        else
//        {
//            tweenMap.PlayForward();
//        }
//        mapShown = !mapShown;
//
//    }
//
//    #endregion
//
//    #region GhostMode
//
//    public void SetGhostTrue()
//    {
//        ghostMode = true;
//    }
//
//    public void SetGhostFalse()
//    {
//
//        ghostMode = false;
//    }
//
//    public void EnterGhostMode()
//    {
//        SetGhostTrue();
//        curtPlayer.GetComponent<GrilGunController>().StartGhostMode();
//        UIManager._instance.EnableCoolDown();
//        Invoke("ExitGhostMode", ghostTime);
//        //firstPortal.GetComponent<Portal>().DisableCollider();
//        //secondPortal.GetComponent<Portal>().DisableCollider();
//        Camera.main.GetComponent<Dreamy>().enabled = true;
//        Camera.main.GetComponent<CircularBlur>().enabled = true;
//    }
//
//    public void ExitGhostMode()
//    {
//        SetGhostFalse();
//        curtPlayer.GetComponent<GrilGunController>().EndGhostMode();
//        UIManager._instance.DisableCoolDown();
//        //firstPortal.GetComponent<Portal>().EnableCollider();
//        //secondPortal.GetComponent<Portal>().EnableCollider();
//        Camera.main.GetComponent<Dreamy>().enabled = false;
//        Camera.main.GetComponent<CircularBlur>().enabled = false;
//    }
//    #endregion
//
//    #region ApplySection
//    public void ApplySection(string section)
//    {
//        switch (section)
//        {
//            case "One":
//                ApplySectionOne();
//                break;
//            case "Two":
//                ApplySectionTwo();
//                break;
//            case "TwoHalf":
//                ApplySectionTwoHalf();
//                break;
//            case "Roof":
//                ApplySectionRoof();
//                break;
//            case "Three":
//                ApplySectionThree();
//                break;
//            case "Four":
//                ApplySectionFour();
//                break;
//            case "Epilogue":
//                ApplySectionBoss();
//                break;
//        }        
//    }
//
//    public void ApplySectionOne()
//    {
//        Debug.Log("enter section1");
//        curSection = 1;
//        director.Stop();
//       // Section3.transform.parent.Find("DirLight").gameObject.SetActive(false);
//
//    }
//
//    public void ApplySectionTwo()
//    {
//        Debug.Log("enter section2");
//        upLight.SetActive(true);
//        leftLight.SetActive(true);
//        rightLight.SetActive(true);
//        curSection = 2;
//    }
//    public void ApplySectionTwoHalf()
//    {
//        Debug.Log("enter section2.5");
//        curSection = 25;
//        upLight.SetActive(false);
//        leftLight.SetActive(false);
//        rightLight.SetActive(false);
//
//    }
//
//    public void ApplySectionRoof()
//    {
//        Debug.Log("enter roof");
//        upLight.SetActive(false);
//        leftLight.SetActive(false);
//        rightLight.SetActive(true);
//        bots.transform.Find("Section3").gameObject.SetActive(true);
//
//    }
//
//    public void ApplySectionThree()
//    {
//        Debug.Log("enter section3");
//        upLight.SetActive(false);
//        leftLight.SetActive(true);
//        rightLight.SetActive(true);
//        curSection = 3;
//        bots.transform.Find("Section3").gameObject.SetActive(true);
//    }
//
//    public void ApplySectionFour()
//    {
//        Debug.Log("enter section4");
//        curSection = 4;
//        ResetPodAndUFO();
//        Section3.transform.Find("Device/LaserSource").gameObject.SetActive(false);
//        curtPlayer.GetComponentInChildren<AudioSource>().PlayOneShot(UIManager._instance.opening);
//        healer.SetActive(true);
//        deserter.SetActive(true);
//        harbinger.SetActive(true);
//        upLight.SetActive(false);
//        leftLight.SetActive(true);
//        rightLight.SetActive(true);
//        
//    }
//
//    public void ApplySectionBoss()
//    {
//        Debug.Log("enter sectionboss");
//        curSection = 5;
//        if(GameObject.Find("Bots")) GameObject.Find("Bots").SetActive(false);
//        SectionBoss.SetActive(true);
//        Section1.SetActive(false);
//        Section2.SetActive(false);
//        Section3.SetActive(false);
//        Section4.SetActive(false);
//        SectionHalf.SetActive(false);
//        Exterior.SetActive(false);
//        Section1.transform.parent.transform.Find("Epilogue").GetComponent<Collider>().enabled = false;
//
//        if (victory == false)
//        {
//            curtPlayer.GetComponent<GrilGunController>().soundController.PlaySound("Audio/Boss/TwoStep");
//            boss.SetActive(true);
//            foreach (var wall in bossWallCage)
//            {
//                wall.GetComponent<BossWallCage>().ToggleRise();
//            }
//            Invoke("EnableCombatUI", 1f);
//        }
//    }
//
//    public void BossDefeated()
//    {
//        victory = true;
//        Invoke("ResetBossSection", 3f);
//        UIManager._instance.EnableVictoryUI();
//    }
//
//    public void ResetBossSection()
//    {
//        foreach (var wall in bossWallCage)
//        {
//            wall.GetComponent<BossWallCage>().ToggleRise();
//        }
//        curtPlayer.GetComponent<GrilGunController>().soundController.PlaySound("Audio/Boss/TwoStep");
//        SectionBoss.transform.Find("Exit").gameObject.SetActive(true);
//        SectionBoss.transform.Find("Colliders").gameObject.SetActive(false);
//        DisableCombatUI();
//    }
//
//    #endregion
//
//
//    #region Apply Device
//
//
//  
//
//    public void SelectInteractMotion()
//    {
//    ///    HideHint();
//        if (interactOperator == null) Debug.Log("null interactorprtot");
//        else if (interactOperator == doorOperator)
//        {
//            OpenDoorToSection2();
//           // Invoke("OpenDoorToSection2", 2f);
//        }
//        else if (interactOperator == bracketOperator)
//        {
//            ToggleCarrier();
//           // Invoke("ToggleCarrier", 2f);
//
//        }
//        else if (interactOperator == laserOperator)
//        {
//            ApplyLaserOperator();
//        }      
//        else
//            //并不尽然，player被击中，调用DisabeProgress，从而调用此函数。然而progressBar未激活状态下
//            //也会调用至此，若此时主角恰好接触operator而无操作，触发此log error。
//            Debug.LogWarning("Warning: No null delayed operator without operation was called. ");
//
//    }
//
//    public void SelectInteractInstance()
//    {
//        HideHint();
//      //  if (interactOperator == null) Debug.Log("null interactoprator");
//        if (interactOperator == healer)
//        {
//            ChatWithHealer();
//        }
//        else if (interactOperator == deserter)
//        {
//            ChatWithDeserter();
//        }
//        else if(interactOperator==harbinger)
//        {
//            ChatWithHarbinger();
//        }
//        else if(interactOperator.name=="Attachable")
//        {
//            ToggleAttachable();
//        }
//        //no null operator can actually be delayed operators.
//        else Debug.LogWarning("Warning: No null instant operator without application was called:"
//            +interactOperator.name );
//    }
//
//    public void HealPlayer()
//    {
//        curtPlayer.GetComponent<GrilGunController>().SetHealedTrue();
// 
//    }
//
//    public void HealComplete()
//    {
//        curtPlayer.GetComponent<GrilGunController>().SetHealedFalse();
//        UIManager._instance.HealPlayerFully();
//    }
// 
//
//    public void ChatWithHealer()//secret talking
//    {
//        Debug.Log("Chat with healer."+this.name);
//        healer.GetComponent<NpcChatterController>().SetSecretTrue();
//     //   UIManager._instance.HealerRandomChat();//由动画event决定pop时刻效果更好
//        curtPlayer.GetComponent<GrilGunController>().SetChatTrue();
//     //   UIManager._instance.PlayerChat();
//
//    }
//
//    public void ChatWithDeserter()
//    {
//        Debug.Log("Chat with deserter." + this.name);
//        deserter.GetComponent<NpcChatterController>().SetSecretTrue();
//        curtPlayer.GetComponent<GrilGunController>().SetChatTrue();
//
//    }
//    public void ChatWithHarbinger()
//    {
//        Debug.Log("Chat with deserter." + this.name);
//        harbinger.GetComponent<NpcChatterController>().SetSecretTrue();
//        curtPlayer.GetComponent<GrilGunController>().SetChatTrue();
//
//    }
//
//    public void ToggleCarrier()
//    {
//        riseFallBracket.GetComponent<AnimMotion>().ToggleRise();
//        riseFallBracket.GetComponent<AudioSource>().Play();
//    }
//
//    public void ToggleAttachable()
//    {
//       
//     //   if (interactOperator==null||interactOperator.name != "Attachable") Debug.LogError("null attachable called");
//        interactOperator.GetComponent<Attachable>().TogggleAttach();
//
//    }
//
//    public void OpenDoorToSection2()
//    {
//      //  Debug.Log("time to open door to section2");
//        Animator anim = DoorToSection2.GetComponent<Animator>();
//        if (anim.GetBool("Open") == false) DoorToSection2.GetComponent<AudioSource>().Play();
//        anim.SetBool("Open", true);
//        DoorToSection2.GetComponent<Collider>().enabled = false;
//        
//
//    }
//
//    //which door to open? same operate state
//    public void OpenDoorToLaserCube()
//    {
//      //  Debug.Log("time to unlock laserCube");
//        Animator anim = DoorToLaserCube.GetComponent<Animator>();
//        anim.SetBool("Open", true);
//        DoorToLaserCube.GetComponent<Collider>().enabled = false;
//        DoorToLaserCube.GetComponent<AudioSource>().Play();
//
//    }
//
//    public void LaunchPodAndUFO()
//    {
//        Pod.GetComponent<AnimMotion>().SetOrginHeight(11.3f);
//        Pod.GetComponent<AnimMotion>().SetRiseFalse();
//        UFO.GetComponent<AnimMotion>().SetRiseTrue();
//        UFO.GetComponent<AudioSource>().enabled = true;
//    }
//
//    public void ResetPodAndUFO()
//    {
//        Pod.GetComponent<AnimMotion>().SetRiseTrue();
//        UFO.GetComponent<AnimMotion>().SetRiseFalse();
//        UFO.GetComponent<AudioSource>().enabled = false;
//
//    }
//
//    public void ApplyLaserOperator()
//    {
//        Section3.transform.Find("Device/LaserSource").gameObject.SetActive(true);
//        AnimMotion risefall = Section3.transform.Find("Device/Sanctuary").gameObject.GetComponent<AnimMotion>();
//        if (risefall == null) Debug.LogError("find no sanctuary");
//        risefall.SetRiseFalse();
//        risefall.SetOrginHeight(-2.5f);
//        risefall = Section3.transform.Find("Device/Platform").gameObject.GetComponent<AnimMotion>();
//        if (risefall == null) Debug.LogError("find no sanctuary");
//        risefall.SetRiseTrue();
//
//    }
//
//
//    #endregion
//    //	public void SwitchPlayer()
//    //	{
//    //		foreach (GameObject go in playerGo )
//    //		{
//    //	//	 moveScrpt = go.GetComponent <PlayerMove> ();
//    //			moveScrpt = go.GetComponent <ThirdPersonUserControl> (); //更改命名空间，利用using？如何查看脚本所属的命名空间？
//    //		 moveScrpt.enabled = false;
//    //		}
//    //
//    //		if (playerIndex ==playerGo .Length -1)
//    //			playerIndex =0;
//    //		else 
//    //			playerIndex ++;
//    //
//    //		curtPlayer =playerGo [playerIndex];
//    //	//	curtPlayer.GetComponent <PlayerMove >().enabled=true ;
//    //	curtPlayer.GetComponent <ThirdPersonUserControl >().enabled=true ;
//    //
//    //
//    //	}
//
//
//
//
//
//}
