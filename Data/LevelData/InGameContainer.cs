using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Attributes.Abilities;
using Control.Core;

namespace DataContainer
{
    public class InGameContainer : MonoBehaviour
    {
        public GameObject StaminaCounter;
        public GameObject HealthCounter;
        public GameObject ShieldCounter;
        public LevelDataContainer[] Levels;
        public PrefabContainer[] Abilities;
        public PassiveDebuffCont[] PassiveDebuffPrefab;
        public ActContainer[] Acts;
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
        public ActData FindAct(Act act)
        {
            foreach (ActContainer cont in this.Acts)
            {
                if (cont.act == act)
                {
                    return cont.data;
                }
            }
            Debug.Log("act not found");
            return null;
        }
        public void SpawnAbilityPrefab(string name)
        {
            GameObject prefab = GameObject.Find(name);
            if (prefab == null)
            {
                foreach (PrefabContainer Cont in this.Abilities)
                {
                    if (Cont.Prefab.name == name)
                    {
                        GameObject Object = Instantiate(Cont.Prefab);
                        Object.name = name;
                    }
                }
            }
        }
        public GameObject SpawnAbilityPrefab(GameObject name)
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
    [Serializable]
    public class PrefabContainer
    {
        public string Id;
        public GameObject Prefab;
    }
    [Serializable]
    public class ActContainer
    {
        public Act act;
        public ActData data;
    }
    [Serializable]
    public class PassiveDebuffCont
    {
        public Debuffs debuff;
        public GameObject prefab;
    }
}