using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Attributes.Abilities;
using Control.Core;
using Control.Combat;

namespace Control.Deck
{
    public class CardDeck : MonoBehaviour
    {
        List<GameObject> ActiveDeck = new List<GameObject>();
        public GameObject CardPrefab;
        public PlayerController Owner;
        public int CardSep = 20;
        public int DeckCurve = 1000;
        GameObject ActiveCard;
        /// <summary>auto refresh before highlighting</summary>
        public void HighlightCard(GameObject Card)
        {         
            List<float> PosX = this.CalcCardsXPos(this.ActiveDeck.Count);
            int Index = this.ActiveDeck.IndexOf(Card);
            this.ActiveCard = Card;
            this.RefreshCardPos(Index, PosX[Index]);
        }
        /// <summary>ClickedCardInit refer to x position</summary>
        public void RefreshCardPos(int ClickedCard = -1, float ClickedCardInit = -1f)
        {
            List<float> PosX = this.CalcCardsXPos(this.ActiveDeck.Count, ClickedCard, ClickedCardInit);
            for (int i = 0; i < PosX.Count;i++)
            {
                float PosY = CalcCardsYPos(PosX[i]);
                float Angle = CalcCardAngle(PosX[i], PosY);
                // TODO: keep an eye on this until the card prefab choice is settled, inc. the vector target below
                // this.ActiveDeck[i].transform.parent = this.gameObject.transform;
                // this.ActiveDeck[i].transform.SetParent(this.gameObject.transform);
                // get component => set target
                CardHandler Script = ActiveDeck[i].GetComponent<CardHandler>();
                RectTransform rect = this.ActiveDeck[i].GetComponent<RectTransform>();
                Script.AddMoveTarget(new Vector2(PosX[i] - rect.rect.width/2,PosY-this.DeckCurve - rect.rect.height/2));
                this.ActiveDeck[i].transform.rotation = Quaternion.Euler(0,0,Angle);
            }
        }
        public void AddCard(AbilityContainer Ability)
        {
            GameObject Card = Instantiate(this.CardPrefab, new Vector2(), new Quaternion(), gameObject.transform) as GameObject;
            CardHandler Script = Card.GetComponent<CardHandler>();
            Script.Ability = Ability;
            Script.UpdateText();
            Script.TargetOwner = Owner.gameObject;
            Script.InitOwner();
            Script.TheDeck = this;
            //TODO: remove this temp card color identifier later
            // Image tmpcolorid = Card.GetComponent<Image>();
            // tmpcolorid.color = Ability==AbilitiesRepository.OneAttack?Color.blue:Ability==AbilitiesRepository.TwoAttack?Color.red:Color.gray;
            this.ActiveDeck.Add(Card);
            this.RefreshCardPos();
        }
        public void RemoveActiveCard()
        {
            this.ActiveDeck.Remove(this.ActiveCard);
            this.ActiveCard.GetComponent<CardHandler>().Destroy();
            // Destroy(this.ActiveCard);
            this.RefreshCardPos();
            this.ActiveCard = null;
        }
        public void ClearDeck()
        {
            foreach (GameObject Card in this.ActiveDeck)
            {
                this.Owner.MoveToUsedDeck(Card.GetComponent<CardHandler>().Ability);
                Card.GetComponent<CardHandler>().Destroy();
                // Destroy(Card);
            }
            this.ActiveDeck.Clear();
        }
        private float CalcCardsYPos(float X)
        {
            int Curve = this.DeckCurve*this.DeckCurve;
            return (float)Math.Sqrt(Curve-X*X);
        }
        private float CalcCardAngle(float x, float y)
        {
            return (float)(Math.Asin(x/this.DeckCurve)*(180 / Math.PI))*-0.5f;
        }
        /// <summary>Clicked card order(start from 0), make sure CardCount not 0</summary>
        private List<float> CalcCardsXPos(int CardCount, int ClickedCard = -1, float ClickedCardInit = -1, float MidPoint = 0)
        {
            List<float> Positions = new List<float>();
            float point;
            if (ClickedCard!=-1)
            {
                Positions.Add(ClickedCardInit);
                for (int i = ClickedCard - 1;i>= 0;i--)
                {
                    point = Positions[0] - CalcSep(this.CardSep,
                     Convert.ToInt32(i), CardCount, ClickedCard);
                    Positions.Insert(0, point);
                }
                for (int i = ClickedCard + 1;i < CardCount;i++)
                {
                    point = Positions[Positions.Count-1] + CalcSep(this.CardSep,
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
                    point = Positions[Positions.Count-1] + this.CardSep;
                    Positions.Add(point);
                    point = Positions[0] - this.CardSep;
                    Positions.Insert(0, point);
                }
            } 
            else 
            {
                Positions.Add(MidPoint-this.CardSep/2);Positions.Add(MidPoint+this.CardSep/2);
                decimal length = (CardCount+1)/2;
                for (int i = 1;i<Math.Floor(length);i++)
                {
                    point = Positions[Positions.Count-1] + this.CardSep;
                    Positions.Add(point);
                    point = Positions[0] - this.CardSep;
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
                float SepMlt = (float)(1/Math.Sqrt(StartPoint));
                return BaseSep * SepMlt * 1.5f;
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                this.RefreshCardPos();
            }
        }
    }

}
