using UnityEngine;
using UnityEngine.AI;

public class UnloadingTask : Task
{
    private VehicleController _vehicleController;

    public UnloadingTask(TaskDataUnloadingSO taskData)
    {
        TaskDataUnloading = taskData;
        TaskDataUnloading.CargoUnloadedEventChannel.OnCargoUnload += OnCargoUnloadedUnloaded;
    }

    private void OnCargoUnloadedUnloaded(VehicleController vehicleController)
    {
        if (vehicleController == this._vehicleController)
        {
            IncreaseCurrentAmount();
        }
    }

    private void FinishTask()
    {
        _vehicleController.GetComponent<NavMeshAgent>().destination = TaskDataUnloading.DespawnPosition.position;
    }

    public override void IncreaseCurrentAmount()
    {
        CurrentTaskGoalAmount++;
        TaskDataUnloading.TaskProgressionEventChannel.RaiseEvent(this);
        if (CurrentTaskGoalAmount >= TaskDataUnloading.RequiredAmount)
        {
            FinishTask();
            TaskDataUnloading.TaskCompletedEventChannel.RaiseEvent(this);
        }
    }

    public override void StartTask()
    {
        TaskDataUnloading.InstantiateVehicle();
        _vehicleController = TaskDataUnloading.Vehicle.GetComponent<VehicleController>();

        var navAgent = TaskDataUnloading.Vehicle.GetComponentInChildren<NavMeshAgent>();
        navAgent.enabled = true;
        navAgent.destination = TaskDataUnloading.UnloadTargetPosition.position;
    }
}