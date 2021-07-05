using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ContainerStockController : MonoBehaviour
{
    [SerializeField] private ContainerStockEventChannelSO containerStockEventChannelSo;
    [field: SerializeField] public StockZone StockZone { get; private set; }
    [SerializeField] private List<HookableBase> containerStockList = new List<HookableBase>();
    [SerializeField] private List<HookableBase> availableContainerStockList = new List<HookableBase>();


    private void OnEnable()
    {
        containerStockEventChannelSo.OnContainerStockRequested += OnContainerStockRequest;
    }

    private void OnDisable()
    {
        containerStockEventChannelSo.OnContainerStockRequested -= OnContainerStockRequest;
    }

    private void OnTriggerEnter(Collider other)
    {
        var hookable = other.gameObject.GetComponent<HookableBase>();
        if (hookable != null && !containerStockList.Contains(hookable))
        {
            containerStockList.Add(hookable);
            availableContainerStockList.Add(hookable);
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

        if (availableContainerStockList.Contains(hookable))
        {
            availableContainerStockList.Remove(hookable);
        }
    }

    private void OnContainerStockRequest(InGameTask task, StockZone stockZone, int amount)
    {
        if (StockZone == stockZone)
        {
            var cargoList = containerStockList.OrderBy(cargo => Random.value).Take(amount).ToList();
            availableContainerStockList = availableContainerStockList.Except(cargoList).ToList();
            
            containerStockEventChannelSo.RaiseContainerStockDeliveredEvent(task, cargoList);
        }
    }
}

public enum StockZone
{
    Red,
    Green
}