using UnityEngine;


public abstract class TaskDataCargoTransportSO : TaskDataBaseSO
{
            [field: SerializeField] public Transform SpawnPosition { get; private set; }
            [field: SerializeField] public Transform UnloadTargetPosition { get; private set; }
            [field: SerializeField] public Transform DespawnPosition { get; private set; }
            [field: SerializeField] public ContainerStockEventChannelSO ContainerStockEventChannel { get; private set; }
            [field: SerializeField] public VoidEventChannelSO SignalToTruckEventChannel { get; private set; }
            [field: SerializeField] public VehicleEventChannelSO VehicleEventChannel { get; private set; }
        
            [SerializeField]private GameObject vehicle;
        
            public GameObject Vehicle { get; private set; }
        
            public void InstantiateVehicle()
            {
                Vehicle = Instantiate(vehicle, SpawnPosition.position, Quaternion.identity);
            }
}
