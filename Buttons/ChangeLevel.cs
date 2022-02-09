using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LevelManager;
using Control.Core;
using DataContainer;

public class ChangeLevel : MonoBehaviour
{
    public string target;
    public string SceneId;
    public Character CharacterName;
    private void OnMouseDown() 
    {
        LoadedSave.Loaded.InitCharacter(InGameContainer.GetInstance().FindCharacter(CharacterName));
        Debug.Log(target);
        LevelLoader.LoadLevel(target, SceneId);
        LevelLoader.testing = true;
    }
    public void onClick()
    {
        this.OnMouseDown();
    }
}
