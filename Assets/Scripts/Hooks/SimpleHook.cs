using UnityEngine;

public class SimpleHook : HookBase
{
    protected override void CheckForHookableObject()
    {
        var boxCollider = GetComponent<BoxCollider>();
        var pos = boxCollider.center;
        pos.y -= boxCollider.size.y / 2;
        var rayStartingPosition = transform.TransformPoint(pos);
        
        RaycastHit hit;
        if (!Physics.Raycast(rayStartingPosition, Vector3.down, out hit, maxAttachToObjectDistance, hookableLayerMask)) return;
        
        var hookable = hit.collider.GetComponent<SimpleHookable>();

        if (hookable != null)
        {
            AttachHookableObject(hookable);    
        }
    }
}
