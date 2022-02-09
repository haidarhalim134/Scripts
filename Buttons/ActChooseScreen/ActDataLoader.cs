using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataContainer;
using UnityEngine.SceneManagement;

namespace Control.Core
{
    public class ActDataLoader : MonoBehaviour
    {
        public static void onClick(Act act)
        {
            LoadedActData.loadedActData = InGameContainer.GetInstance().FindAct(act);
            LoadedActData.CurrAct = act;
            ChangeScene.LoadActMap();
            LoadedSave.Loaded.QueuedLevel.FillQueue();
        }
    }
    public class LoadedActData
    {
        public static ActData loadedActData;
        public static Act CurrAct;
        public static void ToActScene()
        {
            SceneManager.LoadScene("ActMap");
        }
    }
}