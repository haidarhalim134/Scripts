using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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
        public void Enable(List<AbilityContainer> Abilities, PlayerController owner = null, bool ordered = false, Action<CardHandlerVisual> clickFunc = null)
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
                Script.UpdateText(owner);
                if (clickFunc != null)
                {
                    Button btn = Script.GetComponent<Button>();
                    btn.onClick.AddListener(()=>clickFunc(Script));
                }
            }
            Destroy(Prefab);
            this.gameObject.SetActive(true);
        }
        public void Enable(List<AbilityContainer> Abilities, GameObject newPrefab)
        {
            var tmp = Prefab;
            Prefab = newPrefab;
            Enable(Abilities);
            Prefab = tmp;
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