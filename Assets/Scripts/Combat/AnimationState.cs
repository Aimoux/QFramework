using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public abstract class AnimState
{
    private  ANIMATIONSTATE m_State = ANIMATIONSTATE.IDLE; // 状态
    public ANIMATIONSTATE State
    {
        get{ return m_State; }
        set{ m_State = value; }// read only??
    }

    // 状态拥有者
    protected Role m_Controller = null;
    public ANIMATIONTYPE AnimCategory;
    public int FrameCount;//物理帧数fixed delta Time 时长
    public int CurFrame;

    #region AttackAnim
    protected int FrameStart;//enable sensor
    protected int FrameEnd;//disable sensor
    public bool IsAttackState = false;
    public float DamageRatio = 1f;//招式的伤害因子
    public float ImpactAtkRatio = 1f;//招式的削韧因子
    public float ImpactDefRatio = 1f;//招式的受击韧性因子
    public ImpactType Impact = ImpactType.NONE;//冲击力类型，影响硬直反应
    public bool IsWeaponEnable
    {
        get
        {
            return IsAttackState && CurFrame >= FrameStart && CurFrame <= FrameEnd ;
        }
    }
    #endregion


    // 构造
    public AnimState(Role Controller)
    { 
        m_Controller = Controller; 
        
    }

    // 开始
    public virtual void OnStateEnter()
    {
        CurFrame = 0;
        m_Controller.Controller.SetState(this);
        if(IsAttackState )
        {
            //起始到打击检测结束进行出手韧性保护
            m_Controller.Steady *= m_Controller.CurWeapon.Data.ImpactRatio * ImpactDefRatio;
        }
    }

    //当前动作被打断（比如受攻击）
    public virtual void OnStateBreak(ANIMATIONSTATE state)
    {
        m_Controller.Steady = m_Controller.Data.Steady;
        OnStateExit();
        //m_Controller.SetState()//反射创建AnimState类??

    }


    // 結束
    public virtual void OnStateExit()
    {
        //CurFrame = FrameCount;//??push idle??
    }

    // 更新
    public virtual void OnStateUpdate()
    {
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
        if(IsAttackState )
        {
            //起始到打击检测结束进行出手韧性保护
            m_Controller.Steady /= (m_Controller.CurWeapon.Data.ImpactRatio * ImpactDefRatio);
        }

    }

    public override string ToString ()//??for what purpose??
    {
        return string.Format ("[AnimationState: StateName={0}]", State);
    }

    //为避免过多new，直接为每个state创建实例，之后使用这些实例not new?? 
    public virtual bool CanTransit(AnimState next)
    {
        return true;
    }

}