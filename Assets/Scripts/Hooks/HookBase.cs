using UnityEngine;

public abstract class HookBase : MonoBehaviour
{
    [SerializeField] protected LayerMask hookableLayerMask;
    [SerializeField] private DistanceMeter distanceMeter;
    [SerializeField] protected Vector3 hookPosition;
    [SerializeField] protected float maxAttachToObjectDistance;
    [SerializeField] private float maxHookSwingAngle;

    private HingeJoint _hingeJoint;
    private HookableBase _hookableSlot;
    private bool _isHookEquipped;

    protected abstract void CheckForHookableObject();
    
    protected virtual void Update()
    {
        if (!_isHookEquipped) return;

        distanceMeter.CalculateDistance();
        
        ToggleHook();
    }

    public void DetachFromCrane()
    {
        _isHookEquipped = false;
        Destroy(_hingeJoint);
        transform.SetParent(null);
        
        if (_hookableSlot != null)
        {
            DetachHookableObject();
        }
    }

    private void AttachToCrane(CraneHook craneHook)
    {
        transform.SetParent(craneHook.transform);
        transform.localPosition = hookPosition;
        craneHook.HookSlot = this;
        _isHookEquipped = true;
        distanceMeter.SetupDistanceMeterText(craneHook.DistanceInMeterText);

        _hingeJoint = gameObject.AddComponent<HingeJoint>();
        _hingeJoint.connectedBody = craneHook.GetComponent<Rigidbody>();

        var axis = _hingeJoint.axis;
        axis.z = 1;
        axis.x = 0;
        _hingeJoint.axis = axis;

        _hingeJoint.useLimits = true;
        var limits = _hingeJoint.limits;
        limits.min = -maxHookSwingAngle;
        limits.max = maxHookSwingAngle;
        _hingeJoint.limits = limits;
    }

    protected virtual void AttachHookableObject(HookableBase hookableBase)
    {
        _hookableSlot = hookableBase;
        var rigidbody = hookableBase.GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;
        hookableBase.IsHooked = true;
        hookableBase.transform.SetParent(this.transform);
    }

    protected virtual void DetachHookableObject()
    {
        _hookableSlot.transform.SetParent(null);
        var rigidbody = _hookableSlot.GetComponent<Rigidbody>();
        rigidbody.useGravity = true;
        rigidbody.isKinematic = false;
        _hookableSlot.IsHooked = false;
        _hookableSlot = null;
    }
    
    private void ToggleHook()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            if (_hookableSlot != null)
            {
                DetachHookableObject();
            }

            else
            {
                CheckForHookableObject();
            }
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        var craneHook = other.GetComponent<CraneHook>();

        if (craneHook != null && craneHook.HookSlot == null)
        {
            if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            {
                AttachToCrane(craneHook);
            }
        }
    }
}
