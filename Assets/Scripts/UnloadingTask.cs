using UnityEngine.AI;

public class UnloadingTask : Task
{
    private VehicleController _vehicleController;

    public UnloadingTask(TaskDataBaseSO taskData)
    {
        TaskDataUnloading = (TaskDataUnloadingSO) taskData;
        TaskDataUnloading.CargoUnloadedEventChannel.OnCargoUnload += OnCargoUnloadedUnloaded;
    }

    private void OnCargoUnloadedUnloaded(VehicleController vehicleController)
    {
        if (vehicleController == this._vehicleController)
        {
            IncreaseCurrentAmount();
        }
    }

    public override void IncreaseCurrentAmount()
    {
        CurrentTaskGoalAmount++;
        TaskDataUnloading.TaskProgressionEventChannel.RaiseEvent(this);
        if (CurrentTaskGoalAmount >= TaskDataUnloading.RequiredAmount)
        {
            TaskDataUnloading.TaskCompletedEventChannel.RaiseEvent(this);
        }
    }

    public override void StartTask()
    {
        TaskDataUnloading.InstantiateVehicle();
        _vehicleController = TaskDataUnloading.Vehicle.GetComponent<VehicleController>();

        var vehicle = TaskDataUnloading.Vehicle;
        var navAgent = vehicle.GetComponent<NavMeshAgent>();
        navAgent.enabled = true;
        navAgent.destination = TaskDataUnloading.UnloadTargetPosition.position;
    }
}