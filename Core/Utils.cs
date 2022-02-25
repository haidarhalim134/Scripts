using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;  

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
