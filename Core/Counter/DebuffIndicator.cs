using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Control.Core;

public class DebuffIndicator : MonoBehaviour
{
    public TextMeshProUGUI charge;
    public PassiveDebuff passive;
    void Update()
    {
        if (passive != null)
        {
            charge.text = passive.charge.ToString();
        }
        if (passive.charge <= 0)
        {
            Destroy(gameObject);
        }
    }
}
