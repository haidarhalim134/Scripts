using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Control.Combat;
using Attributes.Abilities;
using DataContainer;

public class CardPlayActivate : BasePower
{
    public CardType cardType;
    public int repetitionRequired = 1;
    string rep {get{return repetitionRequired > 1 ? $" {repetitionRequired}" : ""; }}
    string turn {get{return repetitionRequired > 1 ? $" in a single turn" : "";}}
    override public IEnumerator Ability(BaseCreature caster, BaseCreature target, AbilityData data)
    {
        void debuff(ActiveDebuffCardPlay data, AbilityContainer lastPlayed)
        {
            if (repetitionRequired > 1)
            {
                if (lastPlayed.GetManager().type == cardType)data.update(1);
                if (data.charge >= repetitionRequired)
                {
                    activate(data);
                    if (repetitionRequired > 1) data.update(data.charge*-1);
                }
            }else
            {
                if (lastPlayed.GetManager().type == cardType)activate(data);
            }
        }
        void reseter(ActiveDebuff data)
        {
            data.sibling.update(data.sibling.charge*-1);
        }
        string desc(ActiveDebuff Data)
        {
            string rep = repetitionRequired>1?$" {repetitionRequired}":"";
            string turn = repetitionRequired > 1 ? $" in a single turn" : "";
            return $"everytime your play{rep} attack card{turn}, " + abilityMng.GetDesc(Data.data, null, null) + closingDesc;
        }
        var cont = new ActiveDebuffCardPlay(Mng.AbName, hideCharge ? int.MaxValue : repetitionRequired > 1 ? 0 : data.Add(ability.Data).Sum(), data.Add(ability.Data), caster, target, debuff, desc);
        cont.destroyWhen0 = false;
        target.DebuffAddActive(target.buffDebuff.attackPlayActivate, cont, debuffIcon);
        if (repetitionRequired > 1)
        {
            var conti = new ActiveDebuff(Mng.AbName, int.MaxValue, data.Add(ability.Data), caster, target, reseter, desc);
            conti.sibling = cont;
            target.DebuffAddActive(target.buffDebuff.endTurnActivate, conti, null);
        }
        yield return null;
    }
    override public string Text(AbilityData data, PlayerController caster, BaseCreature target)
    {
        return $"everytime your play{rep} attack card{turn}, " + abilityMng.GetDesc(data.Add(ability.Data), null, null) + closingDesc;
    }
}
