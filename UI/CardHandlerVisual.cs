using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Control.Deck;
using Attributes.Abilities;
using DG.Tweening;
using Control.Core;

namespace Control.UI
{
    public class CardHandlerVisual : MonoBehaviour
    {
        /// <summary>call UpdateText after assigning</summary>
            public AbilityContainer Ability;
            protected bool MouseOnCard;
            private TextMeshProUGUI NameTXT;
            public TextMeshProUGUI desctxt;
            Sequence sequence;
            public Vector2 Exit;
            public void UpdateText(PlayerController caster = null, BaseCreature target = null)
            {
               AbilityManager Mng = this.Ability.GetManager();
               TextMeshProUGUI[] txtlist =  GetComponentsInChildren<TextMeshProUGUI>();
               this.NameTXT = txtlist[0];
               this.desctxt = txtlist[1];
               for (var i = 1;i<Mng.GetStaminaCost(Ability.Data) +1;i++)
               {
                   this.gameObject.transform.Find("Cost"+i).gameObject.SetActive(true);
               }
               this.NameTXT.text = this.Ability.name;
            //    txtlist[1].text = Mng.GetDesc(Ability.Data);
            if (caster != null)
            {
                this.desctxt.text = this.Ability.GetManager().GetDesc(this.Ability.Data, caster, target);
            }else
            {
                this.desctxt.text = this.Ability.GetManager().GetDesc(this.Ability.Data);
            }
        }
            public void Destroy(RemoveStatus type) 
            {
                if (type == RemoveStatus.used||type == RemoveStatus.discard)
                {
                    // this.transform.DOMove((Vector2)this.transform.position+new Vector2(0,5), 0.5f)
                    // .OnComplete(()=>this.transform.DOMove(this.Exit, 0.2f).SetEase(Ease.Linear)
                    // .SetDelay(0.2f).OnComplete(()=>Destroy(this.gameObject)));

                    this.transform.DOMove(this.Exit- new Vector2(0f, 3f), 0.1f).SetEase(Ease.Linear)
                    .OnComplete(()=>Destroy(this.gameObject));
                    // Destroy(this.gameObject);
                } else
                {
                    this.DOKill();
                    Destroy(this.gameObject);
                }
            }
    }
}
