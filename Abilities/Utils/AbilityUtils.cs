using System;
using System.Collections.Generic;
using UnityEngine;
using DataContainer;
using DG.Tweening;

namespace Attributes.Abilities
{
    public class AbilityUtils
    {
        public static string g = "<color=#29b400>";
        public static string r = "<color=\"red\">";
        public static string b = "<b>";
        public static Dictionary<CardModifier, string> cMPastTense = new Dictionary<CardModifier, string>()
        {{CardModifier.normal, "used"},{CardModifier.keep, "kept"}};
        public static string CalcColor(int basenumber, int calcnumber)
        {
            if (basenumber == calcnumber)return "";
            else if (basenumber < calcnumber)return g;
            else return r;
        }
    }
    [Serializable]
    public class AbilityContainer
    {
        public AbilityData Data = new AbilityData();
        public string name;
        public AbilityManager GetManager()
        {
            GameObject Object = GameObject.Find(name);
            if (Object == null)
            {
                InGameContainer.GetInstance().SpawnAbilityPrefab(name);
            }
            return GameObject.Find(name).GetComponent<AbilityManager>();
        }
    }
    [Serializable]
    public class BotAbilityCont
    {
        public GameObject Ability;
        public AbilityData Data;
        public AbilityContainer ToNormalContainer()
        {
            return new AbilityContainer(){name=Ability.GetComponent<AbilityManager>().AbName, Data=Data};
        }
    }
    [Serializable]
    public class AbilityData
    {
        [SerializeField] int _level;
        public int Level {get{return _level+tempAbData.level;} set{_level = value;}}
        [SerializeField] int _damage;
        public int Damage {get{return _damage+tempAbData.damage;} set{_damage = value;}}
        [SerializeField] int _shield;
        public int Shield {get{return _shield+tempAbData.shield;} set{_shield = value;}}
        [SerializeField] int _staminaCost;
        public int Staminacost {get{return _staminaCost+tempAbData.staminaCost;} set{_staminaCost = value;}}
        [SerializeField] int _attackRep;
        public int AttackRep {get{return _attackRep+tempAbData.attackRep;} set{_attackRep = value;}}
        [NonSerialized]
        public TempAbData tempAbData = new TempAbData();
        public AbilityData Add(AbilityData data)
        {
            var res = new AbilityData(){
                Level = this.Level + data.Level,
                Damage = this.Damage + data.Damage,
                Shield = this.Shield + data.Shield,
                Staminacost = this.Staminacost + data.Staminacost,
                AttackRep = this.AttackRep + data.AttackRep
            };
            return res;
        }
    }
    public class TempAbData
    {
        public int level;
        public int damage;
        public int  shield;
        public int staminaCost;
        public int attackRep;
    }
    [Serializable]
    public class OverrideDesc
    {
        public bool Override;
        public string desc;
    }
    public enum Debuffs{vulnerable, weakened, marked, evasive, strength, wound}
    public enum Stance{rage, excited, noStance}
    public enum Targeting{caster,target}
    public enum AbilityType{attack,buff,debuff,shield}
    public enum AbTarget{self, allies, enemy, allEnemy}
    public enum ModType{preAttack,preDamage,postDamage,postAttack}
    public enum CardModifier{normal,keep, exhaust}
}