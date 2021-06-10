using UnityEngine;

[CreateAssetMenu(fileName = "new Unloading Task Data", menuName = "TaskData/Unloading")]
public class TaskDataUnloadingSO : TaskDataBaseSO
{
    [SerializeField]private GameObject vehicle;
    public Transform spawnPosition;
    public Transform unloadTargetPosition;
    public CargoEventChannelSO cargoUnloadedEventChannel;
    
    public GameObject Vehicle { get; set; }

    public void InstantiateVehicle()
    {
        Vehicle = Instantiate(vehicle, spawnPosition.position, Quaternion.identity);
    }
}
