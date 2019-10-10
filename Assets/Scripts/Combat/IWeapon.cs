using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon 
{
    //rather read form data??
    //int type { get; set; }//obsolete??
    //int length { get; set; }
    //int damage { get; set; }

    float OnHitTarget();
    bool OnAttack { get; set; }

}
