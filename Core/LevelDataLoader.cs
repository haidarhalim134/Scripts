using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;
using DataContainer;
using Control.Core;
using Attributes.Abilities;

namespace LevelManager
{public class LevelDataLoader : MonoBehaviour
{
    /// <summary>default spreader : random</summary>
    public static void LoadLevel(LevelDataContainer Level)
        {
            // TODO: create a simpler version of this, use prefab with object named 1 through n, then replace each number with actual creature
            Func<GameObject, int, float[]> Spreader = RandomXY;
            if (Level.SpreadType == "straight")
            {
                Spreader = StraightXY;
            }
            foreach (BotDataContainer Cont in Level.Creature)
            {
                GameObject Creature = Instantiate(Level.Prefab) as GameObject;
                Creature.AddComponent<BotController>();
                BotController Script = Creature.GetComponent<BotController>();
                SpawnCreature(Creature, Script, Cont.CreatureAsset, Cont.TeamId, Level.GetCreatureCount(Cont.TeamId), Spreader);
            }
            float[] RandomXY(GameObject SpawnPlace, int TotalCreature)
            { 
                Vector2 Rect = SpawnPlace.GetComponent<RectTransform>().sizeDelta;
                return new float[2] { Rect.x*Range(-0.4f,0.4f), Rect.y*Range(-0.4f,0.4f)};
            }
            float[] StraightXY(GameObject SpawnPlace, int TotalCreature)
            {
                Vector2 Rect = SpawnPlace.GetComponent<RectTransform>().sizeDelta;
                int Order = SpawnPlace.transform.childCount;
                TotalCreature+= 1;
                float sep = Rect.x/TotalCreature;
                return new float[2] { Rect.x*-0.5f+sep*(Order),0 };
            }
        }
        private static void SpawnCreature(GameObject Object, BotController Script, CreatureDataContainer Data, int TeamId, int TeamCount, Func<GameObject, int, float[]> PosGenerator)
        {
            Object.GetComponent<SpriteRenderer>().sprite = Data.Skin;
            // initiate gameobject's stamina and health container
            Script.InitStats();
            // assigning the health
            Script.health.Max = Data.MaxHealth;
            Script.stamina.Max = Data.MaxStamina;
            Script.TeamId = TeamId;

            GameObject SpawnPlace = GameObject.Find("Team"+TeamId);
            Object.transform.parent = SpawnPlace.transform;

            float[] Pos = PosGenerator(SpawnPlace, TeamCount);
            Object.transform.localPosition = new Vector3(Pos[0], Pos[1], -2835f);
            SpawnCounter(Object);
            foreach (string name in Data.Abilities)
                {
                    Script.Skills.Add(AbilitiesRepository.GetAbility(name));
                }
        }
        public static void SpawnCounter(GameObject Object)
        {
            SpriteRenderer Renderer = Object.GetComponent<SpriteRenderer>();
            GameObject StaminaC = SpawnCounterPrefab(InGameContainer.GetInstance().StaminaCounter, Object);
            GameObject HealthC = SpawnCounterPrefab(InGameContainer.GetInstance().HealthCounter, Object);
            StaminaC.transform.localPosition = new Vector2(0, Renderer.bounds.size.y);
            HealthC.transform.localPosition = new Vector2(0, Renderer.bounds.size.y*-1);
        }
        public static GameObject SpawnCounterPrefab(GameObject Prefab, GameObject Parent)
        {
            GameObject Obj = Instantiate(Prefab);
            Obj.GetComponent<BaseCounter>().Target = Parent;
            Obj.transform.SetParent(Parent.transform);
            return Obj;
        }
    }
}