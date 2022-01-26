using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
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
        // Debug.Log(""+position.x+"-"+this.backgroundRT.rect.width);
        // Debug.Log(""+(position.x*2 + this.backgroundRT.rect.width*2) +"-"+GameObject.Find("UI").GetComponent<RectTransform>().rect.width);
        SetText(tooltiptext);
        if (position.x * 2 + this.backgroundRT.rect.width * 2 > GameObject.Find("UI").GetComponent<RectTransform>().rect.width)
        {
            position.x = GameObject.Find("UI").GetComponent<RectTransform>().rect.width / 2 - backgroundRT.rect.width;
        }
        this.transform.position = position;
        this.background.gameObject.GetComponent<Image>().DOFade(1f, 0.1f);
        this.text.DOFade(1f, 0.1f);
    }
    public void Hide()
    {
        this.background.gameObject.GetComponent<Image>().DOFade(0f,0.1f);
        this.text.DOFade(0f, 0.1f);
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
