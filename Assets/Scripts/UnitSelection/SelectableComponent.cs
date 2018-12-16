﻿using UnityEngine;

public class SelectableComponent : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    public Color SelectColor = Color.green;
    protected Color OriginalColor;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        OriginalColor = spriteRenderer.color;
    }

    public void Select()
    {
        spriteRenderer.color = SelectColor;
        spriteRenderer.sortingLayerName = "Selected";
    }

    public void Deselect()
    {
        spriteRenderer.color = OriginalColor;
        spriteRenderer.sortingLayerName = "Deselected";
    }
}
