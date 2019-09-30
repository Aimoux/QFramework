using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using QF.Res;

public class TimeLineManager : MonoBehaviour
{
    public static TimeLineManager Instance;
    private ResLoader loader;
    private PlayableDirector director;

    public Animator playerAnim;
    public Animator botAnim;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        loader.Recycle2Cache();
        loader = null;
    }

    void Start()
    {
        director = GetComponent<PlayableDirector>();
        if (director == null)
            Debug.LogError("find no director: " + this.name);

        loader = ResLoader.Allocate();
        LaunchDirector("Resources/TimeLines/Meet");
    }
    
    public void LaunchDirector(string path)
    {
        Debug.Log("Binding Timeline Tracks!");
        //director.playableAsset = Resources.Load<PlayableAsset>(path);
        director.playableAsset = loader.LoadSync<PlayableAsset>(path);//“Ï≤Ωº”‘ÿ¥Ê‘⁄“˛ªº??        
        foreach (var playableAssetOutput in director.playableAsset.outputs)
        {
            if (playableAssetOutput.streamName == "PlayerAnim")
            {
                director.SetGenericBinding(playableAssetOutput.sourceObject, playerAnim);
            }
            else if (playableAssetOutput.streamName == "BotAnim")
            {
                director.SetGenericBinding(playableAssetOutput.sourceObject, botAnim);
            }
        }
        director.Play();       
    }

    public void Stop()
    {
        director.Stop();
        loader.Recycle2Cache();
    }

}
