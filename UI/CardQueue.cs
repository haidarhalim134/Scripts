using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Control.Combat;
using Control.Deck;
using DataContainer;
using Attributes.Abilities;
using DG.Tweening;

public class CardQueue : MonoBehaviour
{
    [SerializeField]
    CardDeck owner;
    List<CardQ> queue;
    bool runnningQueue;
    public float delayPercentageApply;
    public float delayPercentageMoving;
    public GameObject waitPlace;
    public GameObject applyPlace;
    public void AddQueue(CardHandler ability, BaseCreature target)
    {
        queue.Add(new CardQ(ability, target));
        ability.enableHover = false;
        ability.transform.SetParent(transform);
        ability.transform.DOMove(waitPlace.transform.position, InGameContainer.GetInstance().delayBetweenTurn*delayPercentageMoving);
        ability.transform.SetAsFirstSibling();
        startQueue();
    }
    void startQueue()
    {
        runnningQueue = true;
        StartCoroutine(RunQueue());
    }
    IEnumerator RunQueue()
    {
        while (queue.Count > 0)
        {
            if (runnningQueue) break;
            queue[0].ability.transform.DOMove(applyPlace.transform.position, InGameContainer.GetInstance().delayBetweenTurn * delayPercentageMoving)
            .OnComplete(()=>queue[0].ability.Destroy(RemoveStatus.used));
            owner.Owner.OrderAbility(queue[0].ability.Ability, false);
            owner.Owner.AbilitySendOrdered(queue[0].target);
            queue.RemoveAt(0);
            if (queue.Count == 0) runnningQueue = false;
            yield return InGameContainer.GetInstance().delayBetweenTurn*delayPercentageApply;
        }
    }
}
public class CardQ
{
    public CardHandler ability;
    public BaseCreature target;
    public CardQ(CardHandler ability, BaseCreature target)
    {
        this.ability = ability;
        this.target = target;
    }
}
