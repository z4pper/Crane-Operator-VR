using UnityEngine;

public abstract class InGameTask
{
    public TaskDataBaseSO TaskData { get; protected set; }
    public float CurrentTaskGoalAmount { get; protected set; }
    public int RequiredTaskGoalAmount { get; protected set; }

    public Color OutlineColor { get; protected set; }
    public StockZone StockZone { get; set; }
    public abstract void IncreaseCurrentAmount();
    public abstract void StartTask();
    protected abstract void FinishTask();
}