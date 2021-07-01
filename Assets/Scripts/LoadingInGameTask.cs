using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class LoadingInGameTask : InGameTask
{
    private VehicleController _vehicleController;
    private readonly TaskDataLoadingSO _taskDataLoadingSo;

    public LoadingInGameTask(TaskDataLoadingSO taskData)
    {
        TaskData = taskData;
        _taskDataLoadingSo = taskData;
        
        taskData.SignalToTruckEventChannel.OnEventRaised += FinishTask;
        taskData.VehicleEventChannel.OnVehicleArrivedAtDeliveryZone += ChooseCargo;
        taskData.ContainerStockEventChannel.OnContainerStockDelivered += RegisterCargo;
        taskData.CargoEventChannel.OnCargoLoad += OnCargoLoaded;
    }

    private void FinishTask()
    {
        if (CurrentTaskGoalAmount >= TaskData.RequiredAmount)
        {
            TaskData.TaskCompletedEventChannel.RaiseEvent(this);
            _vehicleController.GetComponent<NavMeshAgent>().destination = _taskDataLoadingSo.DespawnPosition.position;
            
            OutlineColorHandler.ReturnOutlineColor(_taskDataLoadingSo.OutlineColor);
            
            _taskDataLoadingSo.SignalToTruckEventChannel.OnEventRaised -= FinishTask;
            _taskDataLoadingSo.ContainerStockEventChannel.OnContainerStockDelivered -= RegisterCargo;
            _taskDataLoadingSo.CargoEventChannel.OnCargoLoad -= OnCargoLoaded;
        }
    }
    
    public override void IncreaseCurrentAmount()
    {
        CurrentTaskGoalAmount++;
        TaskData.TaskProgressionEventChannel.RaiseEvent(this);
    }
    
    public override void StartTask()
    {
        _taskDataLoadingSo.InstantiateVehicle();
        _vehicleController = _taskDataLoadingSo.Vehicle.GetComponent<VehicleController>();

        var navAgent = _taskDataLoadingSo.Vehicle.GetComponentInChildren<NavMeshAgent>();
        navAgent.enabled = true;
        navAgent.destination = _taskDataLoadingSo.UnloadTargetPosition.position;
    }
    
    private void ChooseCargo(VehicleController vehicleController)
    {
        var numOfCargo = Random.Range(1, _vehicleController.CargoSlots.Count + 1);
        var randomZone = (StockZone) Random.Range(0, Enum.GetValues(typeof(StockZone)).Length);
        _taskDataLoadingSo.ContainerStockEventChannel.RaiseContainerStockRequestedEvent(randomZone, numOfCargo);
        TaskData.RequiredAmount = numOfCargo;
    }
    
    private void RegisterCargo(List<HookableBase> requestedCargo)
    {
        _taskDataLoadingSo.CargoList = requestedCargo;
        _vehicleController.SetTargetCargoList(requestedCargo);
        
        _taskDataLoadingSo.OutlineColor = OutlineColorHandler.GetOutlineColor();
        requestedCargo.ForEach(cargo => cargo.MarkOutline(_taskDataLoadingSo.OutlineColor));
    }

    private void OnCargoLoaded(VehicleController vehicleController)
    {
        if (_vehicleController == vehicleController)
        {
            IncreaseCurrentAmount();
        }
    }
}
