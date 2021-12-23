using System;
using System.Collections.Generic;
using UnityEngine;
using Control.Combat;

namespace Control.Core
{
    public class BaseCreature : MonoBehaviour
    {
        public int TeamId;
        public int EnemyId;
        public Stamina stamina;
        public int MaxHealth;
        // [HideInInspector]
        public int CurrHealth;
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
        void OnMouseUp()
        {
            if (this.AsTarget)
            {
                if (CombatEngine.isUIOverride)
                {
                    Debug.Log("over ui");
                }
                else
                {
                    CombatEngine.SendTargetToPlayer(this);
                    Debug.Log("ok");
                }
            }
        }
        public void BaseInit()
        {
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
        public void UpdateHealth(int by)
        {
            this.ActiveShield += by;
            if (this.ActiveShield < 0)
            {
                this.CurrHealth += this.ActiveShield;
                this.ActiveShield = 0;
            }
            foreach (HealthCounter Counter in HealthCounters)
            {
                Counter.UpdateCounter();
            }
        }
        public void TakeDamage(int damage)
        {
            if (damage >= this.CurrHealth)
            {
                this.UpdateHealth(this.CurrHealth * -1);
                this.Death();
            }
            else
            {
                this.UpdateHealth(damage * -1);
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