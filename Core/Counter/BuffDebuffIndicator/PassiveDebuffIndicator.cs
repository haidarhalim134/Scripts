using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Control.Core;
using DataContainer;
using DG.Tweening;

public class PassiveDebuffIndicator : MonoBehaviour
{
    public TextMeshProUGUI charge;
    public GameObject icon;
    public bool isPassive = true;
    public PassiveDebuff passive;
    public StanceBuffCont stance;
    string description;
    EventTrigger trigger;
    TooltipManager tooltipManager;
    public void Init()
    {
        if (isPassive)
            description = "<b>"+passive.debuff+"</b>\n"+ InGameContainer.GetInstance().FindPassiveDebuff(passive.debuff).GetDesc();
        else
        {
            charge.text = "<b>"+stance.charge.ToString();
            description = "<b>" + stance.stance + "</b>\n" + InGameContainer.GetInstance().FindStance(stance.stance).description;
        }
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
        Animations.ShakySoulEffect(icon);
    }
    void showDesc()
    {
        tooltipManager.SpawnTooltip(description, (Vector2)this.transform.position+
        new Vector2(0,this.icon.GetComponent<RectTransform>().rect.height/2));
    }
    public void update()
    {
        if (isPassive)
        {
            charge.text = "<b>" + passive.charge.ToString();
            if (passive.charge <= 0)
            {
                charge.gameObject.transform.localScale = new Vector3(2, 2, 1);
                charge.gameObject.transform.DOScale(1, 0.5f);
                charge.DOFade(0,0.5f);
                icon.GetComponent<Image>().DOFade(0,0.5f).OnComplete(()=>Destroy(gameObject));
            }else
            {
                charge.gameObject.transform.localScale = new Vector3(2,2,1);
                charge.gameObject.transform.DOScale(1, 0.5f);
            }
        }
    }
    void Update()
    {
        if (!isPassive)
        {
            if (stance.charge <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
