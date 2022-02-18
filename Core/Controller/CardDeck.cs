using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Attributes.Abilities;
using Control.Core;
using Control.Combat;
using static UnityEngine.Random;
using DG.Tweening;
using DataContainer;

namespace Control.Deck
{
    public class CardDeck : MonoBehaviour
    {
        public List<CardHandler> ActiveDeck = new List<CardHandler>();
        public GameObject CardPrefab;
        public PlayerController Owner;
        public CardHandler isCardHovered;
        public int CardSep;
        public int DeckCurve;
        public int maxCardSep;
        public int maxDeckCurve;
        [Tooltip("used as card enter route")]
        public GameObject ReserveDeck;
        [Tooltip("used as card exit route")]
        public GameObject UsedDeck;
        public CardHandler ActiveCard;
        /// <summary>auto refresh before highlighting</summary>
        public void HighlightCard(CardHandler Card)
        { 
            int deckSep = CalcBetweenNumber(ActiveDeck.Count, 10, maxCardSep, CardSep);
            List<float> PosX = this.CalcCardsXPos(deckSep, this.ActiveDeck.Count);
            int Index = this.ActiveDeck.IndexOf(Card);
            this.ActiveCard = Card;
            this.RefreshCardPos(Index, PosX[Index]);
            ChangeCardRaycast(false);
        }public void SemiHighlightCard(CardHandler Card)
        {
            int deckSep = CalcBetweenNumber(ActiveDeck.Count, 10, maxCardSep, CardSep);
            List<float> PosX = this.CalcCardsXPos(deckSep, this.ActiveDeck.Count);
            int Index = this.ActiveDeck.IndexOf(Card);
            this.RefreshCardPos(Index, PosX[Index]);
        }
        public void ChangeCardRaycast(bool to)
        {
            ActiveDeck.ForEach((card)=>card.GetComponent<Image>().raycastTarget = to);
        }
        public void updateActiveCardText(PlayerController caster = null, BaseCreature target = null)
        {
            if (this.ActiveCard!=null)
            {
                CardHandler script = this.ActiveCard;
                if (caster!=null)script.UpdateText(caster,target);
                else script.UpdateText();
            }
        }
        public void ClearHighlight()
        {
            this.Owner.AbilityClearOrder();
            this.ActiveCard = null;
            this.ChangeCardRaycast(true);
            this.RefreshCardPos();
        }
        /// <summary>ClickedCardInit refer to x position</summary>
        public void RefreshCardPos(int ClickedCard = -1, float ClickedCardInit = -1f)
        {
            int deckCurve = CalcBetweenNumber(ActiveDeck.Count, 10, DeckCurve, maxDeckCurve);
            int deckSep = CalcBetweenNumber(ActiveDeck.Count, 10, maxCardSep, CardSep);
            List<float> PosX = this.CalcCardsXPos(deckSep, this.ActiveDeck.Count, ClickedCard, ClickedCardInit);
            for (int i = 0; i < PosX.Count;i++)
            {
                float PosY = CalcCardsYPos(deckCurve, PosX[i]);
                float Angle = CalcCardAngle(deckCurve, PosX[i], PosY);
                // get component => set target
                CardHandler Script = ActiveDeck[i];
                RectTransform rect = this.ActiveDeck[i].GetComponent<RectTransform>();      
                if (i != ClickedCard)
                {
                    Script.AddMoveTarget(new Vector2(PosX[i], PosY - deckCurve),InGameContainer.GetInstance().delayBetweenTurn*0.3f);
                    Script.Highlight(false);
                    this.ActiveDeck[i].transform.rotation = Quaternion.Euler(0, 0, Angle);
                }
            }
        }
        public void AddCard(AbilityContainer Ability)
        {
            Vector2 spawnPlace = (Vector2)this.ReserveDeck.gameObject.transform.position - new Vector2(0f, 1f);
            GameObject Card = Instantiate(this.CardPrefab, spawnPlace, new Quaternion(), gameObject.transform);
            CardHandler Script = Card.GetComponent<CardHandler>();
            Script.SmallToBig();
            Script.Ability = Ability;
            Script.UpdateText();
            Script.TargetOwner = Owner.gameObject;
            Script.InitOwner();
            Script.Exit = this.UsedDeck.transform.position;
            Script.deck = this;
            this.ActiveDeck.Add(Script);
            this.RefreshCardPos();
        }
        public IEnumerator RefillReserve()
        {
            float totalTime = InGameContainer.GetInstance().delayBetweenTurn*0.75f;
            float cardNum = this.Owner.UsedDeck.Count;
            Tween spawn()
            {
                Vector2 spawnPlace = (Vector2)this.UsedDeck.gameObject.transform.position;
                // spawnPlace.y += 50;
                Vector2 exitPlace = this.ReserveDeck.gameObject.transform.position;
                // exitPlace.y-= 50;
                GameObject Card = Instantiate(this.CardPrefab, spawnPlace, new Quaternion(), gameObject.transform);
                Card.GetComponent<CardHandler>().enableHover = false;
                Tween res = Card.transform.DOMoveX(exitPlace.x, totalTime)
                .OnComplete(() =>
                {
                    int RandIndex = Range(0, this.Owner.UsedDeck.Count);
                    this.Owner.ReserveDeck.Add(this.Owner.UsedDeck[RandIndex]);
                    this.Owner.UsedDeck.RemoveAt(RandIndex);
                    Destroy(Card);
                }).SetEase(Ease.Linear);
                // Card.transform.DOMoveY(exitPlace.y + 50, totalTime).SetEase(Ease.OutQuad);
                // Card.transform.DOMoveY(exitPlace.y, totalTime / 2f).SetDelay(totalTime / 2f).SetEase(Ease.InQuad);
                Card.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                Card.transform.DOScale(1, totalTime);
                Card.transform.DOScale(0.3f, totalTime / 2f).SetDelay(totalTime / 2f);
                totalTime += this.Owner.CardOutSpeed * 5 / cardNum;
                return res;
            }
            for (var i = 0;i<this.Owner.UsedDeck.Count-1;i++)
            {
                spawn();
            }
            var wait = spawn();
            yield return wait.WaitForCompletion();
            yield return new WaitForSeconds(0.1f);
        }
        public void RemoveActiveCard()
        {
            this.ActiveDeck.Remove(this.ActiveCard);
            CardHandler handler = this.ActiveCard;
            handler.animator.SetBool("Active", false);
            handler.Destroy(RemoveStatus.used);
            // Destroy(this.ActiveCard);
            this.RefreshCardPos();
            this.ActiveCard = null;
            ChangeCardRaycast(true);
        }
        public void ClearDeck()
        {
            foreach (CardHandler Card in this.ActiveDeck)
            {
                this.Owner.DeckMoveToUsed(Card.Ability);
                Card.Destroy(RemoveStatus.discard);
                // Destroy(Card);
            }
            this.ActiveDeck.Clear();
        }
        int CalcBetweenNumber(int currCard, int cardMax, int min, int max)
        {
            int res = (int)Math.Floor((currCard/(float)cardMax)*(max-min)+min);
            return res;
        }
        private float CalcCardsYPos(int DeckCurve, float X)
        {
            int Curve = DeckCurve*DeckCurve;
            return (float)Math.Sqrt(Curve-X*X);
        }
        private float CalcCardAngle(int DeckCurve, float x, float y)
        {
            return (float)(Math.Asin(x/DeckCurve)*(180 / Math.PI))*-0.5f;
        }
        /// <summary>Clicked card order(start from 0), make sure CardCount not 0</summary>
        private List<float> CalcCardsXPos(int CardSep, int CardCount, int ClickedCard = -1, float ClickedCardInit = -1, float MidPoint = 0)
        {
            List<float> Positions = new List<float>();
            float point;
            if (ClickedCard!=-1)
            {
                Positions.Add(ClickedCardInit);
                for (int i = ClickedCard - 1;i>= 0;i--)
                {
                    point = Positions[0] - CalcSep(CardSep,
                     Convert.ToInt32(i), CardCount, ClickedCard);
                    Positions.Insert(0, point);
                }
                for (int i = ClickedCard + 1;i < CardCount;i++)
                {
                    point = Positions[Positions.Count-1] + CalcSep(CardSep,
                     Convert.ToInt32(i), CardCount, ClickedCard);
                    Positions.Add(point);
                }
            } 
            else if (CardCount%2==1)
            {
                Positions.Add(MidPoint);
                decimal length = (CardCount+1)/2;
                for (int i = 1;i<Math.Floor(length);i++)
                {
                    point = Positions[Positions.Count-1] + CardSep;
                    Positions.Add(point);
                    point = Positions[0] - CardSep;
                    Positions.Insert(0, point);
                }
            } 
            else 
            {
                Positions.Add(MidPoint-CardSep/2);Positions.Add(MidPoint+CardSep/2);
                decimal length = (CardCount+1)/2;
                for (int i = 1;i<Math.Floor(length);i++)
                {
                    point = Positions[Positions.Count-1] + CardSep;
                    Positions.Add(point);
                    point = Positions[0] - CardSep;
                    Positions.Insert(0, point);
                }
            }
            return Positions;
        }
        /// <summary>target card start from 0</summary>
        private float CalcSep(int BaseSep, int TargetCard, int CardCount, int ClickedCard = -1)
        {
            if (ClickedCard!= -1)
            {
                decimal Count = CardCount;
                int Median = Convert.ToInt32(Math.Floor(Count/2));
                int StartPoint = Math.Abs(-1 * ClickedCard + TargetCard);
                float SepMlt = (float)(1/Math.Pow(StartPoint,1/1.1));
                return Math.Max(BaseSep*0.99f,BaseSep * SepMlt * 1.3f);
            }
            return BaseSep;
        }
        // Start is called before the first frame update
        void Start()
        {

        }
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                this.ClearHighlight();
                CombatEngine.ClearTarget();
            }
        }
    }
    public enum RemoveStatus { discard, used };
}
