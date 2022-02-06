using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Control.Core;
using DataContainer;

public class PassiveDebuffIndicator : MonoBehaviour
{
    public TextMeshProUGUI charge;
    public GameObject icon;
    public PassiveDebuff passive;
    string description;
    EventTrigger trigger;
    TooltipManager tooltipManager;
    void Awake()
    {
        description = "<b>"+passive.debuff+"</b>\n"+ InGameContainer.GetInstance().FindPassiveDebuff(passive.debuff).GetDesc();
        trigger = icon.GetComponent<EventTrigger>();
        tooltipManager = TooltipManager.GetInstance();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => { showDesc(); });
        trigger.triggers.Add(entry);
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerExit;
        entry.callback.AddListener((eventData) => { tooltipManager.Hide(); });
        trigger.triggers.Add(entry);
    }
    void showDesc()
    {
        tooltipManager.SpawnTooltip(description, (Vector2)this.transform.position+
        new Vector2(0,this.icon.GetComponent<RectTransform>().rect.height/2));
    }
    void Update()
    {
        if (passive != null)
        {
            charge.text = passive.charge.ToString();
        }
        if (passive.charge <= 0)
        {
            Destroy(gameObject);
        }
    }
}
