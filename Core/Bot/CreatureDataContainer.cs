using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attributes.Abilities;
using UnityEngine.AddressableAssets;

namespace DataContainer
{
    [CreateAssetMenu(fileName = "CreatureData", menuName = "Creature/new Creature")]
    public class CreatureDataContainer : ScriptableObject
    {
        public int MaxHealth;
        public int MaxStamina;
        public AbilityContainer[] Abilities;
        public Sprite Skin;
    }
}

