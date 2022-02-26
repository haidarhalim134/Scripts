using System;
using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

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
    public static void ShakySoulEffect(GameObject target, float duration = 2, float strength = 1, bool fadeout = true)
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
            ObjectS.transform.DOShakePosition(duration-0.5f,strength, fadeOut: fadeout);
            ObjectS.transform.DOScale(new Vector3(1,1,1), duration).OnComplete(()=>GameObject.Destroy(ObjectS));
            ObjectS.GetComponent<Image>().DOFade(0,duration/4f);
        }
    }
    /// <summary>for image component only</summary>
    public static void FadingEffect(GameObject obj, float from, float to, float time)
    {
        Image comp = obj.GetComponent<Image>();
        Color color = comp.color;
        color.a = from;
        comp.color = color;
        comp.DOFade(to, time);
    }
    public static void ArcEffect(GameObject obj, Vector2 to, float height, float totalTime, Action onEnd = null)
    {
        obj.transform.DOMoveX(to.x, totalTime).SetEase(Ease.Linear);
        obj.transform.DOMoveY(to.y, totalTime).SetEase(Ease.InQuad);
        obj.GetComponent<TextMeshProUGUI>().DOFade(0, totalTime/3).SetDelay(totalTime*2/3f);
    }
    /// <summary>for tmprougui component only</summary>
    public static void BigAndUp(GameObject gameObject, float height, float time, bool randomAngle = true)
    {
        gameObject.transform.DOMoveY(gameObject.transform.position.y + height, time);
        if (randomAngle)
        {
            Random rnd = new Random();
            gameObject.transform.DOMoveX(gameObject.transform.position.x+rnd.Next(-20,20), time);
        }
        gameObject.transform.DOScale(1.5f, 0.1f).OnComplete(()=>gameObject.transform.DOScale(1, 0.1f));
        gameObject.GetComponent<TextMeshProUGUI>().DOFade(0, time/3).SetDelay(time*2/3f).OnComplete(()=>GameObject.Destroy(gameObject));
    }
    public static void BigThenSmall(GameObject gameObject, float tobig, float tosmall, float maxsize)
    {
        gameObject.transform.DOScale(maxsize, tobig).OnComplete(()=>gameObject.transform.DOScale(1, tosmall));
    }
}
