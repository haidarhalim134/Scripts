using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DataContainer
{
    public class InGameContainer : MonoBehaviour
    {
        public GameObject StaminaCounter;
        public GameObject HealthCounter;
        public LevelDataContainer[] Levels;
        public AssetReferenceContainer[] LevelsTest;
        private AsyncOperationHandle<LevelDataContainer> LevelHandle;
        public LevelDataContainer FindLevel(string name)
        {
            // TODO: finalize the method, this is potentialy bad
            foreach (LevelDataContainer Cont in this.Levels)
            {
                if (Cont.LevelId == name)
                {
                    return Cont;
                }
            }
            // Addressables.Release(LevelHandle);
            // foreach (AssetReferenceContainer Cont in this.LevelsTest)
            // {
            //     if (Cont.Id == name)
            //     {
            //         LevelHandle = Cont.Ref.LoadAssetAsync<LevelDataContainer>();
            //         return LevelHandle.Result;
                    
            //     }
            // }
            Debug.Log("no found");
            return null;
        }
        public static InGameContainer GetInstance()
        {
            return GameObject.Find("Container").GetComponent<InGameContainer>();
        }
    }
    [Serializable]
    public class AssetReferenceContainer
    {
        public string Id;
        public AssetReference Ref;
    }
}