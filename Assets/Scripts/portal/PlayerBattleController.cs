//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Playables;
//
//public class PlayerBattleController : MonoBehaviour {
//
//    private CapsuleCollider combatSensor;
//    public GrilGunController girlController;
//    public BossController  bossController;
//	// Use this for initialization
//	void Start () {
//        combatSensor = GetComponent<CapsuleCollider>();
//        girlController = EventManager._instance.curtPlayer.GetComponent<GrilGunController>();
//        bossController = EventManager._instance.boss.GetComponent<BossController>();
//        if (combatSensor == null ||girlController ==null || bossController ==null)
//            Debug.LogError("null component: "+this.name);
//	}
//
//    private void OnTriggerEnter(Collider other)
//    {
//        if(other.name=="MeleeWeapon")
//        {
//     //       Debug.Log("hit by melee weapon");
//            girlController.GetCaught();
//            bossController.CatchPlayer();
//        }
//    }
//
//    public void DisableSensor()
//    {
//        combatSensor.enabled = false;
//    }
//
//    public void EnableSensor()
//    {
//        combatSensor.enabled = true;
//    }
//
//    public void PopReply()
//    {
//        UIManager._instance.ReplyPlayerChat(UIManager._instance.chatPos.name);
//    }
//
//    public void ExitHealChat()
//    {
//        UIManager._instance.ExitHealedChat();
//    }
//
//}
