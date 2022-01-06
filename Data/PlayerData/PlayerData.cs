using System;
using System.Collections.Generic;
using UnityEngine;
using Attributes.Abilities;

namespace Attributes.Player
{
    public class PlayerData
    {
    }
    [Serializable]
    public class PlayerDataContainer
    {
        public int MaxHealth { get;set; }
        public int MaxStamina { get;set; }
        public List<AbilityContainer> FullDeck { get;set; }
    }
}

