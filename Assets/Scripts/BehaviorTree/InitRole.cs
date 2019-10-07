//using UnityEngine;
//using UnityEngine.AI;
//using BehaviorDesigner.Runtime;
//using BehaviorDesigner.Runtime.Tasks;
//using System;
//
//public class InitRole : BehaviorDesigner.Runtime.Tasks.Action, ICreateInstance
//{
//    [HideInInspector] public IInitRole init;//���Զ�̬�ı�
//    public SharedBTRoleData BTCasterData;
//    public string typeName;
//    public readonly float radius = 1f;//NavMeshAgent�Դ���ײ����
//    //public int TalentCount = 5;
//
//    public override void OnAwake()
//    {
//        //��ȡUnity����
//        if (BTCasterData.Value == null) BTCasterData.Value = new BTRoleData();
//
//        //ִ��˳����������?�˽ڵ���InitTalent֮��ִ��?
//        //for (int i = 1; i <= TalentCount; i++)
//        //{
//        //    //Debug.Log(Owner.name+": init role create talent: " + i);
//        //    BTCasterData.Value.TalentTreeDict.Add(i, new BTTalentData(i));
//        //}
//
//
//
//        BTCasterData.Value.obj = this.gameObject;
//        BTCasterData.Value.Position = this.gameObject.transform.position;
//        BTCasterData.Value.anim = GetComponent<Animator>();
//        BTCasterData.Value.nav = GetComponent<NavMeshAgent>();
//        UnityMethods.Instance.EndNav(BTCasterData.Value);
//        // BTCaster.Value.nav.radius = radius;
//        BTCasterData.Value.tree = GetComponent<BehaviorTree>();
//
//        CreateInstanceByName();
//        if (init == null) Debug.LogError("null init role interface :" + this.gameObject.name);
//        else init.InitRole(BTCasterData.Value);
//
//        BTCasterData.Value.Radius = radius;
//        BTCasterData.Value.AntiSide = -(int)BTCasterData.Value.Side;
//
//    }
//
//    public override void OnStart()
//    {
//        UnityMethods.Instance.InitHPBar(BTCasterData.Value);
//    }
//
//
//    public override TaskStatus OnUpdate()
//    {
//        if (BTCasterData.Value == null)
//            return TaskStatus.Failure;
//        else
//        {
//            //Debug.Log("init role successs");
//            return TaskStatus.Success;
//        }
//
//    }
//
//    public override void OnEnd()
//    {
//        this.Disabled = true;
//    }
//
//    public void CreateInstanceByName(string name = null)
//    {
//        if (name == null) name = typeName;
//
//        if (name == null)
//        {
//            init = (IInitRole)Activator.CreateInstance(Type.GetType("InitRoleBase"));
//        }
//        else
//        {
//            init = (IInitRole)Activator.CreateInstance(Type.GetType("InitRole" + name));
//        }
//
//    }
//
//}