using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Control.Core
{
    public class UsedDeckCounter : BaseCounter
    {
        PlayerController Player;
        public GameObject Viewer;
        public Image icon;
        int before;
        // Start is called before the first frame update
        void Start()
        {
            this.Awoke();
            this.Player = this.Creature.gameObject.GetComponent<PlayerController>();
        }
        public void OnClick()
        {
            this.Viewer.GetComponent<CardViewer>().Enable(this.Player.UsedDeck);
        }
        public void Bump()
        {
            icon.transform.DOKill();
            Animations.BigThenSmall(icon.gameObject, 0.1f, 0.2f, 1.3f);
        }
        // Update is called once per frame
        void Update()
        {
            this.Counter.text = this.Player.UsedDeck.Count.ToString();
            // if (before<this.Player.UsedDeck.Count)Bump();
            // before = this.Player.UsedDeck.Count;
        }
    }
}