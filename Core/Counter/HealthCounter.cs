using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Control.Core
{
    public class HealthCounter : BaseCounter
    {
        public GameObject Bar;
        public void UpdateCounter()
        {
            this.Counter.text = this.Creature.health.Curr+"/"+this.Creature.health.Max;
            var percentage = this.Creature.health.Curr / (float)this.Creature.health.Max;
            Bar.transform.localScale = new Vector2(percentage,1);
            // Debug.Log(""+percentage+"-"+((1 - percentage) / 2f)+"-"+Bar.GetComponent<SpriteRenderer>().bounds.size.x/ Bar.transform.localScale.x);
            Bar.transform.localPosition = new Vector2(0 - ((1-percentage)/2f)*Bar.GetComponent<SpriteRenderer>().bounds.size.x/Bar.transform.localScale.x*4,0);
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