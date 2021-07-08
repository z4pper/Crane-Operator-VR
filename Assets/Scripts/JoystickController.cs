using UnityEngine;

public class JoystickController : MonoBehaviour, ICraneInupt
{
    [SerializeField] private float maxAngle;
    [SerializeField] private float startRegisterInputAngleThreshold;
    [SerializeField] private float startRegisterAxisAngleThreshold;
    [SerializeField] private Transform craneRotatable;
    [SerializeField] private OVRInput.Button handTrigger;

    private Quaternion _startingRotation;

    private void Start()
    {
        _startingRotation = transform.localRotation;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (OVRInput.Get(handTrigger) && other.CompareTag("Player"))
        {
            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, Vector3.up);
            if (angle < maxAngle)
            {
                transform.LookAt(other.transform, transform.up);
                var angles = transform.localEulerAngles;
                angles.z = -angles.y;
                transform.localEulerAngles = angles;
            }
            else
            {
                Quaternion newRot = Quaternion.LookRotation(direction.normalized);
                transform.localEulerAngles =
                    new Vector3(transform.eulerAngles.x, newRot.eulerAngles.y - craneRotatable.eulerAngles.y, -(newRot.eulerAngles.y - craneRotatable.eulerAngles.y));
            }
        }

        if (OVRInput.GetUp(handTrigger))
        {
            transform.localRotation = _startingRotation;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.localRotation = _startingRotation;
        }
    }

    public float GetHorizontalInput()
    {
        var currentAngleX = transform.localRotation.eulerAngles.x;
        var currentAngleY = transform.localRotation.eulerAngles.y;
        var horizontalInputAxis = 0f;

        var inputIntensity = currentAngleX > 270 + startRegisterInputAngleThreshold
            ? Mathf.Abs(270 - currentAngleX) * 1 / maxAngle
            : 0f;

        if (inputIntensity == 0) return 0f;
        
        // negative movement on the horizontal axis
        if (currentAngleY > 0 + startRegisterAxisAngleThreshold &&
            currentAngleY < 180 - startRegisterAxisAngleThreshold)
        {
            horizontalInputAxis = -(1 - Mathf.Abs(90 - currentAngleY) * 1 / 75);
        }
        // positive movement on the horizontal axis
        else if (currentAngleY < 360 - startRegisterAxisAngleThreshold &&
                 currentAngleY > 180 + startRegisterAxisAngleThreshold)
        {
            horizontalInputAxis = (1 - Mathf.Abs(270 - currentAngleY) * 1 / 75);
        }
        
        return inputIntensity * horizontalInputAxis;
    }

    public float GetVerticalInput()
    {
        var currentAngleX = transform.localRotation.eulerAngles.x;
        var currentAngleY = transform.localRotation.eulerAngles.y;
        var verticalInputAxis = 0f;
        
        var inputIntensity = currentAngleX > 270 + startRegisterInputAngleThreshold
            ? Mathf.Abs(270 - currentAngleX) * 1 / maxAngle
            : 0f;

        if (inputIntensity == 0) return 0f;
        
        // positive movement on the vertical axis
        if (currentAngleY > 90 + startRegisterAxisAngleThreshold &&
            currentAngleY < 270 - startRegisterAxisAngleThreshold)
        {
            verticalInputAxis = (1 - Mathf.Abs(180 - currentAngleY) * 1 / 75);
        }
        
        // negative movement on the vertical axis
        else if (!(currentAngleY > 90 - startRegisterAxisAngleThreshold &&
                   currentAngleY < 270 + startRegisterAxisAngleThreshold))
        {
            verticalInputAxis = currentAngleY > 270
                ? -(1 - (Mathf.Abs(360 - currentAngleY) % 360) * 1 / 75)
                : -(1 - (Mathf.Abs(0 - currentAngleY) % 360) * 1 / 75);
        }

        return inputIntensity * verticalInputAxis;
    }
}