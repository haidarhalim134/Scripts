using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


[CreateAssetMenu(fileName = "AttackAnim", menuName = "create AttackAnim")]
public class AnimData : ScriptableObject
{
    public float toHit;
    public float toBack;
    public Ease inEase;
    public Ease OutEase;
    public float distance;
}
