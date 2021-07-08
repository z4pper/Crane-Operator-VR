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
    [SerializeField] private float minCableSwingAngle;
    [SerializeField] private float maxCableSwingAngle;

    [SerializeField] private float cablePlateMoveSpeed;
    [SerializeField] private float hookMoveSpeed;
    [SerializeField] private float rotateSpeed;

    [SerializeField] private JoystickController leftJoystick;
    [SerializeField] private JoystickController rightJoystick;

    private void Update()
    {
        RotateCrane(leftJoystick.GetHorizontalInput());
        MoveCablePlate(leftJoystick.GetVerticalInput());
        MoveHook(rightJoystick.GetVerticalInput());
    }
    
    private void RotateCrane(float speed)
    {
        // upperCranePart.RotateAround(upperCranePart.transform.position, Vector3.up,
        //     speed * rotateSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up, speed * rotateSpeed * Time.deltaTime);
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
        //
        // var scale = 1 - (pos.y - minHookPosY) / (maxHookPosY - minHookPosY);
        //
        // var cableScale = cable.localScale;
        // cableScale.y = scale;
        // cable.localScale = cableScale;
    }
}