using UnityEngine;

public class Crane : MonoBehaviour
{
    [SerializeField] private Transform upperCranePart;
    [SerializeField] private Transform cablePlate;
    [SerializeField] private Transform hook;
    [SerializeField] private Transform cable;

    [SerializeField] private float maxCablePlatePosZ;
    [SerializeField] private float minCablePlatePosZ;
    [SerializeField] private float maxHookPosY;
    [SerializeField] private float minHookPosY;

    [SerializeField] private float cablePlateMoveSpeed;
    [SerializeField] private float hookMoveSpeed;
    [SerializeField] private float rotateSpeed;

    private void Update()
    {
        OVRInput.Update();
        var primaryThumbX = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x;
        var primaryThumbY = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y;
        var secondaryThumbY = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y;
        
        if (primaryThumbX != 0f)
        {
            RotateCrane(primaryThumbX);
        }

        if (primaryThumbY != 0f)
        {
            MoveCablePlate(primaryThumbY);
        }

        if (secondaryThumbY != 0f)
        {
            MoveHook(secondaryThumbY);
        }
    }


    private void RotateCrane(float speed)
    {
        upperCranePart.RotateAround(upperCranePart.transform.position, Vector3.up,
            speed * rotateSpeed * Time.deltaTime);
    }

    private void MoveCablePlate(float direction)
    {
        var pos = cablePlate.localPosition;
        pos.z -= direction * cablePlateMoveSpeed * Time.deltaTime;

        if (pos.z > maxCablePlatePosZ) pos.z = maxCablePlatePosZ;
        if (pos.z < minCablePlatePosZ) pos.z = minCablePlatePosZ;

        cablePlate.localPosition = pos;
    }

    private void MoveHook(float direction)
    {
        var pos = hook.localPosition;
        pos.y -= direction * hookMoveSpeed * Time.deltaTime;

        if (pos.y > maxHookPosY) pos.y = maxHookPosY;
        if (pos.y < minHookPosY) pos.y = minHookPosY;

        hook.localPosition = pos;

        var scale = 1 - pos.y / maxHookPosY;

        var cableScale = cable.localScale;
        cableScale.y = scale;
        cable.localScale = cableScale;
    }
}