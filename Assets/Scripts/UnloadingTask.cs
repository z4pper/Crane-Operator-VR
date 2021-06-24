using System;
using Oculus.Platform;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class UnloadingTask : Task
{
    private VehicleController _vehicleController;

    public UnloadingTask(TaskDataUnloadingSO taskData)
    {
        TaskDataUnloading = taskData;
        TaskDataUnloading.StockZone = (StockZone) Random.Range(0, Enum.GetValues(typeof(StockZone)).Length);
        
        ConfigureDescription();
        //TaskDataUnloading.CargoUnloadedEventChannel.OnCargoUnload += OnCargoUnloadedUnloaded;
        TaskDataUnloading.ContainerStockEventChannel.OnContainerStockEnter += OnCargoEnterContainerStock;
        TaskDataUnloading.ContainerStockEventChannel.OnContainerStockExit += OnCargoExitContainerStock;
    }

    private void OnCargoEnterContainerStock(ContainerStockController stockController, HookableBase hookable)
    {
        if (stockController.StockZone == TaskDataUnloading.StockZone && _vehicleController.CargoList.Contains(hookable))
        {
            IncreaseCurrentAmount();
        }
    }

    private void OnCargoExitContainerStock(ContainerStockController stockController, HookableBase hookable)
    {
        if (stockController.StockZone == TaskDataUnloading.StockZone && _vehicleController.CargoList.Contains(hookable))
        {
            CurrentTaskGoalAmount--;
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

    private void ConfigureDescription()
    {
        TaskDataUnloading.Description = TaskDataUnloading.Description.Replace("{StockZone}",
            Enum.GetName(typeof(StockZone), TaskDataUnloading.StockZone));
    }
}