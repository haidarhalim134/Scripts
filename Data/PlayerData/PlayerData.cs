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
        public int MaxStamina = 3;
        public List<AbilityContainer> FullDeck;
        public void CardAdd(AbilityContainer Ability)
        {
            this.FullDeck.Add(Ability);
            this.DeckShuffle();
        }
        public void DeckShuffle()
        {
            Random rng = new Random();
            int n = this.FullDeck.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                AbilityContainer value = this.FullDeck[k];
                this.FullDeck[k] = this.FullDeck[n];
                this.FullDeck[n] = value;
            }
        }
    }
}

