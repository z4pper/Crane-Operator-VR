using System;
using UnityEngine;


[CreateAssetMenu(fileName = "new Task Event Channel", menuName = "Channel/Task", order = 0)]
public class TaskEventChannelSO : ScriptableObject
{
    public event Action<Task> OnEventRaised;

    public void RaiseEvent(Task task)
    {
        OnEventRaised?.Invoke(task);
    }
}
