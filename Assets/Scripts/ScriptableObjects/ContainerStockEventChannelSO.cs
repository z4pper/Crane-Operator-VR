using System;
using UnityEngine;

[CreateAssetMenu(fileName = "new Container Stock Event Channel", menuName = "Channel/Container Stock Event Channel", order = 0)]
public class ContainerStockEventChannelSO : ScriptableObject
{
    public event Action<ContainerStockController, HookableBase> OnContainerStockEnter;
    public event Action<ContainerStockController, HookableBase> OnContainerStockExit;

    public void RaiseContainerStockEnterEvent(ContainerStockController stockController, HookableBase hookable)
    {
        OnContainerStockEnter?.Invoke(stockController, hookable);
    }
    
    public void RaiseContainerStockExitEvent(ContainerStockController stockController, HookableBase hookable)
    {
        OnContainerStockExit?.Invoke(stockController, hookable);
    }
}
