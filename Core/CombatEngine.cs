using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Random;
using Control.Core;
using Attributes.Abilities;
using DataContainer;
using LevelManager;

namespace Control.Combat
{
    public class CombatEngine : MonoBehaviour
    {
        /// <summary>array index indicate team id, use Inturn to get current active creature</summary>
        public static List<BaseCreature>[] RegisteredCreature = { new List<BaseCreature>(),
         new List<BaseCreature>()};
        private static int index = 0;
        private static BaseCreature InTurn;
        public static bool isUIOverride { get; private set; }
        /// <summary>called by a creature when they finished their turn</summary>
        public static void ActionFinished()
        {
            ChangeTurn(index, false);
            index = NextAction();
            ChangeTurn(index, true);
        }
        /// <summary>progress index by 1, reset if index at the end of the creature list</summary>
        private static int NextAction()
        {
            index+= 1;
            if (index >= RegisteredCreature[0].Count+RegisteredCreature[1].Count)
            {
                index = 0;
            }
            return index;
        }
        /// <returns>true if request accepted else false</returns>
        public static bool RequestCast(AbilityContainer Ability, BaseCreature caster, BaseCreature target = null)
        {
            if (caster.stamina.Enough(Ability.cost))
            {
                caster.UseStamina(Ability.cost);
                Ability.ability(caster, target);
                return true;
            } else 
            {
                return false;
            }
        }
        /// <summary>add a creature to circulation, optional second argument</summary>
        public static void RegisterCreature(BaseCreature Creature, bool Player = false)
        {
            if (Player)
            {
                RegisteredCreature[Creature.TeamId].Insert(0, Creature);
            } else
            {
                RegisteredCreature[Creature.TeamId].Add(Creature);
            }
            Debug.Log(RegisteredCreature[0].Count);
        }
        public static void UnRegisterCreature(BaseCreature Creature)
        {
            int CreatureIndex = RegisteredCreature[Creature.TeamId].IndexOf(Creature);
            if (Creature.TeamId == 1)
            {
                CreatureIndex+= RegisteredCreature[0].Count;
            }
            if (CreatureIndex <= index)
            {
                index--;
            }
            RegisteredCreature[Creature.TeamId].Remove(Creature);
        }
        public static void SendTargetToPlayer(BaseCreature Creature)
        {
            InTurn.gameObject.GetComponent<PlayerController>().SendOrderedAbility(Creature);          
        }
        public static void ClearTarget()
        {
            foreach (List<BaseCreature> Lis in RegisteredCreature)
            {
                foreach (BaseCreature Creature in Lis)
                {
                    Creature.IsTarget(false);
                }
            }
        }
        /// <summary>setup target for Creature currently InTurn</summary>
        public static void SetupTarget(AbilityContainer Ability)
        {
            ClearTarget();
            if (Ability.target == 1)
            {
                foreach (BaseCreature Creature in RegisteredCreature[InTurn.TeamId])
                {
                    if (Creature!= InTurn)
                    {
                        Creature.IsTarget(true);
                    }
                }
            } else if (Ability.target == 2)
            {
                foreach (BaseCreature Creature in RegisteredCreature[InTurn.EnemyId])
                {
                    if (Creature!= InTurn)
                    {
                        Creature.IsTarget(true);
                    }
                }
            } else if (Ability.target == 0)
            {
                InTurn.IsTarget(true);
            }
        }
        /// <summary>class internal function to change turn</summary>
        private static void ChangeTurn(int index, bool turn)
        {
            Debug.Log(index+":"+turn);
            ClearTarget();
            if (index >= RegisteredCreature[0].Count)
            {
                int indexF1 = index - RegisteredCreature[0].Count;
                RegisteredCreature[1][indexF1].turn(turn);
                if (turn)
                {
                    InTurn = RegisteredCreature[1][indexF1];
                }                
            } else
            {
                RegisteredCreature[0][index].turn(turn);
                if (turn)
                {
                    InTurn = RegisteredCreature[0][index];
                }
            }
        }
        private static void SetupPlayerUI(PlayerController Creature)
        {
            Creature.SetupUI();
        }
        void OnMouseUp()
        {
            Debug.Log("click");
            if (InTurn.IsPlayer)
            {
                PlayerController Controller = InTurn.gameObject.GetComponent<PlayerController>();
                Controller.ClearOrder();
                Controller.Deck.RefreshCardPos();
                
            }
        }
        /// <summary>call level loader and remove prev level garbage</summary>
        void Awake() 
        {
            RegisteredCreature[0].Clear();RegisteredCreature[1].Clear();
            InTurn = null;index = 0;
            LevelDataLoader.LoadLevel(LevelLoader.ToLoad);
            if (LevelLoader.ToLoad == null)
            {
                Debug.Log("Combat LevelData not found");
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            index = 0;
            ChangeTurn(index, true);
        }
        void Update() 
        {
            isUIOverride = EventSystem.current.IsPointerOverGameObject();
        }
    }
}