using System;
using UnityEngine;

public abstract class HookBase : MonoBehaviour
{
    [SerializeField] private DistanceMeter distanceMeter;
    [SerializeField] protected Vector3 HookPosition;
    [SerializeField] protected float maxAttachToObjectDistance;
    
    public HookableBase HookableSlot { get; private set; }
    public bool IsHookActive { get; set; }

    public bool IsHookEquipt { get; set; }

    protected abstract void CheckForHookableObject();

    protected virtual void Update()
    {
        if (!IsHookEquipt) return;

        distanceMeter.CalculateDistance();
        
        if (IsHookActive && HookableSlot == null)
        {
            CheckForHookableObject();
        }
        ToggleHook();
        
        OVRInput.Update();
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

    public virtual void DetachHookableObject()
    {
        HookableSlot.transform.SetParent(null);
        var rigidbody = HookableSlot.GetComponent<Rigidbody>();
        rigidbody.useGravity = true;
        rigidbody.isKinematic = false;
        HookableSlot.IsHooked = false;
        HookableSlot = null;
    }
    
    protected void ToggleHook()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            IsHookActive = false;
            if (!IsHookActive && HookableSlot != null)
            {
                DetachHookableObject();
            }
        }

        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            IsHookActive = true;
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        var craneHook = other.GetComponent<CraneHook>();

        if (craneHook != null && craneHook.HookSlot == null)
        {
            if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
            {
                transform.SetParent(other.transform);
                transform.localPosition = HookPosition;
                craneHook.HookSlot = this;
                IsHookEquipt = true;
                distanceMeter.SetupDistanceMeterText(craneHook.DistanceInMeterText);
            }
        }
    }
}
