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
        _distanceInMeterStartingText = DistanceInMeterText.text;
    }

    private void Update()
    {
        if (HookSlot != null && OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
        {
            HookSlot.IsHookActive = false;
            HookSlot.IsHookEquipt = false;
            HookSlot.transform.SetParent(null);
            if (HookSlot.HookableSlot != null)
            {
                HookSlot.DetachHookableObject();
            }
            HookSlot = null;

            DistanceInMeterText.color = _distanceInMeterStartingColor;
            DistanceInMeterText.text = _distanceInMeterStartingText;
        }
    }
}
