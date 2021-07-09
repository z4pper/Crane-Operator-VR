using UnityEngine;

public class Crane : MonoBehaviour
{
    [SerializeField] private Transform upperCranePart;
    [SerializeField] private Transform cablePlate;
    [SerializeField] private Transform cablePlateRB;
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

    [SerializeField] private HingeJoint cableHJ;

    [SerializeField] private JoystickController leftJoystick;
    [SerializeField] private JoystickController rightJoystick;

    private void Update()
    {
        RotateCrane(leftJoystick.GetHorizontalInput());
        MoveCablePlate(leftJoystick.GetVerticalInput());
        MoveHook(rightJoystick.GetVerticalInput());
    }
    
    private void RotateCrane(float direction)
    {
        if (direction == 0) return;
        // upperCranePart.RotateAround(upperCranePart.transform.position, Vector3.up,
        //     speed * rotateSpeed * Time.deltaTime);
        upperCranePart.Rotate(Vector3.up, direction * rotateSpeed * Time.deltaTime);
    }

    private void MoveCablePlate(float direction)
    {
        if (direction == 0) return;

        var pos = cablePlate.localPosition;
        pos.z -= direction * cablePlateMoveSpeed * Time.deltaTime;

        if (pos.z > maxCablePlatePosZ) pos.z = maxCablePlatePosZ;
        if (pos.z < minCablePlatePosZ) pos.z = minCablePlatePosZ;

        cablePlate.localPosition = pos;
    }

    private void MoveHook(float direction)
    {
        if (direction == 0) return;

        var posDelta = direction * hookMoveSpeed * Time.deltaTime;
        
        var pos = cablePlateRB.localPosition;
        pos.y -= posDelta;
        
        if (pos.y > maxHookPosY)
        {
            posDelta -= (pos.y - maxHookPosY);
            pos.y = maxHookPosY;
        }

        if (pos.y < minHookPosY)
        {
            posDelta -= (minHookPosY - pos.y);
            pos.y = minHookPosY;
        }

        cablePlateRB.localPosition = pos;

        var positionInPercentage = (pos.y - minHookPosY) / (maxHookPosY - minHookPosY);
        //var cableSwingAngleLimit = maxCableSwingAngle - positionInPercentage * (maxCableSwingAngle - minCableSwingAngle);
        var cableSwingAngleLimit = Mathf.SmoothStep(maxCableSwingAngle, minCableSwingAngle, positionInPercentage);
        if (cableSwingAngleLimit < 0) cableSwingAngleLimit = 0;

        var cableLimits = cableHJ.limits;
        cableLimits.max = cableSwingAngleLimit;
        cableLimits.min = -cableSwingAngleLimit;
        cableHJ.limits = cableLimits;
        
        // cableHJ.autoConfigureConnectedAnchor = false;
        
        // var connectedAnchor = cableHJ.connectedAnchor;
        // connectedAnchor.y -= posDelta;
        // cableHJ.connectedAnchor = connectedAnchor;
        //
        // var anchor = cableHJ.anchor;
        // anchor.y -= posDelta;
        // cableHJ.anchor = anchor;
        
        // cableHJ.autoConfigureConnectedAnchor = true;
    }
}