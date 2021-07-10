using System;
using UnityEngine;

[CreateAssetMenu(fileName = "new Task Data Event Channel", menuName = "Channel/Task Data", order = 0)]
public class TaskDataEventChannelSO : ScriptableObject
{
    public event Action<InGameTask, TaskDataBaseSO> OnEventRaised;

    public void RaiseEvent(InGameTask task, TaskDataBaseSO taskData)
    {
        OnEventRaised?.Invoke(task, taskData);
    }
}
