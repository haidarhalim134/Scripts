using System;
using System.Linq;
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
        public GameObject debuffCounter;
        public LevelDataContainer[] Levels;
        public CharacterDataCont[] Characters;
        public GameObject[] Abilities;
        public PassiveDebuffCont[] PassiveDebuffPrefab;
        public ActContainer[] Acts;
        public AssetReferenceContainer[] LevelsTest;
        private AsyncOperationHandle<LevelDataContainer> LevelHandle;
        /// <summary>for testing purpose only</summary>
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
            ActData result = Array.Find(this.Acts,(cont)=>cont.act == act).data;
            if (result == null) Debug.Log("act not found");
            return result;
        }
        public CharacterDataCont FindCharacter(string name)
        {
            CharacterDataCont result = Array.Find(this.Characters, (cont) => cont.Name == name);
            if (result == null) Debug.Log("character not found");
            return result;
        }
        public PassiveDebuffCont FindPassiveDebuff(Debuffs debuff)
        {
            PassiveDebuffCont result = Array.Find(this.PassiveDebuffPrefab, (cont) => cont.debuff == debuff);
            if (result == null) Debug.Log("debuff not found");
            return result;
        }
        public void SpawnAbilityPrefab(string name)
        {
            GameObject prefab = GameObject.Find(name);
            if (prefab == null)
            {
                GameObject fab = Array.Find(this.Abilities, (prefab)=> prefab.GetComponent<AbilityManager>().AbName == name);
                GameObject Object = Instantiate(fab);
                Object.name = name;
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
        public string description;
        public bool reduceCharge;
        public string GetDesc()
        {
            return description.Replace("\\n", "\n");
        }
    }
}