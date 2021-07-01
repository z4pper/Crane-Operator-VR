using System;
using UnityEngine;

[CreateAssetMenu(fileName = "new Vehicle Event Channel", menuName = "Channel/Vehicle", order = 0)]
public class VehicleEventChannelSO : ScriptableObject
{
    public event Action<VehicleController> OnVehicleArrivedAtDeliveryZone;

    public void RaiseVehicleArrivedAtDeliveryZoneEvent(VehicleController vehicleController)
    {
        OnVehicleArrivedAtDeliveryZone?.Invoke(vehicleController);
    }
}
