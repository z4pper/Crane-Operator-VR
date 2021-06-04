using System;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "new Task Event Channel", menuName = "Channel/Task", order = 0)]
    public class TaskEventChannelSO : ScriptableObject
    {
        public event Action<TaskDataSO> OnEventRaised;

        public void RaiseEvent(TaskDataSO taskData)
        {
            OnEventRaised?.Invoke(taskData);
        }
    }
}