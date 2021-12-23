using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataContainer
{
    [CreateAssetMenu(fileName = "CreatureData", menuName = "Creature/new Creature")]
    public class CreatureDataContainer : ScriptableObject
    {
        public int MaxHealth;
        public int MaxStamina;
        public string[] Abilities;
        public Sprite Skin;
    }
}

