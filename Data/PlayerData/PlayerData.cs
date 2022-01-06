using System;
using System.Collections.Generic;
using UnityEngine;
using Attributes.Abilities;

namespace Attributes.Player
{
    public class PlayerData
    {
        public static PlayerDataContainer One = new PlayerDataContainer() { 
            MaxHealth = 100, 
            MaxStamina = 3, 
            FullDeck = new List<AbilityContainer>(){
            new AbilityContainer(){name="OneAttack", Data=new AbilityData()},new AbilityContainer(){name="TwoAttack", Data=new AbilityData()},
            new AbilityContainer(){name="ShieldUp", Data=new AbilityData()},
            new AbilityContainer(){name="OneAttack", Data=new AbilityData()},new AbilityContainer(){name="TwoAttack", Data=new AbilityData()},
            new AbilityContainer(){name="ShieldUp", Data=new AbilityData()},
            new AbilityContainer(){name="OneAttack", Data=new AbilityData()},new AbilityContainer(){name="TwoAttack", Data=new AbilityData()},
            new AbilityContainer(){name="ShieldUp", Data=new AbilityData()},
            new AbilityContainer(){name="OneAttack", Data=new AbilityData()},new AbilityContainer(){name="TwoAttack", Data=new AbilityData()},
            new AbilityContainer(){name="ShieldUp", Data=new AbilityData()},
        }};
    }
    [Serializable]
    public class PlayerDataContainer
    {
        public int MaxHealth { get;set; }
        public int MaxStamina { get;set; }
        public List<AbilityContainer> FullDeck { get;set; }
    }
}

