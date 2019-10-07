//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class LaserDirectionCorrect : MonoBehaviour {
//
//    public bool correctBegin=false;
//    public float slerper=0.3f;
//    public Vector2 correction;
//    private Transform player;
//    private Vector3 targetDir;
//
//	// Use this for initialization
//	void Start ()
//    {
//        player = EventManager._instance.curtPlayer.transform;
//		
//	}
//	
//	// Update is called once per frame
//	void Update () {
//
//
//        if(correctBegin )
//        {
//            targetDir = (player.position+Vector3.up*correction.y+transform.right*correction.x 
//                - transform.position).normalized ;
//            // transform.forward =Vector3.Slerp(transform.forward, targetDir, slerper);
//            transform.forward = targetDir;
//
//        }
//		
//	}
//
//    
//}
