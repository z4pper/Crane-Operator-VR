using System;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private List<TaskDataSO> possibleTasks;
    [SerializeField] private VoidEventChannelSO taskCreatedEventChannel;

    private void Start()
    {
        RaiseEvent();
    }

    private void RaiseEvent()
    {
        taskCreatedEventChannel.RaiseEvent();
    }
}
