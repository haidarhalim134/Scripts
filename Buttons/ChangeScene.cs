using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ChangeScene
{
    public static void LoadActMap()
    {
        SceneManager.LoadScene("ActMap");
    }
    public static void LoadCharacteChoose()
    {
        SceneManager.LoadScene("CharacterChooseScreen");
    }
}
