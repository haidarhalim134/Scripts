using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DataContainer;
using Control.Core;

public class GoldCounter : MonoBehaviour
{
    public TextMeshProUGUI text;

    // Update is called once per frame
    void Update()
    {
        if (Loaded.loaded!=null) text.text = ""+Loaded.loaded.Gold;
    }
}
