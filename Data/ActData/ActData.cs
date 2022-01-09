using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using DataContainer;

namespace Control.Core
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "new Act")]
    public class ActData : ScriptableObject
    {
        public int Length;
        public ActsLevelCont[] LevelList;
        public LevelDataContainer GetRandomLevel(NodeType type)
        {
            Random rnd = new Random();
            LevelDataContainer[] levelcont = {};
            levelcont = this.GetCont(type);
            return levelcont[rnd.Next(0,levelcont.Length)];
        }
        public LevelDataContainer GetLevel(NodeType type, string id)
        {
            foreach (LevelDataContainer cont in this.GetCont(type))
            {
                if (cont.LevelId == id)
                {
                    return cont;
                }
            }
            Debug.Log("level not found");
            return null;
        }
        LevelDataContainer[] GetCont(NodeType type)
        {
            foreach (ActsLevelCont cont in this.LevelList)
            {
                if (cont.Type == type)
                {
                    return cont.Level;
                }
            }
            Debug.Log("ActCont not found");
            return null;
        }
    }
    public enum NodeType{Enemy,Home,Event}
    [Serializable]
    public class ActsLevelCont
    {
        public NodeType Type;
        public LevelDataContainer[] Level;
    }
}