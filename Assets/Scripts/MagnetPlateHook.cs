using UnityEngine;

public class MagnetPlateHook : HookBase
{
    protected override void CheckForHookableObject()
    {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position, Vector3.down, out hit, maxAttachToObjectDistance)) return;
        
        var hookable = hit.collider.GetComponent<MagnetPlateHookable>();

        if (hookable != null)
        {
            AttachHookableObject(hookable);    
        }
    }
}
