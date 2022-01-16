using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Attributes.Abilities;
using DataContainer;

public class DebuffCounter : MonoBehaviour
{
    public BaseCreature Creature;
    public bool Active = false;
    void Awake()
    {

    }
    void Enable()
    {
        this.Creature.buffDebuff.passiveDebuffs.ForEach((debuff) => {
            this.SpawnPassive(debuff);
        });
    }
    void Disable()
    {
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }
    void SpawnPassive(PassiveDebuff debuff)
    {
        GameObject prefab = Array.Find(InGameContainer.GetInstance()
        .PassiveDebuffPrefab, (cont) => cont.debuff == debuff.debuff).prefab;
        GameObject Object = Instantiate(prefab, this.transform);
        DebuffIndicator indicator = Object.GetComponent<DebuffIndicator>();
        indicator.passive = debuff;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (this.Active)
            {
                this.Disable();
                this.Active = false;
            } else
            {
                this.Enable();
                this.Active = true;
            }
        }
    }
}
