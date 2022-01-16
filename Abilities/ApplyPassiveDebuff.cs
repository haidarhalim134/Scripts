using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;

namespace Attributes.Abilities
{
    public class ApplyPassiveDebuff : MonoBehaviour
    {
        public Debuffs type;
        public int charge;
        public GameObject effect;
        static StatProcessor Calc = new StatProcessor();
        public void Ability(BaseCreature caster, BaseCreature target, AbilityData Data = null)
        {
            caster.DebuffsAddPassive(this.type, this.charge);
            if (effect!=null)Instantiate(effect, caster.transform).transform.localPosition = new Vector2();
        }
        public string Text()
        {
            return $"Apply {this.type} for {this.charge} turn";
        }
        void Awake()
        {
            AbilityManager Mng = gameObject.GetComponent<AbilityManager>();
            Mng.ContainedAbilities.Add(this.Ability);
            Mng.DescGrabber.Add(this.Text);
        }
    }
}
