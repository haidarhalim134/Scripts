using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Attributes.Abilities;
using DataContainer;

public class AddCard : BaseModifier
{
    public int amount = 1;
    public GameObject ability;
    AbilityManager abilityMng;
    override public void Ability(BaseCreature caster, BaseCreature target, AbilityData data)
    {
        PlayerController ctrl = caster.GetComponent<PlayerController>();
        IEnumerator drawer()
        {
            for (int x = 0;x<amount+Mng.GetLevelBonus(data).DrawCard;x++)
            {
                ctrl.Deck.AddCard(new AbilityContainer() { name = abilityMng.AbName });
                yield return new WaitForSeconds(ctrl.CardOutSpeed);
            }
        }
        StartCoroutine(drawer());
    }
    override public string Text(AbilityData data, PlayerController caster, BaseCreature target)
    {
        int amo = amount + Mng.GetLevelBonus(data).DrawCard;
        return $"add {AbilityUtils.CalcColor(amount, amo)}{amo}{AbilityUtils.c} {abilityMng.AbName} to your hand{closingDesc}";
    }
    override public void Awake()
    {
        base.Awake();
        abilityMng = InGameContainer.GetInstance().SpawnAbilityPrefab(ability);
    }
}
