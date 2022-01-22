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
        public BotAbilityCont[] Abilities;
        public Sprite Skin;
    }
}

