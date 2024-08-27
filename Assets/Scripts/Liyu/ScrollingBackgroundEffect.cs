using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingBackgroundEffect : MonoBehaviour
{
    public RawImage starBackground;  // Reference to the first background image
    public RawImage starBackground2; // Reference to the second background image

    public float scrollSpeed = 0.1f; // Speed at which the background scrolls

    private RectTransform starRect;
    private RectTransform starRect2;
    private float imageWidth;

    void Start()
    {
        // Get the RectTransforms of the images
        starRect = starBackground.GetComponent<RectTransform>();
        starRect2 = starBackground2.GetComponent<RectTransform>();

        // Calculate the width of the image in the world space
        imageWidth = starRect.rect.width;
    }

    void Update()
    {
        // Move the backgrounds to the left
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed * imageWidth, imageWidth);

        starRect.anchoredPosition = new Vector2(-newPosition, starRect.anchoredPosition.y);
        starRect2.anchoredPosition = new Vector2(imageWidth - newPosition, starRect2.anchoredPosition.y);
    }
}

