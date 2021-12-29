using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;
using Control.Combat;
using Attributes.Abilities;

namespace Control.Core
{
    public class BotController : BaseCreature
    {
        public List<AbilityManager> Skills = new List<AbilityManager>();
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
            this.stamina.Max = 3;
            this.health.Max = 100;
            this.stamina.Fill();
            this.health.Fill();
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
            CombatEngine.RequestCast(this.Skills[Range(0, this.Skills.Count)], this,CombatEngine.RegisteredCreature[this.EnemyId][
                Range(0,CombatEngine.RegisteredCreature[this.EnemyId].Count)]);
            if (this.stamina.Curr<1)
            {
                CombatEngine.ActionFinished();
                this.Control = false;
            } else {
                 StartCoroutine(Decide());
            }
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