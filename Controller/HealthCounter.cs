using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Control.Core
{
    public class HealthCounter : BaseCounter
    {
        public void UpdateCounter()
        {
            this.Counter.text = this.Creature.CurrHealth+"/"+this.Creature.MaxHealth;
        }
        // Start is called before the first frame update
        void Start()
        {
            this.Awoke();
            this.Creature.HealthCounters.Add(this);
            this.UpdateCounter();
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}