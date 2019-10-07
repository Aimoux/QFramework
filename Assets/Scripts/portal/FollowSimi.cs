//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class FollowSimi : MonoBehaviour {
//
//    public Transform target;
//    public float lerper = 0.3f;
//    public float damper = 0.1f;
//
//	// Use this for initialization
//	void Start () {
//
//        if (target == null) Debug.LogError("null for follow simi: " + this.name);
//
//	}
//	
//	// Update is called once per frame
//	void Update () {
//
//        transform.position = Vector3.Slerp(transform.position, target.position, lerper);
//        transform.forward = Vector3.Slerp(transform.forward, target.forward,lerper);
//
//		
//	}
//}
