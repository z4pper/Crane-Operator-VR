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
        taskData.ContainerStockEventChannel.OnContainerStockDelivered += RegisterCargo;
        taskData.CargoEventChannel.OnCargoLoad += OnCargoLoaded;
    }

    private void FinishTask()
    {
        if (CurrentTaskGoalAmount >= TaskData.RequiredAmount)
        {
            TaskData.TaskCompletedEventChannel.RaiseEvent(this);
            _vehicleController.GetComponent<NavMeshAgent>().destination = _taskDataLoadingSo.DespawnPosition.position;
            
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

        var numOfCargo = Random.Range(1, _vehicleController.CargoSlots.Count + 1);
        var randomZone = (StockZone) Random.Range(0, Enum.GetValues(typeof(StockZone)).Length);
        _taskDataLoadingSo.ContainerStockEventChannel.RaiseContainerStockRequestedEvent(randomZone, numOfCargo);
        TaskData.RequiredAmount = numOfCargo;
        
        var navAgent = _taskDataLoadingSo.Vehicle.GetComponentInChildren<NavMeshAgent>();
        navAgent.enabled = true;
        navAgent.destination = _taskDataLoadingSo.UnloadTargetPosition.position;
    }

    private void RegisterCargo(List<HookableBase> requestedCargo)
    {
        _taskDataLoadingSo.CargoList = requestedCargo;
        _vehicleController.SetTargetCargoList(requestedCargo);
        requestedCargo.ForEach(cargo =>
        {
            var outline = cargo.gameObject.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = Color.red;
            outline.OutlineWidth = 5f;
        });
    }

    private void OnCargoLoaded(VehicleController vehicleController)
    {
        if (_vehicleController == vehicleController)
        {
            IncreaseCurrentAmount();
        }
    }
}
