//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using BehaviorDesigner.Runtime.Tasks.Movement;
//
//public class AIPlayerController : MonoBehaviour {
//
//    [Header ("===Locomotive Variables===") ]
//    public GameObject childModel;
//    public PhysicMaterial zeroFriction;
//    public bool retainVelo=false ;
//    public bool retainAnim=false ;   
//    public Vector3 thrust;   
// //   public float moveSpeed=4.5f;
//    public float hitPower=2f;   
//    public float maxAngle=0.5f;
//    public float minAngle=-0.5f;
// //   public float verticalNorm = 0.2f;
//    public float verticalToSpeed=2.85f;
//    public Vector3 desiredVelo;
//
//    [HideInInspector ]public int idVertical=Animator.StringToHash ("Vertical");
//    [HideInInspector ]public int idGround=Animator.StringToHash ("OnGround");  
//    [HideInInspector ]public string combatLayerName="Combat";
//    [HideInInspector ]public string motionLayerName="Locomotive";
//    [HideInInspector ]public string hitedLayerName="Hited";
//    [HideInInspector ]public  string fallState="Fall";
//    [HideInInspector ]public string groundState = "Grounded";
//    [HideInInspector ]public int idCombatLayer;
//    [HideInInspector ]public int idMotionLayer;
//    [HideInInspector ]public int idHitedLayer;
//    private CapsuleCollider capsule;   
//    private Rigidbody rigid;
//    private Animator anim;
//    private float damper;
//    private UnityEngine.AI.NavMeshAgent  nav;
//  //  private AIFollowTask fok;
//   
//
//    // Use this for initialization
//    void Awake ()
//    {
//  //      fok = GetComponent <AIFollowTask >();
//        anim = childModel.GetComponent <Animator >();
//        rigid = GetComponent <Rigidbody >();
//        capsule = GetComponent <CapsuleCollider >();
//        nav = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
//        if (rigid == null | anim == null | nav == null|childModel ==null|capsule ==null)
//            Debug.LogError("null component");
//        
//        idCombatLayer = anim.GetLayerIndex(combatLayerName);
//        idMotionLayer = anim.GetLayerIndex(motionLayerName);
//        idHitedLayer = anim.GetLayerIndex(hitedLayerName);
//    }	
//	
//	
//	void Update () 
//    {
//        desiredVelo = nav.desiredVelocity;//desiredVelocity vs speed?
//   //     anim.SetFloat(idVertical ,nav.speed *verticalNorm  );
//        //nav速度方向变化快属于正常？，而速度的积分，transform.forward更为稳定
//     //   childModel.transform.forward = Vector3.Lerp(childModel.transform.forward, desiredVelo.normalized, 0.3f);// ????鬼畜？
//        childModel.transform.forward = Vector3.Lerp(childModel.transform.forward, transform.forward , 0.3f);
//        nav.speed = anim.GetFloat(idVertical) * verticalToSpeed;
//		
//	}
//    void FixedUpdate()
//    {
//
//
//    }
//
//    public void SetVertical(float targetVertical)//
//    {
//
//        float curVertical = anim.GetFloat(idVertical); 
//        curVertical  = Mathf.SmoothDamp (curVertical , targetVertical , ref damper, 0.3f); 
//        anim.SetFloat(idVertical, curVertical);//设定行进速度   
////        Debug.Log("targetVertical:" + targetVertical);
////        Debug.Log("vertical："+anim.GetFloat(idVertical) );
////        Debug.Log("cur vertical："+curVertical );           
//
//    }
//
//    public bool CheckState( int layerIndex, string statename) //查询是否当前处于某层的某个动画状态
//    {
//        bool result = anim.GetCurrentAnimatorStateInfo (layerIndex).IsName (statename );
//        //      if (result)
//        //          Debug.Log ("Matched:" + statename);
//        //      else
//        //          Debug.Log ("Unmatched:"+statename );
//        return result ;
//
//    }
//
//    public void OnGroundEnter()
//    {
//        retainAnim  = false;
//        retainVelo   = false;  
//        capsule.material = null;    
//    }
//    public void OnGroundUpdate()//传送落地后，修正玩家Up与世界一致。
//    {
//
//        transform.up = Vector3.Lerp(transform.up, Vector3.up, 0.3f);
//
//    }
//
//
//    public void LandOnGround()
//    {
//        anim.SetBool(idGround, true);
//        //      Debug.Log("land On grond");
//    }
//
//    public void OffGround()
//    {
//        anim.SetBool(idGround, false);
//        //     Debug.Log("off Ground");
//
//    }
//
//    public void OnInjuredEnter()
//    {
//        retainAnim = false;
//        retainVelo = false;
//
//        capsule.material = null;    
//    }
//        
//    public void OnFallEnter()//若坠落中撞竖直墙，水平面速度被锁定导致无限大Force把角色压在墙面粘住。
//    { //排除方法，把墙layer设置为ground或者更改physics material摩擦力系数。
//        retainAnim  =false ;//可以动作
//        retainVelo   = true;    //保留速度
//        capsule .material =zeroFriction ;
//    }
//
//    public void OnFallExit()
//    {
//
//    }
//
//    public void OnCombatIdleUpdate()//清除多余的OnAUpdate
//    {
//        float weight = anim.GetLayerWeight (idCombatLayer );
//        weight=Mathf.Lerp (weight, 0f, 0.3f);
//        //  Debug.Log ("weightUpdate=:" + weight);
//        anim.SetLayerWeight (idCombatLayer  , weight );//
//
//    }       
//
//    public void OnHitedEnter()
//    {
//        thrust = -childModel.transform.forward * hitPower;
//    }
//
//    public void OnHitedUpdate()
//    {
//        float weight = anim.GetLayerWeight (idHitedLayer );
//        weight=Mathf.Lerp (weight, 1f, 0.3f);   
//        anim.SetLayerWeight (idHitedLayer  , weight );
//    }
//
//    public void OnHitedIdleUpdate()
//    {
//        float weight = anim.GetLayerWeight (idHitedLayer );
//        weight=Mathf.Lerp (weight, 0f, 0.3f);
//        anim.SetLayerWeight (idHitedLayer  , weight );
//    }
//
//    public void OnHealOtherEnter()
//    {
//        //应立即停止
//        retainAnim = true;
//        retainVelo = false;
//   
//    }
//
//    public void OnHealOtherUpdate()
//    {
//        //      float dx = anim.GetFloat (idHealProcess )*2f ;
//        //      thrust =- childModel.transform.forward * dx;
//    }
//
//
//    public void OnKnockedOutEnter()
//    {
//        //应立即停止
//        retainAnim = true;
//        retainVelo = false;
//    
//
//    }
//
//
//
//
//
//
//}
