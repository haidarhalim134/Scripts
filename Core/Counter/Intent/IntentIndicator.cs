using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class IntentIndicator : MonoBehaviour
{
    public TextMeshProUGUI damage;
    public Image icon;
    public IEnumerator Destroy()
    {
        Animations.ShakySoulEffect(icon.gameObject,1f,4);
        yield return new WaitForSeconds(0.5f);
        icon.DOFade(0,0.5f).OnComplete(()=>Destroy(gameObject));
    }
    void OnEnable()
    {
        Color color = icon.color;
        color.a = 0;
        icon.color = color;
        color = damage.color;
        color.a = 0;
        icon.DOFade(1,0.5f);
        damage.DOFade(1,0.5f);
    }
    void OnDisable()
    {
        
    }
}
