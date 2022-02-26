using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Attributes.Abilities;
using DataContainer;
using DG.Tweening;
using Control.Core;

public class IntentsCounter : MonoBehaviour
{
    GameObject currentIndicator;
    public BotController owner;
    AbilityManager currMng;
    AbilityData currData;
    public void Spawn(AbilityManager Mng, AbilityData data)
    {
        if (currentIndicator!=null)StartCoroutine(currentIndicator.GetComponent<IntentIndicator>().Destroy());
        currMng = Mng;
        currData = data;
        var prefab = InGameContainer.GetInstance().FindIntent(currMng.types.ToList());
        currentIndicator = Instantiate(prefab, transform);
        currentIndicator.transform.localPosition = new Vector3();
        currentIndicator.SetActive(false);
    }
    IEnumerator textUpdater()
    {
        while (true)
        {
            if (currentIndicator!=null)
            {
                var script = currentIndicator.GetComponent<IntentIndicator>();
                var fulldata = currMng.GetData(currData);
                var fulldamage = currMng.GetDamageDeal(currData, owner, owner.nextTarget);
                if (currMng.types.Any(type => type == AbilityType.attack))
                {
                    if (fulldata.AttackRep > 1) script.damage.text = $"{fulldamage}X{fulldata.AttackRep}";
                    else script.damage.text = $"{fulldamage}";
                }

            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void Activate()
    {
        currentIndicator.SetActive(true);
    }
    public void Destroy()
    {
        if (currentIndicator!=null)
        {
            currentIndicator.GetComponent<IntentIndicator>().icon.DOFade(0, 0.2f).OnComplete(()=> Destroy(gameObject));
        }else Destroy(gameObject);
    }
    void Awake()
    {
        transform.DOMoveY(transform.position.y+50, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
        StartCoroutine(textUpdater());  
    }
}
