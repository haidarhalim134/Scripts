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
        public CardDeck Deck;
        public bool Active = false;
        public Animator animator;
        public void OnClick() 
        {
            bool Sucess = this.Owner.OrderAbility(this.Ability);
            if (Sucess)
            {
                this.Deck.HighlightCard(this);
                this.Highlight(true);
            } else
            {
                
            }
        }
        public void Highlight(bool to)
        {
            if (to)
            {
                this.Active = true;
                this.animator.SetBool("Active", true);
                this.gameObject.transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), 0.1f);
                this.gameObject.transform.DOLocalMoveY(5 - this.GetComponent<RectTransform>().rect.height / 2, 0.1f);
                this.gameObject.transform.SetAsLastSibling();
            }else
            {
                this.Active = false;
                this.animator.SetBool("Active", false);
                this.gameObject.transform.SetAsLastSibling();
                this.UpdateText();
            }
        }
        public void AddMoveTarget(Vector2 to, float duration = 0.1f,bool overrideTarget = false)
        {
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