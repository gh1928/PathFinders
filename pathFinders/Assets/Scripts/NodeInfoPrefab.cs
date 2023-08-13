using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NodeInfoPrefab : MonoBehaviour
{
    public static string infStr = "Inf";

    public RectTransform rect;
    public RectTransform childRect1;
    public RectTransform childRect2;
    public TextMeshProUGUI nodeNum;
    public TextMeshProUGUI dist;

    public void SetData(int number, int dist)
    {
        nodeNum.text = number.ToString();
        SetDist(dist);
    }
    public void SetWidth(int width)
    {
        rect.sizeDelta = new Vector2(width, 100);
        Vector2 childSize = new Vector2(width, 50);
        childRect1.sizeDelta = childSize;
        childRect2.sizeDelta = childSize;
    }
    public void SetDist(int dist)
    {
        if (dist == int.MaxValue)
        {
            this.dist.text = infStr;
            return;
        }

        this.dist.text = dist.ToString();
    }
}
