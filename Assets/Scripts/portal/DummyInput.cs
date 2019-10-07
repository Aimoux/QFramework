//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class DummyInput : MonoBehaviour {
//
//    public float drUp;
//    public float drRight;
//    private float drUpVelocity;
//    private float drRightVelocity;
//    private float targetDrUp;
//    private float targetDrRight;
//    public bool retainAnim = false;
//    public float drMag;
//    public Vector3 drVec;
//    public  GameObject player;
//
//    // Use this for initialization
//    void Start () {
//
//        player = EventManager._instance.curtPlayer;
//
//    }
//
//     void Update()
//    {       
//	
//		if (!retainAnim )
//        { //软禁用操控
//            targetDrUp = (transform.position.z <player.transform.position.z) ? 1.0f : -1.0f;
//            targetDrRight = (transform.position.x < player.transform.position.x) ? 1.0f : -1.0f;
//	
//		}
//        else
//        {
//			targetDrUp = 0f;
//			targetDrRight =0f;
//		}        
//
//	    drUp = Mathf.SmoothDamp(drUp, targetDrUp, ref drUpVelocity, 0.1f); //平滑过渡
//	    drRight = Mathf.SmoothDamp(drRight, targetDrRight, ref drRightVelocity, 0.1f);
//	    Vector2 circle = SquareToCircle(drRight, drUp);//化方为圆，解决左/右前方速度合成为1.414的问题
//        drMag = circle.magnitude;	
//	    drVec = Vector3.forward* drUp +Vector3.right* drRight;//	
//
//	}
//
//	Vector2 SquareToCircle(float x, float y) //不断迭代？按住后再不同帧迭代？
//     {
//        Vector2 circle = Vector2.zero;
//        circle.x = x * Mathf.Sqrt(1 - 0.5f * y * y);
//        circle.y = y * Mathf.Sqrt(1 - 0.5f * x * x);
//        return circle;
//    }
//
//
//}
