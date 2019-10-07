//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Playables;
//using UnityStandardAssets.Cameras;
//using MorePPEffects;
//
//public class GrilGunController : MonoBehaviour
//{
//    [Header("===Components===")]
//    public PlayerInput pi;
//    public GameObject childModel;
//    public Attachable attachCube;
//    private CapsuleCollider capsule;
//    private Animator anim;
//    private PlayerBattleController battle;
//    private BattleWithBot battleBot;
//    private Rigidbody rigid;
//    private AudioSource voice;
//    [Header("===Bool===")]
//    public bool retainVelo = false;
//    public bool canOperate = false;
//    public bool flipped = false;
//    public bool canFire = true;//�����޶��ڱ����������������У���������fire
//    private bool ethereal = false;
//    private bool alive = true;
//    private bool canPortal = true;
//    public bool CanPortal { get { return canPortal; } }
//    [Header("===Locomotive Variables===")]
//    public float walkSpeed = 0.95f;//Ӧ��ȥ1��
//    public float runMultiplier = 6f;
//    public float jumpPower = 4f;
//    public float tossPlaneSpeed = 10f;
//    public float tossYSpeed = 2f;
//    public float tossDamper = 0.15f;
//    public float flipDamper = 0.1f;
//    public float flipAngle = 60f;
//    public float bulletDelayTime = 0.2f;
//    public float maxAngle = 0.5f;
//    public float minAngle = -0.2f;
//    private float fireAngle;
//    private Vector3 drFire;
//    private Vector3 newPortalPos;
//    private Vector3 planarVec;
//    private Vector3 thrust;
//    private Vector3 deltaPos;
//    [Header("===Combat Parameters===")]
//    public Transform cameraPos;
//    public SkinnedMeshRenderer girlMesh;
//    public MeshRenderer girlGun;
//    public PhysicMaterial zeroFriction;
//    public Material ghost;
//    public Material normal;
//    public GameObject bulletPfb;
//    public GameObject bullet;
//    public Transform bulletPos;
//    public WeaponManager bossWM;
//    public PortalGun ptlGun;
//    private Transform explode;
//    [Header("===SoundSetting===")]
//    public SoundPlayer soundController;
//    public AudioClip[] moan;
//    public AudioClip[] operating;
//    private AudioClip sound;
//
//    #region animator const
//    [Header("===Animator Const===")]
//    private int idVertical = Animator.StringToHash("Vertical");
//    private int idJump = Animator.StringToHash("Jump");
//    private int idGround = Animator.StringToHash("OnGround");
//    private int idFire = Animator.StringToHash("Fire");
//    private int idFireAngle = Animator.StringToHash("FireAngle");
//    private int idFlipped = Animator.StringToHash("Flipped");
//    private int idOperate = Animator.StringToHash("Operate");
//    private string combatLayerName = "Combat";
//    private string motionLayerName = "Locomotive";
//    private string hitedLayerName = "Hited";
//    private string jumpState = "Jump";
//    private string healState = "HealOther";
//    private string hitedState = "Hited";
//    private int idCombatLayer;
//    private int idMotionLayer;
//    private int idHitedLayer;
//    #endregion
//
//    void Awake()
//    {
//
//        anim = childModel.GetComponent<Animator>();
//        voice = childModel.GetComponent<AudioSource>();
//        pi = GetComponent<PlayerInput>();
//        rigid = GetComponent<Rigidbody>();
//        capsule = GetComponent<CapsuleCollider>();
//        battle = childModel.GetComponent<PlayerBattleController>();
//        battleBot = GetComponentInChildren<BattleWithBot>();
//        soundController = childModel.GetComponent<SoundPlayer>();
//        explode = transform.Find("Explode");
//
//        if (anim == null | pi == null | rigid == null | childModel == null || girlMesh ==
//            null | capsule == null | zeroFriction == null | battle == null || cameraPos == null
//            || bulletPfb == null || bulletPos == null || normal == null || ghost == null || explode == null
//            || moan.Length == 0 || operating.Length == 0 || battleBot == null)
//            Debug.LogError("Null component!");
//
//        idCombatLayer = anim.GetLayerIndex(combatLayerName);
//        idMotionLayer = anim.GetLayerIndex(motionLayerName);
//        idHitedLayer = anim.GetLayerIndex(hitedLayerName);
//
//
//    }
//    private void Start()
//    {
//        ptlGun = EventManager._instance.ptlGun;
//        if (ptlGun == null) Debug.LogError("null for girl");
//    }
//
//    // Update is called once per frame
//    void Update()
//    {
//        float verticalMultiplier = (pi.run ? 2f : 1f) * pi.drMag;//pi.drMag��inputEnable���ƿ��أ���
//        //������/�ܣ���pi.inputenable���ƿ��أ�
//        anim.SetFloat(idVertical, Mathf.Lerp(anim.GetFloat(idVertical), verticalMultiplier, 0.5f));
//        if (pi.drMag > 0.1f && pi.retainAnim == false)//��ֹdrVec˥����0����ɫ�����Ļָ�ԭ��λ��
//        {
//            //pi.drVecl������inputEnabled����Ҫ�����ٶȣ���lockPlanar��inputenableһ����״̬��
//            //����ɫ��ֵת����������
//            childModel.transform.forward = Vector3.Slerp(childModel.transform.forward, pi.drVec, 0.3f);
//  
//        }
//        if (!retainVelo)
//        {
//            planarVec = childModel.transform.forward * walkSpeed * pi.drMag * ((pi.run) ? runMultiplier : 1f);
//        }
//        anim.SetBool(idJump, pi.jump);
//        if (ethereal) canFire = false;
//        if (canFire && UICamera.isOverUI == false) anim.SetBool(idFire, pi.openFire);
//        if (canFire && pi.openFire && UICamera.isOverUI == false)
//        {
//            Invoke("FireBullet", bulletDelayTime);
//        }
//        //ʵ�ְ���������ʱ����
//        if (pi.operate && EventManager._instance.interactOperator != null)
//            EventManager._instance.SelectInteractInstance();
//        if (canOperate) anim.SetBool(idOperate, pi.operate);//���Ų������Կ��Ŷ���
//        flipped = (Vector3.Angle(childModel.transform.up, Vector3.up) > flipAngle) ? true : false;
//        anim.SetBool(idFlipped, flipped);
//
//    }
//
//    void FixedUpdate()
//    {
//
//        //ȷ��thrustֻ��һ֡��Ч����
//        rigid.position += deltaPos;//rigid.postionֻ��fixedDeltaTime����ʱ���ã�
//        rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z) + thrust;
//        thrust = Vector3.zero;
//        deltaPos = Vector3.zero;
//        if (rigid.velocity.magnitude > 50f) ValarMorghulis();
//
//    }
//
//    public void SetRetainVeloTrue()
//    {
//        retainVelo = true;
//    }
//
//    public void StopMove()
//    {
//
//        planarVec = Vector3.zero;
//        rigid.velocity = Vector3.zero;
//
//    }
//
//
//    public bool CheckState(int layerIndex, string statename) //��ѯ�Ƿ���ǰ����ĳ����ĳ������״̬
//    {
//        bool result = anim.GetCurrentAnimatorStateInfo(layerIndex).IsName(statename);
//        //		if (result)
//        //			Debug.Log ("Matched:" + statename);
//        //		else
//        //			Debug.Log ("Unmatched:"+statename );
//        return result;
//
//    }
//
//    public void OnGroundEnter()
//    {
//        pi.retainAnim = false;
//        retainVelo = false;
//        canFire = true;
//        capsule.material = null;
//    }
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
//        pi.retainAnim = false;
//        retainVelo = false;
//        canFire = true;
//        capsule.material = null;
//    }
//
//    public void OnJumpEnter()//������Ծ����״̬��sendMessageUpwards���ô˷���
//    {
//
//        pi.retainAnim = true;//��������
//        retainVelo = true;  //�ٶȱ���
//        thrust = Vector3.up * jumpPower;
//        capsule.material = zeroFriction;
//        battleBot.DisableSensor();
//
//    }
//
//    public void OnJumpUpdate()
//    {
//        //ʹ��fallOffset��ֹ��ɫ��������
//        GetComponent<GroundChecker>().SetOffGround();
//        //    Debug.Log("jump update calls");
//    }
//
//    public void OnJumpExit()
//    {
//        battleBot.EnableSensor();
//    }
//
//    public void OnFallEnter()//��׹����ײ��ֱǽ��ˮƽ���ٶȱ������������޴�Force�ѽ�ɫѹ��ǽ��ճס��
//    { //�ų���������ǽlayer����Ϊground���߸���physics materialĦ����ϵ����
//        pi.retainAnim = true;//����ת�򣬿�ǹת����������
//        retainVelo = true;  //�����ٶ�
//        capsule.material = zeroFriction;
//        canFire = true;
//    }
//
//    public void OnFallExit()
//    {
//        soundController.PlaySound("Audio/Lily/FallStep");
//    }
//    
//    public void OnFireEnter()//Enter��Update�ĸ����ã���ǹ��ת����̫�׵�
//    {
//        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//        RaycastHit hit;
//        if (Physics.Raycast(ray, out hit, 100, ptlGun.portalMask) == false)
//        {
//            drFire = Camera.main.transform.forward;
//            drFire = new Vector3(drFire.x, 0f, drFire.z).normalized;
//            fireAngle = 0f;
//
//        }
//        else
//        {
//            newPortalPos = hit.point;
//            drFire = new Vector3(newPortalPos.x - transform.position.x, 0f, newPortalPos.z - transform.position.z);
//            fireAngle = Mathf.Atan2((newPortalPos.y - transform.position.y), drFire.magnitude); //0.8Ĭ���������ĸ߶�
//            fireAngle = Mathf.Clamp(fireAngle, minAngle, maxAngle);
//            drFire = drFire.normalized;
//
//        }
//
//    }
//
//    public void FireBullet()
//    {
//        bullet = GameObject.Instantiate(bulletPfb, bulletPos.position, bulletPos.rotation);
//        bullet.GetComponent<PortalBullet>().enabled = true;
//
//    }
//
//    public void OnFireUpdate() //OnAUpdate��OnAEnter��OnAExitʱ����ִ�У�OnBEnter����������ʱ���ڡ�
//    {
//        float weight = anim.GetLayerWeight(idCombatLayer);
//        weight = Mathf.Lerp(weight, 1f, 0.3f);
//        anim.SetLayerWeight(idCombatLayer, weight);//����combat��Ȩ��Ϊ1
//        childModel.transform.forward = Vector3.Slerp(childModel.transform.forward, drFire, 0.3f);//ʹ��ɫ����drFire
//        float curAngle = anim.GetFloat(idFireAngle);//���ÿ�ǹ�ظ����Ƕ�
//        curAngle = Mathf.Lerp(curAngle, fireAngle, 0.3f);
//        anim.SetFloat(idFireAngle, curAngle);//
//
//    }
//
//
//    public void OnCombatIdleUpdate()//����������OnAUpdate
//    {
//        float weight = anim.GetLayerWeight(idCombatLayer);
//        weight = Mathf.Lerp(weight, 0f, 0.3f);
//        //	Debug.Log ("weightUpdate=:" + weight);
//        anim.SetLayerWeight(idCombatLayer, weight);//
//
//    }
//
//    public void PlayerHitbyLaser()//hited by laser
//    {
//        // if (anim.GetBool("Hited") == false)
//        anim.SetBool("Hited", true);
//        UIManager._instance.DeactivateOperateUI();
//        //else    Debug.Log("hited is already true");
//
//    }
//
//    public void EvadeHit()// evade laser hit
//    {
//        if (CheckState(idHitedLayer, hitedState))
//        {
//            anim.SetBool("Hited", false);
//            canFire = true;
//        }
//    }
//
//    public void OnHitedEnter()
//    {
//        //thrust = -childModel.transform.forward * hitPower;        
//        canFire = false;
//        soundController.PlaySound("Audio/Lily/SkyrimHited2");
//        UIManager._instance.PlayerLaserHit();
//        UIManager._instance.PlayerLaseredSay();
//
//    }
//
//    public void OnHitedUpdate()
//    {
//        float weight = anim.GetLayerWeight(idHitedLayer);
//        weight = Mathf.Lerp(weight, 1f, 0.3f);
//        anim.SetLayerWeight(idHitedLayer, weight);
//    }
//
//    public void OnHitIdleEnter()
//    {
//        canFire = true;
//    }
//
//    public void OnHitedIdleUpdate()
//    {
//        float weight = anim.GetLayerWeight(idHitedLayer);
//        weight = Mathf.Lerp(weight, 0f, 0.3f);
//        anim.SetLayerWeight(idHitedLayer, weight);
//    }
//
//    public void HitByBullet()
//    {
//        //Hit�������ٴα����У�������ͷ���ţ�exit time����ѡ
//        anim.SetBool("BulletHit", true);
//        UIManager._instance.PlayerLaserHit();
//        UIManager._instance.DeactivateOperateUI();
//
//    }
//
//
//    public void OnBulletHitEnter()
//    {
//        // canFire = false;//permition to counter strike!
//        anim.SetBool("BulletHit", false);
//        soundController.PlaySound("Audio/Lily/SkyrimHited2");
//        UIManager._instance.PlayerHitedSay();
//    }
//
//    public void SetThrust(Vector3 impact)
//    {
//        thrust = impact;
//    }
//
//    public void SetPlanarVec(Vector3 planar)
//    {
//        if (planar != Vector3.zero) planarVec = planar;
//    }
//
//    public void SetHealedTrue()
//    {
//        anim.SetBool("Healed", true);
//
//    }
//
//    public void SetHealedFalse()
//    {
//        anim.SetBool("Healed", false);
//    }
//
//    public void OnHealedEnter()
//    {
//        //Ӧ����ֹͣ
//        pi.retainAnim = true;
//        retainVelo = false;
//        canFire = false;//ʩ���в��ܿ�ǹ
//    }
//
//    public void OnHealedUpdate()
//    {
//        Vector3 FWD = -EventManager._instance.healer.GetComponent<InfantryController>().childModel.transform.forward;
//        childModel.transform.forward = Vector3.Slerp(childModel.transform.forward, FWD, 0.2f);
//    }
//
//    public void SetChatTrue()
//    {
//        anim.SetBool("Chat", true);
//    }
//
//    public void SetChatFalse()
//    {
//        anim.SetBool("Chat", false);
//    }
//
//    public void OnChatEnter()
//    {
//        pi.retainAnim = true;
//        retainVelo = false;
//        canFire = false;//ʩ���в��ܿ�ǹ
//        UIManager._instance.StartPlayerChat(UIManager._instance.chatPos.name);
//        //    SetChatFalse();
//    }
//
//    public void OnChatUpdate()
//    {
//        Vector3 FWD = UIManager._instance.chatPos.transform.position - this.transform.position;
//        FWD = new Vector3(FWD.x, 0f, FWD.z).normalized;
//        childModel.transform.forward = Vector3.Slerp(childModel.transform.forward, FWD, 0.2f);
//    }
//
//    public void ValarMorghulis()
//    {
//        if (alive)
//        {
//            alive = false;
//            anim.SetBool("Death", true);
//            pi.retainAnim = true;
//            retainVelo = false;
//            canFire = false;//
//            battle.DisableSensor();
//            battleBot.DisableSensor();
//            soundController.PlaySound("Audio/Lily/ForTheHorde");
//            Camera.main.GetComponent<Drunk>().enabled = true;
//            EventManager._instance.DisableCombatUI();
//            UIManager._instance.PlayerDeathSay();
//            UIManager._instance.EnableDefeatedUI();
//        }
//        else Debug.Log("One can only die once, doesn't it?");     
//        
//    }
//
//    public void OnUpdateRM(object _deltaPos)
//    {
//        if (CheckState(idMotionLayer, jumpState) || CheckState(idMotionLayer, healState))
//        {
//            //��FixedUpdate�е���һ�μ����㣬���Դ˴�������ʼ���ǵ�������ʱ�䲽��λ�ơ�
//            deltaPos += 0.2f * deltaPos + (Vector3)_deltaPos * 0.8f;            
//            //Debug.Log ("in jump state");
//        }
//    }
//
//
//    public void GetCaught()
//    {
//        // Debug.Log("get caught");
//        Camera.main.GetComponent<ThirdPersonCamera>().enabled = false;
//        Camera.main.GetComponent<FollowSimi>().enabled = true;
//        battle.DisableSensor();
//        rigid.isKinematic = true;
//        GetComponent<FollowSimi>().enabled = true;
//        anim.SetTrigger("Caught");
//        canPortal = false;
//
//    }
//
//
//    public void OnCaughtEnter() //��Boss��ս������
//    {
//
//        retainVelo = false;
//        pi.retainAnim = true;
//        canFire = false;
//        //�����໥������,������ײ�嵼��boss�ܷ�����������ת�����ر���toss�ڼ�player���ܴ�ǽ
//        rigid.isKinematic = true;
//        UIManager._instance.PlayerMeleeHit();
//        UIManager._instance.PlayerCaughtSay();
//        soundController.PlaySound("Audio/Lily/SkyrimHited1");
//
//    }
//
//    public void TriggerTossed()
//    {
//        canPortal = true;
//        Camera.main.GetComponent<ThirdPersonCamera>().enabled = true;
//        Camera.main.GetComponent<FollowSimi>().enabled = false;
//        GetComponent<FollowSimi>().enabled = false;
//        transform.position = GetComponent<FollowSimi>().target.transform.position;
//        transform.rotation = Quaternion.LookRotation(-bossWM.transform.forward, Vector3.up);
//        cameraPos.localPosition = new Vector3(0f, 0f, -2.5f);
//        cameraPos.parent.localEulerAngles = new Vector3(20f, 0f, 0f);
//        cameraPos.parent.position = Camera.main.transform.TransformPoint(0f, 0f, 2.5f);
//        childModel.transform.up = Vector3.up;
//        childModel.transform.forward = transform.forward;
//        anim.SetTrigger("Tossed");
//        // Debug.Log("Reset Lily to  toss her correctly.");
//    }
//
//    public void OnTossedEnter()
//
//    {
//        anim.ResetTrigger("Caught");
//        anim.ResetTrigger("Tossed");
//        rigid.isKinematic = false;
//        bossWM.SetTossFalse();
//        retainVelo = true;
//        pi.retainAnim = true;
//        canFire = false;
//        UIManager._instance.PlayerTossedHit();
//        UIManager._instance.PlayerTossedSay();
//        sound = moan[Random.Range(0, moan.Length)];
//        voice.PlayOneShot(sound);
//        planarVec = bossWM.transform.forward * tossPlaneSpeed;//ÿ֡10f����λ�ƴ��棬��δ����caught�� 
//        thrust = Vector3.up * tossYSpeed;
//    }
//
//    public void OnTossedUpdate()
//    {
//        Vector3 orginPos = new Vector3(0f, 0.7f, 0f);
//        cameraPos.parent.localPosition = Vector3.Slerp(cameraPos.parent.localPosition, orginPos, 0.3f);
//
//    }
//
//    public void HitByImpact()
//    {
//        anim.SetTrigger("Impact");
//    }
//
//    public void OnImpactEnter()
//    {
//        anim.ResetTrigger("Impact");
//        retainVelo = true;
//        pi.retainAnim = true;
//        // canFire = false;
//        UIManager._instance.PlayerTossedHit();
//        sound = moan[Random.Range(0, moan.Length)];
//        voice.PlayOneShot(sound);
//
//    }
//
//    public void OnImpactUpdate()
//    {
//        GetComponent<GroundChecker>().SetOffGround();
//    }
//
//
//    public void OnFlipEnter()
//    {
//        canFire = false;
//        anim.SetBool("Flipped", false);
//        Vector3 trgtRight = childModel.transform.up;
//        trgtRight = new Vector3(-trgtRight.z, 0f, trgtRight.x).normalized;
//        float angleX = Vector3.Angle(childModel.transform.up, Vector3.up);
//        float angleY = Vector3.Angle(trgtRight, childModel.transform.right);
//        Vector3 crossX = Vector3.Cross(childModel.transform.up, Vector3.up);
//        Vector3 crossY = Vector3.Cross(childModel.transform.right, trgtRight);
//        if (Vector3.Dot(childModel.transform.up, crossY) < 0) angleY = -angleY;
//        childModel.transform.Rotate(0f, angleY, 0f);
//        if (Vector3.Dot(childModel.transform.right, crossX) < 0) angleX = -angleX;
//        childModel.transform.Rotate(angleX, 0f, 0f);
//
//    }
//
//    public void OnFlipUpdate()
//    {
//        Vector3 velocity = Vector3.zero;
//        planarVec = Vector3.SmoothDamp(planarVec, Vector3.zero, ref velocity, flipDamper);
//    }
//
//    public void OnGetUpEnter()
//    {
//        //  retainVelo = false; //��Update��ʹ��smoothdamp����  
//        anim.ResetTrigger("Tossed");
//        pi.retainAnim = true;
//        canFire = true;//��������
//        battle.EnableSensor();
//        capsule.enabled = true;
//
//    }
//
//    public void OnGetUpUpdate()
//    {
//        // planarVec = Vector3.Slerp(planarVec, Vector3.zero, 0.1f);
//        Vector3 velocity = Vector3.zero;
//        planarVec = Vector3.SmoothDamp(planarVec, Vector3.zero, ref velocity, tossDamper);
//
//    }
//
//    public void ShotByFireBall()
//    {
//        //canFire = false;
//        explode.gameObject.SetActive(true);
//        Invoke("DeactiveExplode", 1f);
//    }
//
//    public void DeactiveExplode()
//    {
//        explode.gameObject.SetActive(false);
//    }
//
//    public void OnOperateEnter()
//    {
//        anim.SetBool(idOperate, false);
//        pi.retainAnim = true;
//        retainVelo = false;
//        canFire = false;
//        canOperate = false;
//        soundController.PlaySound("Audio/Operator/Operating");
//        EventManager._instance.HideHint();
//        //  EventManager._instance.EnableProgress();
//        UIManager._instance.ActivateOperateUI();
//        UIManager._instance.PlayerOperateSay();
//        sound = operating[Random.Range(0, operating.Length)];
//        voice.PlayOneShot(sound);
//    }
//
//    public void OnOperateExit()//��������ǰ����
//    {
//        UIManager._instance.DeactivateOperateUI();
//    }
//
//    public void SetOperateTrue()
//    {
//        canOperate = true;
//        //  Debug.Log("set true operate");
//    }
//
//    public void SetOperateFalse()
//    {
//        canOperate = false;
//        //  Debug.Log("set false operate");
//    }
//
//    public void SetMeshTrue()
//    {
//        girlGun.enabled = true;
//        girlMesh.enabled = true;
//        if (attachCube != null) attachCube.SetMeshTrue();
//        else Debug.Log("null attachable: " + this.name);
//
//    }
//    public void SetMeshFalse()
//    {
//        girlGun.enabled = false;
//        girlMesh.enabled = false;
//        if (attachCube != null) attachCube.SetMeshFalse();
//        else Debug.Log("null attachable: " + this.name);
//    }
//
//    public void StartGhostMode()
//    {
//        ethereal = true;
//        canPortal = false;
//        canFire = false;
//        girlGun.enabled = false;
//        girlMesh.material = ghost;
//        battle.DisableSensor();
//        battleBot.DisableSensor();
//    }
//
//    public void EndGhostMode()
//    {
//        ethereal = false;
//        canPortal = true;
//        canFire = true;
//        girlGun.enabled = true;
//        girlMesh.material = normal;
//        battle.EnableSensor();
//        battleBot.EnableSensor();
//    }
//
//    public void EnableCombatSensor()
//    {
//        battle.EnableSensor();
//    }
//
//    public void DisableCombatSensor()
//    {
//        battle.DisableSensor();
//    }
//
//    //collision with boss
//    private void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject == EventManager._instance.boss && EventManager._instance.victory == false
//            && ethereal==false)
//        {
//            Debug.Log("collision with boss: " + this.name);
//            SetThrust(Vector3.up * 3f);
//            anim.SetTrigger("Impact");
//            retainVelo = true;
//            SetPlanarVec(-transform.forward * 3f);
//        }
//    }
//
//
//}
