using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class ContainerStockArrangementInGameTask : InGameTask
{
    private readonly TaskDataContainerStockArrangement _taskDataContainerStockArrangement;
    private List<HookableBase> _containerToArrange = new List<HookableBase>();
    private readonly StockZone _startingStockZone;
    
    public ContainerStockArrangementInGameTask(TaskDataContainerStockArrangement taskData)
    {
        TaskData = taskData;
        _taskDataContainerStockArrangement = taskData;
        StockZone = (StockZone) Random.Range(0, Enum.GetValues(typeof(StockZone)).Length);
        _startingStockZone = StockZone.Next();

        taskData.ContainerStockEventChannel.OnContainerStockEnter += OnCargoEnterContainerStock;
        taskData.ContainerStockEventChannel.OnContainerStockExit += OnCargoExitContainerStock;
        taskData.ContainerStockEventChannel.OnContainerStockDelivered += RegisterContainer;
    }
    
    public override void IncreaseCurrentAmount()
    {
        CurrentTaskGoalAmount++;
        TaskData.TaskProgressionEventChannel.RaiseEvent(this);
        if (CurrentTaskGoalAmount >= RequiredTaskGoalAmount)
        {
            FinishTask();
        }
    }

    public override void StartTask()
    {
        var numOfContainer = Random.Range(1, _taskDataContainerStockArrangement.MaxAmountContainerToArrange + 1);
        _taskDataContainerStockArrangement.ContainerStockEventChannel.RaiseContainerStockRequestedEvent(this, _startingStockZone, numOfContainer);
        RequiredTaskGoalAmount = numOfContainer;
        _taskDataContainerStockArrangement.TaskCreatedEventChannel.RaiseEvent(this);
    }

    private void RegisterContainer(InGameTask task, List<HookableBase> requestedContainer)
    {
        if (task != this) return;
        _containerToArrange = requestedContainer;
        
        OutlineColor = OutlineColorHandler.GetOutlineColor();
        requestedContainer.ForEach(cargo => cargo.MarkOutline(OutlineColor));
    }
    
    private void OnCargoEnterContainerStock(ContainerStockController stockController, HookableBase hookable)
    {
        if (stockController.StockZone == StockZone &&
            _containerToArrange.Contains(hookable))
        {
            hookable.UnmarkOutline();
            IncreaseCurrentAmount();
        }
    }

    private void OnCargoExitContainerStock(ContainerStockController stockController, HookableBase hookable)
    {
        if (stockController.StockZone == StockZone &&
            _containerToArrange.Contains(hookable))
        {
            hookable.MarkOutline(OutlineColor);
            CurrentTaskGoalAmount--;
            TaskData.TaskProgressionEventChannel.RaiseEvent(this);
        }
    }

    protected override void FinishTask()
    {
        _taskDataContainerStockArrangement.TaskCompletedEventChannel.RaiseEvent(this);
        
        _taskDataContainerStockArrangement.ContainerStockEventChannel.OnContainerStockEnter -= OnCargoEnterContainerStock;
        _taskDataContainerStockArrangement.ContainerStockEventChannel.OnContainerStockExit -= OnCargoExitContainerStock;
        _taskDataContainerStockArrangement.ContainerStockEventChannel.OnContainerStockDelivered -= RegisterContainer;
    }
}
