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
    [SerializeField] private float maxAttachHookDistance;
    [SerializeField] private  LayerMask hookLayerMask;
    [SerializeField] private AudioSource craneHookAudioSource;
    [SerializeField] private BooleanSO isCraneMotorStarted;
    
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
        if (!isCraneMotorStarted.Value && craneHookAudioSource.isPlaying)
        {
            craneHookAudioSource.Stop();
        }
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
            var boxCollider = GetComponent<BoxCollider>();
            var pos = boxCollider.center;
            pos.y -= boxCollider.size.y / 2;
            var rayStartingPosition = transform.TransformPoint(pos);
        
            RaycastHit hit;
            Debug.DrawRay(rayStartingPosition, Vector3.down, Color.blue, 999f);
            if (!Physics.Raycast(rayStartingPosition, Vector3.down, out hit, maxAttachHookDistance, hookLayerMask)) return;
        
            var hook = hit.collider.GetComponent<HookBase>();

            if (hook != null)
            {
                hook.AttachToCrane(this);
            }
        }
    }
    
    public void ToggleAttachable()
    {
        if(HookSlot != null) HookSlot.ToggleHook();
    }
    
    public void MoveHook(float direction)
    {
        if (direction == 0)
        {
            craneHookAudioSource.Stop();
            return;
        }
        if (!craneHookAudioSource.isPlaying)
        {
            craneHookAudioSource.Play();
        }
        
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
