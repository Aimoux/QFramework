using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����������壬����Ƿ�����
//��ɫ����Ҫ���������ɫvs���� ��ѡһ?��
public class HitSensor : MonoBehaviour
{
    public Role Sadism;//��ʼ���и�ֵ
    public IWeapon wp;

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.tag =="")//
        {
            if (wp.OnAttack )
            {
                //�������⣺ͬ�������ˣ���ս������������2�������
                Role Masochism = GetMasochism();
                if(Masochism != null)
                {
                    Sadism.OnSadism(Masochism);
                    Masochism.OnMasoch(Sadism.WeaponForce, Masochism);//�˺�����??
                }
               


            }

        }



    }

    private Role GetMasochism()
    {


        return null;
    }



}
