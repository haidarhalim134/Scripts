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
            Loaded.loaded.currAct = act;
            if (Loaded.loaded.QueuedLevel.Queue.Count == 0)Loaded.loaded.QueuedLevel.FillAllQueue();
        }
        public static void nextAct()
        {
            int index = Array.IndexOf(actOrder, Loaded.loaded.currAct) + 1;
            if (index >= actOrder.Length)
            {
                // TODO: create game finish func
            }
            Loaded.loaded.currAct = actOrder[index];
            loadedActData = InGameContainer.GetInstance().FindAct(actOrder[index]);
            Loaded.loaded.QueuedLevel.FillAllQueue();
            Debug.Log(loadedActData.name+"picked"+Loaded.loaded.QueuedLevel.Queue[0]);
            ChangeScene.LoadActMap();
        }
    }
}