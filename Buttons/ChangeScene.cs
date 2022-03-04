using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DataContainer;


public class ChangeScene
{
    public static void LoadActMap()
    {
        InGameContainer.GetInstance().StartCoroutine(CloseTransition("ActMap"));
    }
    public static void LoadCharacteChoose()
    {
        InGameContainer.GetInstance().StartCoroutine(CloseTransition("CharacterChooseScreen"));
    }
    public static void LoadCombatScene()
    {
        InGameContainer.GetInstance().StartCoroutine(CloseTransition("CombatScene"));
    }
    public static void LoadCardUpgradeScene()
    {
        InGameContainer.GetInstance().StartCoroutine(CloseTransition("CardUpgradeScene"));
    }
    public static IEnumerator CloseTransition(string scene)
    {
        Transition.Close();
        yield return new WaitForSeconds(Transition.animDuration);
        SceneManager.LoadSceneAsync(scene);
    }
}
