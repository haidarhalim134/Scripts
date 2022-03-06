using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Attributes.Abilities;
using Control.UI;

namespace Control.Core
{
    public class ShopItem : MonoBehaviour
    {
        public TextMeshProUGUI Cost;
        public CardHandlerVisual Card;
        public CardShopCont item;
        public void Init(CardShopCont cont)
        {
            item = cont;
            Cost.text = "a"+cont.cost;
            CardHandlerVisual Script = Card.GetComponent<CardHandlerVisual>();
            Script.Ability = item.item;
            Script.UpdateText();
        }
        public void onClick()
        {
            if (item.cost<= Loaded.loaded.Gold)
            {
                Loaded.loaded.Gold-= item.cost;
                Loaded.loaded.Player.CardAdd(item.item);
                Loaded.loaded.queuedCardShop.queue.Remove(item);
                Destroy(Card.gameObject);
                Destroy(Cost.gameObject);
            }
        }
    }
}