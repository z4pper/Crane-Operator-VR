using UnityEngine;

public class CraneHook : MonoBehaviour
{
    public HookBase HookSlot { get; set;}

    private void Update()
    {
        if (HookSlot != null && OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
        {
            HookSlot.IsHookActive = false;
            HookSlot.IsHookEquipt = false;
            HookSlot.transform.SetParent(null);
            if (HookSlot.HookableSlot != null)
            {
                HookSlot.DetachHookableObject();
            }
            HookSlot = null;
        }
    }
}
