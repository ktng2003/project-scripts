using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetting : MonoBehaviour
{
    public RectTransform canvasRect;
    public RectTransform top;
    public RectTransform bottom;
    public RectTransform left;
    public RectTransform right;

    void Start()
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;

        float scaleheight = ((float)Screen.width / Screen.height) / ((float)9 / 16);
        float scalewidth = 1f / scaleheight;

        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }

        camera.rect = rect;

        UpdatePanels();
    }

    void UpdatePanels()
    {
        float targetAspect = 9f / 16f;
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;
        float screenAspect = canvasWidth / canvasHeight;

        if (screenAspect > targetAspect)
        {
            float barWidth = (canvasWidth - canvasHeight * targetAspect) / 2f;

            left.sizeDelta = new Vector2(barWidth, canvasHeight);
            right.sizeDelta = new Vector2(barWidth, canvasHeight);
            left.anchoredPosition = new Vector2(-canvasWidth / 2 + barWidth / 2, 0);
            right.anchoredPosition = new Vector2(canvasWidth / 2 - barWidth / 2, 0);
            top.sizeDelta = Vector2.zero;
            bottom.sizeDelta = Vector2.zero;
        }
        else
        {
            float barHeight = (canvasHeight - canvasWidth / targetAspect) / 2f;

            top.sizeDelta = new Vector2(canvasWidth, barHeight);
            bottom.sizeDelta = new Vector2(canvasWidth, barHeight);
            top.anchoredPosition = new Vector2(0, canvasHeight / 2 - barHeight / 2);
            bottom.anchoredPosition = new Vector2(0, -canvasHeight / 2 + barHeight / 2);
            left.sizeDelta = Vector2.zero;
            right.sizeDelta = Vector2.zero;
        }
    }
}