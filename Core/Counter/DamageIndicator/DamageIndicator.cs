using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using DataContainer;

public class DamageIndicator : MonoBehaviour
{
    public float height;
    public float totalTime;
    public float speed;
    public Vector2 delta;
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    public static void Init(Vector2 initPos, string text)
    {
        var obj = Instantiate(InGameContainer.GetInstance().damageIndicator, GameObject.Find("DamageIndicatorSpace").transform);
        obj.transform.position = initPos;
        obj.GetComponent<DamageIndicator>().text.text = text;
        Animations.BigAndUp(obj, 50, 1f);
    }
}
