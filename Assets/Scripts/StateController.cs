using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class SceneStateController//内嵌于role中??
//{
//    private AnimationState m_State; 
//    private bool    m_bRunBegin = false;
//
//    public SceneStateController()
//    {}
//
//    // 设定状态
//    public void SetState(AnimationState State, string LoadSceneName)
//    {
//        //Debug.Log ("SetState:"+State.ToString());
//        m_bRunBegin = false;
//
//        // 载入场景
//        LoadScene( LoadSceneName );
//
//        // 通知前一个State結束
//        if( m_State != null )
//            m_State.OnStateExit();
//
//        // 设定
//        m_State=State;  
//    }
//
//    // 载入
//    private void LoadScene(string LoadSceneName)
//    {
//        if( LoadSceneName==null || LoadSceneName.Length == 0 )
//            return ;
//        SceneManager.LoadSceneAsync(LoadSceneName);
//    }
//
//    // 更新
//    public void StateUpdate()
//    {
//        // 是否還在載入
//        if( Application.isLoadingLevel)
//            return ;
//
//        // 通知新的State開始
//        if( m_State != null && m_bRunBegin==false)
//        {
//            m_State.OnStateEnter();
//            m_bRunBegin = true;
//        }
//
//        if( m_State != null)
//            m_State.OnStateUpdate();
//    }
//}


//
//public class GameLoop : MonoBehaviour 
//{
//    // 场景状态持有者
//    SceneStateController m_SceneStateController = new SceneStateController();
//
//    // 
//    void Awake()
//    {
//        // 保证场景切换时不会被删除
//        GameObject.DontDestroyOnLoad( this.gameObject );         
//    }
//
//    // Use this for initialization
//    void Start () 
//    {
//        // 设定起始场景
//        m_SceneStateController.SetState(new IdleState(m_SceneStateController), "");
//    }
//
//    // Update is called once per frame
//    void Update () 
//    {
//        m_SceneStateController.StateUpdate();   
//    }
//}
