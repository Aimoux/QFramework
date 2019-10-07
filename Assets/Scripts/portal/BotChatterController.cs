//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class BotChatterController : MonoBehaviour {
//
//    public PopChatter pop;
//    public string saying;
//    private UIFollowTarget follow;
//    private Transform chatPos;
//
//
//    // Use this for initialization
//    void Start () {
//    
//        follow = pop.transform.parent.GetComponent<UIFollowTarget>();
//        if ( pop==null||follow==null)
//            Debug.LogError("null for bot chatter:" + this.name);
//        chatPos = GetComponent<InfantryController>().childModel.GetComponent 
//            <InfantryBattleController >().chatPos ;
//        follow.target = chatPos;
//
//    }
//  
//
//    public void BotPatrolSay()
//    {
//        follow.target = chatPos;
//        saying = UIManager.botPatrolSay[Random.Range(0, UIManager.botPatrolSay.Length)];
//        if(pop.gameObject.activeSelf) pop.PopChat(saying);
//
//    }
//
//    public void BotChaseSay()
//    {
//        follow.target = chatPos;
//        saying = UIManager.botChaseSay[Random.Range(0, UIManager.botChaseSay.Length)];
//        if (pop.gameObject.activeSelf) pop.PopChat(saying);
//
//    }
//
//    public void EnterPortalSay()
//    {
//        follow.target = chatPos;
//        saying = UIManager.botFallSay[Random.Range(0, UIManager.botFallSay.Length)];
//        if (pop.gameObject.activeSelf) pop.PopChat(saying);
//
//    }
//
//    public void OnFlipEnterSay()
//    {
//        follow.target = chatPos;
//        saying = UIManager.botFlipSay[Random.Range(0, UIManager.botFlipSay.Length)];
//        if (pop.gameObject.activeSelf) pop.PopChat(saying);
//
//    }
//
//    public void OnHitedEnterSay()
//    {
//        follow.target = chatPos;
//        saying = UIManager.botHitedSay[Random.Range(0, UIManager.botHitedSay.Length)];
//        if (pop.gameObject.activeSelf) pop.PopChat(saying);
//
//    }
//
//    public void OnShootEnterSay()
//    {
//        follow.target = chatPos;
//        saying = UIManager.botShootSay[Random.Range(0, UIManager.botShootSay.Length)];
//        if (pop.gameObject.activeSelf) pop.PopChat(saying);
//    }
//
//    public void OnDeathEnterSay()
//    {
//        follow.target = chatPos;
//        saying = UIManager.botDeathSay[Random.Range(0, UIManager.botDeathSay.Length)];
//        if (pop.gameObject.activeSelf) pop.PopChat(saying);
//    }
//}
