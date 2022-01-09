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
        /// <summary>for testing purpose only shit old code</summary>
        public static void LoadLevel(string target, string SceneId = null)
        {
            SceneManager.LoadScene(target);
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
            SceneManager.LoadScene("CombatScene");
        }
    }
}

