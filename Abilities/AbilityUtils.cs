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
    public class AbilityData
    {
        public int Level;
        public int Damage;
        public int Shield;
        // TODO: either copy the container or create non serializable field called temp
        public AbilityData Copy()
        {
            return new AbilityData(){ Level=Level, Damage=Damage, Shield=Shield };
        }
    }

    [Serializable]
    public class BotAbilityCont
    {
        public GameObject Ability;
        public AbilityData Data;
    }
    public enum Debuffs{vulnerable, weakened}
}