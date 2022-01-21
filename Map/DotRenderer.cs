using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class DotRenderer : MonoBehaviour
{
    LineRenderer line;
    void Awake()
    {
        line = this.gameObject.GetComponent<LineRenderer>();
    }
    void Update()
    {
        float width = line.startWidth;
        line.material.mainTextureScale = new Vector2(1f / width, 1.0f);
    }
}
