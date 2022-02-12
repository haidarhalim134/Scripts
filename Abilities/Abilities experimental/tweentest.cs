using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class tweentest : MonoBehaviour
{
    public Ease ease;
    // Start is called before the first frame update
    void Start()
    {
        transform.DOMoveX(transform.position.x+100,2f).SetEase(ease);
    }
}
