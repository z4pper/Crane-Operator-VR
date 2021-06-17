using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskUI : MonoBehaviour
{
    [SerializeField] private TaskEventChannelSO taskCreatedEventChannel;
    [SerializeField] private TaskEventChannelSO taskCompletedEventChannel;
    [SerializeField] private TaskEventChannelSO taskProgressionEventChannel;
    
    [SerializeField] private GameObject taskTable;
    [SerializeField] private GameObject taskTableEntryPrefab;
    [SerializeField] private int taskLimit;

    private Dictionary<Task, GameObject> taskToTableEntry = new Dictionary<Task, GameObject>();

    private void OnEnable()
    {
        taskCreatedEventChannel.OnEventRaised += CreateTask;
        taskCompletedEventChannel.OnEventRaised += RemoveTask;
        taskProgressionEventChannel.OnEventRaised += UpdateTaskProgression;
    }

    private void OnDisable()
    {
        taskCreatedEventChannel.OnEventRaised -= CreateTask;
        taskCompletedEventChannel.OnEventRaised -= RemoveTask;
        taskProgressionEventChannel.OnEventRaised -= UpdateTaskProgression;
    }

    private void CreateTask(Task task)
    {
        var tableEntry = Instantiate(taskTableEntryPrefab, taskTable.transform, false);
        taskToTableEntry.Add(task, tableEntry);

        var taskTableEntry = tableEntry.GetComponent<TaskTableEntry>();

        taskTableEntry.Priority.text = task.TaskDataUnloading.Priority.ToString();
        taskTableEntry.Description.text = task.TaskDataUnloading.Description;
        taskTableEntry.Progress.text = $"{task.CurrentTaskGoalAmount}/{task.TaskDataUnloading.RequiredAmount}";
    }

    private void RemoveTask(Task task)
    {
        var tableEntry = taskToTableEntry[task];
        taskToTableEntry.Remove(task);
        Destroy(tableEntry);
    }

    private void UpdateTaskProgression(Task task)
    {
        var tableEntry = taskToTableEntry[task];
        tableEntry.GetComponent<TaskTableEntry>().Progress.text = $"{task.CurrentTaskGoalAmount}/{task.TaskDataUnloading.RequiredAmount}";
    }
}
