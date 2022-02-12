using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Attributes.Abilities;
using DataContainer;

public class IntentsCounter : MonoBehaviour
{
    GameObject currentIndicator;
    public void Spawn(AbilityManager Mng, AbilityData data)
    {
        if (currentIndicator!=null)Destroy(currentIndicator);
        var prefab = InGameContainer.GetInstance().FindIntent(Mng.types.ToList());
        currentIndicator = Instantiate(prefab, transform);
        currentIndicator.transform.localPosition = new Vector3();
        var script = currentIndicator.GetComponent<IntentIndicator>();
        var fullData = Mng.GetData(data);
        if (Mng.types.Any(type=>type == AbilityType.attack))
        {
            if(fullData.AttackRep > 1)script.damage.text = $"{fullData.Damage}X{fullData.AttackRep}";
            else script.damage.text = $"{fullData.Damage}";
        }
        currentIndicator.SetActive(false);
    }
    public void Activate()
    {
        currentIndicator.SetActive(true);
    }
}
