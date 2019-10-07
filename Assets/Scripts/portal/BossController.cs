//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Playables;
//
//public class BossController : MonoBehaviour
//{
//    private SphereCollider sphere;
//    private WeaponManager wm;
//    private Animator anim;
//    private DummyInput di;
//    private GameObject player;
//    private Rigidbody rigid;
//    private Vector3 deltaPos;
//    private float rigidSpeed;
//    private int idVertical = Animator.StringToHash("Vertical");
//    private int idDistance = Animator.StringToHash("Distance");
//    private BossSoundController soundController;
//    [Header("===Locomotive===")]
//    public bool retainVelo = false;
//    public bool run = true;
//    public GameObject childModel;
//    public Vector3 vecToPlayer;
//    public Vector3 planarVec;
//    public float distToPlayer;
//    public float walkSpeed = 0.95f;
//    public float runMultiplier = 6f;
//    public float MoveSpeedMultiplier = 1f;
//    [Header("===Combat===")]
//    public GameObject powerBall;
//    public GameObject blockWall;
//    public SkinnedMeshRenderer bossMesh;
//    public Material defeat;
//    public Transform weakNess;
//    [Header("===Playable===")]
//    public PlayableAsset catchAndToss;
//    public PlayableAsset conjured;
//    public PlayableAsset meteror;
//    private PlayableDirector director;
//
//    void Start()
//    {
//        wm = GetComponentInChildren<WeaponManager>();
//        di = GetComponent<DummyInput>();
//        anim = childModel.GetComponent<Animator>();
//        soundController = childModel.GetComponent<BossSoundController>();
//        player = EventManager._instance.curtPlayer;
//        rigid = GetComponent<Rigidbody>();
//        sphere = GetComponent<SphereCollider>();
//        director = childModel.GetComponent<PlayableDirector>();
//        if (anim == null || wm == null || player == null || rigid == null || powerBall == null
//            || bossMesh == null || defeat == null || weakNess == null || catchAndToss == null||
//            conjured==null||meteror==null||sphere==null)
//            Debug.LogError("null for boss");
//        wm.am = this;       
//
//    }
//
//    void Update()
//    {
//        vecToPlayer = player.transform.position - transform.position;
//        vecToPlayer = new Vector3(vecToPlayer.x, 0f, vecToPlayer.z);
//        distToPlayer = vecToPlayer.magnitude;
//        anim.SetFloat(idDistance, distToPlayer);
//        //    run = (distToPlayer > 20f) ? false : true;//保持行走状态观感更好
//        float verticalMultiplier = (run ? 2f : 1f) * di.drMag;//pi.drMag由inputEnable控制开关！！
//        anim.SetFloat(idVertical, Mathf.Lerp(anim.GetFloat(idVertical), verticalMultiplier, 0.5f)); //设置走/跑
//        if (di.drMag > 0.1f && di.retainAnim == false)//阻止drVec衰减成0，角色错误的恢复原方位。>0.1???
//        {    //pi.drVecl来自于inputEnabled，故要锁定速度，需lockPlanar和inputenable一起设状态。              
//            childModel.transform.forward = Vector3.Slerp(childModel.transform.forward, vecToPlayer, 0.3f);
//            //Debug.Log("still rotate to player");
//        }
//        if (retainVelo==false)
//        {
//            planarVec = childModel.transform.forward * walkSpeed * di.drMag * (run ? runMultiplier : 1f);
//        }
//
//    }
//
//    public void SetTearTrigger()
//    {
//        anim.SetTrigger("Tear");
//    }
//
//    public bool CheckState(int layerIndex, string statename) //查询是否当前处于某层的某个动画状态
//    {
//        bool result = anim.GetCurrentAnimatorStateInfo(layerIndex).IsName(statename);
//        return result;
//    }
//
//    public void OnGroundEnter()
//    {
//        //    Debug.Log("enter ground state");
//        di.retainAnim = false;
//        retainVelo = false;
//        wm.meleeWeapon.SetActive(false);
//        wm.laserWeapon.SetActive(false);
//        wm.laserBall.SetActive(false);
//        powerBall.SetActive(false);
//        rigid.isKinematic = false;
//
//    }
//
//    public void OnMeleeEnter()
//    {
//        wm.meleeWeapon.SetActive(true);
//        wm.laserWeapon.SetActive(false);
//        wm.laserBall.SetActive(false);
//        if (wm.rangeWeapon != null) Object.Destroy(wm.rangeWeapon);
//
//    }
//
//    public void OnMeleeRandomEnter()
//    {
//        wm.WeaponDisable();
//    }
//
//
//    public void CatchPlayer()
//    {
//        wm.WeaponDisable();
//        anim.SetBool("Catch", true);
//        director.playableAsset = catchAndToss;
//        director.Play();
//    }
//
//
//    public void OnCatchPlayerEnter()
//    {
//        wm.WeaponDisable();
//        rigid.isKinematic = true;
//        anim.SetBool("Catch", false);
//        di.retainAnim = true;
//        retainVelo = false;
//        soundController.PlaySound("Audio/Boss/Rua");
//    }
//
//    public void OnTossPlayerEnter()
//    {
//        wm.WeaponDisable();
//        di.retainAnim = true;
//        retainVelo = false;
//        UIManager._instance.BossAttackSay();
//    }
//
//    public void OnTossPlayerExit()
//    {
//        rigid.isKinematic = false;
//    }
//
//    public void OnRangeEnter()
//    {
//        //    Debug.Log("enter rang state");
//        di.retainAnim = false;//可转向
//        retainVelo = false;//不冻结速度
//        wm.meleeWeapon.SetActive(false);
//        wm.laserWeapon.SetActive(false);
//    }
//
//
//    public void ShotPlayer() //重置rangeHit
//    {
//        anim.SetTrigger("RangeHit");
//    }
//
//    public void OnPowerUpEnter()
//    {
//        powerBall.SetActive(true);
//        soundController.PlaySound("Audio/Boss/Roar");
//    }
//
//    public void OnLaserEnter()
//    {
//        //  Debug.Log("enter laser state");
//        wm.meleeWeapon.SetActive(false);
//        anim.SetBool("Laser", false);
//    }
//
//    public void OnHoldEnter()
//    {
//        di.retainAnim = true;
//        retainVelo = false;
//        wm.laserCorrect.correctBegin = false;
//    }
//
//    public void HitedByLaser()// called by laserBoss
//    {
//        //  Debug.Log("hit by laser");
//        anim.SetBool("Hit", true);
//
//    }
//
//    public void HitLaserMiss()// Laser Reflection Miss Boss,boss未被激光击中
//    {
//        //  Debug.Log("miss laser reflection attack");
//        anim.SetBool("Hit", false);
//
//    }
//
//    public void OnBossLaseredEnter()
//    {
//        // anim.SetBool("Hit", false);
//        wm.laserBall.SetActive(false);
//        wm.laserWeapon.SetActive(false);
//        UIManager._instance.BossLaseredHit();
//        anim.SetInteger("Damage", anim.GetInteger("Damage") + 1);
//        soundController.PlaySound("Audio/Boss/RoarAtk");
//        UIManager._instance.BossLaseredSay();
//
//    }
//
//    public void OnSummonEnter()
//    {
//        if(anim.GetInteger("Damage")==1) director.playableAsset = conjured;
//        else director.playableAsset = meteror;
//        director.Play();
//    }
//
//    public void OnMeteorExit()
//    {
//        StopDirector();
//    }
//
//    public void OnDeathEnter()
//    {
//        wm.laserBall.SetActive(false);
//        wm.laserWeapon.SetActive(false);
//        rigid.isKinematic = true;
//       // sphere.enabled = false;
//        EventManager._instance.BossDefeated();
//        soundController.PlaySound("Audio/Boss/Shatter");
//
//    }
//
//    public void OnShoutEnter()
//    {
//        weakNess.gameObject.SetActive(false);
//        bossMesh.material = defeat;
//        
//    }
//
//    public void MotionMove()
//    {
//        Vector3 v = (anim.deltaPosition * MoveSpeedMultiplier) / Time.deltaTime;
//        v.y = rigid.velocity.y;
//        rigid.velocity = v;
//    }
//
//    public void StopDirector()
//    {
//        director.Stop();
//    }
//
//
//}
//
//
