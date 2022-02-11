using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataContainer;
using UnityEngine.SceneManagement;

namespace Control.Core
{
    public class ActDataLoader
    {
        public static Act[] actOrder = {Act.Act1, Act.Act2};
        public static ActData loadedActData;
        public static void firstLoadAct(Act act)
        {
            loadedActData = InGameContainer.GetInstance().FindAct(act);
            LoadedSave.Loaded.currAct = act;
            if (LoadedSave.Loaded.QueuedLevel.Queue.Count == 0)LoadedSave.Loaded.QueuedLevel.FillQueue();
        }
        public static void nextAct()
        {
            int index = Array.IndexOf(actOrder, LoadedSave.Loaded.currAct) + 1;
            if (index >= actOrder.Length)
            {
                // TODO: create game finish func
            }
            LoadedSave.Loaded.currAct = actOrder[index];
            loadedActData = InGameContainer.GetInstance().FindAct(actOrder[index]);
            LoadedSave.Loaded.QueuedLevel.FillQueue();
            Debug.Log(loadedActData.name+"picked"+LoadedSave.Loaded.QueuedLevel.Queue[0]);
            ChangeScene.LoadActMap();
        }
    }
}