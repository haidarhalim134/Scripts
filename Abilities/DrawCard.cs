using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Attributes.Abilities;

public class DrawCard : MonoBehaviour
{
    public int card = 1;
    public ModType modType;
    static StatProcessor Calc = new StatProcessor();
    AbilityManager Mng;
    public void Ability(BaseCreature caster, BaseCreature target, AbilityData Data)
    {
        PlayerController control = caster.transform.GetComponent<PlayerController>();
        IEnumerator draw()
        {
            for (var x = 0;x<this.card;x++)
            {
                control.DeckAddTo(0);
                yield return new WaitForSeconds(control.CardOutSpeed);
            }
        }
        StartCoroutine(draw());
    }
    public string Text(AbilityData data, PlayerController caster, BaseCreature target)
    {
        return $"draw {this.card} card. ";
    }
    void Awake()
    {
        Mng = gameObject.GetComponent<AbilityManager>();
        Mng.modifier.modifier[modType].Add(this.Ability);
        Mng.DescGrabber.Add(this.Text);
    }
}
