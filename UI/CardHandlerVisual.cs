using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Attributes.Abilities;
using DG.Tweening;

namespace Control.UI
{
    public class CardHandlerVisual : MonoBehaviour
    {
        /// <summary>call UpdateText after assigning</summary>
            public AbilityContainer Ability;
            protected bool MouseOnCard;
            private TextMeshProUGUI NameTXT;
            private TextMeshProUGUI CostTXT;
            Sequence sequence;
            public void UpdateText()
            {
               AbilityManager Mng = this.Ability.GetManager();
               TextMeshProUGUI[] txtlist =  GetComponentsInChildren<TextMeshProUGUI>();
               this.CostTXT = txtlist[0];
               this.NameTXT = txtlist[1];
               this.CostTXT.text = Mng.cost.ToString();
               this.NameTXT.text = this.Ability.name;
               txtlist[2].text = Mng.GetDesc();
            }
            public void Destroy() 
            {
                this.DOKill();
                Destroy(gameObject);
            }
    }
}
