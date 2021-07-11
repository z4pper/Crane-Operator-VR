using UnityEngine;

public class HookableBase : MonoBehaviour
{
    [field: SerializeField] public AudioSource AudioSource { get; private set; }
    public bool IsHooked { get; set; }

    private const float OutlineWidth = 5f;
    private const Outline.Mode OutlineMode = Outline.Mode.OutlineAll;

    public void MarkOutline(Color color)
    {
        var outline = GetComponent<Outline>();
        if (outline == null)
        {
            outline = this.gameObject.AddComponent<Outline>();
            outline.OutlineMode = OutlineMode;
            outline.OutlineColor = color;
            outline.OutlineWidth = OutlineWidth;
        }
        else
        {
            outline.OutlineColor = color;
            outline.enabled = true;
        }
    }

    public void UnmarkOutline()
    {
        var outline = GetComponent<Outline>();
        if (outline == null) return;

        outline.enabled = false;
    }
}