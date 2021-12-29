using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attributes.Abilities;

namespace Attributes.Player
{
    public class PlayerData : MonoBehaviour
    {
        public static PlayerDataContainer One = new PlayerDataContainer() { MaxHealth = 100, MaxStamina = 3, FullDeck = new List<AbilityContainer>(){ 
            OneAttack,OneAttack,OneAttack,OneAttack,OneAttack
        }};
        public static AbilityContainer OneAttack = new AbilityContainer(){name="OneAttack", GUID="XD"};
    }
    public class PlayerDataContainer
    {
        public int MaxHealth { get;set; }
        public int MaxStamina { get;set; }
        public List<AbilityContainer> FullDeck { get;set; }
    }
}

