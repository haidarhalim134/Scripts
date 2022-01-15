using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;

public class ShieldCounter : BaseCounter
{
    public void UpdateCounter()
    {
        this.Counter.text = this.Creature.shield.Curr.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        this.Awoke();
        this.Creature.ShieldCounters.Add(this);
        this.UpdateCounter();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
