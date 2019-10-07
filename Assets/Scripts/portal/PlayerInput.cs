using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class PlayerInput : MonoBehaviour {

    //private ConfigCtrl
    //InitCtrl(read bin, derserialize)??
    //private Animator playerAnim;
	[Header ("===Variables===")]
    public float drUp;
    public float drRight;
	private float drUpVelocity;
	private float drRightVelocity;
	private float targetDrUp;
	private float targetDrRight;
	public bool retainAnim=false;
	public float drMag;
	public Vector3 drVec;
	[Header("===ActionBool===")]
	public bool run = false ;
	public bool jump = false;
	public bool lastJump =false;
	public bool openFire = false;
    public bool operate = false;
	
    void Start()
    {
        //playerAnim = GamingManager.Instance.PlayerAnim;
    }

	// Update is called once per frame
	void Update () {
		
		if (!retainAnim ) { //软禁用操控
            targetDrUp = (Input.GetKey (ConfigControll.Instance.keyUp) ? 1.0f : 0f) - (Input.GetKey (ConfigControll.Instance.keyDown) ? 1.0f : 0f);
            targetDrRight = (Input.GetKey (ConfigControll.Instance.keyRight) ? 1.0f : 0f) - (Input.GetKey (ConfigControll.Instance.keyLeft) ? 1.0f : 0f);
	
		} else {
			targetDrUp = 0f;
			targetDrRight =0f;
		}

		drUp = Mathf.SmoothDamp (drUp, targetDrUp, ref drUpVelocity, 0.1f); //平滑过渡
		drRight = Mathf.SmoothDamp (drRight, targetDrRight, ref drRightVelocity, 0.1f);
		Vector2 circle = SquareToCircle (drRight, drUp);//化方为圆，解决左/右前方速度合成为1.414的问题
		drMag = circle.magnitude;	
        drVec = GamingManager.Instance.PlayerRoot.forward * drUp + GamingManager.Instance.PlayerRoot.right * drRight;//此处的transform是model的父object,VS Vector3.up ?方便摄像机？

//        float vtc = playerAnim.GetFloat(CommonAnim.Vertical); 
//        vtc = Mathf.Lerp(vtc, drMag, 0.5f);
//        playerAnim.SetFloat(CommonAnim.Vertical, vtc);
//        playerAnim.transform.forward = Vector3.Slerp(playerAnim.transform.forward, drVec, 0.3f);

        if (Input.GetKeyUp (ConfigControll.Instance.keyRun))
        { //释放按键时
			run = !run;
		}		
        jump = Input.GetKeyDown (ConfigControll.Instance.keyJump);//按下jump键为true，效率低于下面的手写代码？
		openFire =Input.GetKeyDown (KeyCode.Mouse0);
        operate = Input.GetKeyDown(ConfigControll.Instance.keyOperate);
			//跳跃的判定，浮空
//		bool newJump=Input.GetKey (keyJump );
//		if(newJump !=lastJump && newJump==true  )
//		{
//			jump = true;
//		}
//		else
//		{                            //利用trigger判断，松开jump，即复位，取消下落动画？正确播放连按两次的效果？
//			jump = false  ; //仅在按下Jump的那一帧为true，与GetKeyDown()的区别？存储按下的时间，反馈跳跃高度？JumpPower
//		}
//		lastJump = newJump;	
	}

	Vector2  SquareToCircle( float x,  float  y) //不断迭代？按住后再不同帧迭代？
	{
		Vector2 circle = Vector2.zero;
		 circle.x = x * Mathf.Sqrt (1-0.5f*y*y);
		 circle.y = y * Mathf.Sqrt (1-0.5f*x*x);
		 return circle;
	}



}
