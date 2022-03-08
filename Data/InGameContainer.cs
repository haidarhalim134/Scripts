using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Attributes.Abilities;
using Control.Core;
using Control.Deck;

namespace DataContainer
{
    public class InGameContainer : MonoBehaviour
    {
        public PlayerController currPlayer;
        public CardDeck currDeck;
        public CardUpgrader currCardUpgrader;
        public Menu currMenu;
        public CardViewer currCardViewer;
        public GameObject cardViewPrefab;
        public SimpleActiveDebuffIndicator activeDebuffIndicatorPrefab;
        public CardShopProportion shopProportion;
        public CardShopCost shopCost;
        public float delayBetweenTurn;
        public GameObject StaminaCounter;
        public GameObject HealthCounter;
        public GameObject ShieldCounter;
        public GameObject debuffCounter;
        public GameObject intentCounter;
        public GameObject damageIndicator;
        public IntentCont[] intentCont;
        public LevelDataContainer[] Levels;
        public CharacterDataCont[] Characters;
        public GameObject[] Abilities;
        public PassiveDebuffCont[] PassiveDebuffPrefab;
        public StanceCont[] stance;
        public ActContainer[] Acts;
        public AssetReferenceContainer[] LevelsTest;
        private AsyncOperationHandle<LevelDataContainer> LevelHandle;
        public static float defaultScreenWidth = 572;
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
        public CharacterDataCont FindCharacter(Character name)
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
        public StanceCont FindStance(Stance stance)
        {
            StanceCont result = Array.Find(this.stance, (cont) => cont.stance == stance);
            if (result == null) Debug.Log("stance not found");
            return result;
        }
        public GameObject FindIntent(List<AbilityType> types)
        {
            var len = Array.FindAll(intentCont, (cont)=>cont.type.Length==types.Count).ToList();
            GameObject res = len.Find((cont)=>
            {
                return types.All(type=>cont.type.Any(typi=>typi == type));
            }).prefab;
            if (res==null)Debug.Log("intent not found");
            return res;
        }
        public AbilityManager SpawnAbilityPrefab(string name)
        {
            GameObject prefab = GameObject.Find(name);
            if (prefab == null)
            {
                GameObject fab = Array.Find(this.Abilities, (prefab)=> prefab.GetComponent<AbilityManager>().AbName == name);
                GameObject Object = Instantiate(fab);
                Object.name = name;
                return Object.GetComponent<AbilityManager>();
            }
            return prefab.GetComponent<AbilityManager>();
        }
        public AbilityManager SpawnAbilityPrefab(GameObject name)
        {
            // AsyncOperationHandle<GameObject> handle = name.InstantiateAsync();
            // GameObject prefab = handle.Result;
            string abname = name.GetComponent<AbilityManager>().AbName;
            GameObject find = GameObject.Find(abname);
            if (find == null)
            {
                GameObject Object = Instantiate(name);
                Object.name = abname;
                return Object.GetComponent<AbilityManager>();
            }
            else
            {
                return find.GetComponent<AbilityManager>();
            }
        }
        public static InGameContainer GetInstance()
        {
            return GameObject.Find("Container").GetComponent<InGameContainer>();
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (currMenu.gameObject.activeSelf) currMenu.Disable();
                else currMenu.Enable();
            }
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
        [TextArea]
        public string description;
        public bool reduceCharge;
        public ReduceChargeTime reduceChargeTime;
        public bool hideWhen0Charge;
        /// <summary>else destroy when charge reaces 0 or less than zero</summary>
        [Tooltip("else destroy when charge reaces 0 or less than zero")]
        public bool allowNegative;
        public string GetDesc()
        {
            return description.Replace("\\n", "\n");
        }
    }
    [Serializable]
    public class StanceCont
    {
        public Stance stance;
        public GameObject prefab;
        public string description;
        [Tooltip("showing basic number")]
        public int charge;
    }
    [Serializable]
    public class IntentCont
    {
        public AbilityType[] type;
        public GameObject prefab;
    }
    public enum ReduceChargeTime{onEndTurn, onHitByAttack}
}