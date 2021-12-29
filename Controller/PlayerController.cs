using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Combat;
using Control.Deck;
using Attributes.Abilities;
using Attributes.Player;
using static UnityEngine.Random;

namespace Control.Core
{
    public class PlayerController : BaseCreature
    {
        public PlayerDataContainer PlayerStats = PlayerData.One;
        private List<AbilityContainer> FullDeck = new List<AbilityContainer>();
        private List<AbilityContainer> ReserveDeck = new List<AbilityContainer>();
        private List<AbilityContainer> UsedDeck = new List<AbilityContainer>();
        private AbilityContainer OrderedAbility;
        public Dictionary<string, string> Modifier = new Dictionary<string, string>();
        public float CardOutSpeed = 0.1f;
        // TODO: removing this hardcoded assignment is preferred, can assign the instance directly
        public GameObject TEMP;
        public CardDeck Deck;
        /// <returns>true if order accepted else false</returns>
        public bool OrderAbility(AbilityContainer name)
        {
            AbilityManager Mng = name.GetManager();
            if (this.stamina.Enough(Mng.cost))
            {
                CombatEngine.SetupTarget(Mng);
                this.OrderedAbility = name;
                return true;
            } else
            {
                return false;
            }
        }
        public void ClearOrder()
        {
            this.OrderedAbility = null;
        }
        public void SendOrderedAbility(BaseCreature Target)
        {
            if (this.OrderedAbility != null && this.Control)
            {
                AbilityManager ability = this.OrderedAbility.GetManager();
                bool Success = CombatEngine.RequestCast(ability, this, Target, this.OrderedAbility.GUID);
                if (Success)
                {
                    this.RemoveFromDeck(this.OrderedAbility);
                    this.OrderedAbility = null;
                }
            }
        }
        public void SetupUI(bool T = false)
        {
            if (T)
            {
            StartCoroutine(this.InitDeck());
            }
        }
        public IEnumerator InitDeck(int Card = 0)
        {
            yield return new WaitForSeconds(this.CardOutSpeed);   
            if (this.ReserveDeck.Count==0)
            {
                this.RefillReservedCard();
            }
            AddToDeck(0);
            if (Card<4)
            {
                StartCoroutine(InitDeck(Card+1));
            }
        }
        private void AddToDeck(int index)
        {
            this.Deck.AddCard(this.ReserveDeck[index]);
            this.ReserveDeck.RemoveAt(index);
        }
        /// <summary>called by player instance then order its deck to also remove it</summary>
        public void RemoveFromDeck(AbilityContainer Ability)
        {
            this.Deck.RemoveActiveCard();
            this.UsedDeck.Add(Ability);
        }
        public void MoveToUsedDeck(AbilityContainer Ability)
        {
            this.UsedDeck.Add(Ability);
        }
        private void RefillReservedCard()
        {
            while (this.UsedDeck.Count>0)
            {
                int RandIndex = Range(0,this.UsedDeck.Count);
                this.ReserveDeck.Add(this.UsedDeck[RandIndex]);
                this.UsedDeck.RemoveAt(RandIndex);
            }
        }
        void Awake() {
            this.BaseInit();
            this.IsPlayer = true;
            this.stamina.Max = this.PlayerStats.MaxStamina;
            this.health.Max = this.PlayerStats.MaxHealth;
            this.FullDeck = new List<AbilityContainer>(this.PlayerStats.FullDeck);
            this.ReserveDeck = new List<AbilityContainer>(this.PlayerStats.FullDeck);
            this.TeamId = 0;
            this.EnemyId = 1;
            this.Setup = this.SetupUI;
            CombatEngine.RegisterCreature(this, true);
            this.stamina.Fill();
            this.health.Fill();
            this.Deck = this.TEMP.GetComponent<CardDeck>();
        }
        // Start is called before the first frame update
        void Start()
        {

        }
        // Update is called once per frame
        void Update()
        {
            if (this.Control)
            {
                if (this.stamina.Curr<1)
                {
                    CombatEngine.ActionFinished();
                    this.Deck.ClearDeck();
                    this.Control = false;
                }
            }
        }
    }
}