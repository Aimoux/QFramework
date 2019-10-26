using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//绑定于武器物体，检测是否命中
//角色不需要检测器（角色vs武器 二选一?）
public class HitSensor : MonoBehaviour
{
    public Weapon wp;//初始化中赋值
    private void OnTriggerEnter(Collider other)
    {
        if(!wp.Owner.Status.IsWeaponEnable)//能否确保被打断时此处为false??
            return;

        if (other.gameObject.tag == "")
        {
            //阵容问题：同阵容免伤，混战阵容数量超过2个的情况
            Role Masochism = GetMasochism();
            if (Masochism != null)
            {
                wp.OnHit();//武器自身的命中特殊效果
                wp.Owner.OnSadism(wp, Masochism);//攻击方收益计算
                Masochism.OnMasoch(wp);//受击方损失计算
                //一个打击事件的信息传递
                //攻击方需要知道自己的哪一个武器（武器状态，计算伤害）击中了哪一个敌人
                //受击方需要知道伤害（效果）数值和攻击者是谁（反击、仇恨转移）
            }
        }
    }

    //阵容、隐藏、无敌判定
    //role不继承mono，位于内存中，如何获取?? behaviortree.get vaiable value??
    //玩家同样添加行为树?? 避免地形卡死??
    private Role GetMasochism()// how to get??
    {


        return null;
    }




}
