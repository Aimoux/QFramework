//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class PopChatter : MonoBehaviour {
//
////    [SerializeField] private HUDText hud;
//    [SerializeField] private UILabel popText;
//    [SerializeField] private UIFollowTarget follow;
////    public string chatter;
//    public bool reached = false;
//    public float alphaSpeed = 1f;
//    public float riseSpeed = 5f;
//    public float orginLocalY=25f;
//    public float endLocalY=90f;
//    private Vector3 deltaPos;
//
//	// Use this for initialization
//	void Start () {
//
//        popText = GetComponent<UILabel>();
//        follow = transform.parent.GetComponent<UIFollowTarget>();
//        orginLocalY = transform.localPosition.y;
//        if (popText==null||follow==null)
//            Debug.LogError("null for chatter");
//        
//
//    }
//	
//	// Update is called once per frame
//	void Update () {
//
//        transform.Translate(Rise());
//       // Debug.Log("current alpha=:" + popText.alpha);
//        if(reached) TweenAlpha();
//
//    }
//
//    public void PopChat(string chatter)
//    {
//        ResetChat();
//        popText.text = chatter;
//
//    }   
//
//    public void ResetChat()
//    {
//        popText.text = "";
//        popText.alpha = 1f;
//        transform.localPosition = orginLocalY*Vector3.up;
//
//    }
//
//    public void TweenAlpha()
//    {
//
//        popText.alpha -= Time.deltaTime*alphaSpeed;
//        popText.alpha = Mathf.Clamp(popText.alpha, 0f, 1f);
//        //   else Debug.Log("Not reach end Y yet."+this.name);
//
//    }
//
//    public Vector3 Rise()
//    {
//        if (transform.localPosition.y < endLocalY)
//        {
//            deltaPos = Vector3.up * riseSpeed * Time.deltaTime;
//            reached = false;
//            return deltaPos;
//        }
//        else
//        {
//            reached = true;
//            return Vector3.zero;
//        }
//           
//    }
//
//    public Vector3 Descend()
//    {
//        if (transform.localPosition.y > orginLocalY)
//        {
//            deltaPos = -Vector3.up * riseSpeed * Time.deltaTime;
//            return deltaPos;
//        }
//        else
//            return Vector3.zero;
//
//    }
//
//
//
//}
