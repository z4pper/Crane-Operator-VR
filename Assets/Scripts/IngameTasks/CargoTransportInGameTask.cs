using UnityEngine;

public abstract class CargoTransportInGameTask : InGameTask
{
    protected VehicleController VehicleController;

    protected GameObject Vehicle;

    protected abstract void FinishTask();

    protected abstract void OnDeliveryArrived(VehicleController vehicleController);
}
