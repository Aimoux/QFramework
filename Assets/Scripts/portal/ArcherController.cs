//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class ArcherController : MonoBehaviour {
//    [Header("===Bool Parameters===")]
//    public bool retainAnim = false;
//    public bool retainVelo = false;
//    public bool flipped = false;
//    public bool inRange = false;
//    public bool arrived = false;
//    [Header("===Player Parameters===")]
//    public GameObject player;
//    public Vector3 targetFWD;
//    public Vector3 vecToPyr;
//    public float disToPyr;
//    public float rayVFix = 0f;
//    public float maxVsqr = 2f;
//    public float spotRange = 11f;
//    public float shootRange = 6f;
//    public float stepPortal = 1f;
//    [Header ("===Mecanim===")]
//    public float vertical;
//    public float walkVertical = 0.5f;
//    public float runVertical = 1f;
//    public float interval = 3f;
//    public float arriveSqr = 0.04f;
//    public float MoveSpeedMultiplier = 1f;
//    public float hitPower = 2f;
//    public float minSpeed = 5f;
//    public float maxSpeed = 50f;
//    public float damper = 0.1f;
//    public GameObject childModel;
//    public Vector3 thrust;
//    [Header ("===Components===")]
//    [SerializeField] private Animator anim;
//    [SerializeField] private Rigidbody rigid;
//    [SerializeField] private CapsuleCollider caps;
//    [SerializeField] private GroundChecker grdChecker;
//    [Header("===SoundSetting===")]
//    [SerializeField ]private AudioSource voice;
//    public AudioClip fireSound;
//    public AudioClip hitedSound;
//    [Header("===CombatSetting===")]
//    public GameObject bulletPfb;
//    public GameObject bullet;
//    public GameObject bulletPos;
//  
//
//	// Use this for initialization
//	void Start () {
//
//        arrived = true;
////        player = EventManager._instance.curtPlayer;
//        anim = childModel.GetComponent<Animator>();
//        voice = GetComponent<AudioSource>();
//        rigid = GetComponent<Rigidbody>();
//        caps = GetComponent<CapsuleCollider>();
//        grdChecker = GetComponent<GroundChecker>();
//        if (player == null ||childModel==null||anim==null||rigid==null||voice==null||fireSound==null
//            ||hitedSound==null||caps==null||grdChecker ==null ||bulletPfb==null||bulletPos==null)
//            Debug.LogError("null for archer");
//
//        StartCoroutine(TurnInterval());
//
//
//	}
//
//    private void OnDrawGizmos()
//    {
//        Gizmos.color = Color.yellow;
//        Gizmos.DrawLine(transform.position,transform.position+childModel.transform.forward*shootRange);
//    }
//
//
//    // Update is called once per frame
//    void Update () {
//       
//        vecToPyr = player.transform.position - transform.position;
//        float altitude = vecToPyr.y;
//        vecToPyr = new Vector3(vecToPyr.x, 0f, vecToPyr.z);
//        disToPyr = vecToPyr.magnitude;
//
//        if (disToPyr < shootRange && altitude * altitude < maxVsqr )
//        {
//            Ray ray = new Ray(transform.position+Vector3.up*rayVFix, childModel.transform.forward);
//            RaycastHit hit;
//            if (Physics.Raycast(ray, out hit, shootRange,
//                LayerMask.GetMask("Ground", "SimiGround", "Default","Player")))
//            {
//                if (hit.collider.gameObject == EventManager._instance.curtPlayer)
//                    inRange = true;
//                else inRange = false;
//            }
//            else inRange =false;
//        } 
//        else inRange = false;
//        //add ray cast check in range 
//        if (CheckState(anim.GetLayerIndex("Hited"), "Hited")) inRange = false;
//
//       if(retainAnim==false && disToPyr < spotRange && altitude * altitude < maxVsqr )//追逐分支
//        {           
//            vertical = runVertical;
//            if (grdChecker.blocked) arrived = false;
//             else
//            {
//                targetFWD = vecToPyr.normalized;
//                ///  arrived = true;//引起鬼畜
//            }
//            if (arrived==false)
//            {
//                targetFWD = (grdChecker.tempPoint - transform.position).normalized;
//                inRange = false;
//                if ((grdChecker.tempPoint - transform.position).sqrMagnitude < arriveSqr) arrived = true;
//            }
//                    
//            childModel.transform.forward = Vector3.Slerp(childModel.transform.forward, targetFWD, 0.3f);
//
//        }
//        else if(retainAnim ==false && retainVelo==false && anim.GetBool("OnGround"))//巡逻分支
//        {
//            vertical = walkVertical;
//            if (grdChecker.blocked) targetFWD = childModel.transform.right;//一直打转？
//            childModel.transform.forward = Vector3.Slerp(childModel.transform.forward,targetFWD,0.3f);            
//           
//        }
//
//        anim.SetFloat("Vertical", vertical);
//        flipped = (Vector3.Angle(childModel.transform.up, Vector3.up) > 45f) ? true : false;
//        anim.SetBool("Flipped", flipped);
//        anim.SetBool("InRange", inRange);
//
//    }
//
//    private void FixedUpdate()
//    {
//        rigid.velocity += thrust;
//        thrust = Vector3.zero;
//    }
//
//    public bool CheckState(int layerIndex, string statename) //查询是否当前处于某层的某个动画状态
//    {
//        bool result = anim.GetCurrentAnimatorStateInfo(layerIndex).IsName(statename);
//        //if (result)
//        //    Debug.Log("Matched:" + statename);
//        //else
//        //    Debug.Log("Unmatched:" + statename);
//        return result;
//
//    }
//
//    public void LandOnGround()
//    {
//        anim.SetBool("OnGround", true);
//
//    }
//
//    public void OffGround()
//    {
//    
//        anim.SetBool("OnGround", false);
//
//    }
//
//
//    public void OnFallEnter()
//    {
//        retainAnim = true;
//        retainVelo = true;
//      
//    }
//
// 
//
//    public void OnGetUpEnter()
//    {
//        retainAnim = true;
//        retainVelo = true;
//        anim.SetBool("Flipped", false);
//        Vector3 trgtRight = childModel.transform.up;
//        trgtRight = new Vector3(-trgtRight.z, 0f, trgtRight.x).normalized;
//        float angleX = Vector3.Angle(childModel.transform.up, Vector3.up);
//        float angleY = Vector3.Angle(trgtRight, childModel.transform.right);
//        Vector3 crossX = Vector3.Cross(childModel.transform.up, Vector3.up);
//        Vector3 crossY = Vector3.Cross(childModel.transform.right, trgtRight);
//
//        if (Vector3.Dot(childModel.transform.up, crossY) < 0) angleY = -angleY;
//        childModel.transform.Rotate(0f, angleY, 0f);
//        if (Vector3.Dot(childModel.transform.right, crossX) < 0) angleX = -angleX;
//        childModel.transform.Rotate(angleX, 0f, 0f);
//
//    }
//
//    public void OnGetUpdate()
//    {
//        Vector3 velocity = Vector3.zero;
//        rigid.velocity = Vector3.SmoothDamp(rigid.velocity, Vector3.zero, ref velocity, damper);
//
//    }
//
//    public void FireBullet()
//    {
//        voice.PlayOneShot(fireSound);
//        bullet = GameObject.Instantiate(bulletPfb, bulletPos.transform.position,bulletPos.transform.rotation);
//        bullet.GetComponent<ArcherBullet>().enabled = true;
//    }
//
//
//    public void OnGroundEnter()
//    {
//        retainAnim = false;
//        retainVelo = false;
//
//    }
//
//    public void HitByLaser()
//    {
//        anim.SetBool("Hited", true);
//
//    }
//
//
//    public void OnHitedEnter()
//    {
//        thrust = -childModel.transform.forward * hitPower;
//        voice.PlayOneShot(hitedSound );
//      //  Debug.Log("enter hited archer");
//        anim.SetBool("Hited", false);
//        anim.SetInteger("HitPoint", anim.GetInteger("HitPoint")-1);
//  
//    }
//
//
//    public void OnHitedUpdate()
//    {
//        int idHitedLayer = anim.GetLayerIndex("Hited");
//        float weight = anim.GetLayerWeight(idHitedLayer);
//        weight = Mathf.Lerp(weight, 1f, 0.3f);
//        anim.SetLayerWeight(idHitedLayer, weight);
//        inRange = false;
//    }
//
//    public void OnHitedIdleUpdate()
//    {
//        int idHitedLayer = anim.GetLayerIndex("Hited");
//        float weight = anim.GetLayerWeight(idHitedLayer);
//        weight = Mathf.Lerp(weight, 0f, 0.3f);
//        anim.SetLayerWeight(idHitedLayer, weight);
//    }
//
//    public void OnDeathEnter()
//    {
//        retainAnim = true;
//        retainVelo = false;
//        rigid.isKinematic = true;
//
//    }
//
//
//    IEnumerator TurnInterval()
//    {
//        while(true)
//        {
//            yield return new WaitForSeconds(interval);
//            targetFWD = RandomDirection();
//        }
//
//    }
//
//
//    public Vector3 RandomDirection()
//    {
//
//        Vector2 random = Random.insideUnitCircle;
//        Vector3 newFWD = new Vector3(random.x, 0f, random.y);
//        Ray ray = new Ray(transform.position, childModel.transform.forward);
//        RaycastHit hit;
//        if (Physics.Raycast(ray, out hit, 3f))
//        {
//            newFWD = childModel.transform.right;
//        }
//        return newFWD;
//    }
//
//
//    public void MotionMove()//错误的进入此函数
//    {
//        // we implement this function to override the default root motion.
//        // this allows us to modify the positional speed before it's applied.
//        if (retainVelo == false)
//        {
//            Vector3 v = (anim.deltaPosition * MoveSpeedMultiplier) / Time.deltaTime;
//            // we preserve the existing y part of the current velocity.
//            v.y = rigid.velocity.y;
//            rigid.velocity = v;
//        }
//
//    }
//
//    public void TakePortal(GameObject portalPt)
//    {
//        OffGround();//不能本帧马上执行完毕？
//        retainAnim = true;
//        retainVelo = true;
//        float speed = GetComponent<GroundChecker>().fallSpeed;
//        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
//        GameObject portalGo = portalPt.GetComponent<Portal>().portalGo;
//
//        rigid.isKinematic = true;//防止set parent过程中，因rigid受力特性，不能完全执行rotation与position
//        transform.parent = portalPt.transform;
//        Quaternion worldOrginRote = transform.rotation;
//        Quaternion localEnterRote = transform.localRotation;//出门信息暂且与入门一致
//        transform.parent = portalGo.transform;
//        transform.localPosition = new Vector3(0, 0, stepPortal);//step portal
//        transform.localRotation = localEnterRote;
//        Vector3 worldExitPos = transform.position;//最终出门位置                                                                
//        portalGo.transform.forward *= -1;//翻转出门前后
//        Quaternion worldExitRote = childModel.transform.rotation;//最终出门朝向  
//        
//        transform.parent = null;//脱离parent
//        transform.position = worldExitPos;
//        transform.rotation = worldOrginRote;
//        childModel.transform.rotation = worldExitRote;
//        rigid.isKinematic = false;
//        portalGo.transform.forward *= -1;  //翻转回复
//
//        rigid.velocity = portalGo.transform.forward * speed;//设定速度
//    }
//
//    private void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject == EventManager._instance.curtPlayer)
//            rigid.isKinematic = true;
//    }
//
//    private void OnCollisionExit(Collision collision)
//    {
//        if (collision.gameObject == EventManager._instance.curtPlayer)
//            rigid.isKinematic = false;
//    }
//
//}
