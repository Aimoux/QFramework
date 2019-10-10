using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//绑定于武器物体，检测是否命中
//角色不需要检测器（角色vs武器 二选一?）
public class HitSensor : MonoBehaviour
{
    public Role Sadism;//初始化中赋值
    public IWeapon wp;

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.tag =="")//
        {
            if (wp.OnAttack )
            {
                //阵容问题：同阵容免伤，混战阵容数量超过2个的情况
                Role Masochism = GetMasochism();
                if(Masochism != null)
                {
                    Sadism.OnSadism(Masochism);
                    Masochism.OnMasoch(Sadism.WeaponForce, Masochism);//伤害类型??
                }
               


            }

        }



    }

    private Role GetMasochism()
    {


        return null;
    }



}
