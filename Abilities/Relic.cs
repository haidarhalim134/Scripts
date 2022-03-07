using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Control.Combat;
using Attributes.Abilities;
using DataContainer;


public class Relic : MonoBehaviour
{
    public AbTarget relicTargeting;
    public AbilityManager power;
    public void Activate(BaseCreature owner)
    {
        var relic = Instantiate(gameObject).GetComponent<Relic>();
        AbilityManager mng = InGameContainer.GetInstance().SpawnAbilityPrefab(power.gameObject);
        if (relicTargeting == AbTarget.self) mng.Activate(owner, owner, new AbilityData());
        else
        {
            CombatEngine.GetTarget(owner, relicTargeting).ForEach(item => relic.StartCoroutine(mng.Activate(owner, item, new AbilityData())));
        }
    }
}
