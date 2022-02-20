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
        /// <summary>pass an on click method if there is any</summary>
        public void Enable(List<AbilityContainer> Abilities, bool ordered = false)
        {
            if (!ordered)Abilities = CardSorter.SortByWord(Abilities);
            GameObject Prefab = new GameObject("box");
            Prefab.AddComponent<RectTransform>();
            foreach (AbilityContainer cont in Abilities)
            {
                GameObject Parent = Instantiate(Prefab,this.Grid.transform);
                GameObject Object = Instantiate(this.Prefab, Parent.transform);
                Object.transform.localPosition = new Vector3();
                CardHandlerVisual Script = Object.GetComponent<CardHandlerVisual>();
                Script.Ability = cont;
                Script.enableHover = true;
                Script.UpdateText();
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