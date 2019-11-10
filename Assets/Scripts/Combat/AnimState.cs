using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using GameData;

public abstract class AnimState
{
    private  ANIMATIONSTATE m_State = ANIMATIONSTATE.IDLE; // 状态
    public ANIMATIONSTATE State
    {
        get{ return m_State; }
        set{ m_State = value; }// read only??
    }
    public int ID;

    // 状态拥有者
    protected Role Caster = null;
    public AnimationData Data;
    public int FrameCount;//物理帧数fixed delta Time 时长
    public int CurFrame;
    //public bool IsBreak;

    #region AttackAnim
    protected int FrameStart;//enable sensor
    protected int FrameEnd;//disable sensor
    public bool IsWeaponEnable
    {
        get
        {
            return Data.AnimationType == (int)ANIMATIONTYPE.ATTACK  && CurFrame >= FrameStart && CurFrame <= FrameEnd ;
        }
    }
    public int TalentId;//Anim与Talent关联极高,考虑TalentXXXOnFrameStart??
    public Role Target;//可以与Controller.Target不同
    #endregion

    public AnimState(Role Controller, int id)
    { 
        Caster = Controller;
        ID = id;
        Data = DataManager.Instance.Animations[ID];
        State = (ANIMATIONSTATE)Data.State;
        try 
        {
            int[] frames = DataManager.Instance.WeaponFrames[Controller.CurWeapon.Data.ID][(int)State];
            FrameCount = frames[0];
            FrameStart = frames[1];
            FrameEnd = frames[2];

            //根据物理时间步长进行换算
            FrameCount = Mathf.RoundToInt(FrameCount * Common.Const.DeltaFrame / Time.fixedDeltaTime);
            FrameStart = Mathf.RoundToInt(FrameStart * Common.Const.DeltaFrame / Time.fixedDeltaTime);
            FrameEnd = Mathf.RoundToInt(FrameEnd * Common.Const.DeltaFrame / Time.fixedDeltaTime);
        }
        catch 
        {
            Debug.LogError("wp id=: " + Controller.CurWeapon.Data.ID + " st=: " + State);
        }
        
       
    }

    public virtual void Init()//OnStateEnter已替代此功能??
    {
        CurFrame = 0;
    }

    // 开始
    public virtual void OnStateEnter()
    {
        CurFrame = 0;
        //IsBreak = false;
        Caster.Controller.SetState(this);

        if( Data.AnimationType == (int)ANIMATIONTYPE.ATTACK)
        {
            //起始到打击检测结束进行出手韧性保护
            Caster.Steady *= Caster.CurWeapon.Data.ImpactRatio * Data.DefenseImpactRatio;
            Caster.StopNav();
        } 
        else if(Data.AnimationType ==(int)ANIMATIONTYPE.MOTION)
        {
            Caster.StartNav();
        }
        else if(Data.AnimationType ==(int)ANIMATIONSTATE.DEATH)
            Caster.IsDead = true;

        // if(Caster.ID == 2)
        //     Debug.LogError(Caster.ID + " enter state: " + State);
    }

    public virtual void OnStateBreak()//walk to target
    {
        //IsBreak = true;
        //m_Controller.Cmds.Clear();//??打断一个,整个策略放弃??
        //m_Controller.Steady = m_Controller.Data.Steady;
        OnStateExit();
        //m_Controller.SetState()//反射创建AnimState类??

    }


    // 結束
    public virtual void OnStateExit()
    {
        //motion以及idle设置为loop true
        // if(Data.AnimationType ==(int)ANIMATIONTYPE.MOTION)
        // {
        //     Caster.StopNav();
        // }

    }

    // 更新
    public virtual void OnStateUpdate()
    {
        // if(Caster.ID == 2)
        //     Debug.LogError(Caster.ID + " " + State + " " + CurFrame);

        //break trans 判定??

        CurFrame++;
        CurFrame = CurFrame > FrameCount ? FrameCount : CurFrame;

        if(CurFrame == FrameStart )//enable weapon sensor
        {
            OnFrameStart();
        }

        if(CurFrame == FrameEnd )//disable
        {
            OnFrameEnd();
        }
    }

    public virtual void OnFrameStart()//atk frame start
    {
        

    }

    //被打断时，此处是否仍应调用??
    public virtual void OnFrameEnd()//atk frame end
    {
        if( Data.AnimationType == (int)ANIMATIONTYPE.ATTACK)
        {
            //起始到打击检测结束进行出手韧性保护
            Caster.Steady /= (Caster.CurWeapon.Data.ImpactRatio * Data.DefenseImpactRatio);
        }

    }

    public override string ToString ()//??for what purpose??
    {
        return string.Format ("[AnimationState: StateName={0}]", State);
    }
  
    //边走边嗑药、表情会运用到layer以及AvatarMask
    //嗑药动作放在UpBody层，表情放在Head层


    public virtual int CanTransit(AnimState next)
    {
        Dictionary<int, int> trans;
        if(DataManager.Instance.Transits.TryGetValue((int)State, out trans))
        {
            int cantrans;
            if(trans.TryGetValue((int)next.State, out cantrans))
                return cantrans;
            else
                Debug.LogError("find no next key: " + next.State);

        }
        else
            Debug.LogError("find no first key: " + State);

        return 0;
    }


}

