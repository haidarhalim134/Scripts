using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntentIndicator : MonoBehaviour
{
    public TextMeshProUGUI damage;
    public Image icon;
    public IEnumerator Destroy()
    {
        Animations.ShakySoulEffect(gameObject,1);
        yield return new WaitForSeconds(1.1f);
        Destroy(gameObject);
    }
}
