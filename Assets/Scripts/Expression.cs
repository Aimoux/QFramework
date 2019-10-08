using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class Expression : MonoBehaviour
{
    public SkinnedMeshRenderer blwRenderer;
    public SkinnedMeshRenderer eyeRenderer;
    public SkinnedMeshRenderer elRenderer;
    public SkinnedMeshRenderer mthRenderer;

    [Tooltip("����һ�����鶯������ʱ��")]
    public float animationDuration = 5;

    private float timeElapse = 0;

    //#region DisstractAnim
    //float[] blwWeight = new float[6]
    //{
    //    0, 0, 0, 0, 0, 0,
    //};

    //float[] eyeWeight = new float[7]
    //{
    //    11.3f, 0, 0, 0, 87.7f, 0, 25.64f
    //};

    //float[] elWeight = new float[7]
    //{
    //    1, 0, 0, 0, 0, 0, 40,
    //};

    //float[] mthWeight = new float[6]
    //{
    //    0, 0, 0, 0, 0, 0,
    //};
    //#endregion

    //void Update()
    //{
    //    timeElapse += Time.deltaTime;
    //    if (timeElapse > animationDuration)
    //    {
    //        timeElapse = 0;
    //    }
    //    float normalizeTime = timeElapse / animationDuration;

    //    for (int i = 0; i < blwWeight.Length; i++)
    //    {
    //        blwRenderer.SetBlendShapeWeight(i, Mathf.Lerp(0, blwWeight[i], normalizeTime));
    //    }

    //    for (int i = 0; i < eyeWeight.Length; i++)
    //    {
    //        eyeRenderer.SetBlendShapeWeight(i, Mathf.Lerp(0, eyeWeight[i], normalizeTime));
    //    }

    //    for (int i = 0; i < elWeight.Length; i++)
    //    {
    //        elRenderer.SetBlendShapeWeight(i, Mathf.Lerp(0, elWeight[i], normalizeTime));
    //    }

    //    for (int i = 0; i < mthWeight.Length; i++)
    //    {
    //        mthRenderer.SetBlendShapeWeight(i, Mathf.Lerp(0, mthWeight[i], normalizeTime));
    //    }
    //}



    void Test()
    {
        SkinnedMeshRenderer rd = new SkinnedMeshRenderer();
        rd.SetBlendShapeWeight(1, 100);

        //����blend weight(һһ��Ӧ��ʵ����)������ģ�����б���??

        //solution1
        //�Լ�����������ݣ������excel��
        //enum facial { smile =0, talk =1,...    }
        //expdata = DataManager.Instance.Exps[(int)facial.smile];
        //foreach(var part in expdata)
        //{
        //rd.SetBlendShapeWeight();
        //}

        //solution2
        //�Լ�����(¼��)�������ܷ��ض���?? ���أ�ת�������鶯����Դ


    }






}
