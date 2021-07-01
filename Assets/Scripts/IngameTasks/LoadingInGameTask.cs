using System;
using System.Collections.Generic;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class LoadingInGameTask : CargoTransportInGameTask
{
    private VehicleController _vehicleController;
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
        _vehicleController = Vehicle.GetComponent<VehicleController>();

        var navAgent = Vehicle.GetComponentInChildren<NavMeshAgent>();
        navAgent.enabled = true;
        navAgent.destination = _taskDataLoadingSo.UnloadTargetPosition.position;
    }
    
    public override void IncreaseCurrentAmount()
    {
        CurrentTaskGoalAmount++;
        TaskData.TaskProgressionEventChannel.RaiseEvent(this);
    }
    
    protected override void OnDeliveryArrived(VehicleController vehicleController)
    {
        var numOfCargo = Random.Range(1, _vehicleController.CargoSlots.Count + 1);
        var randomZone = (StockZone) Random.Range(0, Enum.GetValues(typeof(StockZone)).Length);
        _taskDataLoadingSo.ContainerStockEventChannel.RaiseContainerStockRequestedEvent(randomZone, numOfCargo);
        TaskData.RequiredAmount = numOfCargo;
    }
    
    private void RegisterCargo(List<HookableBase> requestedCargo)
    {
        _vehicleController.SetTargetCargoList(requestedCargo);
        
        OutlineColor = OutlineColorHandler.GetOutlineColor();
        requestedCargo.ForEach(cargo => cargo.MarkOutline(OutlineColor));
    }

    private void OnCargoLoaded(VehicleController vehicleController)
    {
        if (_vehicleController == vehicleController)
        {
            IncreaseCurrentAmount();
        }
    }
    
    protected override void FinishTask()
    {
        if (CurrentTaskGoalAmount >= TaskData.RequiredAmount)
        {
            TaskData.TaskCompletedEventChannel.RaiseEvent(this);
            _vehicleController.GetComponent<NavMeshAgent>().destination = _taskDataLoadingSo.DespawnPosition.position;
            
            OutlineColorHandler.ReturnOutlineColor(OutlineColor);
            
            _taskDataLoadingSo.SignalToTruckEventChannel.OnEventRaised -= FinishTask;
            _taskDataLoadingSo.VehicleEventChannel.OnVehicleArrivedAtDeliveryZone -= OnDeliveryArrived;
            _taskDataLoadingSo.ContainerStockEventChannel.OnContainerStockDelivered -= RegisterCargo;
            _taskDataLoadingSo.CargoEventChannel.OnCargoLoad -= OnCargoLoaded;
        }
    }
}
