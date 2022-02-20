using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Control.Core;

public class EndTurnButton : MonoBehaviour
{
    public PlayerController Owner;
    private bool Clickable;
    public void Disable()
    {
        this.Clickable = false;
        this.GetComponent<Image>().DOColor(new Color32(150, 150, 150, 255), 0.1f);
    }
    public void Enable()
    {
        this.Clickable = true;
        this.GetComponent<Image>().DOColor(new Color32(255, 255, 255, 255), 0.1f);
    }
    public void onClick()
    {
        if (this.Clickable) this.Owner.FinishTurn();
    }
}
