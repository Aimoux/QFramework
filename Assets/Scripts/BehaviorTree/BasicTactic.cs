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
	private Dictionary<int, List<AnimState>> All = new Dictionary<int, List<AnimState>>();
	private Stack<AnimState> acts = new Stack<AnimState> ();
	public override void OnStart()
	{
        //把移动st移除,改用行为树前置task的方式实现(比较聪明的AI,可直接在树中去掉此节点)
        //生成移动\等待类行动指令时,可灵活设置其帧数?
		int[] combos = new int[] {101,201,202,0,201,201,0,101,202 };
        int num = 0;
        foreach(int st in combos)//所有连招用0分隔? string?? abcde?f
        {
            if (st == 0)
                num++;
            AnimState anst = AnimState.Create(sdself.Value, (ANIMATIONSTATE)st);
			if(!All.ContainsKey(num))
                All[num] = new List<AnimState> { anst };
			else
            	All[num].Add(anst);
        }


	}

	public override TaskStatus OnUpdate()
	{
		if(!sdself.Value.OnTactic)
		{
 			int key = Random.Range(0, All.Count);
        	// int[] combos = new int[] { };
        	// int dir = UnityEngine.Random.Range(0, 4);
        	// cmds.Push(Walks[dir]);
			foreach(AnimState anst in All[key])
			{
            	sdself.Value.Cmds.Push(anst);
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