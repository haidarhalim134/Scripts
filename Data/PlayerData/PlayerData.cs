using System;
using Random = System.Random;
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
        public int MaxHealth;
        public int MaxStamina;
        public int initialCardNumber;
        public List<AbilityContainer> FullDeck;
        public void CardAdd(AbilityContainer Ability)
        {
            this.FullDeck.Add(Ability);
            this.DeckShuffle();
        }
        public void DeckShuffle()
        {
            this.FullDeck.Shuffle<AbilityContainer>(true);
        }
    }
}

