using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Attributes.Player;
using Attributes.Abilities;
using Tree = Map.Tree;
using DataContainer;

namespace Control.Core
{
    public class Loaded
    {
        public static SaveFile loaded = new SaveFile();
        public static CharacterDataCont LoadedCharacter;
    };
    [Serializable]
    public class SaveFile
    {
        public PlayerDataContainer Player;
        public Character CharacterId;
        public int Gold = 500;
        public bool LastLevelWin;
        public QueuedLevel QueuedLevel = new QueuedLevel();
        public Act currAct = Act.Act1;
        // TODO: add every new arc to the initialization
        public Dictionary<Act,ActCont> act = new Dictionary<Act, ActCont>(){
            {Act.Act1,new ActCont()},{Act.Act2,new ActCont()}
        };
        public List<ActSaveWrapper> actsave = new List<ActSaveWrapper>(){
            new ActSaveWrapper(){act=Act.Act1,cont = new ActCont()},
            new ActSaveWrapper(){act=Act.Act2,cont = new ActCont()}};
        public static void Save(SaveFile save)
        {
            foreach(Act act in save.act.Keys)
            {
                foreach (ActSaveWrapper wrap in save.actsave)
                {
                    if (wrap.act == act)
                    {
                        wrap.cont = save.act[act];
                    }
                }
            }
            FileProcessor.WriteToXmlFile(Application.persistentDataPath + "/save.json", save);
        }
        public static SaveFile Load()
        {
            SaveFile file = FileProcessor.ReadFromXmlFile<SaveFile>(Application.persistentDataPath + "/save.json");
            foreach (ActSaveWrapper wrap in file.actsave)
            {
                file.act[wrap.act] = wrap.cont;
                if (wrap.cont.tree.Count == 0)
                {
                    wrap.cont.tree = null;
                }
            }
            return file;
        }
        public static bool CheckIfExist()
        {
            return File.Exists(Application.persistentDataPath+"/save.json");
        }
        public void InitCharacter(CharacterDataCont cont)
        {
            LoadCharacter(cont);
            this.CharacterId = cont.Name;
            this.Player = new PlayerDataContainer();
            this.Player.MaxHealth = cont.StartingHealth;
            this.Player.MaxStamina = cont.MaxStamina;
            this.Player.FullDeck = new List<AbilityContainer>(cont.StartingAbilitiy.Select((cont)=>cont.ToNormalContainer()));
            this.Player.initialCardNumber = cont.initialCardNumber;
            this.Player.DeckShuffle();
        }
        public static void LoadCharacter(CharacterDataCont cont)
        {
            Loaded.LoadedCharacter = cont;
        }
    }
    public enum Act{ Act1, Act2 }
    [Serializable]
    public class ActCont
    {
        public List<Tree> tree = null;
    }
    [Serializable]
    public class ActSaveWrapper
    {
        public Act act;
        public ActCont cont;
    }
    [Serializable]
    public class QueuedLevel
    {
        public List<TypeLevelIdCont> Queue = new List<TypeLevelIdCont>();
        public NodeType[] types = {NodeType.Enemy};
        public string GetQueued(NodeType type)
        {
            TypeLevelIdCont cont = Queue.Find(cont => cont.Type == type);
            string result = cont.Levelid;
            cont.Levelid = ActDataLoader.loadedActData
            .GetRandomLevel(cont.Type).LevelId;
            return result;
        }
        public void FillQueue()
        {
            foreach (NodeType type in this.types)
            {
                TypeLevelIdCont cont = Queue.Find(cont => cont.Type == type);
                if (cont == null)
                {
                   string level = ActDataLoader.loadedActData
                    .GetRandomLevel(type).LevelId;
                    Queue.Add(new TypeLevelIdCont(){Type=type,Levelid=level});
                } 
            }
        }
        public void ClearQueue(NodeType type)
        {
            var clear = Queue.FindAll((cont)=>cont.Type == type);
            clear.ForEach((cont)=>Queue.Remove(cont));
        }
    }
    [Serializable]
    public class TypeLevelIdCont
    {
        public NodeType Type;
        public string Levelid;
    }
}