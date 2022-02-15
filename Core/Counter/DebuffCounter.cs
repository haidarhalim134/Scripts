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
    void Start()
    {
        Creature.debuffCounter = this;
    }
    // void Enable()
    // {
    //     this.Creature.buffDebuff.passiveDebuffs.ForEach((debuff) => {
    //         this.SpawnPassive(debuff);
    //     });
    // }
    // void Disable()
    // {
    //     foreach (Transform child in this.transform)
    //     {
    //         Destroy(child.gameObject);
    //     }
    // }
    public PassiveDebuffIndicator AddPassiveDebuff(PassiveDebuff debuff)
    {
        GameObject prefab = InGameContainer.GetInstance().FindPassiveDebuff(debuff.debuff).prefab;
        GameObject Object = Instantiate(prefab, this.transform);
        PassiveDebuffIndicator indicator = Object.GetComponent<PassiveDebuffIndicator>();
        indicator.passive = debuff;
        indicator.Init();
        return indicator;
    }
    public void SpawnStance(StanceBuffCont stance)
    {
        GameObject prefab = InGameContainer.GetInstance().FindStance(stance.stance).prefab;
        GameObject Object = Instantiate(prefab, this.transform);
        PassiveDebuffIndicator indicator = Object.GetComponent<PassiveDebuffIndicator>();
        indicator.isPassive = false;
        indicator.stance = stance;
        indicator.Init();
    }
    public void SpawnSimpleActive(GameObject prefab, ActiveDebuff debuff)
    {
        GameObject Object = Instantiate(prefab, this.transform);
        SimpleActiveDebuffIndicator indicator = Object.GetComponent<SimpleActiveDebuffIndicator>();
        indicator.active = debuff;
    }
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.LeftControl))
        // {
        //     if (this.Active)
        //     {
        //         this.Disable();
        //         this.Active = false;
        //     } else
        //     {
        //         this.Enable();
        //         this.Active = true;
        //     }
        // }
    }
}
