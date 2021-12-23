using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LevelManager;

public class ChangeLevel : MonoBehaviour
{
    public string target;
    public string SceneId;

    private void OnMouseDown() 
    {
        Debug.Log(target);
        LevelLoader.LoadLevel(target, SceneId);
    }
}
