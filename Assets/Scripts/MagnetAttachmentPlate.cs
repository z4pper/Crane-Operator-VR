using System;
using UnityEngine;

public class MagnetAttachmentPlate : MonoBehaviour
{
    [SerializeField] private Transform magnetPlate;

    private Hookable hookableSlot;
    private Boolean isMagnetPlateActive;

    private void Update()
    {
        OVRInput.Update();

        if (OVRInput.Get(OVRInput.Button.One))
        {
            isMagnetPlateActive = false;
            Debug.Log("Magnet is: " + isMagnetPlateActive);
            if (!isMagnetPlateActive && hookableSlot != null)
            {
                DetachHookableObject();
            }
        }
        if (OVRInput.Get(OVRInput.Button.Two))
        {
            isMagnetPlateActive = true;
            Debug.Log("Magnet is: " + isMagnetPlateActive);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        var hookable = other.GetComponent<Hookable>();
        if (isMagnetPlateActive && hookable != null)
        {
            AttachHookableObject(hookable);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (hookableSlot != null) return;
        
        var hookable = other.GetComponent<Hookable>();
        if (isMagnetPlateActive && hookable != null)
        {
            AttachHookableObject(hookable);
        }
    }
    
    private void AttachHookableObject(Hookable hookable)
    {
        Debug.Log("Hooked!");
        hookableSlot = hookable;
        var rigidbody = hookable.GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;
        hookable.transform.SetParent(magnetPlate);
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
