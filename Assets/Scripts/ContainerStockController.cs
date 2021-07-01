using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ContainerStockController : MonoBehaviour
{
    [SerializeField] private ContainerStockEventChannelSO containerStockEventChannelSo;
    [field: SerializeField] public StockZone StockZone { get; private set; }
    [SerializeField] private List<HookableBase> containerStockList = new List<HookableBase>();


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

    private void OnContainerStockRequest(StockZone stockZone, int amount)
    {
        if (StockZone == stockZone)
        {
            var cargoList = containerStockList.OrderBy(cargo => Random.value).Take(amount).ToList();
            
            containerStockEventChannelSo.RaiseContainerStockDeliveredEvent(cargoList);
        }
    }
}

public enum StockZone
{
    Red,
    Green
}