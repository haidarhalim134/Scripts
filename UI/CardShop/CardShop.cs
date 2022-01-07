using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attributes.Abilities;
using Control.UI;

namespace Control.Core
{
    public class CardShop : MonoBehaviour
    {
        List<AbilityContainer> Cards;
        public GameObject Prefab;
        public GameObject Grid;
        void Awake()
        {

        }
        /// <summary>pass an on click method if there is any</summary>
        public void Enable(List<AbilityContainer> Abilities)
        {
            foreach (AbilityContainer cont in Abilities)
            {
                GameObject Object = Instantiate(this.Prefab, this.Grid.transform);
                ShopItem Script = Object.GetComponent<ShopItem>();
                Script.Ability = cont;
                Script.Init();
            }
            this.gameObject.SetActive(true);
        }
        public void Disable()
        {
            this.gameObject.SetActive(false);
            foreach (Transform child in this.Grid.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}