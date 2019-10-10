using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using QF;

public class WeaponManager : MonoSingleton<WeaponManager>
{
    protected WeaponManager()
    {


    }

    //武器制造、分配
    public Weapon CreateWeapon(int id, int lv, Role owner)
    {
        Weapon wp = new Weapon(id, lv, owner);
        Weapons.Add(wp);
        return wp;
    }

    public void Release()
    {
        foreach (var wp in Weapons)
            wp.OnDestroy();

        Weapons.Clear();

    }


    public delegate void WeaponEvent();//有无必要??
    public WeaponEvent wpt;

    public float deltaTime = 1f;
    public List<Weapon> Weapons = new List<Weapon>();

    private void Update()
    {
        deltaTime -= Time.deltaTime;
        if (deltaTime <= 0)
        {
            foreach (var wp in Weapons)// null check??
            {
                wp.Update(deltaTime);

            }
            deltaTime = 1f;
        }



    }






}



//
//public class WeaponManager : MonoBehaviour {
//
//    [Header ("===Weapons===")]
//    public GameObject rangeWeapon;
//    public GameObject laserWeapon;
//    public GameObject meleeWeapon;
//    public GameObject meleeTrail;
//    public BossController  am;
//    public GrilGunController girlController;
//    public Transform rangePos;
//    private bool tossPlayer = false;
//    public bool TossPlayer
//    {
//        get { return tossPlayer; }
//    }
//    [Header("===FireBall Parameters===")]    
//    public float missileSpeed = 10f;
//    public float heightCorrection = 1f;
//    private FireBall fireBall;
//    [Header("===Laser ParaMeters===")]
//    public GameObject laserBall;
//    public Transform laserPos;
//    public LaserDirectionCorrect laserCorrect;
//    public float slerper=0.3f;
//    [Header("===Sound ParaMeters===")]
//    public BossSoundController bossSound;
//
//	// Use this for initialization
//	void Start () {
//
//        am = EventManager._instance.boss.GetComponent<BossController>();
//        girlController = EventManager._instance.curtPlayer.GetComponent<GrilGunController>();
//        girlController.bossWM = this;
//        bossSound = GetComponent<BossSoundController>();
//        if (laserWeapon == null || meleeWeapon == null || laserBall==null 
//            || bossSound ==null||laserPos==null)
//            Debug.LogError("Null weapon!");
//    }
//
//    public void WeaponEnable()
//    {
//
//      //  Debug.Log("meleeweapon collider enable");
//        meleeWeapon.GetComponentInChildren<Collider>().enabled = true;
//        bossSound.PlaySwordSound();
//        meleeTrail.SetActive(true);
//        //UIManager._instance.BossAttackSay();
//    }
//
//    public void WeaponDisable()
//    {          
//       meleeWeapon.GetComponentInChildren<Collider>().enabled = false;
//       /// meleeTrail.SetActive(false);
//    }
//
//    public void SetTossTrue()
//    {
//       // Debug.Log("time to toss release:"+Time.time);
//        tossPlayer = true;
//
//    }
//
//    public void SetTossFalse()
//    {
//        tossPlayer = false;
//    }
//
//
//    public void FireAway()
//    {
//  //      Debug.Log("time to fly ball");
//        if(rangeWeapon !=null)
//        {
//            rangeWeapon.transform.parent = null;
//            //瞄准高度修正，防止过远时，未到达player，就过度膨胀到触地
//            Vector3 fireDir = EventManager._instance.curtPlayer.transform.position+
//                Vector3.up*heightCorrection  - rangeWeapon.transform.position;
//            rangeWeapon.GetComponent<Rigidbody>().isKinematic = false;
//            rangeWeapon.GetComponent<Rigidbody>().velocity = fireDir.normalized * missileSpeed;
//            rangeWeapon.GetComponent<Collider>().enabled = true;
//            fireBall = rangeWeapon.GetComponent<FireBall>();
//            fireBall.SetGrowBeginTrue();
//           // UIManager._instance.BossAttackSay();
//            
//        }    
//
//    }
//
//    public void LaserLaunch()
//    {
//
//        //Debug.Log("Time to launch and correct laser direction");
//        laserWeapon.SetActive(true);
//        laserBall.SetActive(true);
//        laserWeapon.transform.position = laserPos.position;
//        laserCorrect = laserWeapon.GetComponent<LaserDirectionCorrect>();
//        laserCorrect.correctBegin = true;
//        laserCorrect.slerper = slerper;
//        UIManager._instance.BossAttackSay();
//        if (rangeWeapon != null) Object.Destroy(rangeWeapon);
//
//    }
//
//    public void PowerSay()
//    {
//        UIManager._instance.BossPowerSay();
//    }
//
//    public void TearSay()
//    {
//        UIManager._instance.BossTearSay();
//    }
//
//    public void DeathSay()
//    {
//        UIManager._instance.BossDeathSay();
//        bossSound.PlaySound("Audio/Boss/Alduin");
//    }
//
//    public void Teleport()
//    {
//       
//        float orginH = transform.position.y;
//        Vector3 targetPos = girlController.transform.TransformPoint(0f,0f,5f);
//        am.transform.position = new Vector3(targetPos.x, orginH, targetPos.z);
//        bossSound.PlaySound("Audio/Boss/Thunder");
//        ActivateTeleportEffect();
//
//    }
//
//    public void ActivateTeleportEffect()
//    {
//        transform.Find("Teleport").gameObject.SetActive(true);
//        Invoke("DeactivateTeleportEffect", 1f);
//
//    }
//
//    public void DeactivateTeleportEffect()
//    {
//        transform.Find("Teleport").gameObject.SetActive(false);
//    }
//}
