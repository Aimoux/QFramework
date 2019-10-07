//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class MiniMapCamera : MonoBehaviour
//{
//    private Camera miniCamera;
//    private AudioSource voice;
//    public AudioClip clickSound;
//    public Vector3 watchPosition;
//    public float zoomSpeed = 0.5f;
//    public float minSize = 0.25f;
//    public float maxSize = 3f;
//    // Use this for initialization
//    void Start()
//    {
//
//        miniCamera = GetComponent<Camera>();
//        voice = GetComponent<AudioSource>();
//        if (miniCamera == null || voice == null || clickSound == null)
//            Debug.Log("Warning: null component for mapcamera!");
//       // watchPosition = transform.position - EventManager._instance.curtPlayer.transform.position;
//
//    }
//
//    // Update is called once per frame
//    void Update()
//    {
//
//        transform.position = EventManager._instance.curtPlayer.transform.position + watchPosition;
//        miniCamera.orthographicSize = Mathf.Clamp(miniCamera.orthographicSize, minSize, maxSize);
//
//    }
//
//    public void ZoomIn()
//    {
//        miniCamera.orthographicSize -= zoomSpeed;
//        voice.PlayOneShot(clickSound);
//
//    }
//
//
//    public void ZoomOut()
//    {
//        miniCamera.orthographicSize += zoomSpeed;
//        voice.PlayOneShot(clickSound);
//    }
//
//
//
//
//}
