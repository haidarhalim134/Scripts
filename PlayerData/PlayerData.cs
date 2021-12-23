using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attributes.Abilities;

namespace Attributes.Player
{
    public class PlayerData : MonoBehaviour
    {
        public static PlayerDataContainer One = new PlayerDataContainer() { MaxHealth = 100, MaxStamina = 3, FullDeck = new List<AbilityContainer>(){ 
            AbilitiesRepository.TwoAttack,AbilitiesRepository.TwoAttack,AbilitiesRepository.OneAttack,AbilitiesRepository.OneAttack,AbilitiesRepository.ShieldUp,AbilitiesRepository.ShieldUp
        }};
    }
    public class PlayerDataContainer
    {
        public int MaxHealth { get;set; }
        public int MaxStamina { get;set; }
        public List<AbilityContainer> FullDeck { get;set; }
    }
}

