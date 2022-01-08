using System;
using System.Collections.Generic;
using UnityEngine;
using Attributes.Player;
using Attributes.Abilities;
using Tree = Map.Tree;

namespace Control.Core
{
    public class LoadedSave
    {
        public static SaveFile tmp = new SaveFile();
    };
    [Serializable]
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
        public Dictionary<Act,ActCont> act = new Dictionary<Act, ActCont>();
    }
    public enum Act{ Act1, Act2 }
    public class ActCont
    {
        public bool finished = false;
        public Tree tree = null;
    }
}