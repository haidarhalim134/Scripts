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
        public PlayerDataContainer PlayerStats;
        public List<AbilityContainer> FullDeck = new List<AbilityContainer>();
        public List<AbilityContainer> ReserveDeck = new List<AbilityContainer>();
        public List<AbilityContainer> UsedDeck = new List<AbilityContainer>();
        private AbilityContainer OrderedAbility;
        public float CardOutSpeed = 0.1f;
        public int MaxDeckSize = 10;
        // TODO: removing this hardcoded assignment is preferred, can assign the instance directly
        public GameObject TEMP;
        public CardDeck Deck;
        public override void onTurn(bool to)
        {
            if (to)CombatEngine.SetupIntent();
        }
        /// <returns>true if order accepted else false</returns>
        public bool OrderAbility(AbilityContainer name)
        {
            AbilityManager Mng = name.GetManager();
            if (this.stamina.Enough(Mng.GetStaminaCost(name.Data)))
            {
                CombatEngine.SetupTarget(Mng);
                this.OrderedAbility = name;
                return true;
            } else
            {
                return false;
            }
        }
        public void AbilityClearOrder()
        {
            this.OrderedAbility = null;
        }
        public void AbilitySendOrdered(BaseCreature Target)
        {
            if (this.OrderedAbility != null && this.Control)
            {
                AbilityManager ability = this.OrderedAbility.GetManager();
                bool Success = CombatEngine.RequestCast(ability, this, Target, this.OrderedAbility.Data);
                if (Success)
                {
                    this.DeckRemoveActive(this.OrderedAbility);
                    CombatEngine.ClearTarget();
                    this.OrderedAbility = null;
                }
            }
        }
        override public void Setup(bool T = false)
        {
            if (T)
            {
            StartCoroutine(this.DeckInit());
            }
        }
        public IEnumerator DeckInit(int Card = 0)
        {
            yield return new WaitForSeconds(this.CardOutSpeed);   
            if (this.ReserveDeck.Count==0)
            {
                yield return this.DeckRefillReservedCard();
            }
            DeckAddTo(0);
            if (Card<4)
            {
                StartCoroutine(DeckInit(Card+1));
            }
        }
        public void DeckAddTo(int index)
        {
            if (this.Deck.ActiveDeck.Count<MaxDeckSize)
            {
                this.Deck.AddCard(this.ReserveDeck[index]);
                this.ReserveDeck.RemoveAt(index);
            }
        }
        /// <summary>called by player instance then order its deck to also remove it</summary>
        public void DeckRemoveActive(AbilityContainer Ability)
        {
            this.Deck.RemoveActiveCard();
            this.UsedDeck.Add(Ability);
        }
        public void DeckMoveToUsed(AbilityContainer Ability)
        {
            this.UsedDeck.Add(Ability);
        }
        private IEnumerator DeckRefillReservedCard()
        {
            yield return StartCoroutine(this.Deck.RefillReserve());
        }
        override public void OnDeath()
        {
            CombatEngine.EndGame(false);
        }
        void Awake() {
            this.BaseInit();
            this.IsPlayer = true;
            this.PlayerStats = LoadedSave.Loaded.Player;
            this.stamina.Max = this.PlayerStats.MaxStamina;
            this.health.Max = this.PlayerStats.MaxHealth;
            this.FullDeck = new List<AbilityContainer>(this.PlayerStats.FullDeck);
            this.ReserveDeck = new List<AbilityContainer>(this.PlayerStats.FullDeck);
            this.TeamId = 0;
            this.EnemyId = 1;
            CombatEngine.RegisterCreature(this, true);
            this.health.Fill();
            this.Deck = this.TEMP.GetComponent<CardDeck>();
        }
        // Start is called before the first frame update
        void Start()
        {

        }
        public void FinishTurn()
        {
            this.DebuffReduceCharge();
            CombatEngine.ActionFinished();
            this.Deck.ClearDeck();
            this.Control = false;
        }
    }
}