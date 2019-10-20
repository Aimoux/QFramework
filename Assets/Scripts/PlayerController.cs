using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class PlayerController : MonoBehaviour
{
    public PlayerInput pipt;
    private Animator anim;

    void Start()
    {
        anim = GamingManager.Instance.PlayerAnim;
        anim.SetInteger(Common.Const.StateID, (int)Common.ANIMATIONSTATE.WALK);
    }

    // Update is called once per frame
    void Update()
    {

        float vtc = anim.GetFloat(Const.Vertical); 
        vtc = Mathf.Lerp(vtc, pipt.drMag, 0.5f);
        anim.SetFloat(Const.Vertical, vtc);
        anim.transform.forward = Vector3.Slerp(anim.transform.forward, pipt.drVec, 0.3f);  
    }


    void FixedUpdate()
    {
        Vector3 dp = GamingManager.Instance.PlayerAnim.deltaPosition;
        //Debug.Log("delta pos = :" + dp.ToString("f4"));
        GamingManager.Instance.PlayerRoot.position += new Vector3(dp.x, 0, dp.z);       
        //Debug.Log("cur pos =: " + GamingManager.Instance.PlayerRoot.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (GamingManager.Instance != null)
            Gizmos.DrawLine(transform.position, transform.position + GamingManager.Instance.PlayerRoot.forward * 5f);
    }

}
