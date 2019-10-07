using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionControl : MonoBehaviour
{
    private Animator anim;
    void Start()
    {
        anim = GetComponent <Animator >();
    }
	
    // Update is called once per frame
    void Update()
    {
        	//Debug.Log (anim.deltaPosition );
    }

    void OnAnimatorMove()
    {
        //SendMessageUpwards("OnUpdateRM", (object)anim.deltaPosition);
    }


}
