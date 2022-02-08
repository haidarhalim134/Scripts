using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
        public CardDeck deck;
        public bool Active = false;
        public Animator animator;
        public void OnClick() 
        {
            bool Sucess = this.Owner.OrderAbility(this.Ability);
            if (Sucess)
            {
                this.deck.HighlightCard(this);
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
                // this.animator.SetBool("Active", true);
                this.Magnify(true);
                this.transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), 0.1f);
                this.transform.DOLocalMoveY(10 - this.GetComponent<RectTransform>().rect.height / 2, 0.1f);
                this.transform.SetAsLastSibling();
            }else
            {
                this.Active = false;
                // this.animator.SetBool("Active", false);
                this.Magnify(false);
                this.transform.SetAsLastSibling();
                this.UpdateText();
            }
        }
        public void SemiHighlight(bool to)
        {
            if (deck.ActiveCard == null&&enableHover)
            {
                if (to)
                {
                    // this.animator.SetBool("Active", true);
                    this.Magnify(true);
                    deck.SemiHighlightCard(this);
                    this.transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), 0.1f);
                    this.transform.DOLocalMoveY(10 - this.GetComponent<RectTransform>().rect.height / 2, 0.1f);
                    this.gameObject.transform.SetAsLastSibling();
                }
                else if (!this.Active)
                {
                    // this.animator.SetBool("Active", false);
                    this.Magnify(false);
                    this.UpdateText();
                    this.transform.SetAsLastSibling();
                    deck.RefreshCardPos();
                }
            }
        }
        public void AddMoveTarget(Vector2 to, float duration = 0.1f,bool overrideTarget = false)
        {
            this.transform.DOLocalMove(to, duration).OnComplete(()=>enableHover = true);
            // this.MoveTarget.Add(new Move(gameObject.transform.localPosition, to, duration));
        }
        public void InitOwner()
        {
            this.Owner = this.TargetOwner.GetComponent<PlayerController>();
            var trigger = this.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((eventData) => { SemiHighlight(true); });
            trigger.triggers.Add(entry);
            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerExit;
            entry.callback.AddListener((eventData) => { SemiHighlight(false); });
            trigger.triggers.Add(entry);
        }
        // Update is called once per frame
        void Update()
        {
           
        }
    }
}