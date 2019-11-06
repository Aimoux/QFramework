using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using GameData;

public class AssaultExtension : Assault
{
    public AssaultExtension(int id, Role caster): base(id, caster)
    {


    }


    public override void Init()
    {
        base.Init();

    }

    public override void Start(Role target)
    {
        base.Start(target);

    }

    public override Role FindTarget(Role defaultTarget)
    {



        return base.FindTarget(defaultTarget);
    }

    //不走cd,基本无用
    public override void Update(float dt)
    {

        base.Update(dt);

    }

    public override void Break()
    {
        base.Break();

    }

    public override void End()
    {
        base.End();

    }
}

