using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Control.UI;
using DataContainer;

public class CardShop : MonoBehaviour
{
    public GameObject ShopItem;
    public GameObject Background;
    public GameObject Grid;
    public void OpenShop()
    {
        GameObject empty = new GameObject("");
        empty.AddComponent<RectTransform>();
        Loaded.loaded.queuedCardShop.GetQueue().ForEach((item)=>{
            GameObject cont = Instantiate(empty, Grid.transform);
            ShopItem shopitem = Instantiate(ShopItem, cont.transform).GetComponent<ShopItem>();
            shopitem.transform.localPosition = new Vector2();
            shopitem.Init(item);
            shopitem.Card.enableHover = false;
        });
        Destroy(empty);
        Background.SetActive(true);
        Grid.SetActive(true);
    }
    public void CloseShop()
    {
        Background.SetActive(false);
        Grid.SetActive(false);
        foreach (Transform child in Grid.transform)
        {
            Destroy(child.gameObject);
        }
    }
    public void Continue()
    {
        ChangeScene.LoadActMap();
        Loaded.loaded.LastLevelWin = true;
        Loaded.loaded.queuedCardShop.FillQueue();
        SaveFile.Save(Loaded.loaded);
    }
}
