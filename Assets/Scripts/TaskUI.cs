using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public class TaskUI : MonoBehaviour
{
    [SerializeField] private GameObject taskTable;
    [SerializeField] private GameObject taskTableEntry;
    [SerializeField] private int taskLimit;
    [SerializeField] private TaskEventChannelSO taskCreatedEventChannel;

    private List<GameObject> taskTableEntrys = new List<GameObject>();

    private void OnEnable()
    {
        taskCreatedEventChannel.OnEventRaised += CreateTask;
    }

    private void OnDisable()
    {
        taskCreatedEventChannel.OnEventRaised -= CreateTask;
    }

    private void CreateTask(TaskDataSO taskData)
    {
        var go = Instantiate(taskTableEntry, taskTable.transform, false);
        taskTableEntrys.Add(go);
    }
}
