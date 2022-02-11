using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataContainer;
using Attributes.Player;
using Attributes.Abilities;

namespace Control.Core
{
    public class StartNewGameButton : MonoBehaviour
    {
        public void onClick()
        {
            SaveFile newsafe = new SaveFile();
            SaveFile.Save(newsafe);
            LoadedSave.Loaded = newsafe;
            ActDataLoader.firstLoadAct(LoadedSave.Loaded.currAct);
            ChangeScene.LoadCharacteChoose();
        }
    }
}