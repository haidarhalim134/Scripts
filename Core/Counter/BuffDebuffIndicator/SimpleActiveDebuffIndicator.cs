using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Control.Core;
using DataContainer;

public class SimpleActiveDebuffIndicator : MonoBehaviour
{
    public TextMeshProUGUI charge;
    public GameObject icon;
    public ActiveDebuff active;
    EventTrigger trigger;
    TooltipManager tooltipManager;
    void Awake()
    {
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
        tooltipManager.SpawnTooltip(active.description(active), (Vector2)this.transform.position +
        new Vector2(0, this.icon.GetComponent<RectTransform>().rect.height / 2));
    }
    void Update()
    {
        if (active != null)
        {
            charge.text = active.charge.ToString();
        }
        if (active.charge <= 0)
        {
            Destroy(gameObject);
        }
    }
}
