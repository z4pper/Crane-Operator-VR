using UnityEngine;

[CreateAssetMenu(fileName = "new Unloading Task Data", menuName = "TaskData/Unloading")]
public class TaskDataUnloadingSO : TaskDataBaseSO
{
    [field: SerializeField] public Transform SpawnPosition { get; private set; }
    [field: SerializeField] public Transform UnloadTargetPosition { get; private set; }
    [field: SerializeField] public Transform DespawnPosition { get; private set; }
    [field: SerializeField] public CargoEventChannelSO CargoUnloadedEventChannel { get; private set; }
    
    [SerializeField]private GameObject vehicle;

    public GameObject Vehicle { get; private set; }

    public void InstantiateVehicle()
    {
        Vehicle = Instantiate(vehicle, SpawnPosition.position, Quaternion.identity);
    }
}
