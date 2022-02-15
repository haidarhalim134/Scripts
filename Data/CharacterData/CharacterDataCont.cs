using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attributes.Abilities;

[CreateAssetMenu(fileName = "LevelData", menuName = "new Character")]
public class CharacterDataCont : ScriptableObject
{
    public Character Name;
    public int MaxStamina;
    public BotAbilityCont[] StartingAbilitiy;
    public int StartingHealth;
    public int initialCardNumber;
    public BotAbilityCont[] AvailableAbility;
}
public enum Character{Nocharacter,Ironclad, Watcher}
