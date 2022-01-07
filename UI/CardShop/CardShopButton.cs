using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attributes.Abilities;

namespace Control.Core
    {
    public class CardShopButton : MonoBehaviour
    {
        public List<AbilityContainer> Cards = new List<AbilityContainer>(){
            new AbilityContainer(){name="OneAttack", Data=new AbilityData()},new AbilityContainer(){name="TwoAttack", Data=new AbilityData()},
            new AbilityContainer(){name="ShieldUp", Data=new AbilityData()}
        };
        public GameObject Viewer;
        public void onClick()
        {
            this.Viewer.GetComponent<CardShop>().Enable(this.Cards);
        }
    }
}