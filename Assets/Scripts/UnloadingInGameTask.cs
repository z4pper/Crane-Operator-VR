using System;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class UnloadingInGameTask : InGameTask
{
    private VehicleController _vehicleController;
    private readonly TaskDataUnloadingSO _taskDataUnloadingSo;

    public UnloadingInGameTask(TaskDataUnloadingSO taskData)
    {
        TaskData = taskData;
        _taskDataUnloadingSo = taskData;
        taskData.StockZone = (StockZone) Random.Range(0, Enum.GetValues(typeof(StockZone)).Length);
        
        ConfigureDescription();
        taskData.ContainerStockEventChannel.OnContainerStockEnter += OnCargoEnterContainerStock;
        taskData.ContainerStockEventChannel.OnContainerStockExit += OnCargoExitContainerStock;
        taskData.SignalToTruckEventChannel.OnEventRaised += FinishTask;
    }

    private void OnCargoEnterContainerStock(ContainerStockController stockController, HookableBase hookable)
    {
        if (stockController.StockZone == _taskDataUnloadingSo.StockZone && _vehicleController.CargoList.Contains(hookable))
        {
            IncreaseCurrentAmount();
        }
    }

    private void OnCargoExitContainerStock(ContainerStockController stockController, HookableBase hookable)
    {
        if (stockController.StockZone == _taskDataUnloadingSo.StockZone && _vehicleController.CargoList.Contains(hookable))
        {
            CurrentTaskGoalAmount--;
        } 
    }

    private void FinishTask()
    {
        if (CurrentTaskGoalAmount >= TaskData.RequiredAmount)
        {
            TaskData.TaskCompletedEventChannel.RaiseEvent(this);
            _vehicleController.GetComponent<NavMeshAgent>().destination = _taskDataUnloadingSo.DespawnPosition.position;
            
            _taskDataUnloadingSo.ContainerStockEventChannel.OnContainerStockEnter -= OnCargoEnterContainerStock;
            _taskDataUnloadingSo.ContainerStockEventChannel.OnContainerStockExit -= OnCargoExitContainerStock;
            _taskDataUnloadingSo.SignalToTruckEventChannel.OnEventRaised -= FinishTask;
        }
    }

    public override void IncreaseCurrentAmount()
    {
        CurrentTaskGoalAmount++;
        TaskData.TaskProgressionEventChannel.RaiseEvent(this);
    }

    public override void StartTask()
    {
        _taskDataUnloadingSo.InstantiateVehicle();
        _vehicleController = _taskDataUnloadingSo.Vehicle.GetComponent<VehicleController>();

        var navAgent = _taskDataUnloadingSo.Vehicle.GetComponentInChildren<NavMeshAgent>();
        navAgent.enabled = true;
        navAgent.destination = _taskDataUnloadingSo.UnloadTargetPosition.position;
    }

    private void ConfigureDescription()
    {
        TaskData.Description = TaskData.Description.Replace("{StockZone}",
            Enum.GetName(typeof(StockZone), _taskDataUnloadingSo.StockZone));
    }
}