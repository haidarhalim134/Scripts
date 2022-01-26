using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Control.Combat;
using DG.Tweening;
using Attributes.Abilities;

namespace Control.Core
{
    public class BaseCreature : MonoBehaviour
    {
        public int TeamId;
        public int EnemyId;
        public Health health;
        public Stamina stamina;
        public Shield shield;
        [HideInInspector]
        public List<HealthCounter> HealthCounters = new List<HealthCounter>();
        [HideInInspector]
        public List<StaminaCounter> StaminaCounters = new List<StaminaCounter>();
        [HideInInspector]
        public List<ShieldCounter> ShieldCounters = new List<ShieldCounter>();
        public DebuffCounter debuffCounter;
        public BuffDebuff buffDebuff = new BuffDebuff();
        public bool Control = false;
        [HideInInspector]
        public bool Acted;
        public bool AsTarget = false;
        public bool IsPlayer;
        public Action<bool> Setup = (bool T) => { };
        public Action OnDeath = () => { };
        public void SendTarget()
        {
            if (this.AsTarget)
            {
                CombatEngine.SendTargetToPlayer(this);
            }
        }
        public void BaseInit()
        {
            this.GetComponent<Button>().onClick.AddListener(this.SendTarget);
            this.health = new Health(this.HealthCounters);
            this.stamina = new Stamina(this.StaminaCounters);
            this.shield = new Shield(this.ShieldCounters);
        }
        public void IsTarget(bool to)
        {
            this.AsTarget = to;
        }
        public void turn(bool to)
        {
            // var cont = new PassiveDebuff() { charge = 1, debuff = Debuffs.vulnerable };
            // this.buffDebuff.passiveDebuffs.Add(cont);
            this.Control = to;
            this.Acted = false;
            if (to)
            {
                this.stamina.Fill();
            }
            this.stamina.Update(0);
            this.Setup(to);
            if (!to)
            {
                this.buffDebuff.Activate(this.buffDebuff.endTurnActivate);
            }
        }
        /// <returns>return false if dead else true</returns>
        public bool TakeDamage(int damage)
        {
            
            if (this.shield.Curr>0)
            {
                    this.health.Update(this.shield.Damage(damage * -1) *-1);
                    if (this.health.Curr<=0)
                    {
                        return true;
                    }
                return false;
            } else if (damage >= this.health.Curr)
            {
                this.health.Update(this.health.Curr * -1);
                this.Death();
                return false;
            } else
            {
                this.health.Update(damage * -1);
                return true;
            }
        }
        public void DebuffsAddPassive(Debuffs debuff, int charge)
        {
            PassiveDebuff cont = this.buffDebuff.passiveDebuffs.Find((cont) => cont.debuff == debuff);
            if (cont != null)
            {
                cont.charge+= charge;
            }else
            {
                cont = new PassiveDebuff() { charge = charge, debuff = debuff };
                this.buffDebuff.passiveDebuffs.Add(cont);
                this.debuffCounter.AddPassiveDebuff(cont);
            }
        }
        public void DebuffAddActive(List<ActiveDebuff> list, ActiveDebuff debuff)
        {
            ActiveDebuff cont = list.Find((cont) => cont.name == debuff.name);
            switch(cont)
            {
                case null:
                    list.Add(debuff);
                    break;
                default:
                    cont.charge+= debuff.charge;
                    break;
            }
        }
        /// <summary>input positive number to reduce stamina</summary>
        public void UseStamina(int by)
        {
            this.stamina.Update(by * -1);
        }
        /// <summary>call at the end of each turn to reduce debuff charge by 1</summary>
        public void DebuffReduceCharge()
        {
            List<PassiveDebuff> PRemove = new List<PassiveDebuff>();
            this.buffDebuff.passiveDebuffs.ForEach((cont)=>{
                cont.charge-= 1;
                if (cont.charge<= 0)
                {
                    PRemove.Add(cont);
                }
            });
            PRemove.ForEach((cont)=> this.buffDebuff.passiveDebuffs.Remove(cont));
        }
        public void Death()
        {
            if (this.Control)
            {
                CombatEngine.ActionFinished();
            }
            CombatEngine.UnRegisterCreature(this);
            this.HealthCounters.ForEach((counter)=>{
                Destroy(counter.gameObject);
            });
            this.ShieldCounters.ForEach((counter) =>
            {
                Destroy(counter.gameObject);
            });
            Destroy(this.debuffCounter.gameObject);
            this.OnDeath();
            this.GetComponent<SpriteRenderer>().DOFade(0f, 0.25f)
            .OnComplete(()=>Destroy(this.gameObject));
        }
        void Update()
        {
            
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
    public class Shield
    {
        public int Curr = 0;
        public List<ShieldCounter> Counters;
        public Shield(List<ShieldCounter> Counters)
        {
            this.Counters = Counters;
        }
        public bool Enough(int cost)
        {
            return this.Curr >= cost;
        }
        public void Update(int by)
        {
            this.Curr += by;
            Counters.ForEach((counter) => counter.UpdateCounter());
        }
        /// <summary>return excess damage as positive int</summary>
        public int Damage(int by)
        {
            this.Curr += by;
            if (this.Curr >= 0)
            {
                Counters.ForEach((counter) => counter.UpdateCounter());
                return 0;
            } else
            {
                int excess = Curr*-1;
                this.Curr = 0;
                Counters.ForEach((counter)=>counter.UpdateCounter());
                return excess;
            }
        }
    }
    [Serializable]
    public class PassiveDebuff
    {
        public int charge;
        public Debuffs debuff;
    }
    [Serializable]
    public class ActiveDebuff
    {
        public string name;
        public int charge;
        public AbilityData data;
        public BaseCreature caster;
        public BaseCreature target;
        public Action<ActiveDebuff> debuff;
        public void Trigger()
        {
            debuff(this);
        }
        public ActiveDebuff(string name, int charge, AbilityData data, BaseCreature caster, BaseCreature target, Action<ActiveDebuff> function)
        {
            this.charge = charge;
            this.data = data;
            this.caster = caster;
            this.target = target;
            this.debuff = function;
        }
    }
    [Serializable]
    public class BuffDebuff
    {
        public List<PassiveDebuff> passiveDebuffs = new List<PassiveDebuff>();
        public List<ActiveDebuff> endTurnActivate = new List<ActiveDebuff>();
        public void Activate(List<ActiveDebuff> list)
        {
            list.ForEach((debuff)=>debuff.Trigger());
        }
    }
}