using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Common;
using GameData;
using System.Collections;
using System.Collections.Generic;

public class FindTarget : Conditional
{
    public SharedRole sdtarget;
    public SharedRole sdself;

    public override void OnStart()
    {


    }

    public override TaskStatus OnUpdate()
    {
        if (sdtarget.Value == null)
        {
            sdtarget.Value = sdself.Value.FindTarget();
        }

        if (sdtarget.Value == null)
            return TaskStatus.Failure;
        else
            return TaskStatus.Success;

    }

    private Stack<int> acts;
    //content of common tactics generator
    void Start()
    {
        //get all possible combos from weapon data
        int[] combos = new int[] { };
        Dictionary<int, List<int>> All = new Dictionary<int, List<int>>();
        int num = 1;
        foreach(int st in combos)//所有连招用0分隔? string?? abcde?f
        {
            if (st == 0)
                num++;

            All[num].Add(st);
        }


    }

    void Update()
    {

        sdself.Value.PushState()



    }

    public void GenTactic()
    {
        int[] combos = new int[] { };
        int dir = UnityEngine.Random.Range(0, 4);
        cmds.Push(Walks[dir]);

        int act = UnityEngine.Random.Range(0, Combos.Count);
        foreach (ANIMATIONSTATE state in Combos[act])
            cmds.Push(state);

    }












}