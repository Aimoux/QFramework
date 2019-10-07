//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class GroundChecker : MonoBehaviour {
//    [Header("====GroundCheck===")]
//    public bool pyrChecker = false;
//    public bool OnGround = false;
//    public GameObject childModel;
//    public LayerMask groundMask;
//    public LayerMask blockMask;
//    public Collider[] colliders;
//    public Vector3 point1;
//    public Vector3 point2;
//    public float fallOffset = 0.05f;
//    public float jumpOffset = 1.5f;
//    public float groundOffset = 0.05f;
//    [Header("===PortalCheck===")]
//    public bool triggerPortal = false;
//    public Collider[] portalColliders;
//    public Vector3 portalPoint1;
//    public Vector3 portalPoint2;
//    public float portalOffset = 0f;
//    public float fallSpeed;//用于take portal中，解决groundCheck晚于rigid落地，导致速度错误归0的问题
//    public float minSpeed = 5f;
//    public float maxSpeed = 50f;
//    public float stepPortal = 1f;
//    [Header("===BlockCheck===")]
//    public bool checkBlock = true;
//    public bool blocked = false;
//    public Collider[] blockColliders;
//    public Vector3 blockPoint1;
//    public Vector3 blockPoint2;
//    public Vector3 tempCoord=new Vector3 (1.5f,0,1.5f);
//    public Vector3 newCoord;
//    public Vector3 tempPoint;
//    public float blockOffset = 0.3f;
//    private CapsuleCollider caps;
//    private Rigidbody rigid; 
//
//
//
//    // Use this for initialization
//
//    private void Start()
//    {
//        if (pyrChecker) groundMask = LayerMask.GetMask("Ground", "SimiGround", "LightBridge", "Bot");
//        //剔除lightBridge，或者用动态nav，否则nav报错
//        else groundMask = LayerMask.GetMask("Ground", "SimiGround", "BotWall","Player");
//        blockMask = LayerMask.GetMask("SimiGround","Ground","LightBridge","BotWall"); 
//         caps = GetComponent<CapsuleCollider>();
//        rigid = GetComponent<Rigidbody>();
//        if (rigid == null ||childModel==null||caps==null) Debug.LogError("null for checker:"+this.name);
//    }
//
//    private void OnDrawGizmos()
//    {
//        Gizmos.color = Color.red;
//        Gizmos.DrawWireSphere(point2- GetComponent<CapsuleCollider>().radius*Vector3.up, 0.1f);
//    //    Gizmos.DrawSphere(tempPoint, 0.5f);
//    }
//
//    public void SetOnGround()
//    {
//        OnGround = true;
//    }
//    public void SetOffGround()
//    {
//        OnGround = false;
//       // Debug.Log("OnGround is set to false");
//    }
//    // Update is called once per frame
//    void LateUpdate()
//    {
//        if (OnGround) groundOffset = jumpOffset;//防止jump fail
//        else groundOffset = fallOffset;
//        GroundCheck();
//        PortalCheck();
//        if (checkBlock ) BlockCheck();
//
//    }
//
//    public void GroundCheck()
//    {
//        //upper point higher than orgin capscollideer
//        point1 = transform.position + caps.center + transform.up * (0.5f * caps.height + groundOffset - caps.radius);
//        //lower point below orgin capscollider
//        point2 = transform.position + caps.center - transform.up * (0.5f * caps.height + groundOffset - caps.radius);
//        colliders = Physics.OverlapCapsule(point1, point2, caps.radius, groundMask);
//        if (colliders.Length != 0)
//        {
//            SendMessage("LandOnGround");
//            OnGround = true;
//
//        }
//        else
//        {
//            SendMessage("OffGround");
//            //  Debug.Log("no gorund collider offground");
//            OnGround = false;
//            triggerPortal = false;//防止速度衰减过快
//
//        }
//
//    }
//
//    public void PortalCheck()
//    {
//
//        portalPoint1 = transform.position + caps.center + transform.up * (0.5f * caps.height - caps.radius);
//        portalPoint2 = transform.position + caps.center - transform.up * (0.5f * caps.height - caps.radius);
//        portalPoint1 += rigid.velocity * Time.fixedDeltaTime + rigid.velocity.normalized * portalOffset;
//        portalPoint2 += rigid.velocity * Time.fixedDeltaTime + rigid.velocity.normalized * portalOffset;
//        portalColliders = Physics.OverlapCapsule(portalPoint1, portalPoint2, caps.radius, LayerMask.GetMask("Portal"));
//        if (portalColliders.Length == 0)
//        {
//            triggerPortal = false;//一直处于未侦测到portal的状态中
//        }
//        else//一直处于侦测到portal的状态中
//        {
//            if (triggerPortal == false)//第一次由未侦测转入到已经侦测，存储fallSpeed
//            {
//
//             //   Debug.Log("caps about to enter portal");
//                OnGround = false;
//                SendMessage("OffGround");
//                triggerPortal = true;//切换状态
//                fallSpeed = rigid.velocity.magnitude;//速度只保留切换状态发生的那一帧。
//            }
//            else //仍处于triggerPortal中，但并非触发帧，发送check信息，不储存fallSpeed
//            {
//                OnGround = false;
//                SendMessage("OffGround");
//            }
//        }
//
//    }
//
//
//    public void BlockCheck()
//    {
//        blockPoint1 = transform.position + caps.center + transform.up * (0.5f * caps.height - caps.radius);
//        blockPoint2 = transform.position + caps.center - transform.up * (0.5f * caps.height - caps.radius);
//        blockPoint1 += childModel.transform.forward;
//        blockPoint2 += childModel.transform.forward+Vector3.up*blockOffset;
//        if (blockPoint2.y > blockPoint1.y) Debug.LogError("Warning: BlockPoint UpsideDown:"+this.name);
//
//
//        blockColliders = Physics.OverlapCapsule(blockPoint1, blockPoint2, caps.radius, blockMask);
//
//        if (blockColliders.Length == 0) blocked = false;
//        else if (blockColliders.Length == 1)//
//        {
//            blocked = true;
//            Vector3 vecToBlock = blockColliders[0].gameObject.transform.position - transform.position;//若有多个block物，如何优化？
//            if (Vector3.Cross(vecToBlock, childModel.transform.forward).y < 0f)
//                newCoord = new Vector3(-tempCoord.x, tempCoord.y, tempCoord.z);//只翻转一次
//            else newCoord = tempCoord;
//            tempPoint = childModel.transform.TransformPoint(newCoord);//斜前方非垂直
//           // Debug.Log("blocked by:" + blockColliders[0].name);
//
//        }
//        else//blocked by multiple objects just turn back
//        {
//            blocked = true;
//            newCoord = new Vector3(tempCoord.x, tempCoord.y, -tempCoord.z);
//            tempPoint = childModel.transform.TransformPoint(newCoord);//斜前方非垂直                     
//          //  Debug.Log("blocked by more than one blocker."+this.name);
//
//        }
//        //防止Flip或着Fall时被Block，产生无法达到的y值。 
//        tempPoint = new Vector3(tempPoint.x, transform.position.y, tempPoint.z);
//
//    }
//
//    
//}
