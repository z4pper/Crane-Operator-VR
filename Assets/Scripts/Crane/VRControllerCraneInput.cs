
public class VRControllerCraneInput : ICraneInput
{
    private readonly OVRInput.RawAxis2D _controllerStickAxis;

    public VRControllerCraneInput(OVRInput.RawAxis2D controllerStickAxis)
    {
        _controllerStickAxis = controllerStickAxis;
    }
    public float GetHorizontalInput()
    {
        return OVRInput.Get(_controllerStickAxis).x;
    }

    public float GetVerticalInput()
    {
        return OVRInput.Get(_controllerStickAxis).y;
    }
}
