using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using static UnityEngine.Random;
using DataContainer;
using Control.Core;
using Attributes.Abilities;

namespace LevelManager
{
    public class LevelDataLoader : MonoBehaviour
    {
        /// <summary>default spreader : random</summary>
        public static void LoadLevel(LevelDataContainer Level)
        {
            // TODO: create a simpler version of this, use prefab with object named 1 through n, then replace each number with actual creature
            foreach (BotDataContainer Cont in Level.Creature)
            {
                GameObject Creature = Instantiate(Level.CreaturePrefab, GameObject.Find("Team" + Cont.TeamId).transform) as GameObject;
                Creature.AddComponent<BotController>();
                InitCreature(Creature, Cont.CreatureAsset, Level, Cont);
            }
        }
        static float[] RandomXY(GameObject SpawnPlace)
        {
            Vector2 Rect = SpawnPlace.GetComponent<RectTransform>().sizeDelta;
            return new float[2] { Rect.x * Range(-0.4f, 0.4f), Rect.y * Range(-0.4f, 0.4f) };
        }
        static float[] ReadCoord(GameObject SpreadPrefab, BotDataContainer BotCont)
        {
            Transform Slot = SpreadPrefab.transform.Find(BotCont.SpreadPrefabPlace.ToString());
            if (Slot == null)
            {
                Debug.Log("randomized, no spot");
                return RandomXY(SpreadPrefab);
            }
            return new float[2] { Slot.localPosition.x, Slot.localPosition.y };
        }
        private static void InitCreature(GameObject Object, CreatureDataContainer Data, LevelDataContainer LevelCont, BotDataContainer BotCont)
        {
            BotController Script = Object.GetComponent<BotController>();
            int TeamId = BotCont.TeamId;
            Object.GetComponent<SpriteRenderer>().sprite = Data.Skin;
            // initiate gameobject's stamina and health container
            Script.InitStats();
            // assigning the health
            Script.health.Max = Data.MaxHealth;
            Script.stamina.Max = Data.MaxStamina;
            Script.ChanceForTwo = Data.ChanceForTwo;
            Script.health.Fill();
            Script.TeamId = TeamId;

            float[] Pos = ReadCoord(LevelCont.SpreadPrefab, BotCont);
            Object.transform.localPosition = new Vector3(Pos[0], Pos[1], -2835f);
            SpawnCounter(Object);
            Script.Skills.AddRange(Data.Abilities);
        }
        public static void SpawnCounter(GameObject Object)
        {
            RectTransform RT = Object.GetComponent<RectTransform>();
            SpriteRenderer SR = Object.GetComponent<SpriteRenderer>();
            GameObject UI = GameObject.Find("UI");
            GameObject StaminaC = SpawnCounterPrefab(InGameContainer.GetInstance().StaminaCounter, Object);
            GameObject HealthC = SpawnCounterPrefab(InGameContainer.GetInstance().HealthCounter, Object);
            GameObject ShieldC = SpawnCounterPrefab(InGameContainer.GetInstance().ShieldCounter, Object);
            GameObject DebuffC = Instantiate(InGameContainer.GetInstance().debuffCounter, Object.transform);
            DebuffC.GetComponent<DebuffCounter>().Creature = Object.GetComponent<BaseCreature>();
            // float heights = RT.rect.height;
            float height = SR.bounds.size.y / 1.5f;//UI.GetComponent<RectTransform>().localScale.y;
            // Debug.Log(""+height+" "+heights);
            float padding = 0;
            StaminaC.transform.localPosition = new Vector2(0, height);
            HealthC.transform.localPosition = new Vector2(0, height * -1 - padding);
            ShieldC.transform.localPosition = new Vector2(-35, height * -1 - padding);
            DebuffC.transform.localPosition = new Vector2(10, (height * -1) - 9 - padding);
            // HealthC.GetComponent<HealthCounter>().Bar.GetComponent<SpriteRenderer>().bounds.size.x * -17
            HealthC.transform.SetParent(UI.transform);
            ShieldC.transform.SetParent(UI.transform);
            ShieldC.transform.position += new Vector3(0, 0, -2);
            DebuffC.transform.SetParent(UI.transform);
            DebuffC.transform.SetAsFirstSibling();
            ShieldC.transform.SetAsFirstSibling();
            HealthC.transform.SetAsFirstSibling();
        }
        public static GameObject SpawnCounterPrefab(GameObject Prefab, GameObject Parent)
        {
            GameObject Obj = Instantiate(Prefab, Parent.transform);
            Obj.GetComponent<BaseCounter>().Target = Parent;
            Obj.transform.SetAsFirstSibling();
            return Obj;
        }
    }
}