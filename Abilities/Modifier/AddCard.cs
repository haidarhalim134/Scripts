using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Attributes.Abilities;
using DataContainer;

public class AddCard : MonoBehaviour
{
    public int amount = 1;
    public GameObject ability;
    public ModType modType;
    public string closingDesc = ". ";
    AbilityManager abilityMng;
    AbilityManager Mng;
    public void Ability(BaseCreature caster, BaseCreature target, AbilityData data)
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
    public string Text(AbilityData data, PlayerController caster, BaseCreature target)
    {
        int amo = amount + Mng.GetLevelBonus(data).DrawCard;
        return $"add {AbilityUtils.CalcColor(amount, amo)}{amo}{AbilityUtils.c} {abilityMng.AbName} to your hand{closingDesc}";
    }
    void Awake()
    {
        abilityMng = InGameContainer.GetInstance().SpawnAbilityPrefab(ability);
        Mng = gameObject.GetComponent<AbilityManager>();
        Mng.modifier.modifier[modType].Add(this.Ability);
        Mng.DescGrabber.Add(this.Text);
    }
}
