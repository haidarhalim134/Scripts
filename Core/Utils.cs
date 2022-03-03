using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using UnityEngine.UI;
using Attributes.Abilities;
using Control.Core;
using Control.Combat;
using static UnityEngine.Random;
using DG.Tweening;
using DataContainer; 
using Control.Deck;
using UnityEditor;

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
    public static CardHandler SpawnCard(AbilityContainer ability, GameObject owner, Vector2 spawnPlace, GameObject prefab, Transform parent)
    {
        GameObject Card = GameObject.Instantiate(prefab, spawnPlace, new Quaternion(), parent);
        CardHandler Script = Card.GetComponent<CardHandler>();
        Script.SmallToBig();
        Script.Ability = ability;
        Script.UpdateText();
        Script.TargetOwner = owner.gameObject;
        Script.InitOwner();
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
        if (!Application.isPlaying) return;
        string base64Guid = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        Selection.activeGameObject.GetComponent<AbilityManager>().AbName = base64Guid;
    }
    [MenuItem("Tools/cheat/Give 1 stamina")]
    static void menuGiveStamina()
    {
        if (!Application.isPlaying) return;
        InGameContainer.GetInstance().currPlayer.stamina.Update(1);
    }
    [MenuItem("Tools/cheat/Finish game")]
    static void menuEndGame()
    {
        if (!Application.isPlaying) return;
        CombatEngine.EndGame(true);
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
