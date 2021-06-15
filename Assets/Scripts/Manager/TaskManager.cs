using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private List<TaskDataBaseSO> possibleTasks;
    [SerializeField] private TaskEventChannelSO taskCreatedEventChannel;
    [SerializeField] private TaskEventChannelSO taskCompletedEventChannel;

    private Dictionary<TaskDataBaseSO, TaskGoal> taskDataToTaskGoal = new Dictionary<TaskDataBaseSO, TaskGoal>();

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
        switch (taskData.TaskType) 
        {
            case TaskType.Unload:
            {
                var newTask = new UnloadingTask(taskData);
                taskDataToTaskGoal.Add(taskData, newTask);
                taskCreatedEventChannel.RaiseEvent(taskData);
                newTask.StartTask();
                break;
            }
        }
    }

    private void RemoveTask(TaskDataBaseSO taskDataBase)
    {
        taskDataToTaskGoal.Remove(taskDataBase);
    }
}
