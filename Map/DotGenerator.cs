using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotGenerator : MonoBehaviour
{
    public GameObject prefab;
    public Vector2 point1;
    public Vector2 point2;
    public float gap;
    void Generate(Vector2 point1, Vector2 point2)
    {
        float progress = 0;
        float distance = Vector2.Distance(point1, point2);
        while (progress<1)
        {
            Vector2 pos = Vector2.Lerp(point1,point2,progress);
            Instantiate(prefab, pos, new Quaternion());
            progress += gap / distance;
        }
    }
    public void onClick()
    {
        Generate(point1,point2);
    }
}
