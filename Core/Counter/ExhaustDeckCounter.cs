using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Control.Core;

public class ExhaustDeckCounter : BaseCounter
{
    PlayerController Player;
    public GameObject Viewer;
    public Image icon;
    // Start is called before the first frame update
    void Start()
    {
        this.Awoke();
        this.Player = this.Creature.gameObject.GetComponent<PlayerController>();
    }
    public void OnClick()
    {
        this.Viewer.GetComponent<CardViewer>().Enable(this.Player.ExhaustedDeck);
    }
    public void Bump()
    {
        icon.transform.DOKill();
        Animations.BigThenSmall(icon.gameObject, 0.1f, 0.2f, 1.3f);
    }
    // Update is called once per frame
    void Update()
    {
        this.Counter.text = this.Player.ExhaustedDeck.Count.ToString();
    }
}
