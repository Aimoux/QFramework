using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class TimeLineManager : MonoBehaviour
{
    public static TimeLineManager Instance;
    private PlayableDirector director;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        director = GetComponent<PlayableDirector>();
        if (director == null)
            Debug.LogError("find no director: " + this.name);
    }
    
    public void LaunchDirector(string path)
    {
        director.playableAsset = Resources.Load<PlayableAsset>("TimeLines/Meet");
        //director.SetGenericBinding(null,null);//??
        director.Play();
    }

    public void Stop()
    {
        director.Stop();
    }

}
