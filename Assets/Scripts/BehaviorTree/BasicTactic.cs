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
		if(sdself.Value.Cmds.Count == 0)
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

}