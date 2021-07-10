using UnityEngine;

public abstract class TaskDataCargoTransportSO : TaskDataBaseSO
{
    [field: SerializeField] public Transform SpawnPosition { get; private set; }
    [field: SerializeField] public Transform UnloadTargetPosition { get; private set; }
    [field: SerializeField] public Transform DespawnPosition { get; private set; }
    [field: SerializeField] public ContainerStockEventChannelSO ContainerStockEventChannel { get; private set; }
    [field: SerializeField] public VoidEventChannelSO SignalToTruckEventChannel { get; private set; }
    [field: SerializeField] public VehicleEventChannelSO VehicleEventChannel { get; private set; }
    [field: SerializeField] public TaskDataEventChannelSO TaskDataAdjustedEventChannel { get; private set; }
    [field: SerializeField] public TaskDataCargoTransportArrivalSO TaskDataCargoTransportArrivalSo { get; private set; }
    [field: SerializeField] public TaskDataCargoTransportDepartureSO TaskDataCargoTransportDepartureSo { get; private set; }
    
    [SerializeField]private GameObject vehicle;
    
    public GameObject InstantiateVehicle()
    {
        return Instantiate(vehicle, SpawnPosition.position, Quaternion.identity);
    }
}
