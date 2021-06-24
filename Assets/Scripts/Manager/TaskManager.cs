using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private List<TaskDataBaseSO> possibleTasks;
    [SerializeField] private TaskEventChannelSO taskCreatedEventChannel;
    [SerializeField] private TaskEventChannelSO taskCompletedEventChannel;

    private List<InGameTask> _currentTasks = new List<InGameTask>();

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
        StartCoroutine(wait());
    }

    private void CreateTask()
    {
        var taskData = possibleTasks[Random.Range(0, possibleTasks.Count)];
        switch (taskData) 
        {
            case TaskDataUnloadingSO unloadingTaskData:
            {
                var newTask = new UnloadingInGameTask(unloadingTaskData);
                _currentTasks.Add(newTask);
                newTask.StartTask();
                taskCreatedEventChannel.RaiseEvent(newTask);
                break;
            }
            case TaskDataLoadingSO loadingTaskData:
            {
                var newTask = new LoadingInGameTask(loadingTaskData);
                _currentTasks.Add(newTask);
                newTask.StartTask();
                taskCreatedEventChannel.RaiseEvent(newTask);
                break;
            }
        }
    }

    private IEnumerator wait()
    {
        yield return new WaitForSeconds(1f);
        CreateTask();
    }

    private void RemoveTask(InGameTask inGameTask)
    {
        _currentTasks.Remove(inGameTask);
    }
}
