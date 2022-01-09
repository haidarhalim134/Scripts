using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Control.Core
{
    public class ContinueButton : MonoBehaviour
    {
        void Awake()
        {
            this.gameObject.SetActive(SaveFile.CheckIfExist());
        }
        public void onClick()
        {
            LoadedSave.Loaded = SaveFile.Load();
            ToActChooseScreen.LoadScene();
        }
    }
}