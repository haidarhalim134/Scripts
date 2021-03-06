using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using Control.Combat;
using DataContainer;

namespace LevelManager
{
    public class LevelLoader
    {
        public static LevelDataContainer ToLoad;
        public static bool testing;
        /// <summary>for testing purpose only</summary>
        public static void LoadLevel(string target, string SceneId = null)
        {
            ChangeScene.LoadCombatScene();
            if (SceneId!= null&&SceneId!= "")
            {
                LevelDataContainer Data = InGameContainer.GetInstance().FindLevel(SceneId);
                ToLoad = Data;
            }
        }
        public static void LoadLevel(LevelDataContainer cont)
        {
            testing = false;
            ToLoad = cont;
            ChangeScene.LoadCombatScene();
        }
    }
}

