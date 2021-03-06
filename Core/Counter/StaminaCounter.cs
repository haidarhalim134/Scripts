using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DataContainer;

namespace Control.Core
{
    public class StaminaCounter : BaseCounter
    {
        public bool forPlayer = false;
        public PlayerController Owner {get{return InGameContainer.GetInstance().currPlayer;}}
        public void UpdateCounter()
        {
            this.Counter.text = this.Creature.stamina.Curr+"/"+this.Creature.stamina.Max;
        }
        // Start is called before the first frame update
        void Start()
        {
            this.Awoke();
            if (forPlayer) this.Creature = Owner;
            this.Creature.StaminaCounters.Add(this);
            this.UpdateCounter();
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}