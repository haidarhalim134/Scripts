using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public Animator animator;
    public static float animDuration = 0.3f;
    public static void Close()
    {
        var obj = GameObject.Find("Transition").GetComponent<Transition>();
        obj.animator.Play("Out");
    }
}
