using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using Control.UI;
using Attributes.Abilities;
using Control.Core;
using Control.Combat;
using static UnityEngine.Random;
using DG.Tweening;
using DataContainer; 
using Control.Deck;
using UnityEditor;
using Map;
using Random = System.Random;

public class Utils
{
    public static List<RaycastResult> RaycastMouse()
    {

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            pointerId = -1,
        };

        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        return results;
    }
    public static T FindRayCastContaining<T>(List<RaycastResult> result)
    {
        if (result.Count==0)return default(T);
        var res = result.Find((obj)=>obj.gameObject.GetComponent<T>()!=null).gameObject;
        if (res == null)return default(T);
        return res.gameObject.GetComponent<T>();
    }
    public static T SpawnCard<T>(AbilityContainer ability, Vector2 spawnPlace, GameObject prefab, Transform parent) where T : CardHandlerVisual
    {
        GameObject Card = GameObject.Instantiate(prefab, spawnPlace, new Quaternion(), parent);
        T Script = Card.GetComponent<T>();
        Script.Ability = ability;
        Script.UpdateText();
        return Script;
    }
    [MenuItem("CONTEXT/AbilityManager/Get Description")]
    static void menuDesc()
    {
        AbilityManager mng = InGameContainer.GetInstance().SpawnAbilityPrefab(Selection.activeGameObject);
        Debug.Log(mng.GetDesc(new AbilityData()));
        GameObject.DestroyImmediate(mng.gameObject);
    }
    [MenuItem("CONTEXT/AbilityManager/Give to Hand")]
    static void menuGiveHand()
    {
        if (!Application.isPlaying) return;
        AbilityManager mng = InGameContainer.GetInstance().SpawnAbilityPrefab(Selection.activeGameObject);
        InGameContainer.GetInstance().currPlayer.Deck.AddCard(new AbilityContainer(){name = mng.AbName});
    }
    [MenuItem("CONTEXT/AbilityManager/fill name with random GUID")]
    static void menuGiveName()
    {
        string base64Guid = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        Selection.activeGameObject.GetComponent<AbilityManager>().AbName = base64Guid;
    }
    [MenuItem("Tools/combat cheat/Give 1 stamina")]
    static void menuGiveStamina()
    {
        if (!Application.isPlaying) return;
        InGameContainer.GetInstance().currPlayer.stamina.Update(1);
    }
    [MenuItem("Tools/combat cheat/Finish game")]
    static void menuEndGame()
    {
        if (!Application.isPlaying) return;
        CombatEngine.EndGame(true);
    }
    [MenuItem("Tools/map cheat/Proceed node")]
    static void menuPorceedNode()
    {
        if (!Application.isPlaying) return;
        MapHandler map = GameObject.FindObjectOfType<MapHandler>();
        map.CurrentPlayerPos.ProceedNode();
    }
    [MenuItem("Tools/map cheat/Spawn map")]
    static void menuSpawnMap()
    {
        if (!Application.isPlaying) return;
        MapHandler map = GameObject.FindObjectOfType<MapHandler>();
        map.Spawn(testTree:true);
    }
}
public class LoopingIndex
{
    int index = 0;
    int maxIndex;
    public LoopingIndex(int count)
    {
        maxIndex = count;
    }
    public int Next()
    {
        int res = index;
        index = index + 1 >= maxIndex ? 0 : index + 1;
        return res;
    }
}
public static class file_extension
{
    public static List<T> Shuffle<T>(this List<T> list, bool mutate = false)
    {
        Random rng = new Random();
        int n = list.Count;
        var newl = mutate? list:new List<T>(list);
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = newl[k];
            newl[k] = newl[n];
            newl[n] = value;
        }
        return newl;
    }
}
