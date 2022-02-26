using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Attributes.Abilities;
using DataContainer;

public class ApplyCardMod : MonoBehaviour
{
    public CardModifier cardModifier;
    public string closingDesc = ". ";
    public string Text(AbilityData data, PlayerController caster, BaseCreature target)
    {
        return $"{cardModifier}";
    }
    void Awake()
    {
        AbilityManager mng = GetComponent<AbilityManager>();
        mng.cardModifiers.Add(cardModifier);
        mng.DescGrabber.Add(Text);
    }
}
