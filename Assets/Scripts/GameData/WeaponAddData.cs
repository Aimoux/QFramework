using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    [System.Serializable]
    public class WeaponAddData : MonoBehaviour
    {
        public int StrengthAdd { get; set; }//�����˺�������������������ƽ������������˺���Ч
        public int DexterityAdd { get; set; }//��������
        public int MentalAdd { get; set; }//������ƶԷ��������˺���Ч

    }
}
