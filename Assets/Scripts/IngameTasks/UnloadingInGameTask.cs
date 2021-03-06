using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class UnloadingInGameTask : CargoTransportInGameTask
{
    private readonly TaskDataUnloadingSO _taskDataUnloadingSo;

    public UnloadingInGameTask(TaskDataUnloadingSO taskData)
    {
        TaskData = taskData;
        StockZone = (StockZone) Random.Range(0, Enum.GetValues(typeof(StockZone)).Length);
        OutlineColor = OutlineColorHandler.GetOutlineColor();
        _taskDataUnloadingSo = taskData;

        taskData.ContainerStockEventChannel.OnContainerStockEnter += OnCargoEnterContainerStock;
        taskData.ContainerStockEventChannel.OnContainerStockExit += OnCargoExitContainerStock;
        taskData.VehicleEventChannel.OnVehicleArrivedAtDeliveryZone += OnDeliveryArrived;
        taskData.SignalToTruckEventChannel.OnEventRaised += FinishTask;
        
        _taskDataUnloadingSo.TaskCreatedEventChannel.RaiseEvent(this);
    }

    public override void StartTask()
    {
        Vehicle = _taskDataUnloadingSo.InstantiateVehicle();
        VehicleController = Vehicle.GetComponent<VehicleController>();
        RequiredTaskGoalAmount = VehicleController.CargoList.Count;

        var navAgent = Vehicle.GetComponentInChildren<NavMeshAgent>();
        navAgent.enabled = true;
        navAgent.destination = _taskDataUnloadingSo.UnloadTargetPosition.position;
        
        _taskDataUnloadingSo.TaskDataAdjustedEventChannel.RaiseEvent(this, _taskDataUnloadingSo.TaskDataCargoTransportArrivalSo);
    }

    protected override void OnDeliveryArrived(VehicleController vehicleController)
    {
        if (vehicleController != VehicleController) return;
        vehicleController.CargoList.ForEach(cargo => cargo.MarkOutline(OutlineColor));
        _taskDataUnloadingSo.TaskDataAdjustedEventChannel.RaiseEvent(this, _taskDataUnloadingSo);
    }

    public override void IncreaseCurrentAmount()
    {
        CurrentTaskGoalAmount++;
        TaskData.TaskProgressionEventChannel.RaiseEvent(this);

        if (CurrentTaskGoalAmount >= RequiredTaskGoalAmount)
        {
            _taskDataUnloadingSo.TaskDataAdjustedEventChannel.RaiseEvent(this, _taskDataUnloadingSo.TaskDataCargoTransportDepartureSo);
        }
    }

    private void OnCargoEnterContainerStock(ContainerStockController stockController, HookableBase hookable)
    {
        if (stockController.StockZone == StockZone &&
            VehicleController.CargoList.Contains(hookable))
        {
            hookable.UnmarkOutline();
            IncreaseCurrentAmount();
        }
    }

    private void OnCargoExitContainerStock(ContainerStockController stockController, HookableBase hookable)
    {
        if (stockController.StockZone == StockZone &&
            VehicleController.CargoList.Contains(hookable))
        {
            hookable.MarkOutline(OutlineColor);
            CurrentTaskGoalAmount--;
            TaskData.TaskProgressionEventChannel.RaiseEvent(this);
        }
    }

    protected override void FinishTask()
    {
        if (CurrentTaskGoalAmount >= RequiredTaskGoalAmount)
        {
            TaskData.TaskCompletedEventChannel.RaiseEvent(this);
            VehicleController.GetComponent<NavMeshAgent>().destination = _taskDataUnloadingSo.DespawnPosition.position;
            
            OutlineColorHandler.ReturnOutlineColor(OutlineColor);

            _taskDataUnloadingSo.ContainerStockEventChannel.OnContainerStockEnter -= OnCargoEnterContainerStock;
            _taskDataUnloadingSo.ContainerStockEventChannel.OnContainerStockExit -= OnCargoExitContainerStock;
            _taskDataUnloadingSo.VehicleEventChannel.OnVehicleArrivedAtDeliveryZone -= OnDeliveryArrived;
            _taskDataUnloadingSo.SignalToTruckEventChannel.OnEventRaised -= FinishTask;
        }
    }
}