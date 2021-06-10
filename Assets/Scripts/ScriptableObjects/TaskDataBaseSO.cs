using UnityEngine;

public abstract class TaskDataBaseSO : ScriptableObject
{
    public int priority;
    public string description;
    public int requiredAmount;

    public TaskType taskType;
    public TaskEventChannelSO taskCompletedEventChannel;
    public TaskEventChannelSO taskProgressionEventChannel;
    
    public int currentAmount { get; set; }
}

public enum TaskType
{
    Unload,
    Load,
    Arrange,
    HoldPosition
}
