using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataContainer;

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
            Loaded.loaded = SaveFile.Load();
            ActDataLoader.firstLoadAct(Loaded.loaded.currAct);
            if (Loaded.loaded.CharacterId != Character.Nocharacter) 
            {
                ChangeScene.LoadActMap();
                SaveFile.LoadCharacter(InGameContainer.GetInstance().FindCharacter(Loaded.loaded.CharacterId));
            }
            else ChangeScene.LoadCharacteChoose();
        }
    }
}