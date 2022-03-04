using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SimpleChangeScene : MonoBehaviour
{
    public string to;
    public void onpointerup()
    {
        SceneManager.LoadScene(to);
    }
}
