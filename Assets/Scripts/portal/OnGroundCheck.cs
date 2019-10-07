//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class OnGroundCheck : MonoBehaviour {
//	[Header ("====Capsule Parameters===")]
//	public CapsuleCollider capsCollider;
//	public Collider[] colliders;
//	public Vector3 point1;
//	public Vector3 point2;
//	private string groundLayer="Ground";
//    private string bridgeLayer = "LightBridge";
//    private string portalLayer = "Portal";
//    private string simiLayer = "SimiGround";
//    private string botLayer = "Bot";
//    public float groundOffset = 0.5f;
//    public float fallOffset = 0.05f;
//	private float offset=0.05f;
//    public bool triggerPortal = false;
//    public bool OnGround = false;
//    public static float fallSpeed;//用于take portal中，解决groundCheck晚于rigid落地，导致速度错误归0的问题
//
//	// Use this for initialization
//	void Start () {
//
//		capsCollider  = GetComponent <CapsuleCollider > ();
//		if (capsCollider == null)
//			Debug.LogError ("null collider!");
//		
//        
//	}
//
//
//    private void OnDrawGizmos()
//    {
//        Gizmos.color = Color.green;
//        Gizmos.DrawWireSphere(transform.position,0.8f);
//        
//    }
//
//    public void SetOnGround()
//    {
//        OnGround = true;
//    }
//    public void SetOffGround()
//    {
//        OnGround = false;
//        Debug.Log("onGround set to false");
//    }
//    // Update is called once per frame
//    void LateUpdate () {
//
//        //		point1 = transform.position+transform.up *(radius-offset) ;  //transform.forward VS Vector3.forward !!
//        //		point2 = transform.position+transform .up *(capsCollider.height-radius+offset )  ;
//
//        if(OnGround )
//        {
//            offset = groundOffset;//防止jump fail
//        }
//        else
//        {
//            offset = fallOffset;
//        }
//        point1 = transform.position -Vector3.up * offset;  
//        point2 = transform.position +Vector3.up * offset;
//
//        colliders = Physics.OverlapCapsule (point1, point2, capsCollider.radius,
//            LayerMask.GetMask (simiLayer,groundLayer,bridgeLayer,portalLayer,botLayer));
//        triggerPortal = false;//此处重置？？？
//        if (colliders.Length != 0) 
//		{
//            foreach (var collider in colliders)
//            {
//
//                //用于take portal中，解决groundCheck晚于rigid落地，导致错误播放ground动画的问题
//                if (collider.gameObject.layer == LayerManager.portalLayer)
//                {               
//                    SendMessage("OffGround");
//               //     Debug.Log("trigger portal");
//                    triggerPortal = true;
//                    //伪off ground，亦须应储存speed，是否会导致垂直速度丢失？
//                    OnGround = false;
//                    //理想状态保存平面速度
//                    fallSpeed = GetComponent<Rigidbody>().velocity.magnitude;
//                    break;
//                }
//            }
//            //未检测到portal，OnGround，纵向速度归零
//            if (triggerPortal == false)
//            {
//                SendMessage("LandOnGround");
//                fallSpeed = 0f;
//                OnGround = true;
//            }
//
//        }
//        else //true off ground，既没有portal，也没有ground
//		{		
//            SendMessage("OffGround");
//            fallSpeed = GetComponent<Rigidbody>().velocity.magnitude;
//            OnGround = false;
//		}
//
//	}
//}
