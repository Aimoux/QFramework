//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using BehaviorDesigner.Runtime;
//using BehaviorDesigner.Runtime.Tasks.Movement;
//
//
//public class FootManAI	: MonoBehaviour {
//	
//	[Header ("===Behaviour Variables===")]
//	
//	public Vector3 drToPlayer;
//	public float dstToPlayer;
//	public float fireRange=5f;
//	public float firePace=1.3f;  
//    private BehaviorTree footBT; 
//    public SharedTransform  playerInSight;
//    public Vector3 posVec;
//    private string seePlayer="ObjectInSight";
//    public Transform targetTransform;
//
//    [HideInInspector ]public int idVertical=Animator.StringToHash ("Vertical");
//    [HideInInspector ]public int idGround=Animator.StringToHash ("OnGround");
//    [HideInInspector ]public int idFire=Animator.StringToHash ("Fire");
//    [HideInInspector ]public int idFireAngle=Animator.StringToHash ("FireAngle");
//
//    [HideInInspector ]public string combatLayerName="Combat";
//    [HideInInspector ]public string motionLayerName="Locomotive";
//    [HideInInspector ]public string hitedLayerName="Hited";
//    [HideInInspector ]public  string fallState="Fall";
//    [HideInInspector ]public string groundState = "Grounded";
//    [HideInInspector ]public string fireState="Fire";
//    [HideInInspector ]public int idCombatLayer;
//    [HideInInspector ]public int idMotionLayer;
//    [HideInInspector ]public int idHitedLayer;
//
//	public float fireAngle;
//	private Rigidbody rigid;
//	private Animator anim;
//	private UnityEngine.AI.NavMeshAgent  nav;
//	public Vector3 drNav;
//
//	// Use this for initialization
//	void Awake () {
//		anim = GetComponent <Animator > ();
//		rigid = GetComponent <Rigidbody > ();
//        footBT = GetComponent<BehaviorTree>();
//		nav = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ();
//        if(rigid==null |anim==null |nav==null|footBT==null )
//			Debug.LogError ("null component");
//		
//		idCombatLayer = anim.GetLayerIndex (combatLayerName);
//		idMotionLayer = anim.GetLayerIndex (motionLayerName );
//		idHitedLayer = anim.GetLayerIndex (hitedLayerName);
//
//        playerInSight  =(SharedTransform)footBT.GetVariable (seePlayer );
//        if (playerInSight == null)
//            Debug.LogError("wrong name for playerinsight");
//        else
//            targetTransform = playerInSight.Value;
//	}
//	
//	// Update is called once per frame
//	void Update () 
//	{
//	
//        targetTransform = playerInSight.Value;
//      
//      
//           
//   
//
//	}
//	void FixedUpdate()
//	{
//
//
//
//
//
//	}
//
//    //Fireanglei跳动？靠近angle<0?
//	public void OnFireEnter()//Enter与Update哪个更好？开枪中可转向不太妥当
//	{
//		//存储hit.point即可			
//        if(playerInSight.Value  !=null )
//		{
//		fireAngle=Mathf .Atan2 ( (targetTransform .position .y -transform.position.y ),dstToPlayer ); //0.8默认身体中心高度
//		fireAngle =Mathf.Clamp (fireAngle ,-0.5f ,1f);//待定
//		}
//	}
//
//	public void OnFireUpdate() //OnAUpdate在OnAEnter与OnAExit时间段执行，OnBEnter包含在这段时间内。
//	{	
//        if(playerInSight.Value  !=null)
//		{
//		float weight = anim.GetLayerWeight (idCombatLayer );
//		weight=Mathf.Lerp (weight, 1f, 0.3f);	
//		anim.SetLayerWeight (idCombatLayer  , weight );//设置combat层权重为1
//		transform.forward = Vector3.Slerp (transform .forward ,drToPlayer  ,0.3f);//使角色朝向drFire
//		float curAngle = anim.GetFloat (idFireAngle);//设置开枪地俯仰角度
//		curAngle = Mathf.Lerp (curAngle, fireAngle, 0.3f);
//		anim.SetFloat (idFireAngle, curAngle);//
//		}
//
//	}		
//
//    public void OnHitedIdleUpdate()
//    {
//        float weight = anim.GetLayerWeight (idHitedLayer );
//        weight=Mathf.Lerp (weight, 0f, 0.3f);
//        anim.SetLayerWeight (idHitedLayer  , weight );
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
////    public void OnHitedEnter()
////    {
////        thrust = -childModel.transform.forward * hitPower;
////    }
//
//    public void OnHitedUpdate()
//    {
//        float weight = anim.GetLayerWeight (idHitedLayer );
//        weight=Mathf.Lerp (weight, 1f, 0.3f);   
//        anim.SetLayerWeight (idHitedLayer  , weight );
//    }
//
//}
