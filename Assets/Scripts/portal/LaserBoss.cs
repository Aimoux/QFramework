//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//
//public class LaserBoss : MonoBehaviour {
//
//    
//    
//    private BossController  am;
//    private int hitCount = 0;
//    
//
//	// Use this for initialization
//	void Start () {
//
//        
//        am = GetComponent<BossController >();
//        if ( am==null )
//            Debug.LogError("laserBoss has null manager");
//
//	}
//	
//	// Update is called once per frame
//
//    void Update()
//    {
//
//        if (hitCount > 0)
//        {
//            am.HitedByLaser();
//        }
//        else
//        {
//            am.HitLaserMiss();
//        }
//        hitCount = 0;
//
//    }
//
//
//    public void HitCount()
//    {
//        hitCount++;
//    }
//
//
//
//
//}
