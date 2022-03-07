using System;
using System.Collections.Generic;
using UnityEngine;
using Attributes.Abilities;

namespace DataContainer
{
    [CreateAssetMenu(fileName = "CreatureData", menuName = "new Creature")]
    public class CreatureDataContainer : ScriptableObject
    {
        public int MaxHealth;
        public int MaxStamina;
        [Tooltip("chance to get 2 stamina")]
        [Range(0,1)]
        public float ChanceForTwo;
        public BotAbilityCont[] Abilities;
        public AttackPatternCont[] attackPattern;
        public List<Relic> powers;
        public Sprite Skin;
    }
    [Serializable]
    public class AttackPatternCont
    {
        public BotAbContWeight[] abilities;
    }
    [Serializable]
    public class BotAbContWeight : BotAbilityCont
    {
        public int weight;
    }
}

