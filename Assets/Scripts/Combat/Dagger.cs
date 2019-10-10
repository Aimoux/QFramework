using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : Weapon
{ 
    public Dagger(int id, int lv, Role owner): base(id, lv, owner)
    {

    }

    public override void Enchant(Enchantium encho)
    {
        base.Enchant(encho);

    }


}
