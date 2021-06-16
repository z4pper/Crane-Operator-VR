
public abstract class Task
{
    public TaskDataUnloadingSO TaskDataUnloading { get; protected set; }
    public float CurrentTaskGoalAmount { get; protected set; }
    public abstract void IncreaseCurrentAmount();
    public abstract void StartTask();
}