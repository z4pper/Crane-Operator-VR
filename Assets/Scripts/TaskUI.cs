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

    private Dictionary<TaskDataBaseSO, GameObject> taskDataToTableEntry = new Dictionary<TaskDataBaseSO, GameObject>();

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

    private void CreateTask(TaskDataBaseSO taskDataBase)
    {
        var tableEntry = Instantiate(taskTableEntry, taskTable.transform, false);
        taskDataToTableEntry.Add(taskDataBase, tableEntry);

        tableEntry.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = taskDataBase.Priority.ToString();
        tableEntry.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = taskDataBase.Description;
        tableEntry.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"{taskDataBase.currentAmount}/{taskDataBase.RequiredAmount}";
    }

    private void RemoveTask(TaskDataBaseSO taskDataBaseSo)
    {
        var tableEntry = taskDataToTableEntry[taskDataBaseSo];
        taskDataToTableEntry.Remove(taskDataBaseSo);
        Destroy(tableEntry);
    }

    private void UpdateTaskProgression(TaskDataBaseSO taskDataBase)
    {
        var tableEntry = taskDataToTableEntry[taskDataBase];
        tableEntry.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"{taskDataBase.currentAmount}/{taskDataBase.RequiredAmount}";
    }
}
