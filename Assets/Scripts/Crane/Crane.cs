using UnityEngine;

public class Crane : MonoBehaviour
{
    [SerializeField] private CraneHook craneHook;
    [SerializeField] private CraneCablePlate craneCablePlate;
    [SerializeField] private Transform upperCranePart;
    [SerializeField] private float rotateSpeed;

    [SerializeField] private JoystickController joystickControllerLeft;
    [SerializeField] private JoystickController joystickControllerRight;
    [SerializeField] private bool useVRControllerInput;
    [SerializeField] private BooleanSO isCraneMotorStarted;

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
        if (!isCraneMotorStarted.Value) return;
        
        RotateCrane(_controllerInputLeft.GetHorizontalInput());
        craneCablePlate.MoveCablePlate(_controllerInputLeft.GetVerticalInput());
        craneHook.MoveHook(_controllerInputRight.GetVerticalInput());
    }
    
    private void RotateCrane(float direction)
    {
        if (direction == 0) return;
        upperCranePart.Rotate(Vector3.up, direction * rotateSpeed * Time.deltaTime);
    }
}