using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using DG.Tweening;

public class ShieldCounter : BaseCounter
{
    bool shown=true;
    public SpriteRenderer icon;
    public void UpdateCounter()
    {
        this.Counter.text = this.Creature.shield.Curr.ToString();
        if (this.Creature.shield.Curr == 0)this.hide();
        if (this.Creature.shield.Curr > 0)this.show();
    }
    // Start is called before the first frame update
    void Start()
    {
        this.Awoke();
        this.Creature.ShieldCounters.Add(this);
        this.UpdateCounter();
        //TODO: to force rehide it instantly, there might be a better solution
        this.shown = true;
        this.hide(true);
    }
    void show()
    {
        if (!shown){ this.Counter.DOFade(1, 0.2f); this.icon.DOFade(1, 0.2f); shown = true; }
    }
    void hide(bool instant = false)
    {
        if (shown) 
        {
            if (instant)
            {
                Color c;
                c = this.Counter.color;
                c.a = 0;
                this.Counter.color = c;
                c = this.icon.color;
                c.a = 0;
                this.icon.color = c;
            }
            else if (!instant){ this.Counter.DOFade(0, 0.2f); this.icon.DOFade(0, 0.2f);}
            shown = false;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
