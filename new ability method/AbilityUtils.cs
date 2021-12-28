using System;
using System.Collections.Generic;
using UnityEngine;

namespace Attributes.Abilities
{
    public class AbilityUtils : MonoBehaviour
    {
        public static AbilityManager GetAbility(string name)
        {
            return GameObject.Find(name).GetComponent<AbilityManager>();
        }
    }
    public class AbilityContainere
    {
        public string GUID;
        public string name;
    }
}