using System;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class UnloadingInGameTask : CargoTransportInGameTask
{
    private readonly TaskDataUnloadingSO _taskDataUnloadingSo;

    public UnloadingInGameTask(TaskDataUnloadingSO taskData)
    {
        TaskData = taskData;
        taskData.StockZone = (StockZone) Random.Range(0, Enum.GetValues(typeof(StockZone)).Length);
        _taskDataUnloadingSo = taskData;

        ConfigureDescription();
        taskData.ContainerStockEventChannel.OnContainerStockEnter += OnCargoEnterContainerStock;
        taskData.ContainerStockEventChannel.OnContainerStockExit += OnCargoExitContainerStock;
        taskData.VehicleEventChannel.OnVehicleArrivedAtDeliveryZone += OnDeliveryArrived;
        taskData.SignalToTruckEventChannel.OnEventRaised += FinishTask;
    }

    private void ConfigureDescription()
    {
        TaskData.Description = TaskData.Description.Replace("{StockZone}",
            Enum.GetName(typeof(StockZone), _taskDataUnloadingSo.StockZone));
    }

    public override void StartTask()
    {
        _taskDataUnloadingSo.InstantiateVehicle();
        VehicleController = _taskDataUnloadingSo.Vehicle.GetComponent<VehicleController>();

        var navAgent = _taskDataUnloadingSo.Vehicle.GetComponentInChildren<NavMeshAgent>();
        navAgent.enabled = true;
        navAgent.destination = _taskDataUnloadingSo.UnloadTargetPosition.position;
    }

    protected override void OnDeliveryArrived(VehicleController vehicleController)
    {
        _taskDataUnloadingSo.OutlineColor = OutlineColorHandler.GetOutlineColor();
        vehicleController.CargoList.ForEach(cargo => cargo.MarkOutline(_taskDataUnloadingSo.OutlineColor));
    }

    public override void IncreaseCurrentAmount()
    {
        CurrentTaskGoalAmount++;
        TaskData.TaskProgressionEventChannel.RaiseEvent(this);
    }

    private void OnCargoEnterContainerStock(ContainerStockController stockController, HookableBase hookable)
    {
        if (stockController.StockZone == _taskDataUnloadingSo.StockZone &&
            VehicleController.CargoList.Contains(hookable))
        {
            hookable.UnmarkOutline();
            IncreaseCurrentAmount();
        }
    }

    private void OnCargoExitContainerStock(ContainerStockController stockController, HookableBase hookable)
    {
        if (stockController.StockZone == _taskDataUnloadingSo.StockZone &&
            VehicleController.CargoList.Contains(hookable))
        {
            hookable.MarkOutline(_taskDataUnloadingSo.OutlineColor);
            CurrentTaskGoalAmount--;
            TaskData.TaskProgressionEventChannel.RaiseEvent(this);
        }
    }

    protected override void FinishTask()
    {
        if (CurrentTaskGoalAmount >= TaskData.RequiredAmount)
        {
            TaskData.TaskCompletedEventChannel.RaiseEvent(this);
            VehicleController.GetComponent<NavMeshAgent>().destination = _taskDataUnloadingSo.DespawnPosition.position;
            
            OutlineColorHandler.ReturnOutlineColor(_taskDataUnloadingSo.OutlineColor);

            _taskDataUnloadingSo.ContainerStockEventChannel.OnContainerStockEnter -= OnCargoEnterContainerStock;
            _taskDataUnloadingSo.ContainerStockEventChannel.OnContainerStockExit -= OnCargoExitContainerStock;
            _taskDataUnloadingSo.VehicleEventChannel.OnVehicleArrivedAtDeliveryZone -= OnDeliveryArrived;
            _taskDataUnloadingSo.SignalToTruckEventChannel.OnEventRaised -= FinishTask;
        }
    }
}