using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataContainer;
using Control.Core;

public class Menu : MonoBehaviour
{
    public void Continue()
    {
        Disable();
    }
    public void QuitNSave()
    {
        SaveFile.Save(Loaded.loaded);
        Loaded.loaded = null;
        ChangeScene.LoadMainMenu();
    }
    public void Enable()
    {
        if (gameObject.activeSelf) return;
        gameObject.SetActive(true);
    }
    public void Disable()
    {
        if (!gameObject.activeSelf) return;
        gameObject.SetActive(false);
    }
    void Update()
    {
        
    }
}
