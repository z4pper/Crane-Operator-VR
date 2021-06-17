using UnityEngine;

public class MagnetPlateHook : HookBase
{
    [SerializeField] private Vector3 magnetPlateHookHookedPosition;
    [SerializeField] private float maxAttachToObjectDistance;
    
    private bool isMagnetPlateActive;

    private void Update()
    {
        if (!isHookActive) return;
        
        OVRInput.Update();

        if (isMagnetPlateActive && hookableSlot == null)
        {
            CheckForHookableObject();
        }
        
        if (OVRInput.Get(OVRInput.Button.One))
        {
            isMagnetPlateActive = false;
            if (!isMagnetPlateActive && hookableSlot != null)
            {
                DetachHookableObject();
            }
        }
        if (OVRInput.Get(OVRInput.Button.Two))
        {
            isMagnetPlateActive = true;
        }
    }

    private void CheckForHookableObject()
    {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position, Vector3.down, out hit, maxAttachToObjectDistance)) return;
        
        var hookable = hit.collider.GetComponent<MagnetPlateHookable>();

        if (hookable != null)
        {
            AttachHookableObject(hookable);    
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        var craneHook = other.GetComponent<CraneHook>();

        if (craneHook != null && craneHook.HookSlot == null)
        {
            if (OVRInput.Get(OVRInput.Button.One))
            {
                transform.SetParent(other.transform);
                transform.localPosition = magnetPlateHookHookedPosition;
                craneHook.HookSlot = this;
                isHookActive = true;
            }
        }
    }
    
    private void AttachHookableObject(HookableBase hookableBase)
    {
        hookableSlot = hookableBase;
        var rigidbody = hookableBase.GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;
        hookableBase.transform.SetParent(this.transform);
    }
    
    private void DetachHookableObject()
    {
        hookableSlot.transform.SetParent(null);
        var rigidbody = hookableSlot.GetComponent<Rigidbody>();
        rigidbody.useGravity = true;
        rigidbody.isKinematic = false;
        hookableSlot = null;
    }
}
