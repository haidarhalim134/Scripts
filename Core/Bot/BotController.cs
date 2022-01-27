using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;
using Control.Combat;
using Attributes.Abilities;
using DataContainer;

namespace Control.Core
{
    public class BotController : BaseCreature
    {
        public List<BotAbilityCont> Skills = new List<BotAbilityCont>();
        private float ActionDelay = 1f;
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
            this.OnDeath = this.OnDeathBot;
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
            yield return new WaitForSeconds(this.ActionDelay);
            var EnoughMana = this.Skills.Where((cont)=> cont.Ability.GetComponent<AbilityManager>().GetStaminaCost(cont.Data)<=this.stamina.Curr);
            var Cont = EnoughMana.ToList()[Range(0, EnoughMana.Count())];
            AbilityManager Mng = InGameContainer.GetInstance().SpawnAbilityPrefab(Cont.Ability).GetComponent<AbilityManager>();
            CombatEngine.RequestCast(Mng, this,CombatEngine.RegisteredCreature[this.EnemyId][
                Range(0,CombatEngine.RegisteredCreature[this.EnemyId].Count)],Cont.Data);
            if (this.stamina.Curr<1)
            {
                this.DebuffReduceCharge();
                CombatEngine.ActionFinished();
                this.Control = false;
            } else {
                 if (CombatEngine.GameGoing)StartCoroutine(Decide());
            }
        }
        public void OnDeathBot()
        {
            if (CombatEngine.RegisteredCreature[this.TeamId].Count == 0) CombatEngine.EndGame(true);
        }
        // Start is called before the first frame update
        void Start()
        {
            // TODO: call moved
            // this.InitStats();
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