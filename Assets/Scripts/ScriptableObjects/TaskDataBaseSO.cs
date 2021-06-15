using UnityEngine;

public abstract class TaskDataBaseSO : ScriptableObject
{
    [field: SerializeField] public int Priority { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public int RequiredAmount { get; private set; }
    [field: SerializeField] public TaskEventChannelSO TaskCompletedEventChannel { get; private set; }
    [field: SerializeField] public TaskEventChannelSO TaskProgressionEventChannel { get; private set; }
    
    public TaskType TaskType { get; protected set; }
    [field:SerializeField] public int currentAmount { get; set; }
}

public enum TaskType
{
    Unload,
    Load,
    Arrange,
    HoldPosition
}
