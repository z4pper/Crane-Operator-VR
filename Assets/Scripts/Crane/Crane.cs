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
    [SerializeField] private JoystickController joystickControllerLeft;
    [SerializeField] private JoystickController joystickControllerRight;
    [SerializeField] private bool useVRControllerInput;

    private ICraneInput _controllerInputLeft;
    private ICraneInput _controllerInputRight;

    private void Awake()
    {
        if (useVRControllerInput)
        {
            _controllerInputLeft = new VRControllerCraneInput(OVRInput.RawAxis2D.LThumbstick);
            _controllerInputRight = new VRControllerCraneInput(OVRInput.RawAxis2D.RThumbstick);
        }
        else
        {
            _controllerInputLeft = new JoystickCraneInput(joystickControllerLeft);
            _controllerInputRight = new JoystickCraneInput(joystickControllerRight);
        }
    }

    private void Update()
    {
        RotateCrane(_controllerInputLeft.GetHorizontalInput());
        MoveCablePlate(_controllerInputLeft.GetVerticalInput());
        MoveHook(_controllerInputRight.GetVerticalInput());
    }
    
    private void RotateCrane(float direction)
    {
        if (direction == 0) return;
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
        
        var pos = cablePlateRB.localPosition;
        pos.y -= direction * hookMoveSpeed * Time.deltaTime;
        
        if (pos.y > maxHookPosY) pos.y = maxHookPosY;
        if (pos.y < minHookPosY) pos.y = minHookPosY;
        
        cablePlateRB.localPosition = pos;

        var positionInPercentage = (pos.y - minHookPosY) / (maxHookPosY - minHookPosY);
        var cableSwingAngleLimit = Mathf.SmoothStep(maxCableSwingAngle, minCableSwingAngle, positionInPercentage);
        if (cableSwingAngleLimit < 0) cableSwingAngleLimit = 0;

        var cableLimits = cableHJ.limits;
        cableLimits.max = cableSwingAngleLimit;
        cableLimits.min = -cableSwingAngleLimit;
        cableHJ.limits = cableLimits;
    }
}