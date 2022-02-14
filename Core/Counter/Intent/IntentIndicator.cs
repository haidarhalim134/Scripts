using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using DataContainer;

public class IntentIndicator : MonoBehaviour
{
    public TextMeshProUGUI damage;
    public Image icon;
    public IEnumerator Destroy()
    {
        float time = InGameContainer.GetInstance().delayBetweenTurn*4/3f;
        Animations.ShakySoulEffect(icon.gameObject,time,8);
        yield return new WaitForSeconds(time / 2f);
        icon.DOFade(0,time/2f).OnComplete(()=>Destroy(gameObject));
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
