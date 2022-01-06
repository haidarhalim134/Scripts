using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attributes.Player;
using Attributes.Abilities;

namespace Control.Core
{
    public class LoadedSave
    {
        public static SaveFile tmp = new SaveFile();
    };
    public class SaveFile
    {
        public PlayerDataContainer Player = new PlayerDataContainer()
        {
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
            }
        };
        public int Gold = 500;
    }
}