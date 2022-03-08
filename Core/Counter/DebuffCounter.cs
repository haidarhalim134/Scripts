using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Control.Core;
using Attributes.Abilities;
using DataContainer;

public class DebuffCounter : MonoBehaviour
{
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
    /// <summary>if no prefab return null</summary>
    public SimpleActiveDebuffIndicator SpawnSimpleActive(Sprite icon, ActiveDebuff debuff)
    {
        if (icon == null)
        {
            debuff.hideIndicator = true;
            return null;
        }
        SimpleActiveDebuffIndicator indicator = Instantiate(InGameContainer.GetInstance().activeDebuffIndicatorPrefab.gameObject, this.transform)
        .GetComponent<SimpleActiveDebuffIndicator>();
        indicator.icon.GetComponent<Image>().sprite = icon;
        indicator.active = debuff;
        indicator.Init();
        return indicator;
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
