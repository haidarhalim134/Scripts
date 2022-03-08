using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Control.Combat;
using Attributes.Abilities;
using DataContainer;


[CreateAssetMenu(fileName = "Artifact", menuName = "new Artifact")]
public class Relic : ScriptableObject
{
    public AbTarget relicTargeting;
    public AbilityManager power;
    public void Activate(BaseCreature owner)
    {
        InGameContainer cont = InGameContainer.GetInstance();
        AbilityManager mng = InGameContainer.GetInstance().SpawnAbilityPrefab(power.gameObject);
        if (relicTargeting == AbTarget.self) cont.StartCoroutine(mng.Activate(owner, owner, new AbilityData()));
        else
        {
            CombatEngine.GetTarget(owner, relicTargeting).ForEach(item => cont.StartCoroutine(mng.Activate(owner, item, new AbilityData())));
        }
    }
}
