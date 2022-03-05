using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.UI;
using Control.Core;
using Attributes.Abilities;
using DataContainer;

public class CardUpgradeScene : MonoBehaviour
{
    public CardViewer viewer;
    public void OpenCardUpgradeMenu()
    {
        viewer.Enable(Loaded.loaded.Player.FullDeck, clickFunc: CardOnClick);
    }
    void CardOnClick(CardHandlerVisual data)
    {
        InGameContainer.GetInstance().currCardUpgrader.Enable();
        InGameContainer.GetInstance().currCardUpgrader.QUpgradeCard(data.Ability);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            InGameContainer.GetInstance().currCardUpgrader.Disable();
        }
    }
}
