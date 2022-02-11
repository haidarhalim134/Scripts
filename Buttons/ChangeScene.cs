using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ChangeScene
{
    public static void LoadActMap()
    {
        SceneManager.LoadSceneAsync("ActMap");
    }
    public static void LoadCharacteChoose()
    {
        SceneManager.LoadSceneAsync("CharacterChooseScreen");
    }
    public static void LoadCombatScene()
    {
        SceneManager.LoadSceneAsync("CombatScene");
    }
}
