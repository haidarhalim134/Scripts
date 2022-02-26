using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using Map;
using Control.Combat;
using Attributes.Abilities;
using DataContainer;

namespace Control.Core
{
    public class BotController : BaseCreature
    {
        public List<AttackPatternCont> Skills = new List<AttackPatternCont>();
        public BotAbilityCont nextAction;
        public BaseCreature nextTarget;
        public IntentsCounter intentCounter;
        public LoopingIndex attackPLoop;
        /// <summary>please call on start</summary>
        private void Awake() 
        {
            this.AssignTeam(1);
            CombatEngine.RegisterCreature(this);
        }
        public void InitStats()
        {
            this.BaseInit();
            this.Acted = false;
            this.IsPlayer = false;
        }
        public void initAbilLoop()
        {
            attackPLoop = new LoopingIndex(Skills.Count());
        }
        private void AssignTeam(int Id)
        {
            this.TeamId = Id;
            if (Id == 0)
            {
                this.EnemyId = 1;
            } else
            {
                this.EnemyId = 0;
            }
        }
        private IEnumerator Decide()
        {
            var rnd = new Random();
            yield return new WaitForSeconds(InGameContainer.GetInstance().delayBetweenTurn/2f);
            var cont = GetQAbil();
            yield return new WaitForSeconds(InGameContainer.GetInstance().delayBetweenTurn);
            AbilityManager Mng = GetMng(cont);
            var listoftarget = CombatEngine.GetTarget(Mng);
            CombatEngine.RequestCast(Mng, this, nextTarget, cont.Data);
            this.DebuffReduceCharge(ReduceChargeTime.onEndTurn);
            CombatEngine.ActionFinished();
            this.Control = false;
        }
        public BotAbilityCont GetQAbil()
        {
            var abil = nextAction;
            nextAction = GetRandomAbil();
            return abil;
        }
        public BotAbilityCont GetRandomAbil()
        {
            var rnd  = new Random();
            int index = attackPLoop.Next();
            var EnoughMana = this.Skills[index].abilities.Where((cont) => 
            {return InGameContainer.GetInstance().SpawnAbilityPrefab(cont.Ability).GetStaminaCost(cont.Data) <= this.stamina.Curr;});
            var Cont = rnd.Choice(EnoughMana.ToList(), EnoughMana.Select((cont)=>cont.weight).ToList())[0];
            nextTarget = CombatEngine.RegisteredCreature[EnemyId][CombatEngine.RegisteredCreature[EnemyId].Count-1];
            intentCounter.Spawn(InGameContainer.GetInstance().SpawnAbilityPrefab(Cont.Ability), Cont.Data);
            return Cont;
        }
        public AbilityManager GetMng(BotAbilityCont cont)
        {
            return InGameContainer.GetInstance().SpawnAbilityPrefab(cont.Ability);
        }
        public override void OnDeath()
        {
            intentCounter.Destroy();
            if (CombatEngine.RegisteredCreature[this.TeamId].Count == 0) CombatEngine.EndGame(true);
        }
        // Start is called before the first frame update
        void Start()
        {
            this.health.Update(0);
            this.stamina.Update(0);
        }

        // Update is called once per frame
        void Update()
        {
            if (this.Control&&!this.Acted)
            {
                StartCoroutine(this.Decide());
                this.Acted = true;
            }
        }
    }
}