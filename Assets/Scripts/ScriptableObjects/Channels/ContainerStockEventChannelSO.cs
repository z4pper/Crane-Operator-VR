using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Container Stock Event Channel", menuName = "Channel/Container Stock Event Channel", order = 0)]
public class ContainerStockEventChannelSO : ScriptableObject
{
    public event Action<ContainerStockController, HookableBase> OnContainerStockEnter;
    public event Action<ContainerStockController, HookableBase> OnContainerStockExit;

    public event Action<InGameTask, StockZone, int> OnContainerStockRequested;

    public event Action<InGameTask, List<HookableBase>> OnContainerStockDelivered;

    public void RaiseContainerStockEnterEvent(ContainerStockController stockController, HookableBase hookable)
    {
        OnContainerStockEnter?.Invoke(stockController, hookable);
    }
    
    public void RaiseContainerStockExitEvent(ContainerStockController stockController, HookableBase hookable)
    {
        OnContainerStockExit?.Invoke(stockController, hookable);
    }

    public void RaiseContainerStockRequestedEvent(InGameTask task, StockZone stockZone, int amount)
    {
        OnContainerStockRequested?.Invoke(task, stockZone, amount);
    }

    public void RaiseContainerStockDeliveredEvent(InGameTask task, List<HookableBase> deliveredCargo)
    {
        OnContainerStockDelivered?.Invoke(task, deliveredCargo);
    }
}
