
public abstract class CargoTransportInGameTask : InGameTask
{
    protected VehicleController VehicleController;
    protected abstract void FinishTask();

    protected abstract void OnDeliveryArrived(VehicleController vehicleController);
}
