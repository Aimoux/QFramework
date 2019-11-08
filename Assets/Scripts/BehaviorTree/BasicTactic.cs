using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Common;
using GameData;
using System.Collections;
using System.Collections.Generic;

public class BasicTactic : Action
{
	public SharedRole sdtarget;
    public SharedRole sdself;
	public override void OnStart()
	{
        //把移动st移除,改用行为树前置task的方式实现(比较聪明的AI,可直接在树中去掉此节点)
        //生成移动\等待类行动指令时,可灵活设置其帧数?
	}

	public override TaskStatus OnUpdate()
	{
        //if not idle return;
        // target = role.defaultfindtarget by hatred\dist\?
        //null target -> Move To target task- >Use assault node




		if(!sdself.Value.OnTactic)
		{
 			int rand = Random.Range(0, sdself.Value.CurWeapon.ComboLists.Count);
			foreach(int st in sdself.Value.CurWeapon.ComboLists[rand])
			{
            	sdself.Value.PushState(st);
        	}
		}       
        return TaskStatus.Success;
    }

    //暗黑骷髅杂兵AI
    //1.当肉搏范围内没有敌人时，继续接近敌人的几率
    //2. par1,par3检定失败后等待的时间（单位：帧）
    //3. 当肉搏范围内有敌人时，攻击敌人的几率
    //4. 攻击时，以A2代替A1的几率

}