using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attributes.Abilities;

namespace DataContainer
{
    [CreateAssetMenu(fileName = "CreatureData", menuName = "Creature/new Creature")]
    public class CreatureDataContainer : ScriptableObject
    {
        public int MaxHealth;
        public int MaxStamina;
        [Tooltip("chance to get 2 stamina")]
        [Range(0,1)]
        public float ChanceForTwo;
        public BotAbilityCont[] Abilities;
        public Sprite Skin;
    }
}

