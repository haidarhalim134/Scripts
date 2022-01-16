using System;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Attributes.Abilities;
using DataContainer;

public class DebuffCounter : MonoBehaviour
{
    public GameObject Owner;
    void Awake()
    {
        this.Owner.GetComponent<BaseCreature>().debuffCounter = this;
    }
    public void AddPassive(PassiveDebuff debuff)
    {
        
        GameObject prefab = Array.Find(InGameContainer.GetInstance()
        .PassiveDebuffPrefab, (cont) => cont.debuff == debuff.debuff).prefab;
        GameObject Object = Instantiate(prefab, this.transform);
        DebuffIndicator indicator = Object.GetComponent<DebuffIndicator>();
        indicator.passive = debuff;
    }
}
