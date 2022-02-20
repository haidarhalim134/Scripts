using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect
{
    public static LineRenderer GenerateLine(Vector2 from, Vector2 to)
    {
        var prefab = GameObject.Find("LineGenerator");
        var obj = GameObject.Instantiate(prefab);
        var script = obj.GetComponent<LineRenderer>();
        script.SetPosition(0, from);
        script.SetPosition(1, to);
        return script;
    }
    public static void SetLineRenderer(LineRenderer script, Vector2 from, Vector2 to)
    {
        script.SetPosition(0, from);
        script.SetPosition(1, to);
    }
}
