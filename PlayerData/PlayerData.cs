using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attributes.Abilities;

namespace Attributes.Player
{
    public class PlayerData : MonoBehaviour
    {
        public static PlayerDataContainer One = new PlayerDataContainer() { MaxHealth = 100, MaxStamina = 3, FullDeck = new List<AbilityContainer>(){
            new AbilityContainer(){name="OneAttack", GUID="XD"},new AbilityContainer(){name="OneAttack", GUID="XD"},new AbilityContainer(){name="ShieldUp", GUID="XD"},
            new AbilityContainer(){name="TwoAttack", GUID="XD"},new AbilityContainer(){name="TwoAttack", GUID="XD"}
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

