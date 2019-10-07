//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class FootManFSM : StateMachineBehaviour {
//
//	public string[] onEnterMessages;
//	public string[] onUpdateMessages;
//	public string[] onExitMessages;
//
//	// Use this for initialization
//	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
//
//		foreach (var mesg in onEnterMessages) {			
//			animator.gameObject.SendMessage (mesg );
//			//		animator.gameObject.SendMessageUpwards (mesg ,SendMessageOptions.RequireReceiver ); //增加不存在mesg方法的提示语句？
//		}
//	}
//
//	//	 OnStateExit is called when a transition ends and the state machine finishes evaluating this state
//	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
//		foreach (var mesg in onExitMessages) 
//		{			
//			animator.gameObject.SendMessage (mesg);
//		}
//
//	}
//
//	//OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
//	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
//
//		foreach (var mesg in onUpdateMessages) 
//		{
//			animator.gameObject.SendMessage(mesg);
//		}
//	}
//}
