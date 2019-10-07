//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;
//
//public enum BotType
//{
//    Archer,
//    Infantry,
//    Turret,
//    Conjured,
//    Healer,
//    Deserter,
//    Harbinger
//}
//
//
//public class InfantryController: MonoBehaviour {
//
//    [Header("===Bool Parameters===")]
//    public BotType botType;
//    public bool retainAnim = false;
//    public bool retainVelo = false;
//    public bool flipped = false;
//    public bool inRange = false;
//  //  public bool arrived = false;
//    public bool flipDeath = false;
//    public bool chase = false;
//    public bool patrol = true;
//    public bool validPath = true;
//    [Header("===Player Parameters===")]
//    public GameObject target;
//    public Vector3 targetFWD;
//    public Vector3 vecToPyr;
//    public float altitude;
//    public float disToPyr;
//    public float maxVSqr=2f;
//    public float rayVFix = 0f;
//    public float spotRange = 11f;
//    public float shootRange = 6f;
//    [Header ("===Mecanim===")]
//    public float vertical;
//    public float walkVertical = 0.5f;
//    public float runVertical = 1f;
//    public float interval = 3f;
//    public float arriveSqr = 0.04f;
//    public float MoveSpeedMultiplier = 1f;
//    public float hitPower = 2f;
//    public float damper = 0.2f;
//    public float lifeTime = 30f;
//    public Vector3 thrust;
//    public PhysicMaterial deathMaterial;
//    public GameObject childModel;
//    [Header ("===Components===")]
//    [SerializeField] private Animator anim;
//    [SerializeField] private Rigidbody rigid;
//    [SerializeField] private CapsuleCollider caps;
//    [SerializeField] private BoxCollider box;
//    [SerializeField] private GroundChecker grdChecker;
//    [SerializeField] private NavMeshAgent nav;
//    [SerializeField] private GameObject bots;
//    [Header("===SoundSetting===")]
//    [SerializeField ]private AudioSource voice;
//    public AudioClip sound;
//    [Header("===CombatSetting===")]
//    public GameObject bulletPfb;
//    public GameObject bullet;
//    public GameObject bulletPos;
//
//    // Use this for initialization
//    void Start () {
//
//        bots = EventManager._instance.bots;
//        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
//        anim = childModel.GetComponent<Animator>();
//        voice = GetComponent<AudioSource>();
//        rigid = GetComponent<Rigidbody>();
//        caps = GetComponent<CapsuleCollider>();
//        box = GetComponent<BoxCollider>();
//        grdChecker = GetComponent<GroundChecker>();
//        if (target == null ||childModel==null||anim==null||rigid==null||voice==null
//            ||caps==null||grdChecker ==null||bulletPfb==null||bulletPos==null
//     ||deathMaterial==null || nav==null ||box==null||bots==null)
//            Debug.LogError("null for infantry from:"+this.name);
//        if (nav.speed == 0) Debug.LogError("Warning: zero speed of nav:"+this.name);
//        StartCoroutine(TurnInterval());
//        box.material = deathMaterial;
//        box.enabled = false;
//        if (botType == BotType.Conjured) Invoke("EndLife",lifeTime);
//
//	}
//
//    private void OnDrawGizmos()
//    {
//        Gizmos.color = Color.yellow;
//        Gizmos.DrawLine(transform.position + Vector3.up * rayVFix, transform.position+childModel.transform.forward*shootRange);
//    }
//
//    public void EndLife()
//    {
//        anim.SetInteger("HitPoint", 0);
//    }
//
//
//    // Update is called once per frame
//    void Update () {
//       
//        vecToPyr = target.transform.position - transform.position;
//        disToPyr = vecToPyr.magnitude;
//        altitude = vecToPyr.y;
//        vecToPyr = new Vector3(vecToPyr.x, 0f, vecToPyr.z);       
//
//        #region Chase
//        //追逐分支
//        if (retainAnim == false && disToPyr < spotRange && anim.GetBool("OnGround") )//侦测范围忽视高度差
//
//        {
//            // rigid.isKinematic = true;
//            nav.enabled = true;
//            nav.isStopped = false;
//            nav.updatePosition = false;
//            nav.updateRotation = false;
//            nav.SetDestination(target.transform.position);
//            nav.nextPosition = transform.position;//可能在下落时被卡在墙体里，而不能实现此语句？？
//
//            NavMeshPath path = new NavMeshPath();
//            if (NavMesh.CalculatePath(transform.position, nav.destination, NavMesh.AllAreas, path))
//            {
//                if (path.status == NavMeshPathStatus.PathComplete) validPath = true;
//                else validPath = false;
//                if (validPath) ChasePlayer();
//                else OnPatrol();
//
//            }
//            else
//            {
//                Debug.Log("Warning CalculatePath Fail, Stucked in Walls Like:"+this.name);
//                OnPatrol();
//            }
//            #region oldArrive
//            //vertical = runVertical;
//            //if (grdChecker.blocked) arrived = false;
//            //else
//            //{
//            //   targetFWD = vecToPyr.normalized;
//            //}
//            //if (arrived==false)
//            //{
//            //    targetFWD = (grdChecker.tempPoint - transform.position).normalized;
//            //    inRange = false;
//            //    if ((grdChecker.tempPoint - transform.position).sqrMagnitude < arriveSqr) arrived = true;
//            //}                    
//            //childModel.transform.forward = Vector3.Slerp(childModel.transform.forward, targetFWD, 0.3f);
//            #endregion
//
//        }
//        #endregion
//
//        #region Patrol
//        else if (retainAnim == false && retainVelo == false && anim.GetBool("OnGround"))//巡逻分支去掉else
//        {
//            OnPatrol();
//
//        }
//        #endregion
//        else//such as getup state
//        {
//           // Debug.Log("state not patrol or chase disable nav:" + this.name);
//            chase = false;
//            patrol = false;
//            nav.enabled = false;
//        }
//
//        #region InRangeCheck
//        //inRange应只判定是否攻击，不改变方向，方向判定由arrived决定
//        if (disToPyr < shootRange && altitude * altitude < maxVSqr && validPath)//进入射程判断是否射击
//        {
//            //  patrol = false;
//            //  chase = true;
//            Ray ray = new Ray(transform.position + Vector3.up * rayVFix, childModel.transform.forward);
//            RaycastHit hit;
//            if (Physics.Raycast(ray, out hit, shootRange,
//                LayerMask.GetMask("Ground", "SimiGround", "Default", "Player")))
//            {
//                if (hit.collider.gameObject == EventManager._instance.curtPlayer) inRange = true;
//                else
//                {
//                    inRange = false;
//                  //  Debug.Log("out range due to ray miss player:"+this.name);
//                }
//            }
//            else
//            {
//              //  Debug.Log("out range due to ray hit nothing:"+this.name);
//                inRange = false;
//            }
//        }
//        else
//        {
//            inRange = false;
//          //  Debug.Log("out range due to distance,pathinvalid or altitude:"+this.name);
//        }
//        #endregion
//        
//        if (anim.GetBool("Hited") == true) anim.SetBool("InRange", false);
//        else anim.SetBool("InRange", inRange);
//        anim.SetFloat("Vertical", vertical);
//        flipped = (Vector3.Angle(childModel.transform.up, Vector3.up) > 45f) ? true : false;        
//        anim.SetBool("Flipped", flipped);
//        anim.SetFloat("Distance", disToPyr);
//
//    }
//
//    private void FixedUpdate()
//    {
//        rigid.velocity += thrust;
//        thrust = Vector3.zero;
//    }
//
//    public void ReassignTarget(GameObject temp)
//    {
//        target = temp;
//    }
//
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
//    public void OnPatrol()
//    {
//        patrol = true;
//        chase = false;
//        nav.enabled = false;
//        vertical = walkVertical;
//        if (grdChecker.blocked) targetFWD = childModel.transform.right;//一直打转？
//        childModel.transform.forward = Vector3.Slerp(childModel.transform.forward, targetFWD, 0.3f);
//
//    }
//
//
//    public void ChasePlayer()
//    {
//        chase = true;
//        patrol = false;
//        targetFWD = nav.desiredVelocity;
//        targetFWD = new Vector3(targetFWD.x, 0f, targetFWD.z).normalized;
//        vertical = runVertical;
//        childModel.transform.forward = Vector3.Slerp(childModel.transform.forward, targetFWD, 0.3f);
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
//        //nav.enabled=false;//卡在墙体里
//        //sound = fallSound[Random.Range(0, fallSound.Length)];
//        //voice.PlayOneShot(sound);
//    
//    }
//
// 
//
//    public void OnGetUpEnter()
//    {
//      
//        retainAnim = true;
//        retainVelo = true;
//
//        switch (botType)
//        {
//            case BotType.Archer:
//                sound = SoundResources._instance.botGetUp[0];
//                break;
//            case BotType.Infantry:
//                sound = SoundResources._instance.botGetUp[0];
//                break;
//            case BotType.Turret:
//                sound = SoundResources._instance.botGetUp[0];
//                break;
//            case BotType.Healer:
//                sound = SoundResources._instance.npcGetUp;
//                break;
//            case BotType.Deserter:
//                sound = SoundResources._instance.npcGetUp;
//                break;
//            case BotType.Harbinger:
//                sound = SoundResources._instance.npcGetUp;
//                break;
//
//        }
//
//        voice.PlayOneShot(sound);
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
//      //  if(anim.GetInteger("HitPoint")==0) caps.material = deathMaterial;
//
//    }
//
//    public void OnGetUpdate()
//    {
//        inRange = false;
//      //  arrived = true;
//        Vector3 velocity = Vector3.zero;
//        rigid.velocity = Vector3.SmoothDamp(rigid.velocity, Vector3.zero, ref velocity, damper);
//    }
//
//    public void OnGroundEnter()
//    {
//        retainAnim = false;
//        retainVelo = false; 
//
//    }
//
//    public void FireBullet()
//    {
//        
//        switch (botType)
//        {
//            case BotType.Archer:
//                sound = SoundResources._instance.archerAttack;
//                break;
//            case BotType.Infantry:
//                sound = SoundResources._instance.infantryAttack;
//                break;
//            case BotType.Turret:
//                sound = SoundResources._instance.turretAttack;
//                break;
//            case BotType.Conjured:
//                sound= SoundResources._instance.infantryAttack;
//                break;
//        }
//        voice.PlayOneShot(sound);
//        bullet = GameObject.Instantiate(bulletPfb, bulletPos.transform.position, bulletPos.transform.rotation);
//        bullet.GetComponent<ArcherBullet>().enabled = true;
//        bullet.GetComponent<ArcherBullet>().SetSource(childModel);
//
//    }
//
//    public void HitByLaser()
//    {
//      //  if(CheckState ()==false)//解决死亡时hit状态
//        anim.SetBool("Hited", true);
//     //   Debug.Log("I am hit by laser:" + this.name);
//    }
//
//    public void HitByBullet()
//    {
//        anim.SetBool("Hited", true);
//      //  Debug.Log("hit by friendly bullet:"+this.name);
//
//    }
//
//    public void HitByImpact()
//    {
//        anim.SetBool("Hited", true);
//     //   Debug.Log("hit by friendly bullet:" + this.name);
//
//    }
//
//    public void OnHitedEnter()
//
//    {
//        //   Debug.Log("enter hited archer");
//        anim.SetBool("Hited", false);
//        anim.SetInteger("HitPoint", anim.GetInteger("HitPoint") - 1);
//        sound = SoundResources._instance.botHited[Random.Range(0, SoundResources._instance.botHited.Length)];
//        voice.Stop();
//        voice.PlayOneShot(sound);
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
//        //inRange = false;//不起作用
//        //anim.SetBool("InRange", false);
//        //Debug.Log("set inrange false:" + this.name);
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
//    public void OnTeleportEnter()
//    {
//        AudioClip clip = (AudioClip)Resources.Load("Audio/Bots/HouTelp");
//        if (clip == null) Debug.LogError("No clip found:" + this.name);
//        else voice.PlayOneShot(clip);
//    }
//
//    public void OnDeathEnter()
//    {
//        retainAnim = true;
//        retainVelo = false;
//        box.enabled = true;
//        caps.enabled = false;
//        childModel.SendMessage("DisableSensor",SendMessageOptions.DontRequireReceiver);
//        SendMessage("TurretDefeat",SendMessageOptions.DontRequireReceiver);
//        
//        switch (botType)
//        {
//            case BotType.Archer:
//                sound = SoundResources._instance.archerEnd[Random.Range(0,
//                    SoundResources._instance.archerEnd.Length )];
//                break;
//            case BotType.Infantry:
//                sound = SoundResources._instance.infantryEnd[Random.Range(0,
//                    SoundResources._instance.infantryEnd.Length)];
//                break;
//            case BotType.Turret:
//                sound = SoundResources._instance.turretEnd[Random.Range(0,
//                    SoundResources._instance.turretEnd.Length)];
//                break;
//            case BotType.Conjured:
//                sound = SoundResources._instance.conjuredEnd[Random.Range(0,
//                    SoundResources._instance.conjuredEnd.Length)];
//                Invoke("DeactivateSelf", 2f);
//                break;
//
//        }
//        voice.Stop();
//        voice.PlayOneShot(sound);
//    }
//
//    public void OnSwingExit()
//    {
//        EventManager._instance.boss.GetComponent<BossController>().StopDirector();
//        rigid.useGravity = true;
//        transform.parent = null;
//    }
//    public void DeactivateSelf()
//    {
//        this.gameObject.SetActive(false);
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
//    public void SetThrust(Vector3 impact)
//    {
//        thrust = impact;
//    }
//
//    public void SetRetainVeloTrue()
//    {
//        retainVelo = true;
//    }
//
//    public void SetPlanarVec(Vector3 planar)
//    {
//        if (planar != Vector3.zero)
//        {
//            rigid.velocity = new Vector3(planar.x, rigid.velocity.y, planar.z);
//        }
//    }
//
//    public void SetFlipDeathTrue()
//    {
//        flipDeath = true;
//        caps.material = deathMaterial;
//
//    }
//
//    public void TakePortal(GameObject portalPt)
//    {
//        OffGround();//不能本帧马上执行完毕？
//        retainAnim = true;
//        retainVelo = true;
//        float speed = GetComponent<GroundChecker>().fallSpeed;
//        speed = Mathf.Clamp(speed, grdChecker.minSpeed, grdChecker.maxSpeed);
//       // Debug.Log("speed=:" + speed + "  and name is:" + this.name);
//        GameObject portalGo = portalPt.GetComponent<Portal>().portalGo;
//
//        rigid.isKinematic = true;//防止set parent过程中，因rigid受力特性，不能完全执行rotation与position
//        transform.parent = portalPt.transform;
//        Quaternion worldOrginRote = transform.rotation;
//        Quaternion childPrginRote = childModel.transform.rotation;
//        Quaternion localEnterRote = transform.localRotation;//出门信息暂且与入门一致
//        transform.parent = portalGo.transform;
//        transform.localPosition = new Vector3(0, 0, grdChecker.stepPortal);//step portal
//        transform.localRotation = localEnterRote;
//        Vector3 worldExitPos = transform.position;//最终出门位置                                                                
//        portalGo.transform.forward *= -1;//翻转出门前后
//        Quaternion worldExitRote = childModel.transform.rotation;//最终出门朝向  
//
//        if (botType == BotType.Conjured) transform.parent = null;
//        else transform.parent = bots.transform;//脱离parent
//        transform.position = worldExitPos;
//        transform.rotation = worldOrginRote;
//        //死亡后的portal不再进行childmodel翻转
//        if (flipDeath || anim.GetInteger("HitPoint") == 0) childModel.transform.rotation = childPrginRote;
//        else childModel.transform.rotation = worldExitRote;
//        rigid.isKinematic = false;//死亡后不解除kinematic，无出portal速度
//        portalGo.transform.forward *= -1;  //翻转回复
//        rigid.velocity = portalGo.transform.forward * speed;//设定速度
//        if (!flipDeath && anim.GetInteger("HitPoint") != 0)
//        {
//            switch (botType)
//            {
//                case BotType.Archer:
//                    sound = SoundResources._instance.portalBot;
//                    break;
//                case BotType.Infantry:
//                    sound = SoundResources._instance.portalBot;
//                    break;
//                case BotType.Turret:
//                    sound = SoundResources._instance.portalBot;
//                    break;
//                case BotType.Conjured:
//                    sound = SoundResources._instance.portalConjured;
//                    break;
//                case BotType.Healer:
//                    sound = SoundResources._instance.portalNPC;
//                    break;
//                case BotType.Deserter:
//                    sound = SoundResources._instance.portalNPC;
//                    break;
//                case BotType.Harbinger:
//                    sound = SoundResources._instance.portalNPC;
//                    break;
//            }
//            voice.Stop();
//            voice.PlayOneShot(sound);
//            SendMessage("EnterPortalSay", SendMessageOptions.DontRequireReceiver);            
//        }    
//
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
