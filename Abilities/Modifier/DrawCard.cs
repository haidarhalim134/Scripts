using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Attributes.Abilities;

public class DrawCard : BaseModifier
{
    [Header("drawCard")]
    public int card = 1;
    override public void Ability(BaseCreature caster, BaseCreature target, AbilityData Data)
    {
        PlayerController control = caster.transform.GetComponent<PlayerController>();
        IEnumerator draw()
        {
            for (var x = 0;x<card+Mng.GetLevelBonus(Data).DrawCard;x++)
            {
                yield return StartCoroutine(control.DeckAddTo(0));
                yield return new WaitForSeconds(control.CardOutSpeed);
            }
        }
        StartCoroutine(draw());
    }
    override public string Text(AbilityData data, PlayerController caster, BaseCreature target)
    {
        int car = card + Mng.GetLevelBonus(data).DrawCard;
        return $"draw {AbilityUtils.CalcColor(card, car)}{car}{AbilityUtils.c} card{closingDesc}";
    }
}
