using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Animations
{
    public static IEnumerator TowardsCenterAttack(GameObject target, Action onHit, float totalTime = 0.5f, float distance = 50f)
    {
        float tohit = totalTime * 1/2f;
        float toback = totalTime * 1/2f;
        if (target.transform.position.x>0)
        {
            distance*= -1;
        }
        var tween = target.transform.DOLocalMoveX(target.transform.localPosition.x + distance, tohit).SetEase(Ease.InCirc);
        yield return tween.WaitForCompletion();
        onHit();
        tween = target.transform.DOLocalMoveX(target.transform.localPosition.x - distance, toback).SetEase(Ease.Linear);
        yield return tween.WaitForCompletion();
    }
    public static IEnumerator TowardsCenterHit(GameObject target, Action onHit, float totalTime = 0.5f, float distance = 50f)
    {
        float tohit = totalTime * 1 / 2f;
        float toback = totalTime * 1 / 2f;
        if (target.transform.position.x > 0)
        {
            distance *= -1;
        }
        var tween = target.transform.DOLocalMoveX(target.transform.localPosition.x + distance, tohit).SetEase(Ease.Linear);
        yield return tween.WaitForCompletion();
        onHit();
        tween = target.transform.DOLocalMoveX(target.transform.localPosition.x - distance, toback).SetEase(Ease.Linear);
        yield return tween.WaitForCompletion();
    }
    public static void SpawnEffect(GameObject target, GameObject effect)
    {
        if (effect!=null)GameObject.Instantiate(effect, target.transform).transform.localPosition = new Vector2();
    }
}
