using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Control.Deck;
using Attributes.Abilities;
using DG.Tweening;
using Control.Core;
using DataContainer;

namespace Control.UI
{
    public class CardHandlerVisual : MonoBehaviour
    {
        /// <summary>call UpdateText after assigning</summary>
        public AbilityContainer Ability;
        public bool enableHover;
        private TextMeshProUGUI NameTXT;
        public TextMeshProUGUI desctxt;
        Sequence sequence;
        public Vector2 Exit;
        void Awake()
        {
            var trigger = this.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((eventData) => { if (enableHover) Magnify(true,true); });
            trigger.triggers.Add(entry);
            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerExit;
            entry.callback.AddListener((eventData) => { 
                if(enableHover&&(transform.GetComponent<CardHandler>()==null||!transform.GetComponent<CardHandler>().Active))
                Magnify(false,true); 
            });
            trigger.triggers.Add(entry);
        }
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
        public void Magnify(bool to, bool ignoreHover = false)
        {
            if (!enableHover&&!ignoreHover)return;
            if (to)
            {
                this.transform.DOScale(1.3f,0.05f);
            }else
            {
                this.transform.DOScale(1,0.05f);
            }
        }
        public void SmallToBig()
        {
            enableHover = false;
            this.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            this.transform.DOScale(1f, InGameContainer.GetInstance().delayBetweenTurn*0.5f).OnComplete(()=> enableHover = true);
        }
    }
}
