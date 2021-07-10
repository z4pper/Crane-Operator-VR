using System.Collections.Generic;
using UnityEngine;

public class MonitorScreenController : MonoBehaviour
{
    [SerializeField] private List<GameObject> monitorScreensInOrder = new List<GameObject>();

    private int _currentIndex;

    private void Start()
    {
        monitorScreensInOrder.ForEach(screen => screen.SetActive(false));
        _currentIndex = 0;
        monitorScreensInOrder[_currentIndex].SetActive(true);
    }

    public void NextScreen()
    {
        monitorScreensInOrder[_currentIndex].SetActive(false);
        
        _currentIndex = (_currentIndex + 1) % monitorScreensInOrder.Count;
        
        monitorScreensInOrder[_currentIndex].SetActive(true);
    }
}
