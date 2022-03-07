using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using static UnityEngine.Random;
using DataContainer;
using Control.Core;
using Control.Combat;
using Attributes.Abilities;

namespace LevelManager
{
    public class LevelDataLoader : MonoBehaviour
    {
        public static void SpawnPlayer(GameObject prefab)
        {
            GameObject obj = Instantiate(prefab, GameObject.Find("Team0").transform);
            InGameContainer.GetInstance().currPlayer = obj.transform.Find("Player").GetComponent<PlayerController>();
        }
        /// <summary>default spreader : random</summary>
        public static void LoadLevel(LevelDataContainer Level)
        {
            // TODO: create a simpler version of this, use prefab with object named 1 through n, then replace each number with actual creature
            foreach (BotDataContainer Cont in Level.Creature)
            {
                GameObject parent = new GameObject("cont");
                parent.transform.SetParent(GameObject.Find("Team" + Cont.TeamId).transform);
                GameObject Creature = Instantiate(Level.CreaturePrefab, parent.transform) as GameObject;
                BotController ctrl = Creature.AddComponent<BotController>();
                CombatEngine.powerToApply.Add(new PowerQueue(Cont.CreatureAsset.powers, ctrl));
                InitCreature(Creature, Cont.CreatureAsset, Level, Cont);
            }
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
            Script.stamina.Fill();
            Script.TeamId = TeamId;

            float[] Pos = ReadCoord(LevelCont.SpreadPrefab, BotCont);
            Object.transform.parent.transform.localPosition = new Vector3(Pos[0], Pos[1], 0);
            SpawnCounter(Object);
            Script.Skills.AddRange(Data.attackPattern);
            Script.initAbilLoop();
        }
        public static void SpawnCounter(GameObject Object)
        {
            GameObject parent = new GameObject("counter cont");
            parent.transform.SetParent(Object.transform.parent.transform);
            parent.transform.localPosition = new Vector2();
            Canvas canvas = parent.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 1;

            RectTransform RT = Object.GetComponent<RectTransform>();
            SpriteRenderer SR = Object.GetComponent<SpriteRenderer>();
            GameObject UI = GameObject.Find("UI");
            GameObject StaminaC = SpawnCounterPrefab(InGameContainer.GetInstance().StaminaCounter, Object);
            StaminaC.SetActive(false);
            GameObject HealthC = SpawnCounterPrefab(InGameContainer.GetInstance().HealthCounter, Object);
            GameObject ShieldC = SpawnCounterPrefab(InGameContainer.GetInstance().ShieldCounter, Object);
            GameObject DebuffC = Instantiate(InGameContainer.GetInstance().debuffCounter, Object.transform);
            GameObject IntentC = Instantiate(InGameContainer.GetInstance().intentCounter, Object.transform);
            IntentC.GetComponent<IntentsCounter>().owner = Object.GetComponent<BotController>();
            Object.GetComponent<BaseCreature>().debuffCounter = DebuffC.GetComponent<DebuffCounter>();
            Object.GetComponent<BotController>().intentCounter = IntentC.GetComponent<IntentsCounter>();
            // float heights = RT.rect.height;
            float height = SR.bounds.size.y / 1.5f;//UI.GetComponent<RectTransform>().localScale.y;
            // Debug.Log(""+height+" "+heights);
            float padding = 0;
            StaminaC.transform.localPosition = new Vector2(0, height);
            HealthC.transform.localPosition = new Vector2(0, height * -1 - padding);
            ShieldC.transform.localPosition = new Vector2(-35, height * -1 - padding);
            DebuffC.transform.localPosition = new Vector2(10, (height * -1) - 9 - padding);
            IntentC.transform.localPosition = new Vector2(0, height+padding+20);
            // HealthC.GetComponent<HealthCounter>().Bar.GetComponent<SpriteRenderer>().bounds.size.x * -17
            HealthC.transform.SetParent(parent.transform);
            ShieldC.transform.SetParent(parent.transform);
            IntentC.transform.SetParent(parent.transform);
            ShieldC.transform.position += new Vector3(0, 0, -2);
            DebuffC.transform.SetParent(UI.transform);
            DebuffC.transform.SetAsFirstSibling();
            ShieldC.transform.SetAsFirstSibling();
            HealthC.transform.SetAsFirstSibling();
            IntentC.transform.SetAsFirstSibling();
        }
        public static GameObject SpawnCounterPrefab(GameObject Prefab, GameObject Parent)
        {
            GameObject Obj = Instantiate(Prefab, Parent.transform);
            Obj.GetComponent<BaseCounter>().Target = Parent;
            Obj.transform.SetAsFirstSibling();
            return Obj;
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
    }
}