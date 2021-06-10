using System;
using UnityEngine;


[CreateAssetMenu(fileName = "new Task Event Channel", menuName = "Channel/Task", order = 0)]
public class TaskEventChannelSO : ScriptableObject
{
    public event Action<TaskDataBaseSO> OnEventRaised;

    public void RaiseEvent(TaskDataBaseSO taskDataBase)
    {
        OnEventRaised?.Invoke(taskDataBase);
    }
}
