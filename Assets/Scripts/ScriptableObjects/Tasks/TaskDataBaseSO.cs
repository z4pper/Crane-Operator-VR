using UnityEngine;

public abstract class TaskDataBaseSO : ScriptableObject
{
    [field: SerializeField] public int Priority { get; private set; }
    [field: SerializeField] public string Description { get; set; }
    [field: SerializeField] public TaskEventChannelSO TaskCompletedEventChannel { get; private set; }
    [field: SerializeField] public TaskEventChannelSO TaskProgressionEventChannel { get; private set; }
    [field: SerializeField] public TaskEventChannelSO TaskCreatedEventChannel { get; private set; }
}
