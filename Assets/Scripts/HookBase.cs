using UnityEngine;

public abstract class HookBase : MonoBehaviour
{
    [SerializeField] protected LayerMask hookableLayerMask;
    [SerializeField] private DistanceMeter distanceMeter;
    [SerializeField] protected Vector3 hookPosition;
    [SerializeField] protected float maxAttachToObjectDistance;
    [SerializeField] private float maxHookSwingAngle;

    private HingeJoint _hingeJoint;
    public HookableBase HookableSlot { get; private set; }

    public bool IsHookEquipt { get; set; }

    protected abstract void CheckForHookableObject();
    
    protected virtual void Update()
    {
        if (!IsHookEquipt) return;

        distanceMeter.CalculateDistance();
        
        ToggleHook();
    }

    public void DetachFromCrane()
    {
        IsHookEquipt = false;
        Destroy(_hingeJoint);
        transform.SetParent(null);
        
        if (HookableSlot != null)
        {
            DetachHookableObject();
        }
    }

    private void AttachToCrane(CraneHook craneHook)
    {
        transform.SetParent(craneHook.transform);
        transform.localPosition = hookPosition;
        craneHook.HookSlot = this;
        IsHookEquipt = true;
        distanceMeter.SetupDistanceMeterText(craneHook.DistanceInMeterText);

        _hingeJoint = gameObject.AddComponent<HingeJoint>();
        _hingeJoint.connectedBody = craneHook.GetComponent<Rigidbody>();

        var axis = _hingeJoint.axis;
        axis.z = 1;
        _hingeJoint.axis = axis;

        _hingeJoint.useLimits = true;
        var limits = _hingeJoint.limits;
        limits.min = -maxHookSwingAngle;
        limits.max = maxHookSwingAngle;
        _hingeJoint.limits = limits;
    }

    protected virtual void AttachHookableObject(HookableBase hookableBase)
    {
        HookableSlot = hookableBase;
        var rigidbody = hookableBase.GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;
        hookableBase.IsHooked = true;
        hookableBase.transform.SetParent(this.transform);
    }

    protected virtual void DetachHookableObject()
    {
        HookableSlot.transform.SetParent(null);
        var rigidbody = HookableSlot.GetComponent<Rigidbody>();
        rigidbody.useGravity = true;
        rigidbody.isKinematic = false;
        HookableSlot.IsHooked = false;
        HookableSlot = null;
    }
    
    private void ToggleHook()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            Debug.Log("Pressed!");
            if (HookableSlot != null)
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
