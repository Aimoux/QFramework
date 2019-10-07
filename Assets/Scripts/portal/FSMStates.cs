//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class FSMStates : StateMachineBehaviour {
//	public  string[] onEnterMessages;
//	public  string[] onExitMessages;
//	public string[] OnUpdateMessages;
//	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
//	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
//
//		foreach (var mesg in onEnterMessages) {			
//			animator.gameObject.SendMessageUpwards (mesg );
//	//		animator.gameObject.SendMessageUpwards (mesg ,SendMessageOptions.RequireReceiver ); //增加不存在mesg方法的提示语句？
//		}
//		}
//
////	 OnStateExit is called when a transition ends and the state machine finishes evaluating this state
//	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
//		foreach (var mesg in onExitMessages) 
//		{			
//			animator.gameObject.SendMessageUpwards (mesg);
//		}
//
//	}
//
//	 //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
//	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
//	
//		foreach (var mesg in OnUpdateMessages) 
//		{
//			animator.gameObject.SendMessageUpwards (mesg);
//		}
//	}
//
//	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
//	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
//	//
//	//}
//
//	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
//	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
//	//
//	//}
//}
