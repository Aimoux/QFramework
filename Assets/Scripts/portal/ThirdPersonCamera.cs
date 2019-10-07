using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    private Transform cameraHandle;
    private Transform cameraPos;
    private Transform playerhandle;
    private Transform childModel;
    private Transform camTrsfm;
    private Vector3 dampVelocity;
    [Header("===Camera Parameters====")]
    public float horizontalAxis;
    public float verticalAxis;
    public float rotateSpeedH = 3f;
    public float rotateSpeedV = 3f;
    public float pitchAngle;
    public float damper = 0.2f;
    public float scaleSpeed = 60f;
    // Use this for initialization
    void Start()
    {
        camTrsfm = Camera.main.gameObject.transform;
        cameraPos = GamingManager.Instance.CameraPos;
        childModel = GamingManager.Instance.PlayerModel;
        cameraHandle = cameraPos.parent;
        playerhandle = cameraHandle.parent;
        pitchAngle = cameraHandle.eulerAngles.x;

        if (camTrsfm == null | cameraHandle == null | playerhandle == null | childModel == null)
            Debug.LogError("null component camera");

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        horizontalAxis = Input.GetAxis("Mouse X");
        horizontalAxis = Mathf.Clamp(horizontalAxis, -3f, 3f);//防止旋转过快？
        verticalAxis = Input.GetAxis("Mouse Y");
        Vector3 tempAngles = childModel.eulerAngles;
        //控制旋转,camera跟随play switch
        //if (Input.GetMouseButton(1) )
        {
            playerhandle.Rotate(0, rotateSpeedH * horizontalAxis, 0);//绕Y轴的水平面旋转
            pitchAngle -= rotateSpeedV * verticalAxis;//俯仰角旋转
            pitchAngle = Mathf.Clamp(pitchAngle, -40f, 30f);
            cameraHandle.localEulerAngles = new Vector3(pitchAngle, 0, 0);//绕cameraHandle的X轴旋转，调节俯仰。
            childModel.eulerAngles = tempAngles;
        }
        camTrsfm.LookAt(cameraHandle.transform);
        damper = 0.2f;
        camTrsfm.position = Vector3.SmoothDamp(camTrsfm.position, cameraPos.position, ref dampVelocity, damper);
        if (Input.GetAxis("Mouse ScrollWheel") != 0) //每次滚动的变量可在project setting自定义sensitivity，默认为0.1
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            float curFOV = Camera.main.fieldOfView;
            curFOV -= scroll * scaleSpeed;
            curFOV = Mathf.Clamp(curFOV, 20f, 70f); //推荐的视野角范围
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, curFOV, 0.3f);
        }
    }

    //更改lookat对象为targetTsm.parent,更改Chase position为targetTsm.position
    public void SwitchCameraToTarget(Transform targetTransform)
    {
        cameraPos = targetTransform;
        cameraHandle = targetTransform.parent;                                 
    }

    public void SetZoomSpeed(float zoomSpeed)
    {
        scaleSpeed = zoomSpeed;

    }

    public void SetRotateSpeedH(float speedH)
    {
        rotateSpeedH = speedH;
    }

    public void SetRotateSpeedV(float speedV)
    {
        rotateSpeedV = speedV;
    }
}
