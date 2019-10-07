//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class NpcChatterController : MonoBehaviour {
//
//    private GameObject childModel;
//    private Transform chatPos;
//    private GameObject orginTarget;
//    private Animator anim;
//    private AudioSource voice;
//    private InfantryController infantry;
//    private AudioClip sound;
//    public AudioClip[] secretChat;
//
//
//    // Use this for initialization
//    void Start() {
//
//        infantry = GetComponent<InfantryController>();
//        childModel = infantry.childModel;
//        voice = GetComponent<AudioSource>();
//        anim = childModel.GetComponent<Animator>();
//        chatPos=childModel.GetComponent<InfantryBattleController>().chatPos;
//        if (anim == null||childModel==null||chatPos==null||voice==null||secretChat==null)
//            Debug.LogError("null for npc chater:"+this.name);
//
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}
//
//    public void OnWaveExit()
//    {
//        GetComponent<CapsuleCollider>().enabled = true;
//        GetComponent<Rigidbody>().useGravity = true;
//
//    }
//
//
//    public void OnHealEnter()
//    {
//        Debug.Log("enter heal:"+this.name);
//        anim.SetInteger("Falls", 2);
//        EventManager._instance.HealPlayer();
//        UIManager._instance.HealingChat();
//        
//
//    }
//
//    public void OnHealUpdate()
//    {
//        UIManager._instance.HealPlayerOnePoint();
//        EventManager._instance.HideHint();
//    }
//
//    public void OnHealExit()
//    {
//        Debug.Log("finish heal:"+this.name);
//        EventManager._instance.HealComplete();
//        SetSecretFalse();
//        EventManager._instance.curtPlayer.GetComponent<GrilGunController>().SetChatFalse();
//        infantry.ReassignTarget(EventManager._instance.deserter.GetComponent<InfantryController>().target);
//    }
//
//    public void OnDanceEnter()
//    {
//        if(UIManager._instance.chatPos==chatPos )
//        UIManager._instance.DanceChat();
//        
//
//    }
//
//    public void EnterPortalSay()
//    {
//        /// if (UIManager._instance.chatPos == chatPos)
//        UIManager._instance.AssignNpcChatTarget(chatPos);
//        UIManager._instance.FlyChatter();
//        anim.SetInteger("Falls", anim.GetInteger("Falls") + 1);//animator 不存在falls时，也不会报错
//
//    }
//
//    public void OnAgonyEnter()
//    {
//        UIManager._instance.AssignNpcChatTarget(chatPos);
//        UIManager._instance.HealerAgonyChat();
//        EventManager._instance.curtPlayer.GetComponent<GrilGunController>().SetChatFalse();
//
//    }
//
//    //进入谈话，更改target，使NPC朝向Player
//    public void OnSecretEnter()
//    {
//        //infantry target是攻击目标，可用来确定NPC朝向。
//        orginTarget = infantry.target;
//        infantry.ReassignTarget(EventManager._instance.curtPlayer);
//        sound = secretChat[Random.Range(0, secretChat.Length)];
//        voice.PlayOneShot(sound);
//
//    }
//
//    //退出谈话，还原NPC的target
//    public void OnSecretExit()
//    {
//        SetSecretFalse();
//        EventManager._instance.curtPlayer.GetComponent<GrilGunController>().SetChatFalse();
//        infantry.ReassignTarget(orginTarget);
//        if(EventManager._instance.interactOperator==this.gameObject ) EventManager._instance.PopHint();
//    }
//
//
//    //harbinger仰卧在地，不宜令其谈话时朝向Player，harbinger保持原朝向为宜。
//    public void OnLaySecretEnter()
//    {
//        sound = secretChat[Random.Range(0, secretChat.Length)];
//        voice.PlayOneShot(sound);
//
//    }
//
//
//    public void OnLaySecretExit()// 
//    {
//        SetSecretFalse();
//        EventManager._instance.curtPlayer.GetComponent<GrilGunController>().SetChatFalse();
//        if (EventManager._instance.interactOperator == this.gameObject) EventManager._instance.PopHint();
//
//    }
//
//    
//    public void SetSecretTrue()
//    {
//        anim.SetBool("Secret", true);
//        Debug.Log("Set secret true:" + this.name);
//
//    }
//
//    public void SetSecretFalse()
//    {
//        anim.SetBool("Secret", false);
//        Debug.Log("Set secret false");
//    }
//
//    private void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject == EventManager._instance.curtPlayer)
//        {
//            //collision.gameObject.SendMessage("SetOperateTrue");
//            if (EventManager._instance.interactOperator == null)
//            {
//                // if(anim.GetInteger("HitPoint")!=1) 
//                EventManager._instance.PopHint();
//                EventManager._instance.interactOperator = this.gameObject;
//                UIManager._instance.AssignNpcChatTarget(chatPos);
//                Debug.Log("Player holding nothing approaches me: " + this.name);
//            }
//            else if (EventManager._instance.interactOperator == this.gameObject)
//                Debug.Log("Player holding me and apporach me????:" + this.name);
//            else Debug.Log("Player holding another operator approaches me" + this.name);
//
//        }
//
//
//    }
//
//    private void OnCollisionExit(Collision collision)
//    {
//        if (collision.gameObject == EventManager._instance.curtPlayer)
//        {
//
//            EventManager._instance.HideHint();
//            UIManager._instance.AssignNpcChatTarget(EventManager._instance.healer.GetComponent<NpcChatterController>()
//                .chatPos.transform);
//            if (EventManager._instance.interactOperator == this.gameObject)
//            {               
//                EventManager._instance.interactOperator = null;
//                Debug.Log(" Player enter me then left:" + this.name);
//            }
//            //else if (EventManager._instance.interactOperator != null)
//            //    Debug.Log("Player held another operator then left me:" + this.name);
//            //else Debug.Log("Play held nothing then left me:" + this.name);
//
//
//        }
//    }
//
//}
