using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LevelManager;
using Control.Core;
using DataContainer;

public class ChooseCharacter : MonoBehaviour
{
    public Character CharacterName;
    public void onClick()
    {
        Loaded.loaded.InitCharacter(InGameContainer.GetInstance().FindCharacter(CharacterName));
    }
}
