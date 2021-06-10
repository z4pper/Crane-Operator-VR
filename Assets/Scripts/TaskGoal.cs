using UnityEngine;
using UnityEngine.AI;

public abstract class TaskGoal
{
    public abstract void IncreaseCurrentAmount();
    public abstract void StartTask();
}

public class UnloadingTask : TaskGoal
{
    private VehicleController _vehicleController;
    private readonly TaskDataUnloadingSO _taskDataUnloading;

    public UnloadingTask(TaskDataBaseSO taskData)
    {
        _taskDataUnloading = (TaskDataUnloadingSO) taskData;
        _taskDataUnloading.cargoUnloadedEventChannel.OnCargoUnload += OnCargoUnloadedUnloaded;
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
        _taskDataUnloading.currentAmount++;
        _taskDataUnloading.taskProgressionEventChannel.RaiseEvent(_taskDataUnloading);
        Debug.Log("Current amount = " + _taskDataUnloading.currentAmount );
        if (_taskDataUnloading.currentAmount >= _taskDataUnloading.requiredAmount)
        {
            _taskDataUnloading.taskCompletedEventChannel.RaiseEvent(_taskDataUnloading);
        }
    }

    public override void StartTask()
    {
        _taskDataUnloading.currentAmount = 0;
        _taskDataUnloading.InstantiateVehicle();
        _vehicleController = _taskDataUnloading.Vehicle.GetComponent<VehicleController>();

        var vehicle = _taskDataUnloading.Vehicle;
        var navAgent = vehicle.GetComponent<NavMeshAgent>();
        navAgent.enabled = true;
        navAgent.destination = _taskDataUnloading.unloadTargetPosition.position;
    }
}