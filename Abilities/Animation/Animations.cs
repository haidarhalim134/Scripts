using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Animations
{
    public static IEnumerator TowardsCenterAttack(GameObject target, Action onHit,Action postHit, float totalTime = 0.25f, float distance = 25f)
    {
        float tohit = totalTime * 2/3f;
        float toback = totalTime * 1/3f;
        if (target.transform.position.x>0)
        {
            distance*= -1;
        }
        var tween = target.transform.DOLocalMoveX(target.transform.localPosition.x + distance, tohit).SetEase(Ease.InCirc);
        yield return tween.WaitForCompletion();
        onHit();
        tween = target.transform.DOLocalMoveX(target.transform.localPosition.x - distance, toback).SetEase(Ease.Linear);
        yield return tween.WaitForCompletion();
        postHit();
    }
    public static IEnumerator AwayCenterHit(GameObject target, Action onHit, float totalTime = 0.5f, float distance = 50f)
    {
        float tohit = totalTime * 1 / 2f;
        float toback = totalTime * 1 / 2f;
        if (target.transform.position.x < 0)
        {
            distance *= -1;
        }
        var tween = target.transform.DOLocalMoveX(target.transform.localPosition.x + distance, tohit).SetEase(Ease.Linear);
        yield return tween.WaitForCompletion();
        onHit();
        tween = target.transform.DOLocalMoveX(target.transform.localPosition.x - distance, toback).SetEase(Ease.Linear);
        yield return tween.WaitForCompletion();
    }
    public static IEnumerator AwayCenterShot(GameObject target, Action onHit, Action postHit, float totalTime = 0.5f, float distance = 50f)
    {
        float tohit = totalTime * 1 / 2f;
        float toback = totalTime * 1 / 2f;
        if (target.transform.position.x < 0)
        {
            distance *= -1;
        }
        var tween = target.transform.DOLocalMoveX(target.transform.localPosition.x + distance, tohit).SetEase(Ease.Linear);
        yield return tween.WaitForCompletion();
        onHit();
        tween = target.transform.DOLocalMoveX(target.transform.localPosition.x - distance, toback).SetEase(Ease.Linear);
        yield return tween.WaitForCompletion();
        postHit();
    }
    public static void SpawnEffect(GameObject target, GameObject effect)
    {
        if (effect!=null)GameObject.Instantiate(effect, target.transform).transform.localPosition = new Vector2();
    }
    public static void ShakySoulEffect(GameObject target, float duration = 2, float strength = 1)
    {
        var Object = GameObject.Instantiate(target, target.transform);
        Object.transform.localPosition = new Vector2();
        var color = Object.GetComponent<Image>().color;
        color.a = 0.5f;
        Object.GetComponent<Image>().color = color;
        Object.transform.localScale = new Vector3(1.3f, 1.3f, 1);
        // target.transform.DOShakePosition(duration- 0.5f);
        Object.transform.DOScale(new Vector3(1, 1, 1), duration).OnComplete(() => GameObject.Destroy(Object));
        for (var i = 0;i<5;i++)
        {
            var ObjectS = GameObject.Instantiate(Object, target.transform);
            ObjectS.transform.localPosition = new Vector2();
            ObjectS.transform.DOShakePosition(duration-0.5f,strength);
            ObjectS.transform.DOScale(new Vector3(1,1,1), duration).OnComplete(()=>GameObject.Destroy(ObjectS));
        }
    }
}
