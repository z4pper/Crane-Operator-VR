using System.Collections.Generic;
using UnityEngine;

public class ContainerStockController : MonoBehaviour
{
    [SerializeField] private ContainerStockEventChannelSO containerStockEventChannelSo;
    [field: SerializeField] public StockZone StockZone { get; private set; }
    private List<HookableBase> containerStockList = new List<HookableBase>();


    private void OnTriggerEnter(Collider other)
    {
        var hookable = other.gameObject.GetComponent<HookableBase>();
        if (hookable != null && !containerStockList.Contains(hookable))
        {
            containerStockList.Add(hookable);
            containerStockEventChannelSo.RaiseContainerStockEnterEvent(this, hookable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var hookable = other.gameObject.GetComponent<HookableBase>();
        if (hookable != null && containerStockList.Contains(hookable))
        {
            containerStockList.Remove(hookable);
            containerStockEventChannelSo.RaiseContainerStockExitEvent(this, hookable);
        }
    }
}

public enum StockZone
{
    Red,
    Green
}