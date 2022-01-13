using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;

public class EndTurnButton : MonoBehaviour
{
    public PlayerController Owner;
    private bool Clickable;
    public void Disable()
    {
        this.Clickable = false;
    }
    public void Enable()
    {
        this.Clickable = true;
    }
    public void onClick()
    {
        if (this.Clickable) this.Owner.FinishTurn();
    }
    void Update()
    {
        // TODO: add effect on disable and all the conditionals
        Clickable = Owner.Control;
    }
}
