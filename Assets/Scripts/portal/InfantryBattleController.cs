//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class InfantryBattleController : MonoBehaviour {
//
//    public Transform chatPos;
//    public Vector3 teleportPos = new Vector3(2f, 0f, 0f);
//    [SerializeField] private CapsuleCollider caps;
//    [SerializeField] private InfantryController infantry;
//    [SerializeField] private BotChatterController botChat;
//    [SerializeField] private Transform pyrTsfm;
//    [SerializeField] private AudioSource voice;
//    
//
//	void Start () {
//
//        caps = transform.parent.gameObject.GetComponent<CapsuleCollider>();
//        infantry = transform.parent.gameObject.GetComponent<InfantryController>();
//        botChat = transform.parent.gameObject.GetComponent<BotChatterController>();
//        pyrTsfm = EventManager._instance.curtPlayer.transform;
//        voice = infantry.GetComponent<AudioSource>();
//        if (caps == null || infantry == null||chatPos==null||pyrTsfm==null||voice==null)
//            Debug.Log("null for infantry battle");
//	}
//	
//	// Update is called once per frame
//	
//    public void FireBullet()
//    {
//        infantry.FireBullet();
//
//    }
//
//    public void SetFlipDeathTrue()
//    {
//        infantry.SetFlipDeathTrue();
//
//    }
//
//    public void DisableSensor()
//    {
//        CapsuleCollider sensor = GetComponent<CapsuleCollider>();
//        if (sensor != null)
//            sensor.enabled = false;
//
//    }
//
//    public void PatrolSay()
//    {
//        botChat.BotPatrolSay();
//
//    }
//
//    public void ChaseSay()
//    {
//        botChat.BotChaseSay();
//    }
//
//    public void PopRandomChat()
//    {
//        UIManager._instance.PopRandomChat(chatPos.name);
//    }
//
//    public void PopDefault()
//    {
//        if (UIManager._instance.chatPos == chatPos)
//            UIManager._instance.PopDefault(chatPos.name);
//       // else Debug.Log("Ops, I am not the current poper:" + this.name);
//
//    }
//
//    public void PopDancing()
//    {
//        if (UIManager._instance.chatPos == chatPos)
//            UIManager._instance.DanceChat();
//
//    }
//
//    public void Teleport()
//    {
//        float orginH = infantry.transform.position.y;
//        Vector3 targetPos = pyrTsfm.TransformPoint(teleportPos);
//        infantry.transform.position = new Vector3(targetPos.x,orginH,targetPos.z);
//        ActivateTeleportEffect();
//
//    }
//
//    public void ActivateTeleportEffect()
//    {
//        transform.Find("Teleport").gameObject.SetActive(true);
//        Invoke("DeactivateTeleportEffect", 1f);
//        
//    }
//
//    public void DeactivateTeleportEffect()
//    {
//        transform.Find("Teleport").gameObject.SetActive(false);
//    }
//
//
//    public void Explode()
//    {
//        transform.parent.Find("Explode").gameObject.SetActive(true);
//        this.gameObject.SetActive(false);
//    }
//
//
//}
