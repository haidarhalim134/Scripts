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
        T res = result.Find((obj)=>obj.gameObject.GetComponent<T>()!=null).gameObject.GetComponent<T>();
        return res;
    }
}
