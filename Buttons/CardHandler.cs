using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Control.Core;
using Control.UI;
using DG.Tweening;
using Attributes.Abilities;
using DataContainer;
using Control.Combat;

namespace Control.Deck
{
    public class CardHandler : CardHandlerVisual, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
    {
        public GameObject TargetOwner;
        PlayerController Owner;
        public CardDeck deck;
        public bool Active = false;
        public bool stillDown;
        public bool beingDragged;
        public List<AbTarget> draggableAbil = new List<AbTarget>(){AbTarget.self, AbTarget.allEnemy};
        public void OnClick() 
        {
            bool Sucess = this.Owner.OrderAbility(this.Ability, false);
            var abil = InGameContainer.GetInstance().SpawnAbilityPrefab(Ability.name);
            if (Sucess)
            {
                if (draggableAbil.IndexOf(abil.target) == -1)
                {
                    this.deck.HighlightCard(this);
                    this.Highlight(true);
                    CombatEngine.SetupTarget(abil);
                }
            } else
            {
                
            }
        }
        public void Highlight(bool to)
        {
            if (to)
            {
                this.Active = true;
                this.Magnify(true);
                this.transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), 0.1f);
                this.MoveY(25, 0.1f);
                this.transform.SetAsLastSibling();
            }else
            {
                this.Active = false;
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
                    deck.isCardHovered = this;
                    deck.SemiHighlightCard(this);
                    this.transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), 0.1f);
                    this.MoveY(25, 0.1f);
                    this.gameObject.transform.SetAsLastSibling();
                }
                else if (!this.Active)
                {
                    if (deck.isCardHovered == this)deck.isCardHovered = null;
                    this.UpdateText();
                    this.transform.SetAsLastSibling();
                    transform.DOScaleX(1,0f).OnComplete(()=>{if (deck.isCardHovered == null) deck.RefreshCardPos();});
                    deck.RefreshCardPos();
                }
            }
            else if (deck.ActiveCard == this&&!beingDragged)
            {
                transform.DOLocalMoveX(0,0.1f);
                transform.DOLocalMoveY(10, 0.1f);
                Magnify(false);
            }
        }
        public void OnDrag(PointerEventData eventData) 
        {
            var abil = InGameContainer.GetInstance().SpawnAbilityPrefab(Ability.name);
            if (Owner.stamina.Curr < abil.GetStaminaCost(Ability.Data))return;
            if (draggableAbil.IndexOf(abil.target) != -1)
            {
                var UI = GameObject.Find("UI").GetComponent<RectTransform>();
                // Debug.Log(transform.position+"-"+eventData.position);
                transform.position+= (Vector3)eventData.delta*InGameContainer.defaultScreenWidth/Screen.width;
                if (Owner.OrderedAbility != Ability)
                {
                    Owner.OrderAbility(Ability);
                    deck.ActiveCard = this;
                    beingDragged = true;
                };
            }
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            var abil = InGameContainer.GetInstance().SpawnAbilityPrefab(Ability.name);
            if (draggableAbil.IndexOf(abil.target) == -1)return;
            beingDragged = false;
            if (transform.position.y < -20||Owner.stamina.Curr < abil.GetStaminaCost(Ability.Data))
            {
                Owner.AbilityClearOrder();
                deck.ActiveCard = null;
                deck.RefreshCardPos();
                CombatEngine.ClearTarget();
                return;
            }
            if (abil.target == AbTarget.self)
            {
                CombatEngine.SendTargetToPlayer(Owner);
            }
            else if (abil.target == AbTarget.allEnemy)
            {
                CombatEngine.SendTargetToPlayer(CombatEngine.GetRandomTarget(Owner.EnemyId));
            }
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (!enableHover) return;
            var abil = InGameContainer.GetInstance().SpawnAbilityPrefab(Ability.name);
            if (draggableAbil.IndexOf(abil.target) != -1) return;
            var raycastRes = Utils.RaycastMouse();
            var ifCreature = Utils.FindRayCastContaining<BaseCreature>(raycastRes);
            if (ifCreature == null) 
            {
                stillDown = false;
                return;
            }
            else
            {
                CombatEngine.SendTargetToPlayer(ifCreature);
            }
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!enableHover) return;
            var abil = InGameContainer.GetInstance().SpawnAbilityPrefab(Ability.name);
            if (draggableAbil.IndexOf(abil.target) != -1)return;
            stillDown = true;
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if (!enableHover)return;
            var abil = InGameContainer.GetInstance().SpawnAbilityPrefab(Ability.name);
            if (draggableAbil.IndexOf(abil.target) != -1) return;
            if (stillDown)OnClick();
        }
        public void AddMoveTarget(Vector2 to, float duration = 0.3f,bool overrideTarget = false)
        {
            this.transform.DOLocalMove(to, duration).OnComplete(()=>enableHover = true).SetId(this);
        }
        public void MoveY(float y, float duration)
        {
            var pos = this.transform.localPosition;
            pos.y = y;
            if (deck.isCardHovered == this) this.DOKill(this);
            this.transform.DOLocalMove(pos, duration);
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
        public void Destroy(RemoveStatus type)
        {
            enableHover = false;
            float totaltime = InGameContainer.GetInstance().delayBetweenTurn * 0.3f;
            if (type == RemoveStatus.discard)
            {
                // this.transform.DOMove((Vector2)this.transform.position+new Vector2(0,5), 0.5f)
                // .OnComplete(()=>this.transform.DOMove(this.Exit, 0.2f).SetEase(Ease.Linear)
                // .SetDelay(0.2f).OnComplete(()=>Destroy(this.gameObject)));
                this.transform.DOScale(0.3f, totaltime);
                this.transform.DORotate(new Vector3(), totaltime / 2f);
                this.transform.DOMoveY(this.usedExit.y, totaltime / 2f);
                this.transform.DOMoveX(this.usedExit.x, totaltime).SetEase(Ease.Linear)
                .OnComplete(() => {Destroy(this.gameObject); deck.UsedDeck.GetComponent<UsedDeckCounter>().Bump();});
                // Destroy(this.gameObject);
            }
            else if (type == RemoveStatus.used)
            {
                this.transform.DOScale(0.2f, totaltime);
                this.transform.DORotate(new Vector3(0, 0, -45), 0.1f);
                this.transform.DOMoveY(this.usedExit.y, totaltime).SetEase(Ease.InQuad);
                this.transform.DOMoveX(this.usedExit.x, totaltime).SetEase(Ease.Linear)
                .OnComplete(() => {Destroy(this.gameObject); deck.UsedDeck.GetComponent<UsedDeckCounter>().Bump();});
            }
            else if (type == RemoveStatus.exhausted)
            {
                this.transform.DOScale(0.2f, totaltime);
                this.transform.DORotate(new Vector3(0, 0, -45), 0.1f);
                this.transform.DOMoveY(this.exhaustExit.y, totaltime).SetEase(Ease.InQuad);
                this.transform.DOMoveX(this.exhaustExit.x, totaltime).SetEase(Ease.Linear)
                .OnComplete(() => { Destroy(this.gameObject); deck.ExhaustDeck.GetComponent<ExhaustDeckCounter>().Bump(); });
            }
            else
            {
                this.DOKill();
                Destroy(this.gameObject);
            }
        }
    }
}