using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataContainer;
using UnityEngine.SceneManagement;

namespace Control.Core
{
    public class ActDataLoader : MonoBehaviour
    {
        public Act act;
        public bool HaveRequirements;
        public Act Requirements;
        public void onClick()
        {
            if (HaveRequirements)
            {
                if (!LoadedSave.Loaded.act[this.Requirements].finished) return;
            }
            LoadedActData.loadedActData = InGameContainer.GetInstance().FindAct(this.act);
            LoadedActData.CurrAct = this.act;
            ToActMap.LoadScene();
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