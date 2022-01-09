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
        public GameObject Card;
        public AbilityContainer Ability;
        public void Init()
        {
            Card.GetComponent<Button>().onClick.AddListener(this.onClick);
            CardHandlerVisual Script = Card.GetComponent<CardHandlerVisual>();
            Script.Ability = Ability;
            Script.UpdateText();
            Cost.text = Ability.GetManager().GoldCost.ToString();
        }
        void onClick()
        {
            if (Ability.GetManager().GoldCost<= LoadedSave.Loaded.Gold)
            {
                LoadedSave.Loaded.Player.CardAdd(this.Ability);
                Destroy(this.gameObject);
            }
        }
    }
}