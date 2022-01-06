using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Attributes.Abilities;
using Control.UI;

namespace Control.Core
{
    public class CardViewer : MonoBehaviour
    {
        List<AbilityContainer> Cards;
        public GameObject Prefab;
        public GameObject Grid;
        void Awake()
        {
            
        }
        /// <summary>pass an on click method if there is any</summary>
        public void Enable(List<AbilityContainer> Abilities, UnityAction OnClick = null)
        {
            GameObject Prefab = new GameObject("box");
            Prefab.AddComponent<RectTransform>();
            foreach (AbilityContainer cont in Abilities)
            {
                GameObject Parent = Instantiate(Prefab,this.Grid.transform);
                GameObject Object = Instantiate(this.Prefab, Parent.transform);
                CardHandlerVisual Script = Object.GetComponent<CardHandlerVisual>();
                Script.Ability = cont;
                Script.UpdateText();
                if (OnClick != null)
                {
                    Button btn = Object.GetComponent<Button>();
                    btn.onClick.AddListener(OnClick);
                }
            }
            Destroy(Prefab);
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