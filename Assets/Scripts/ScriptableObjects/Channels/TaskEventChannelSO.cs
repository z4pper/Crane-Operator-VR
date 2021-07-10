using System;
using UnityEngine;

[CreateAssetMenu(fileName = "new Task Event Channel", menuName = "Channel/Task", order = 0)]
public class TaskEventChannelSO : ScriptableObject
{
    public event Action<InGameTask> OnEventRaised;

    public void RaiseEvent(InGameTask inGameTask)
    {
        OnEventRaised?.Invoke(inGameTask);
    }
}
