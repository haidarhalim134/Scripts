using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DataContainer;

namespace Control.Core
{
    public class ReserveDeckCounter : BaseCounter
    {
        PlayerController Player { get { return InGameContainer.GetInstance().currPlayer; } }
        public GameObject Viewer;
        public Image icon;
        // Start is called before the first frame update
        void Start()
        {
            this.Awoke();
        }
        public void OnClick()
        {
            this.Viewer.GetComponent<CardViewer>().Enable(this.Player.ReserveDeck, Player);
        }
        public void Bump()
        {
            icon.transform.DOKill();
            Animations.BigThenSmall(icon.gameObject, 0.1f, 0.2f, 1.3f);
        }
        // Update is called once per frame
        void Update()
        {
            if (Player != null) this.Counter.text = this.Player.ReserveDeck.Count.ToString();
        }
    }
}