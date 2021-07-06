using System;
using System.Collections.Generic;
using System.Linq;
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

    private Dictionary<InGameTask, GameObject> taskToTableEntry = new Dictionary<InGameTask, GameObject>();

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

    private void CreateTask(InGameTask inGameTask)
    {
        var tableEntry = Instantiate(taskTableEntryPrefab, taskTable.transform, false);
        taskToTableEntry.Add(inGameTask, tableEntry);

        OrderTableByPriority();

        var taskTableEntry = tableEntry.GetComponent<TaskTableEntry>();

        taskTableEntry.Priority.text = inGameTask.TaskData.Priority.ToString();
        
        taskTableEntry.Description.text = inGameTask.TaskData.Description;
        
        taskTableEntry.Description.text = taskTableEntry.Description.text.Replace(
            "{StockZone}",Enum.GetName(typeof(StockZone), inGameTask.StockZone));
        taskTableEntry.Description.text = taskTableEntry.Description.text.Replace(
            "COLORCODE",$"#{ColorUtility.ToHtmlStringRGBA(inGameTask.OutlineColor)}");
        
        taskTableEntry.Progress.text = $"{inGameTask.CurrentTaskGoalAmount}/{inGameTask.RequiredTaskGoalAmount}";
    }

    private void OrderTableByPriority()
    {
        var orderedTasks = taskToTableEntry.Keys.ToList().OrderBy(task => task.TaskData.Priority).ToList();
        for (var i = 0; i < orderedTasks.Count; i++)
        {
            var key = orderedTasks[i];
            taskToTableEntry[key].transform.SetSiblingIndex(i + 1);
        }
    }

    private void RemoveTask(InGameTask inGameTask)
    {
        var tableEntry = taskToTableEntry[inGameTask];
        taskToTableEntry.Remove(inGameTask);
        Destroy(tableEntry);
    }

    private void UpdateTaskProgression(InGameTask inGameTask)
    {
        var tableEntry = taskToTableEntry[inGameTask];
        tableEntry.GetComponent<TaskTableEntry>().Progress.text = $"{inGameTask.CurrentTaskGoalAmount}/{inGameTask.RequiredTaskGoalAmount}";
    }
}
