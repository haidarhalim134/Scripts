using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attributes.Abilities;
using Control.UI;
using DataContainer;

public class CardUpgrader : MonoBehaviour
{
    [SerializeField] GameObject toUpgradeSpot;
    [SerializeField] GameObject showUpgradeSpot;
    CardHandlerVisual currToUpgrade;
    CardHandlerVisual currShowUpgrade;
    public void QUpgradeCard(AbilityContainer ability)
    {
        gameObject.SetActive(true);
        if (currToUpgrade != null)
        {
            Destroy(currToUpgrade.gameObject);
            Destroy(showUpgradeSpot.gameObject);
        }
        AbilityContainer Uability = new AbilityContainer()
        { name = ability.name, Data = ability.Data.Add(new AbilityData() { Level = 1 }) };
        currToUpgrade = Utils.SpawnCard<CardHandlerVisual>(ability, toUpgradeSpot.transform.position, 
        InGameContainer.GetInstance().cardViewPrefab, toUpgradeSpot.transform);
        currShowUpgrade = Utils.SpawnCard<CardHandlerVisual>(Uability, showUpgradeSpot.transform.position,
        InGameContainer.GetInstance().cardViewPrefab, showUpgradeSpot.transform);
    }
    public void Upgrade()
    {
        Debug.Log(currToUpgrade.Ability);
        currToUpgrade.Ability.Data = currShowUpgrade.Ability.Data;
        Disable();
    }
    public void Enable()
    {
        gameObject.SetActive(true);
    }
    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
