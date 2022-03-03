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
            Loaded.loaded = newsafe;
            ActDataLoader.firstLoadAct(Loaded.loaded.currAct);
            ChangeScene.LoadCharacteChoose();
        }
    }
}