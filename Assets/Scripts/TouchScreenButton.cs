using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class TouchScreenButton : MonoBehaviour
{
    [SerializeField] private UnityEvent OnTouchScreen;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnTouchScreen?.Invoke();
        }
    }
}
