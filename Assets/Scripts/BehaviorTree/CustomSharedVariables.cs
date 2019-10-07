using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using GameData;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedRole : SharedVariable<Role>
    {
        public static implicit operator SharedRole(Role value)
        {
            return new SharedRole { mValue = value };
        }
    }

    [System.Serializable]
    public class SharedNavAgent : SharedVariable<NavMeshAgent>
    {
        public static implicit operator SharedNavAgent(NavMeshAgent value)
        {
            return new SharedNavAgent { mValue = value };
        }
    }

    [System.Serializable]
    public class SharedAnimator : SharedVariable<Animator>
    {
        public static implicit operator SharedAnimator(Animator value)
        {
            return new SharedAnimator { mValue = value };
        }
    }

    [System.Serializable]
    public class SharedFloatList : SharedVariable<List<float>>
    {
        public static implicit operator SharedFloatList(List<float> value)
        {
            return new SharedFloatList { mValue = value };
        }
    }

    [System.Serializable]
    public class SharedIntList : SharedVariable<List<int>>
    {
        public static implicit operator SharedIntList(List<int> value)
        {
            return new SharedIntList { mValue = value };
        }
    }

    [System.Serializable]
    public class SharedStringList : SharedVariable<List<string>>
    {
        public static implicit operator SharedStringList(List<string> value)
        {
            return new SharedStringList { mValue = value };
        }
    }
}
