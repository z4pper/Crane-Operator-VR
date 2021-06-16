using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskUI : MonoBehaviour
{
    [SerializeField] private TaskEventChannelSO taskCreatedEventChannel;
    [SerializeField] private TaskEventChannelSO taskCompletedEventChannel;
    [SerializeField] private TaskEventChannelSO taskProgressionEventChannel;
    
    [SerializeField] private GameObject taskTable;
    [SerializeField] private GameObject taskTableEntry;
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
        var tableEntry = Instantiate(taskTableEntry, taskTable.transform, false);
        taskToTableEntry.Add(task, tableEntry);

        tableEntry.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = task.TaskDataUnloading.Priority.ToString();
        tableEntry.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = task.TaskDataUnloading.Description;
        tableEntry.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"{task.CurrentTaskGoalAmount}/{task.TaskDataUnloading.RequiredAmount}";
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
        tableEntry.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"{task.CurrentTaskGoalAmount}/{task.TaskDataUnloading.RequiredAmount}";
    }
}
