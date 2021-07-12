using UnityEngine;

public class JoystickCraneInput : ICraneInput
{
    private readonly JoystickController _joystickController;
    
    public JoystickCraneInput(JoystickController joystickController)
    {
        _joystickController = joystickController;
    }
    
    public float GetHorizontalInput()
    {
        var currentAngleX = _joystickController.transform.localRotation.eulerAngles.x;
        var currentAngleY = _joystickController.transform.localRotation.eulerAngles.y;
        var horizontalInputAxis = 0f;

        var inputIntensity = currentAngleX > _joystickController.StartingRotation.eulerAngles.x + _joystickController.StartRegisterInputAngleThreshold
            ? Mathf.Abs(_joystickController.StartingRotation.eulerAngles.x - currentAngleX) * 1 / _joystickController.MaxAngle
            : 0f;

        if (inputIntensity == 0) return 0f;
        
        // negative movement on the horizontal axis
        if (currentAngleY > 0 + _joystickController.StartRegisterAxisAngleThreshold &&
            currentAngleY < 180 - _joystickController.StartRegisterAxisAngleThreshold)
        {
            horizontalInputAxis = -(1 - Mathf.Abs(90 - currentAngleY) * 1 / 75);
        }
        // positive movement on the horizontal axis
        else if (currentAngleY < 360 - _joystickController.StartRegisterAxisAngleThreshold &&
                 currentAngleY > 180 + _joystickController.StartRegisterAxisAngleThreshold)
        {
            horizontalInputAxis = (1 - Mathf.Abs(270 - currentAngleY) * 1 / 75);
        }
        
        return inputIntensity * horizontalInputAxis;
    }

    public float GetVerticalInput()
    {
        var currentAngleX = _joystickController.transform.localRotation.eulerAngles.x;
        var currentAngleY = _joystickController.transform.localRotation.eulerAngles.y;
        var verticalInputAxis = 0f;
        
        var inputIntensity = currentAngleX > _joystickController.StartingRotation.eulerAngles.x + _joystickController.StartRegisterInputAngleThreshold
            ? Mathf.Abs(_joystickController.StartingRotation.eulerAngles.x - currentAngleX) * 1 / _joystickController.MaxAngle
            : 0f;

        if (inputIntensity == 0) return 0f;
        
        // positive movement on the vertical axis
        if (currentAngleY > 90 + _joystickController.StartRegisterAxisAngleThreshold &&
            currentAngleY < 270 - _joystickController.StartRegisterAxisAngleThreshold)
        {
            verticalInputAxis = (1 - Mathf.Abs(180 - currentAngleY) * 1 / 75);
        }
        
        // negative movement on the vertical axis
        else if (!(currentAngleY > 90 - _joystickController.StartRegisterAxisAngleThreshold &&
                   currentAngleY < 270 + _joystickController.StartRegisterAxisAngleThreshold))
        {
            verticalInputAxis = currentAngleY > 270
                ? -(1 - (Mathf.Abs(360 - currentAngleY) % 360) * 1 / 75)
                : -(1 - (Mathf.Abs(0 - currentAngleY) % 360) * 1 / 75);
        }

        return inputIntensity * verticalInputAxis;
    }
}
