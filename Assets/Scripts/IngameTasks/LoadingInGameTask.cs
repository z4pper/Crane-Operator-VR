using System;
using System.Collections.Generic;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class LoadingInGameTask : CargoTransportInGameTask
{
    private readonly TaskDataLoadingSO _taskDataLoadingSo;

    public LoadingInGameTask(TaskDataLoadingSO taskData)
    {
        TaskData = taskData;
        _taskDataLoadingSo = taskData;
        
        taskData.SignalToTruckEventChannel.OnEventRaised += FinishTask;
        taskData.VehicleEventChannel.OnVehicleArrivedAtDeliveryZone += OnDeliveryArrived;
        taskData.ContainerStockEventChannel.OnContainerStockDelivered += RegisterCargo;
        taskData.CargoEventChannel.OnCargoLoad += OnCargoLoaded;
    }
    
    public override void StartTask()
    {
        Vehicle = _taskDataLoadingSo.InstantiateVehicle();
        VehicleController = Vehicle.GetComponent<VehicleController>();

        var navAgent = Vehicle.GetComponentInChildren<NavMeshAgent>();
        navAgent.enabled = true;
        navAgent.destination = _taskDataLoadingSo.UnloadTargetPosition.position;
        
        var numOfCargo = Random.Range(1, VehicleController.CargoSlots.Count + 1);
        RequiredTaskGoalAmount = numOfCargo;
    }
    
    public override void IncreaseCurrentAmount()
    {
        CurrentTaskGoalAmount++;
        TaskData.TaskProgressionEventChannel.RaiseEvent(this);
    }
    
    protected override void OnDeliveryArrived(VehicleController vehicleController)
    {
        if (vehicleController != VehicleController) return;
        StockZone = (StockZone) Random.Range(0, Enum.GetValues(typeof(StockZone)).Length);
        _taskDataLoadingSo.ContainerStockEventChannel.RaiseContainerStockRequestedEvent(this, StockZone, RequiredTaskGoalAmount);
    }
    
    private void RegisterCargo(InGameTask task, List<HookableBase> requestedCargo)
    {
        if (task != this) return;
        VehicleController.SetTargetCargoList(requestedCargo);
        
        OutlineColor = OutlineColorHandler.GetOutlineColor();
        requestedCargo.ForEach(cargo => cargo.MarkOutline(OutlineColor));
    }

    private void OnCargoLoaded(VehicleController vehicleController)
    {
        if (VehicleController == vehicleController)
        {
            IncreaseCurrentAmount();
        }
    }
    
    protected override void FinishTask()
    {
        if (CurrentTaskGoalAmount >= RequiredTaskGoalAmount)
        {
            TaskData.TaskCompletedEventChannel.RaiseEvent(this);
            VehicleController.GetComponent<NavMeshAgent>().destination = _taskDataLoadingSo.DespawnPosition.position;
            
            OutlineColorHandler.ReturnOutlineColor(OutlineColor);
            
            _taskDataLoadingSo.SignalToTruckEventChannel.OnEventRaised -= FinishTask;
            _taskDataLoadingSo.VehicleEventChannel.OnVehicleArrivedAtDeliveryZone -= OnDeliveryArrived;
            _taskDataLoadingSo.ContainerStockEventChannel.OnContainerStockDelivered -= RegisterCargo;
            _taskDataLoadingSo.CargoEventChannel.OnCargoLoad -= OnCargoLoaded;
        }
    }
}
