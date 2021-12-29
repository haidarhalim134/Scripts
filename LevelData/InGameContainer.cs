using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Attributes.Abilities;

namespace DataContainer
{
    public class InGameContainer : MonoBehaviour
    {
        public GameObject StaminaCounter;
        public GameObject HealthCounter;
        public LevelDataContainer[] Levels;
        public AssetReferenceContainer[] Abilities;
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
        public void SpawnAbilityPrefab(string name)
        {
            GameObject prefab = GameObject.Find(name);
            if (prefab == null)
            {
                foreach (AssetReferenceContainer Cont in this.Abilities)
                {
                    if (Cont.Id == name)
                    {
                        GameObject Object = Cont.Ref.InstantiateAsync().Result;
                        Object.name = name;
                    }
                }
            }
        }public GameObject SpawnAbilityPrefab(GameObject name)
        {
            // AsyncOperationHandle<GameObject> handle = name.InstantiateAsync();
            // GameObject prefab = handle.Result;
            string abname = name.GetComponent<AbilityManager>().AbName;
            GameObject find = GameObject.Find(abname);
            if (find == null)
            {
                Debug.Log("spawned"+abname);
                GameObject Object = Instantiate(name);
                Object.name = abname;
                return Object;
            }
            else
            {
                return find;
            }
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