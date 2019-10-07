//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;
//
//public class UIManager : MonoBehaviour {
//    public static UIManager _instance;
//    [Header("===UI Icons===")]
//    public BossHealthBar bossHealthBar;
//    public SimpleHealthBar playerHealthBar;
//    public ColdDownBar coolDown;
//    public GameObject victoryBar;
//    public GameObject defeatedBar;
//    public GameObject loadManager;
//    public Progress operateUI;
//    public float operateTime = 3.3f;
//    [Header("===Health Data===")]
//    public int maxPCHealth = 100;
//    public int maxBossHealth = 1000;
//    public int currentPCHealth = 100;
//    public int currentBossHealth = 1000;
//    public int bulletDam = 5;
//    public int tossDam = 10;
//    [Header("===Chat Setting===")]
//    public PopChatter playerChatter;
//    public PopChatter npcChatter;
//    public PopChatter bossChatter;
//    public PopChatter bossEndChatter;
//    public Transform chatPos;
//    public AudioClip opening;
//
//    #region ChatText
//    private string[] startHealerChatter = new string[] { "Heard any rumors?" };
//    private string[] replyHealerChatter = new string[] { "How wonderful!", "Really?", "Bull Shit!", "That's ridiculous !" };
//    private string[] startDeserterChatter = new string[] { "Heard any rumors?" };
//    private string[] replyDeserterChatter = new string[] { "Don't you want a flight ? I can help you with that !",
//        "My portal gun is ready.","Shut up !","Oh,I am so scared ! Come and get me !","This is awkward.",
//        "Hey, what's your problem ?"};
//    private string[] startHarbingerChatter = new string[] { "Heard any rumors?" };
//    private string[] replyHarbingerChatter = new string[] { "Hail Sithis!", "Alright.", "Take care." };
//    private string[] agonyChatter = new string[] { "I wonder if I could ever see you again.",
//    "This moment is 2018-08-06 09:38.","Yeah, I admit these cheap secrets are EMBARRASSING. " ,
//        "It takes great effort for me to overcome the feeling of shame.","For you, perhaps this happened so long ago. "
//    ,"For now, there are only two miracles I wish to witness.","One is the end of capitalism, " +
//        "another is the rise of AI."};
//    private string[] dancingChatter = new string[] { "Have you ever been to NeverWinter?","My name is Joy, by the way.",
//         "You remind me of someone.","Know that I have a sister, the name is light of heaven." };
//    private string[] healingChatter = new string[] { "I am new to this, but I can handle it, don't you worry." };
//    private string[] exitHealChatter = new string[] { "Thanks,I feel much better now." };
//    private string[] flyingChatter = new string[] { "Stop it, asshole!", "You will regret this, I assure you!", "What's wrong with you?" };
//    private string[] healerDefaultChatter = new string[] { "Over here, lass.", "You don't look so well." };
//    private string[] healerRandomChatter = new string[] { "Dark Souls will stretch your asshole this big!!!","Fancy a game of Gwent ? Know you" +
//        "can't refuse !","I used to be an adventure like you, then I took an arrow in the knee.","Make your survival mean something,or we are all doomed",
//    "What does a hero truly need ? It is for you to decide !","Betrayer ? Intruth, it was I who was betrayed.","The cake is a lie.",
//        "Damn these candles!","He who has a why to live can bear almost any how.","I am the bone of my sword..."
//    ,"More passionate than hope, Far deeper than despire.","Mastery of the self is mastery of the world. Loss of the self is the source of suffering.","It is said some lives are linked across time. Connected by an ancient calling that echoes through the ages. Destiny."};
//    private string[] deserterRandomChatter = new string[] { "I heard a rumor that you are an idiot! Any truth to that? " };
//    private string[] deserterDefaultChatter = new string[] { "I said leave me alone.","If it's a cure you want, go for that village girl over there!",
//         "Go play your fancy portal gun somewhere else.","Go bother someone else.","Be gone!","Believe what you want to believe."};
//    private string[] harbingerRandomChatter = new string[] { "Dear sister, I do not spread rumors, I create them.","Nothing is true, Everything is permitted.",
//    "We work in the dark to serve the light."};
//    private string[] harbingerDefaultChatter = new string[] { "My journey ends here, I hope yours won't.", "Take the cube, I have no use of it now." };
//    private string[] playerHitedSay = new string[] { "Damn you !","Go Away !","Enough !" ,"Stop it !"};
//    private string[] playerOperateSay = new string[] { "I get this.", "Well well, Look what I've found.","Open it,come on!" };
//    private string[] playerLaseredSay = new string[] { "Ouch !","Watch Out !" };
//    private string[] playerTossedSay = new string[] {"You want a piece of me ?","That hurts !","I swear I will take you down.","You will pay !" };
//    private string[] playerCaughtSay = new string[] {"Oh,no...","Let go !","Shit !","Come On...","Once again ? Really ?" };
//    private string[] playerDeathSay = new string[] {"My wounds are grave.", "Valar Morghulis !","Time to rest." };
//
//    public static readonly string[] botFallSay = new string[] { "Redirect target.", "Reload gravity adaption system.", "Reload navigation system." };
//    public static readonly string[] botHitedSay = new string[] { "Warning: Friendly fire engaged!" };
//    public static readonly string[] botFlipSay = new string[] { "Warning: Reset position failure." };
//    public static readonly string[] botDeathSay = new string[1] { "Warning: System Failure." };
//    public static readonly string[] botShootSay = new string[] { "Eliminating target.", "Target in range.", "Permission to fire.", "Engage granted." };
//    public static readonly string[] botPatrolSay = new string[] { "Area Clear.", "All done.", "Secured." };
//    public static readonly string[] botChaseSay = new string[] { "Let me guess... someone stole your sweetroll.","Disrespect the law, and you disrespect me!", "Stop right there! Prisoner!", "Wait...,I know you." };
//
//    public static readonly string[] bossAttackSay = new string[] { "DIE, MORTAL !!", "DIE, WORM !!","YOU ARE UNDONE !!","YOU DEFY ME !?" };
//    public static readonly string[] bossPowerSay = new string[] { "NO MAN CAN KILL ME !!","FEEL MY WRATH !!","YOU ARE NOT PREPARED !!" };
//    public static readonly string[] bossDeathSay = new string[] { "Zu'u unslaad ! Zu'u nis oblaan !!" };
//    public static readonly string[] bossTearSay = new string[] { "Nothing stops me !!", "Everything crumbles before me !!" };
//    public static readonly string[] bossLaseredSay = new string[] { "WHAT ?!","HOW DARE !!","AH !!","CURSE YOU !!" };
//
//    private string chatter = "";
//    #endregion
//
//    private void Awake()
//    {
//        _instance = this;
//    }
//    void Start ()
//    {
//        if (bossHealthBar == null || playerHealthBar ==null||coolDown==null||playerChatter==null
//            ||npcChatter==null ||chatPos==null||bossChatter==null||bossEndChatter==null||victoryBar==null
//            ||defeatedBar==null||opening==null||operateUI==null||loadManager==null)
//            Debug.LogError("null for UI Manager:"+this.name);
//       
//	}
//
//    public void ActivateOperateUI()
//    {
//        operateUI.gameObject.SetActive(true);
//        operateUI.GetComponent<UIFollowTarget>().target = EventManager._instance.interactOperator.
//            GetComponent<Operator>().GetChatPos();        
//        operateUI.SetMaxTime(operateTime);
//        operateUI.ResetTime();
//        Invoke("DeactivateOperateUI", operateTime);
//    }
//
//    public void DeactivateOperateUI()
//    {
//        if (operateUI.gameObject.activeSelf == true)
//        {
//            if (operateUI.timing < 0.1f)
//            {              
//                if (EventManager._instance.interactOperator == null) Debug.Log("rejoice, null operator claim operation complete!");
//                else if (EventManager._instance.interactOperator.name == "Attachable") Debug.Log("rejoice, attachable claim operation complete!");
//                else EventManager._instance.SelectInteractMotion();
//            }
//            else Debug.Log("Early disable timing is:" + operateUI.timing);
//            operateUI.gameObject.SetActive(false);
//        }
//
//    }
//
//    public void EnableVictoryUI()
//    {
//        victoryBar.SetActive(true);
//        victoryBar.GetComponent<TweenAlpha>().PlayForward();
//        victoryBar.GetComponent<TweenScale>().PlayForward();
//        Invoke("DisableVictoryUI", 8f);
//
//    }
//
//    public void DisableVictoryUI()
//    {
//        victoryBar.SetActive(false);
//    }
//
//    public void EnableDefeatedUI()
//    {
//        defeatedBar.SetActive(true);
//        defeatedBar.GetComponent<TweenAlpha>().PlayForward();
//        defeatedBar.GetComponent<TweenScale>().PlayForward();
//        Invoke("ReturnToMenuScene", 5f);
//
//    }
//
//    public void ReturnToMenuScene()
//    {
//        loadManager.GetComponent<LO_SelectStyle>().SetStyle("LoadScreen");
//        loadManager.GetComponent<LO_LoadScene>().ChangeToScene("MainMenu");
//       // SceneManager.LoadScene("MainMenu");
//    }
//
//    public void AssignNpcChatTarget(Transform targetTsfm)
//    {
//        //Debug.Log("assign chat target:" + this.name);
//        chatPos = targetTsfm;
//        npcChatter.transform.parent.gameObject.GetComponent<UIFollowTarget>().target = targetTsfm;
//        npcChatter.ResetChat();
//    }
//
//    public void PopRandomChat(string chatPos)
//    {
//        switch (chatPos)
//        {
//            case "HealerChatPos":
//                chatter = healerRandomChatter[Random.Range(0, healerRandomChatter.Length)];
//                npcChatter.PopChat(chatter);
//                break;
//            case "DeserterChatPos":
//                chatter = deserterRandomChatter[Random.Range(0, deserterRandomChatter.Length)];
//                npcChatter.PopChat(chatter);
//                break;
//            case "HarbingerChatPos":
//                chatter = harbingerRandomChatter[Random.Range(0, harbingerRandomChatter.Length)];
//                npcChatter.PopChat(chatter);
//                break;
//        }                    
//
//    }
//
//    public void PopDefault(string chatPos)
//    {
//        switch (chatPos)
//        {
//            case "HealerChatPos":
//                chatter = healerDefaultChatter[Random.Range(0, healerDefaultChatter.Length)];
//                npcChatter.PopChat(chatter);
//                //sound = healerRandomSound[Random.Range(0, healerRandomSound.Length)];
//                //AudioSource.PlayClipAtPoint(sound, this.chatPos.position);
//                break;
//            case "DeserterChatPos":
//                chatter = deserterDefaultChatter[Random.Range(0, deserterDefaultChatter.Length)];
//                npcChatter.PopChat(chatter);
//                break;
//            case "HarbingerChatPos":
//                chatter = harbingerDefaultChatter[Random.Range(0, harbingerDefaultChatter.Length)];
//                npcChatter.PopChat(chatter);
//                break;
//        }
//    }
//
//
//    public void DanceChat()
//    {
//        chatter = dancingChatter[Random.Range(0, dancingChatter.Length)];
//        npcChatter.PopChat(chatter);
//
//    }
//
//    public void BossTearSay()
//    {
//        chatter = bossTearSay[Random.Range(0, bossTearSay.Length)];
//        bossChatter.PopChat(chatter);
//
//    }
//
//    public void BossLaseredSay()
//    {
//        chatter = bossLaseredSay[Random.Range(0, bossLaseredSay.Length)];
//        bossChatter.PopChat(chatter);
//
//    }
//
//    public void BossAttackSay()
//    {
//        chatter = bossAttackSay[Random.Range(0, bossAttackSay.Length)];
//        bossChatter.gameObject.SetActive(true);
//        bossChatter.PopChat(chatter);
//
//    }
//
//    public void BossPowerSay()
//    {
//        chatter = bossPowerSay[Random.Range(0, bossPowerSay.Length)];
//        bossChatter.PopChat(chatter);
//
//    }
//
//    public void BossDeathSay()
//    {
//        chatter = bossDeathSay[Random.Range(0, bossDeathSay.Length)];
//        bossEndChatter.PopChat(chatter);
//
//    }
//
//    public void PlayerHitedSay()
//    {
//        chatter = playerHitedSay[Random.Range(0, playerHitedSay.Length)];
//        playerChatter.PopChat(chatter);
//
//    }
//
//    public void PlayerLaseredSay()
//    {
//        chatter = playerLaseredSay[Random.Range(0, playerLaseredSay.Length)];
//        playerChatter.PopChat(chatter);
//
//    }
//
//    public void PlayerOperateSay()
//    {
//        chatter = playerOperateSay[Random.Range(0, playerOperateSay.Length)];
//        playerChatter.PopChat(chatter);
//
//    }
//
//    public void PlayerCaughtSay()
//    {
//        chatter = playerCaughtSay[Random.Range(0, playerCaughtSay.Length)];
//        playerChatter.PopChat(chatter);
//
//    }
//
//    public void PlayerTossedSay()
//    {
//        chatter = playerTossedSay[Random.Range(0, playerTossedSay.Length)];
//        playerChatter.PopChat(chatter);
//
//    }
//
//    public void PlayerDeathSay()
//    {
//        chatter = playerDeathSay[Random.Range(0, playerDeathSay.Length)];
//        playerChatter.PopChat(chatter);
//
//    }
//
//    public void FlyChatter()
//    {
//        chatter = flyingChatter[Random.Range(0, flyingChatter.Length)];
//        npcChatter.PopChat(chatter);
//
//    }
//
//    public void HealingChat()
//    {
//        chatter = healingChatter[Random.Range(0,healingChatter.Length)];
//        npcChatter.PopChat(chatter);
//
//    }
//
//    public void ExitHealedChat()
//    {
//        chatter = exitHealChatter[Random.Range(0, exitHealChatter.Length)];
//        playerChatter.PopChat(chatter);
//
//    }
//
//    public void HealerAgonyChat()
//    {
//        chatter = agonyChatter[Random.Range(0, agonyChatter.Length)];
//        npcChatter.PopChat(chatter);
//
//    }
//
//    public void StartPlayerChat(string chatPos)
//    {
//        switch (chatPos)
//        {
//            case "HealerChatPos":
//                chatter = startHealerChatter[Random.Range(0, startHealerChatter.Length)];
//                playerChatter.PopChat(chatter);
//                break;
//            case "DeserterChatPos":
//                chatter = startDeserterChatter[Random.Range(0, startDeserterChatter.Length)];
//                playerChatter.PopChat(chatter);
//                break;
//            case "HarbingerChatPos":
//                chatter = startHarbingerChatter[Random.Range(0, startHarbingerChatter.Length)];
//                playerChatter.PopChat(chatter);
//                break;
//        }
//    }
//
//    public void ReplyPlayerChat(string chatPos)
//    {
//        switch (chatPos)
//        {
//            case "HealerChatPos":
//                chatter = replyHealerChatter[Random.Range(0, replyHealerChatter.Length)];
//                playerChatter.PopChat(chatter);
//                break;
//            case "DeserterChatPos":
//                chatter = replyDeserterChatter[Random.Range(0, replyDeserterChatter.Length)];
//                playerChatter.PopChat(chatter);
//                break;
//            case "HarbingerChatPos":
//                chatter = replyHarbingerChatter[Random.Range(0, replyHarbingerChatter.Length)];
//                playerChatter.PopChat(chatter);
//                break;
//        }
//
//    }
//
//    public void UpdateBossHealth()
//    {
//        //  Debug.Log("Update Boss Heath Bar");    
//        bossHealthBar.UpdateHealth(currentBossHealth);
//    }
//
//    public void UpdatePlayerHealth()
//    {
//        //  Debug.Log("update player heath bar");
//        playerHealthBar.UpdateBar(currentPCHealth, maxPCHealth);
//        if (currentPCHealth < 1) EventManager._instance.curtPlayer.
//                     GetComponent<GrilGunController>().ValarMorghulis();
//             
//    }
//    public void BossLaseredHit()
//    {
//        currentBossHealth -= 400;
//        UpdateBossHealth();
//
//    }
//
//    public void PlayerTossedHit()//��CatchAndTossͬ����Tossed State
//    {
//        currentPCHealth -= tossDam;
//        UpdatePlayerHealth();
//    }
//
//    public void PlayerMeleeHit()
//    {
//        currentPCHealth -= bulletDam;
//        UpdatePlayerHealth();
//    }
//
//    public void PlayerLaserHit()
//    {
//        currentPCHealth -= bulletDam;
//        UpdatePlayerHealth();
//    }
//
//    public void HealPlayerOnePoint()
//    {
//        currentPCHealth += 1;
//        UpdatePlayerHealth();
//    }
//
//    public void HealPlayerFully()
//    {
//        currentPCHealth = maxPCHealth;
//        UpdatePlayerHealth();
//
//    }
//
//    public void EnableCoolDown()
//    {
//       // Debug.Log("enable cool down");
//        coolDown.transform.parent.gameObject.SetActive(true);
//        coolDown.ResetTime();
//        float maxCoolDown = coolDown.maxTime;
//        Invoke("DisableCoolDown", maxCoolDown);
//        
//
//    }
//
//    public void DisableCoolDown()
//    {
//      //  Debug.Log("Disable cool down");
//        coolDown.transform.parent.gameObject.SetActive(false);
//    }
//
//   
//
//
//
//
//}
