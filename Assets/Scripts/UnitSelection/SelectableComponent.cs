using UnitTask;
using UnityEngine;

public class SelectableComponent : MonoBehaviour
{
    public string CustomDisplayName = null;
    public string CustomDisplayType = null;
    public string DisplayName => string.IsNullOrWhiteSpace(CustomDisplayName) ? name : CustomDisplayName;
    public string DisplayType => string.IsNullOrWhiteSpace(CustomDisplayType) ? "Object" : CustomDisplayType;

    public Color SelectColor = Color.green;

    protected SpriteRenderer spriteRenderer;
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
