using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attributes.Abilities;

namespace DataContainer
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "new Level")]
    public class LevelDataContainer : ScriptableObject
    {
        public string LevelId;
        public GameObject CreaturePrefab;
        public GameObject SpreadPrefab;
        public BotDataContainer[] Creature;
        public int GetCreatureCount(int TeamId)
        {
            int Count = 0;
            foreach (BotDataContainer Cont in Creature)
            {
                if (Cont.TeamId == TeamId)
                {
                    Count++;
                }
            }
            return Count;
        }
    }
    /// <summary>holds all creature info for a level</summary>
    [Serializable]
    public class BotDataContainer
    {
        public CreatureDataContainer CreatureAsset;
        public int TeamId;
        public int SpreadPrefabPlace;

    }
}

