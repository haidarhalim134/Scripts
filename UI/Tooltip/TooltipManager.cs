using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    RectTransform rectTransform;
    public GameObject background;
    RectTransform backgroundRT;
    public TextMeshProUGUI text;
    void Awake()
    {
        backgroundRT = background.GetComponent<RectTransform>();
        rectTransform = transform.GetComponent<RectTransform>();
        SetText("test");
    }
    void SetText(string text)
    {
        this.text.SetText(text);
        this.text.ForceMeshUpdate();
        Vector2 textsize = this.text.GetRenderedValues(false);
        Vector2 padding = new Vector2(5,5);

        backgroundRT.sizeDelta = textsize + padding;
    }
    public void SpawnTooltip(string tooltiptext, Vector2 position)
    {
        // rectTransform.anchoredPosition = position; 
        this.transform.position = position;
        SetText(tooltiptext);
        this.gameObject.SetActive(true);
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
    public static TooltipManager GetInstance()
    {
        return GameObject.Find("TooltipSpace").GetComponent<TooltipManager>();
    }
    // void Update()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         Debug.Log(this.transform.position);
    //         SpawnTooltip(Input.mousePosition.ToString(),Input.mousePosition*GameObject.Find("UI").GetComponent<RectTransform>().localScale.x);
    //         // / GameObject.Find("UI").GetComponent<RectTransform>().localScale.x
    //     }
    // }
}
