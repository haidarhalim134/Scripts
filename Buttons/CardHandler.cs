using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Control.UI;
using DG.Tweening;
using Attributes.Abilities;

namespace Control.Deck
{
    public class CardHandler : CardHandlerVisual
    {
        public GameObject TargetOwner;
        PlayerController Owner;
        public CardDeck TheDeck;
        public bool Active = false;
        public void OnClick() 
        {
            bool Sucess = this.Owner.OrderAbility(this.Ability);
            if (Sucess)
            {
                this.TheDeck.HighlightCard(this.gameObject);
                this.Active = true;
            } else
            {
                
            }
        }
        public void AddMoveTarget(Vector2 to, float duration = 0.1f,bool overrideTarget = false)
        {
            if (overrideTarget)
            {
                this.MoveTarget.Clear();
            }
            this.transform.DOLocalMove(to, duration);
            // this.MoveTarget.Add(new Move(gameObject.transform.localPosition, to, duration));
        }
        public void InitOwner()
        {
            this.Owner = this.TargetOwner.GetComponent<PlayerController>();
        }
        // Update is called once per frame
        void Update()
        {
           
        }
    }
}