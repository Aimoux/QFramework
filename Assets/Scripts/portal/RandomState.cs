//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class RandomState : MonoBehaviour {
//
//    private Animator anim;
//    private WeaponManager wm;
//    public GameObject rangePfb;
//    public float orginScale = 0.2f;
//    [Header("===0: Laser, 1~3: Melee, 4: Teleport===")]
//    public int[] indexMelee = { 1, 4 };
//    public int[] indexRange = { 0, 5 };
//    private int index=1;
//    private int lastIndex=1;
//
//	// Use this for initialization
//	void Start () {
//
//        anim = GetComponent<Animator>();
//        wm = GetComponent<WeaponManager>();
//        if (anim == null || wm == null || rangePfb == null)
//            Debug.LogError("null for random");
//		
//	}
//
//    public void RandomMelee()//问题出在由idle转向attack abc的transit
//    {
//        //   Debug.Log("time to random Melee attack");
//        if (indexMelee[0] < 1 || indexMelee[1] > 3) Debug.LogError("Index range exceed: " + this.name);
//        if (indexMelee[0] == indexMelee[1]) Debug.LogError("Can not loop out!!! " + this.name);
//        while (index==lastIndex )
//        {
//            index = Random.Range(indexMelee[0], indexMelee[1]+1);
//            
//        }        
//        anim.SetInteger("RandomIndex", index);
//        lastIndex = index;  
//    }
//
//    public void RandomRange()
//    {
//        //   Debug.Log("time to random range");
//        if (indexRange[0] != 0) Debug.LogWarning("Warning: laser not include: " + this.name);
//        if (indexRange[1] > 4) Debug.LogError("Index range exceed: " + this.name);
//        if (indexRange[0] == indexRange[1]) Debug.LogError("Can not loop out!!! " + this.name);
//        while (index == lastIndex)
//        {
//            index = Random.Range(indexRange[0], indexRange[1]+1);           
//        }
//        anim.SetInteger("RandomIndex", index);
//        lastIndex = index;
//        if (index == 0)
//        {
//            // wm.rangeWeapon.SetActive(false);
//            // wm.meleeWeapon.SetActive(false);
//            anim.SetBool("Laser", true);
//            wm.laserBall.SetActive(true);
//            
//        }
//        else if(index==4)
//        {
//            //wm.rangeWeapon.SetActive(false);
//            //wm.meleeWeapon.SetActive(true);
//            
//        }
//        else
//        {
//            wm.rangeWeapon = Object.Instantiate(rangePfb, wm.rangePos.position, wm.rangePos.rotation, wm.rangePos);
//            wm.rangeWeapon.GetComponent<Rigidbody>().velocity = Vector3.zero;
//            wm.rangeWeapon.transform.localScale = orginScale * new Vector3(1f, 1f, 1f);
//            wm.rangeWeapon.GetComponent<FireBall>().enabled = true;
//        }
//              
//
//    }
//
//}
