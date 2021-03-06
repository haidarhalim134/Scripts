using System.Linq;
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
        ability.transform.DOScale(1, 0.1f);
        queue.Add(new CardQ(ability, target));
        ability.enableHover = false;
        ability.transform.SetParent(transform);
        ability.transform.SetAsFirstSibling();
        ability.transform.DOMove(waitPlace.transform.position, InGameContainer.GetInstance().delayBetweenTurn * delayPercentageMoving).WaitForCompletion();
        startQueue();
    }
    void startQueue()
    {
        if (!runnningQueue) StartCoroutine(RunQueue());
        runnningQueue = true;
    }
    IEnumerator RunQueue()
    {
        while (queue.Count > 0)
        {
            if (queue[0].target.dead)
            { 
                ReturnToDeck();
                break;
            }
            destroyQueue.Add(queue[0]);
            queue[0].ability.transform.DOKill();
            owner.Owner.OrderAbility(queue[0].ability.Ability, false);
            owner.Owner.AbilitySendOrdered(queue[0].target);
            yield return queue[0].ability.transform.DOMove(applyPlace.transform.position, InGameContainer.GetInstance().delayBetweenTurn * delayPercentageMoving)
            .OnComplete(()=>{
                StartCoroutine(WaitThenDestroy());
            }).WaitForCompletion();
            queue.RemoveAt(0);
            yield return StartCoroutine(WaitUntilFinish());
            while (owner.Owner.currTween)yield return null;
            if (queue.Count==0)break;
            yield return InGameContainer.GetInstance().delayBetweenTurn*delayPercentageApply;
        }
        runnningQueue = false;
    }
    IEnumerator WaitUntilFinish()
    {
        while (owner.Owner.currTween) yield return null;
    }
    IEnumerator WaitThenDestroy()
    {
        yield return StartCoroutine(WaitUntilFinish());
        if (InGameContainer.GetInstance().SpawnAbilityPrefab(destroyQueue[0].ability.Ability.name).cardModifiers.Any((item)=>item == CardModifier.exhaust))
        {
            destroyQueue[0].ability.Destroy(RemoveStatus.exhausted);
            owner.Owner.DeckMoveToExhausted(destroyQueue[0].ability.Ability);
        }else
        {
            destroyQueue[0].ability.Destroy(RemoveStatus.used);
            owner.Owner.DeckMoveToUsed(destroyQueue[0].ability.Ability);
        }
        destroyQueue.RemoveAt(0);
    }
    void ReturnToDeck()
    {
        var handlers = queue.Select((item)=>item.ability);
        owner.ActiveDeck.AddRange(handlers);
        handlers.ToList().ForEach((item)=>{
            item.transform.SetParent(owner.transform);
            item.enableHover = true;
            item.stillDown = false;
        });
        queue.Clear();
        owner.RefreshCardPos();
        owner.ChangeCardRaycast(true);
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
