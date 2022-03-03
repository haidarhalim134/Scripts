using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Combat;
using Control.Deck;
using Attributes.Abilities;
using Attributes.Player;
using DataContainer;
using static UnityEngine.Random;

namespace Control.Core
{
    public class PlayerController : BaseCreature
    {
        public PlayerDataContainer PlayerStats;
        public List<AbilityContainer> FullDeck = new List<AbilityContainer>();
        public List<AbilityContainer> ReserveDeck = new List<AbilityContainer>();
        public List<AbilityContainer> UsedDeck = new List<AbilityContainer>();
        public List<AbilityContainer> ExhaustedDeck = new List<AbilityContainer>();
        public AbilityContainer OrderedAbility;
        public float CardOutSpeed = 0.1f;
        public int MaxDeckSize = 10;
        public CardDeck Deck {get{return InGameContainer.GetInstance().currDeck; }}
        public EndTurnButton endturnButton;
        /// <returns>true if order accepted else false</returns>
        public bool OrderAbility(AbilityContainer name, bool setupTarget = true)
        {
            AbilityManager Mng = name.GetManager();
            if (this.stamina.Enough(Mng.GetStaminaCost(name.Data)))
            {
                this.OrderedAbility = name;
                if (setupTarget) CombatEngine.SetupTarget(Mng);
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
                    CombatEngine.ClearTarget();
                    lastPlayedAbility = OrderedAbility;
                    this.OrderedAbility = null;
                    buffDebuff.Activate(buffDebuff.attackPlayActivate, lastPlayedAbility);
                }
            }
        }
        public IEnumerator DeckInit(int howMany)
        {
            int card = 0;
            endturnButton.Disable();
            while (card<howMany)
            {
                yield return StartCoroutine(DeckAddTo(0));
                yield return new WaitForSeconds(this.CardOutSpeed); 
                card+= 1;
            }
            StartCoroutine(FinishedTweening());
            IEnumerator FinishedTweening()
            {
                while (true)
                {
                    if (!Deck.AnyCardTweening())break;
                    yield return new WaitForSeconds(0.25f);
                }
                endturnButton.Enable();
            }
        }
        public IEnumerator DeckAddTo(int index)
        {
            if (ReserveDeck.Count<=0)yield return StartCoroutine(DeckRefillReservedCard());
            if (this.Deck.ActiveDeck.Count<MaxDeckSize)
            {
                this.Deck.AddCard(this.ReserveDeck[index]);
                this.ReserveDeck.RemoveAt(index);
            }
        }
        public void DeckMoveToUsed(AbilityContainer Ability)
        {
            this.UsedDeck.Add(Ability);
        }
        public void DeckMoveToExhausted(AbilityContainer Ability)
        {
            this.ExhaustedDeck.Add(Ability);
        }
        private IEnumerator DeckRefillReservedCard()
        {
            yield return StartCoroutine(this.Deck.RefillReserve());
        }
        override public void Setup(bool T = false)
        {
            if (T)
            {
                Deck.ChangeCardRaycast(true);
                StartCoroutine(setup());
            }else
            {
                Deck.ChangeCardRaycast(false);
                endturnButton.Disable();
            }
            IEnumerator setup()
            {
                yield return new WaitForSeconds(InGameContainer.GetInstance().delayBetweenTurn/2);
                StartCoroutine(this.DeckInit(PlayerStats.initialCardNumber));
                CombatEngine.SetupIntent();
            }
        }
        override public void OnDeath()
        {
            CombatEngine.EndGame(false);
        }
        void Awake() {
            this.BaseInit();
            this.IsPlayer = true;
            this.PlayerStats = Loaded.loaded.Player;
            this.stamina.Max = this.PlayerStats.MaxStamina;
            this.health.Max = this.PlayerStats.MaxHealth;
            this.FullDeck = new List<AbilityContainer>(this.PlayerStats.FullDeck);
            this.ReserveDeck = new List<AbilityContainer>(this.PlayerStats.FullDeck);
            this.TeamId = 0;
            this.EnemyId = 1;
            CombatEngine.RegisterCreature(this, true);
            this.health.Fill();
            IEnumerator updater()
            {
                while (true)
                {
                    Deck.UpdateAllCardText(this);
                    yield return new WaitForSeconds(0.1f);
                }
            }
            StartCoroutine(updater());
        }
        public void FinishTurn()
        {
            this.DebuffReduceCharge(ReduceChargeTime.onEndTurn);
            CombatEngine.ActionFinished();
            this.Deck.ClearDeck();
            this.Control = false;
        }
    }
}