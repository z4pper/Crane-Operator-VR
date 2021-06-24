
public abstract class InGameTask
{
    public TaskDataBaseSO TaskData { get; protected set; }
    public float CurrentTaskGoalAmount { get; protected set; }
    public abstract void IncreaseCurrentAmount();
    public abstract void StartTask();
}