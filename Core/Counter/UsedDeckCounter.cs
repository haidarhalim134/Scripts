using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Control.Core
{
    public class UsedDeckCounter : BaseCounter
    {
        PlayerController Player;
        public GameObject Viewer;
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
        // Update is called once per frame
        void Update()
        {
            this.Counter.text = this.Player.UsedDeck.Count.ToString();
        }
    }
}