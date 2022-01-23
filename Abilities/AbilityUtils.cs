using System;
using System.Collections.Generic;
using UnityEngine;
using DataContainer;

namespace Attributes.Abilities
{
    public class AbilityUtils : MonoBehaviour
    {
        
    }
    [Serializable]
    public class AbilityContainer
    {
        public AbilityData Data;
        public string name;
        public AbilityManager GetManager()
        {
            GameObject Object = GameObject.Find(name);
            if (Object == null)
            {
                InGameContainer.GetInstance().SpawnAbilityPrefab(name);
            }
            return GameObject.Find(name).GetComponent<AbilityManager>();
        }
    }
    [Serializable]
    public class BotAbilityCont
    {
        public GameObject Ability;
        public AbilityData Data;
        public AbilityContainer ToNormalContainer()
        {
            return new AbilityContainer(){name=Ability.GetComponent<AbilityManager>().AbName, Data=Data};
        }
    }
    [Serializable]
    public class AbilityData
    {
        public int Level = 1;
        public int Damage;
        public int Shield;
        // TODO: either copy the container or create non serializable field called temp
        public AbilityData Copy()
        {
            return new AbilityData(){ Level=Level, Damage=Damage, Shield=Shield };
        }
    }
    public enum Debuffs{vulnerable, weakened}
}