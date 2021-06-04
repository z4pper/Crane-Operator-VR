using System.Collections.Generic;
using UnityEngine;

public class TaskUI : MonoBehaviour
{
    [SerializeField] private GameObject taskTable;
    [SerializeField] private GameObject taskTableEntry;
    [SerializeField] private int taskLimit;
    [SerializeField] private VoidEventChannelSO taskCreatedEventChannel;

    private List<GameObject> taskTableEntrys = new List<GameObject>();

    private void OnEnable()
    {
        taskCreatedEventChannel.OnEventRaised += CreateTask;
    }

    private void OnDisable()
    {
        taskCreatedEventChannel.OnEventRaised -= CreateTask;
    }

    private void CreateTask()
    {
        //TODO: create channel for task event which has taskDataSO as parameter
        var go = Instantiate(taskTableEntry, taskTable.transform, false);
        taskTableEntrys.Add(go);
    }
}
