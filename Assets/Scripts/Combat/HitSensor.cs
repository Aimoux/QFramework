using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����������壬����Ƿ�����
//��ɫ����Ҫ���������ɫvs���� ��ѡһ?��
public class HitSensor : MonoBehaviour
{
    public Weapon wp;//��ʼ���и�ֵ
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "")
        {
            //�������⣺ͬ�������ˣ���ս������������2�������
            Role Masochism = GetMasochism();
            if (Masochism != null)
            {
                wp.OnHit();//�����������������Ч��
                wp.Owner.OnSadism(wp, Masochism);//�������������
                Masochism.OnMasoch(wp);//�ܻ�����ʧ����
                //һ������¼�����Ϣ����
                //��������Ҫ֪���Լ�����һ������������״̬�������˺�����������һ������
                //�ܻ�����Ҫ֪���˺���Ч������ֵ�͹�������˭�����������ת�ƣ�
            }
        }
    }

    //���ݡ����ء��޵��ж�
    //role���̳�mono��λ���ڴ��У���λ�ȡ?? behaviortree.get vaiable value??
    //���ͬ�������Ϊ��?? ������ο���??
    private Role GetMasochism()
    {


        return null;
    }



}
