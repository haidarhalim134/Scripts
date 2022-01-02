using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;

namespace Attributes.Abilities
{
    public class Shield : MonoBehaviour
    {
        public int shield = 10;
        static StatProcessor Calc = new StatProcessor();
        public void Ability(BaseCreature caster, BaseCreature target, string GUID = null)
        {
            caster.GiveShield(this.shield);
        }
        public string Text()
        {
            return $"give {this.shield}";
        }
        void Start()
        {
            AbilityManager Mng = gameObject.GetComponent<AbilityManager>();
            Mng.ContainedAbilities.Add(this.Ability);
            Mng.DescGrabber.Add(this.Text);
        }
    }
}
