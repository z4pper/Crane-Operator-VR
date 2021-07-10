using TMPro;
using UnityEngine;

public class CraneHook : MonoBehaviour
{
    [field: SerializeField] public TextMeshProUGUI DistanceInMeterText { get; private set; }
    public HookBase HookSlot { get; set;}

    private Color _distanceInMeterStartingColor;
    private string _distanceInMeterStartingText;

    private void Start()
    {
        _distanceInMeterStartingColor = DistanceInMeterText.color;
        _distanceInMeterStartingText = DistanceInMeterText.text;
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && HookSlot != null)
        {
            HookSlot.DetachFromCrane();
            HookSlot = null;

            DistanceInMeterText.color = _distanceInMeterStartingColor;
            DistanceInMeterText.text = _distanceInMeterStartingText;
        }
    }
}
