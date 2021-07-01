using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private List<TaskDataBaseSO> possibleTasks;
    [SerializeField] private TaskEventChannelSO taskCreatedEventChannel;
    [SerializeField] private TaskEventChannelSO taskCompletedEventChannel;

    [SerializeField] private int taskLimit;
    [SerializeField] private float minTimeToSpawnNewTaskSeconds;
    [SerializeField] private float maxTimeToSpawnNewTaskSeconds;

    private float _timeToSpawnNextTask;
    private float _elapsedTime;

    private Dictionary<InGameTask, TaskDataBaseSO> _currentTasksToTaskData = new Dictionary<InGameTask, TaskDataBaseSO>();
    private List<TaskDataBaseSO> _potentialTasks = new List<TaskDataBaseSO>();

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
        _timeToSpawnNextTask = Random.Range(minTimeToSpawnNewTaskSeconds, maxTimeToSpawnNewTaskSeconds);
        _potentialTasks = possibleTasks;
        Debug.Log("Start: " + _potentialTasks.Count);
    }

    private void Update()
    {
        if (_currentTasksToTaskData.Count >= taskLimit || _potentialTasks.Count == 0) return;
        
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= _timeToSpawnNextTask)
        {
            _elapsedTime = 0f;
            _timeToSpawnNextTask = Random.Range(minTimeToSpawnNewTaskSeconds, maxTimeToSpawnNewTaskSeconds);
            
            CreateTask();
        }
    }

    private void CreateTask()
    {
        var taskData = _potentialTasks[Random.Range(0, _potentialTasks.Count)];
        switch (taskData) 
        {
            case TaskDataUnloadingSO unloadingTaskData:
            {
                var newTask = new UnloadingInGameTask(unloadingTaskData);
                _currentTasksToTaskData.Add(newTask, unloadingTaskData);
                newTask.StartTask();
                taskCreatedEventChannel.RaiseEvent(newTask);
                break;
            }
            case TaskDataLoadingSO loadingTaskData:
            {
                var newTask = new LoadingInGameTask(loadingTaskData);
                _currentTasksToTaskData.Add(newTask, loadingTaskData);
                newTask.StartTask();
                taskCreatedEventChannel.RaiseEvent(newTask);
                break;
            }
        }

        _potentialTasks.RemoveAll(data => data.GetType() == taskData.GetType());
        Debug.Log("Task created: " + _potentialTasks.Count);
    }

    private void RemoveTask(InGameTask inGameTask)
    {
        var tasksToAppend = possibleTasks.FindAll(data => data.GetType() == inGameTask.TaskData.GetType()).ToList();
        _potentialTasks.AddRange(tasksToAppend);
        _currentTasksToTaskData.Remove(inGameTask);
        Debug.Log("Task finished: " + _potentialTasks.Count);
    }
}
