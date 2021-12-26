using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Control.Combat;

namespace Control.Core
{
    public class BaseCreature : MonoBehaviour
    {
        public int TeamId;
        public int EnemyId;
        public Health health;
        public Stamina stamina;
        // public int MaxHealth;
        // // [HideInInspector]
        // public int CurrHealth;
        public int ActiveShield = 0;
        [HideInInspector]
        public List<HealthCounter> HealthCounters = new List<HealthCounter>();
        [HideInInspector]
        public List<StaminaCounter> StaminaCounters = new List<StaminaCounter>();
        // [HideInInspector]
        public bool Control = false;
        [HideInInspector]
        public bool Acted;
        public bool AsTarget = false;
        public bool IsPlayer;
        // TODO: simplify this shit
        public Action<bool> Setup = (bool T) => { };
        public void SendTarget()
        {
            CombatEngine.SendTargetToPlayer(this);
        }
        public void BaseInit()
        {
            this.GetComponent<Button>().onClick.AddListener(this.SendTarget);
            this.health = new Health(this.HealthCounters);
            this.stamina = new Stamina(this.StaminaCounters);
        }
        public void IsTarget(bool to)
        {
            this.AsTarget = to;
        }
        public void turn(bool to)
        {
            this.Control = to;
            this.Acted = false;
            if (to)
            {
                this.stamina.Fill();
            }
            this.stamina.Update(0);
            this.Setup(to);
        }
        /// <returns>return false if dead else true</returns>
        public bool TakeDamage(int damage)
        {
            if (damage >= this.health.Curr)
            {
                this.health.Update(this.health.Curr * -1);
                this.Death();
                return false;
            }
            else
            {
                this.health.Update(damage * -1);
                return true;
            }
        }
        public void UseStamina(int by)
        {
            this.stamina.Update(by * -1);
        }
        public void Death()
        {
            CombatEngine.UnRegisterCreature(this);
            Destroy(gameObject);
        }
        public void GiveShield(int by)
        {
            this.ActiveShield += by;
        }
    }
    public class Stamina
    {
        public int Max = 0;
        public int Curr = 0;
        public List<StaminaCounter> Counters;
        public Stamina(List<StaminaCounter> Counters)
        {
            this.Counters = Counters;
        }
        public void Fill(int by = 0)
        {
            if (by == 0)
            {
                this.Curr = this.Max;
            }
            else
            {
                this.Curr += by;
            }
        }
        public bool Enough(int cost)
        {
            return this.Curr >= cost;
        }

        public void Update(int by)
        {
            this.Curr += by;
            foreach (StaminaCounter Counter in this.Counters)
            {
                Counter.UpdateCounter();
            }
        }
    }
    public class Health
    {
        public int Max = 0;
        public int Curr = 0;
        public List<HealthCounter> Counters;
        public Health(List<HealthCounter> Counters)
        {
            this.Counters = Counters;
        }
        public void Fill(int by = 0)
        {
            if (by == 0)
            {
                this.Curr = this.Max;
            }
            else
            {
                this.Curr += by;
            }
        }
        public bool Enough(int cost)
        {
            return this.Curr >= cost;
        }

        public void Update(int by)
        {
            this.Curr += by;
            foreach (HealthCounter Counter in this.Counters)
            {
                Counter.UpdateCounter();
            }
        }
    }
}