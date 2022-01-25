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
    void Awake()
    {
        description = Array.Find(InGameContainer.GetInstance()
        .PassiveDebuffPrefab, (cont) => cont.debuff == passive.debuff).description;
        trigger = icon.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => { showDesc(); });
        trigger.triggers.Add(entry);
    }
    void showDesc()
    {
        Debug.Log(description);
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
