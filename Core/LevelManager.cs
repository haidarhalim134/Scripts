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
        public static void LoadLevel(string target, string SceneId = null)
        {
            SceneManager.LoadScene(target);
            if (SceneId!= null&&SceneId!= "")
            {
                LevelDataContainer Data = GameObject.Find("Container").GetComponent<InGameContainer>().FindLevel(SceneId);
                ToLoad = Data;
            }
        }
    }
}

