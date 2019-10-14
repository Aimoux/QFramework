using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleScene 
{

   //void PauseBattle(PauseType type);

    //void ResumeBattle(PauseType type);

    void CollectLoots(float delay = 0);

    bool AutoBattleMode { get; set; }


    //void OnBattleStart(string type, CombatConfigData battle);


    void ShowDropGolds(int num, Role role);


    //void PopupText(PopupType type, string str, RoleJoint actor, bool crit, RoleSide side, string exType = "", AbilityData abilityData = null);

    /// <summary>
    /// freeze screen �� ��ĻЧ������ͣ�������ֲ�
    /// </summary>
    //void PlayEffect(ScreenEffect effect, float intensity, float duration);




}
