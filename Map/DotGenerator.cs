using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotGenerator
{
    public static void Generate(Vector2 point1, Vector2 point2, GameObject prefab, Transform parent, float gap = 15)
    {
        float progress = 0;
        float distance = Vector2.Distance(point1, point2);
        while (progress<1)
        {
            Vector3 pos = Vector2.Lerp(point1,point2,progress);
            pos.z = 50;
            GameObject Object = GameObject.Instantiate(prefab);
            Object.transform.position = pos;
            Object.transform.SetParent(parent);
            progress += gap / distance;
        }
    }
}
