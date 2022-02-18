using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using DataContainer;
using Random = System.Random;
using Map;

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
        obj.transform.DOScale(3f, 0.1f).OnComplete(() => obj.transform.DOScale(1, 0.1f));
        var script = obj.GetComponent<DamageIndicator>();
        script.text.text = text;
        var rnd = new Random();
        var xdelta = script.delta.x * (rnd.Next(0,2)==0?-1:1);
        Animations.ArcEffect(obj, initPos + new Vector2(xdelta, script.delta.y), script.height, 0.5f);
    }
}
