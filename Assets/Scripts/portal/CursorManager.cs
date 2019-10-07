// using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class CursorManager : MonoBehaviour {
//
//	public static CursorManager _instance;
//    public RectTransform aim_green;
//    public RectTransform aim_blue;
//   // public RectTransform cur_menu;
//    public Vector2 mousePosion;
//    public float cursorSize = 100f;
//    public bool isMenuOn = false;
//    // Use this for initialization
//    void Awake()
//	{
//
//		_instance = this;
//
//	}
//	void Start () {
//				
//       
//        if (aim_blue == null || aim_green == null )
//            Debug.LogError("null cursor");
//
//        aim_blue.sizeDelta = new Vector2(1f,1f)*cursorSize;
//        aim_green.sizeDelta = new Vector2(1f,1f)*cursorSize;
//        //   cur_menu.sizeDelta = new Vector2(1f, 1f) * cursorSize*0.3f;
//        Cursor.visible = false;
//    }
//
//    private void Update()
//    {
//        Cursor.visible = false;
//        if (isMenuOn==false) SetAimCursor();
//        else SetMenuCursor();
//
//    }
//
//    public void SetAimCursor()
//    {
//
//
//      //  cur_menu.gameObject.SetActive(false);
//        mousePosion = Input.mousePosition;
//        if (PortalGun.isFirstPortal)
//        {
//            aim_green.gameObject.SetActive(true);
//            aim_green.position = Input.mousePosition - (Vector3)(aim_green.sizeDelta / 2);
//            aim_blue.gameObject.SetActive(false);
//            
//        }
//
//        else
//        {
//            aim_blue.gameObject.SetActive(true);
//            aim_blue.position = Input.mousePosition - (Vector3)(aim_blue.sizeDelta / 2);
//            aim_green.gameObject.SetActive(false);
//        }
//
//    }
//
//    public void SetMenuCursor()
//    {
//  
//        aim_blue.gameObject.SetActive(false);
//        aim_green.gameObject.SetActive(false);
//        Cursor.visible = false;
//        //mousePosion = Input.mousePosition;
//        //cur_menu.gameObject.SetActive(true);
//        //cur_menu.position = mousePosion;      
//
//
//    }
//
//    public void ToggleMenuOn()
//    {
//
//        isMenuOn = !isMenuOn;
//
//    }
//  
//
//
//}
