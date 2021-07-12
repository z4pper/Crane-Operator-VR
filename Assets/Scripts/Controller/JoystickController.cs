using UnityEngine;

public class JoystickController : MonoBehaviour
{
    [field: SerializeField] public float MaxAngle { get; private set; }
    [field: SerializeField] public float StartRegisterInputAngleThreshold { get; private set; }
    [field: SerializeField] public float StartRegisterAxisAngleThreshold { get; private set; }
    [SerializeField] private Transform craneRotatable;
    [SerializeField] private OVRInput.RawButton handTrigger;

    public Quaternion StartingRotation { get; private set; }

    private void Start()
    {
        StartingRotation = transform.localRotation;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (OVRInput.Get(handTrigger) && other.CompareTag("Player"))
        {
            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, Vector3.up);
            if (angle < MaxAngle)
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
            transform.localRotation = StartingRotation;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.localRotation = StartingRotation;
        }
    }
}