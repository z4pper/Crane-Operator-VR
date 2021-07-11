using TMPro;
using UnityEngine;

public class CraneHook : MonoBehaviour
{
    [field: SerializeField] public TextMeshProUGUI DistanceInMeterText { get; private set; }
    [SerializeField] private Transform cablePlateRB;
    [SerializeField] private HingeJoint cableHJ;
    [SerializeField] private float maxHookPosY;
    [SerializeField] private float minHookPosY;
    [SerializeField] private float minCableSwingAngle;
    [SerializeField] private float maxCableSwingAngle;
    [SerializeField] private float hookMoveSpeed;
    
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
    
    public void MoveHook(float direction)
    {
        if (direction == 0) return;
        
        var pos = cablePlateRB.localPosition;
        pos.y -= direction * hookMoveSpeed * Time.deltaTime;
        
        if (pos.y > maxHookPosY) pos.y = maxHookPosY;
        if (pos.y < minHookPosY) pos.y = minHookPosY;
        
        cablePlateRB.localPosition = pos;

        var positionInPercentage = (pos.y - minHookPosY) / (maxHookPosY - minHookPosY);
        var cableSwingAngleLimit = Mathf.SmoothStep(maxCableSwingAngle, minCableSwingAngle, positionInPercentage);
        if (cableSwingAngleLimit < 0) cableSwingAngleLimit = 0;

        var cableLimits = cableHJ.limits;
        cableLimits.max = cableSwingAngleLimit;
        cableLimits.min = -cableSwingAngleLimit;
        cableHJ.limits = cableLimits;
    }
}
