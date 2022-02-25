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
    List<CardQ> queue = new List<CardQ>();
    List<CardQ> destroyQueue = new List<CardQ>();
    public bool runnningQueue;
    public float delayPercentageApply;
    public float delayPercentageMoving;
    public GameObject waitPlace;
    public GameObject applyPlace;
    public void AddQueue(CardHandler ability, BaseCreature target)
    {
        queue.Add(new CardQ(ability, target));
        ability.enableHover = false;
        ability.transform.SetParent(transform);
        ability.transform.SetAsFirstSibling();
        StartCoroutine(waitMove(ability));
        ability.transform.DOMove(waitPlace.transform.position, InGameContainer.GetInstance().delayBetweenTurn * delayPercentageMoving).WaitForCompletion();
        startQueue();
    }
    IEnumerator waitMove(CardHandler ability)
    {
        yield return null;
    }
    void startQueue()
    {
        if (!runnningQueue) StartCoroutine(RunQueue());
        runnningQueue = true;
    }
    IEnumerator RunQueue()
    {
        Debug.Log(queue.Count+"-"+runnningQueue);
        while (queue.Count > 0)
        {
            destroyQueue.Add(queue[0]);
            queue[0].ability.transform.DOKill();
            yield return queue[0].ability.transform.DOMove(applyPlace.transform.position, InGameContainer.GetInstance().delayBetweenTurn * delayPercentageMoving)
            .OnComplete(()=>{
                destroyQueue[0].ability.Destroy(RemoveStatus.used);
                owner.Owner.DeckMoveToUsed(destroyQueue[0].ability.Ability);
                destroyQueue.RemoveAt(0);
            }).WaitForCompletion();
            owner.Owner.OrderAbility(queue[0].ability.Ability, false);
            owner.Owner.AbilitySendOrdered(queue[0].target);
            queue.RemoveAt(0);
            if (queue.Count>0)yield return InGameContainer.GetInstance().delayBetweenTurn*delayPercentageApply;
        }
        runnningQueue = false;
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
