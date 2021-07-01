using UnityEngine;

[CreateAssetMenu(fileName = "new Unloading Task Data", menuName = "TaskData/Unloading")]
public class TaskDataUnloadingSO : TaskDataCargoTransportSO
{
    public StockZone StockZone { get; set; }
}
