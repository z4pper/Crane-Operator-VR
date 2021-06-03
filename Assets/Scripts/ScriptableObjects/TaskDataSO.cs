using UnityEngine;

[CreateAssetMenu(fileName = "new Task Data", menuName = "Tasks/Data")]
public class TaskDataSO : ScriptableObject
{
    public int priority;
    public string description;
}
