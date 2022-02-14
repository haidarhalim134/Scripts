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
                this.MoveY(25, 0.1f);
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
        // TODO: fix this
        public void SemiHighlight(bool to)
        {
            if (deck.ActiveCard == null&&enableHover)
            {
                if (to)
                {
                    deck.isCardHovered = this;
                    // this.animator.SetBool("Active", true);
                    // this.Magnify(true);
                    deck.SemiHighlightCard(this);
                    this.transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), 0.1f);
                    this.MoveY(25, 0.1f);
                    this.gameObject.transform.SetAsLastSibling();
                }
                else if (!this.Active&&!currMove)
                {
                    if (deck.isCardHovered == this)deck.isCardHovered = null;
                    // this.animator.SetBool("Active", false);
                    // this.Magnify(false);
                    this.UpdateText();
                    this.transform.SetAsLastSibling();
                    transform.DOScaleX(1,0f).OnComplete(()=>{if (deck.isCardHovered == null) deck.RefreshCardPos();});
                    
                }
            }
        }
        public void AddMoveTarget(Vector2 to, float duration = 0.3f,bool overrideTarget = false)
        {
            this.transform.DOLocalMove(to, duration).OnComplete(()=>enableHover = true).SetId(this);
        }
        public void MoveY(float y, float duration)
        {
            if (!currMove)
            {
                currMove = true;
                var pos = this.transform.localPosition;
                pos.y = y;
                this.DOKill(this);
                this.transform.DOLocalMove(pos, duration).OnComplete(()=>currMove = false);
                // var pos = this.transform.localPosition;
                // pos.y = y;
                // this.transform.localPosition = pos;
            }
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