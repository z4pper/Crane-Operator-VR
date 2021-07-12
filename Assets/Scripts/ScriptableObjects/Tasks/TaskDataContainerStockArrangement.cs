using UnityEngine;

[CreateAssetMenu(fileName = "new Container Stock Arrangement Task", menuName = "TaskData/ContainerStockArrangement", order = 0)]
public class TaskDataContainerStockArrangement : TaskDataBaseSO
{
    [field: SerializeField] public int MaxAmountContainerToArrange { get; private set; }
    [field: SerializeField] public ContainerStockEventChannelSO ContainerStockEventChannel { get; private set; }
}
