using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class VehiclePosition : MonoBehaviour
{
    [field: SerializeField] public VehicleTargetPosition VehicleTargetPosition { get; private set; }
}

public enum VehicleTargetPosition
{
    Start,
    End,
    Delivery
}
