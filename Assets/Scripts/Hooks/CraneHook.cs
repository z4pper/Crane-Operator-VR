using System;
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
    private bool _checkForHook;

    private void Start()
    {
        _distanceInMeterStartingColor = DistanceInMeterText.color;
        _distanceInMeterStartingText = DistanceInMeterText.text;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_checkForHook) return;
        var hookBase = other.GetComponent<HookBase>();

        if (hookBase != null)
        {
            hookBase.AttachToCrane(this);
        }

        _checkForHook = false;    
    }

    public void ToggleCraneHook()
    {
        if (HookSlot != null)
        {
            HookSlot.DetachFromCrane();
            HookSlot = null;

            DistanceInMeterText.color = _distanceInMeterStartingColor;
            DistanceInMeterText.text = _distanceInMeterStartingText;
        }
        else
        {
            _checkForHook = true;
        }
    }
    
    public void ToggleAttachable()
    {
        if(HookSlot != null) HookSlot.ToggleHook();
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
