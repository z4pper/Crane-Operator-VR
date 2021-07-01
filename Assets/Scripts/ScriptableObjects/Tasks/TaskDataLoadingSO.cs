using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Task Data Loading", menuName = "TaskData/Loading", order = 0)]
public class TaskDataLoadingSO : TaskDataCargoTransportSO
{
    [field: SerializeField] public CargoEventChannelSO CargoEventChannel { get; private set; }
}
