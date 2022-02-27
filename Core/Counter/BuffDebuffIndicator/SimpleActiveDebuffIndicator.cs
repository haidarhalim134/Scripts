using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public void Init()
    {
        Debug.Log(active.charge);
        trigger = icon.GetComponent<EventTrigger>();
        tooltipManager = TooltipManager.GetInstance();
        if (active.charge == int.MaxValue)charge.text = "";
        update();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => { showDesc(); });
        trigger.triggers.Add(entry);
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerExit;
        entry.callback.AddListener((eventData) => { tooltipManager.Hide(); });
        trigger.triggers.Add(entry);
        Animations.FadingEffect(icon, 0, 1, 0.5f);
        // Animations.ShakySoulEffect(icon);
    }
    void showDesc()
    {
        tooltipManager.SpawnTooltip(active.description(active), (Vector2)this.transform.position +
        new Vector2(0, this.icon.GetComponent<RectTransform>().rect.height / 2));
    }
    public void update()
    {
        if (active != null&&active.charge>0)
        {
            charge.text = "<b>"+active.charge.ToString();
        }
        if (active.charge == 0)
        {
            Destroy(gameObject);
        }
    }
}
