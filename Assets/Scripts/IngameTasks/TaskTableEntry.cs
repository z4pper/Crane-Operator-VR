using TMPro;
using UnityEngine;

public class TaskTableEntry : MonoBehaviour
{
    [field: SerializeField] public TextMeshProUGUI Priority { get; set; }
    [field: SerializeField] public TextMeshProUGUI Description { get; set; }
    [field: SerializeField] public TextMeshProUGUI Progress { get; set; }
}
