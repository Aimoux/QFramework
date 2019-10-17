using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ʵʱ�˺���������??
//Ѫ��\����ʾ����Ļ����??
public class UIFloatingBar : MonoBehaviour
{
    public Role role;

    //��̬׼����(����)ʵ������
    public static UIFloatingBar Create(Role role, RoleController ctrl)
    {
        if (role == null)
        {
            Debug.LogError("null role for floating bar");
            return null;
        }

        UIFloatingBar hpbar = ctrl.floatingBar;
        if (hpbar == null)
        {
            //GameObject floatbar = ObjectPoolManager.Take("UI/prefab/", "UIFloatingBar");
            GameObject floatbar = new GameObject();
            floatbar.name = "UIFloatingBar";
            hpbar = floatbar.GetComponent<UIFloatingBar>();
        }
        hpbar.role = role;
        hpbar.Init();
        hpbar.transform.SetParent(ctrl.gameObject.transform, false);
        //hpbar.transform.localPosition = Vector3.up * 1.7f + new Vector3(0, 0, -1);
        //hpbar.transform.localRotation = Quaternion.identity;
        //hpbar.transform.localScale = Vector3.one * 0.01f / ctrl.transform.localScale.y;
        return hpbar;
    }

    public void Init()
    {


    }


}
