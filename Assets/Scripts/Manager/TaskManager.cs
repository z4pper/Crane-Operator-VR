using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private List<TaskDataBaseSO> possibleTasks;
    [SerializeField] private TaskEventChannelSO taskCreatedEventChannel;
    [SerializeField] private TaskEventChannelSO taskCompletedEventChannel;

    private List<Task> _currentTasks = new List<Task>();

    private void OnEnable()
    {
        taskCompletedEventChannel.OnEventRaised += RemoveTask;
    }

    private void OnDisable()
    {
        taskCompletedEventChannel.OnEventRaised -= RemoveTask;
    }

    private void Start()
    {
        CreateTask();
    }

    private void CreateTask()
    {
        var taskData = possibleTasks[Random.Range(0, possibleTasks.Count)];
        switch (taskData) 
        {
            case TaskDataUnloadingSO unloadingTaskData:
            {
                var newTask = new UnloadingTask(unloadingTaskData);
                _currentTasks.Add(newTask);
                taskCreatedEventChannel.RaiseEvent(newTask);
                newTask.StartTask();
                break;
            }
        }
    }

    private void RemoveTask(Task task)
    {
        _currentTasks.Remove(task);
    }
}
