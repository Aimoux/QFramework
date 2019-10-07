//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class BattleWithBot : MonoBehaviour {
//
//    [SerializeField] private GrilGunController girlController;
//    [SerializeField] private CapsuleCollider caps;
//    public BossController bossController;
//    public bool laserHit = false;
//	// Use this for initialization
//	void Start () {
//
//        girlController = transform.parent.GetComponent<GrilGunController>();
//        caps = GetComponent<CapsuleCollider>();
//        if (girlController == null || bossController == null||caps==null)
//            Debug.LogError("Null for sensor battle bots");
//		
//	}
//	
//	// Update is called once per frame
//	void Update ()
//    {
//        if (laserHit) girlController.PlayerHitbyLaser();
//        else girlController.EvadeHit();
//        laserHit = false;
//		
//	}
//
//    public void HitByLaser()
//    {
//        laserHit = true;
//    //    Debug.Log("hit by laser invoked.");
//    }
//
//    public void EnableSensor()
//    {
//        caps.enabled = true;
//
//    }
//    public void DisableSensor()
//    {
//        caps.enabled = false;
//    }
//
//    private void OnTriggerEnter(Collider other)
//    {
//
//        if (other.name == "FireBall(Clone)")
//        {
//            //       Debug.Log("hit by fireball clone");
//            girlController.ShotByFireBall();
//            bossController.ShotPlayer();
//
//        }
//
//
//    }
//
//}
