using System;
using UnityEngine;

[CreateAssetMenu(fileName = "new Void Event Channel", menuName = "Channel/Void")]
public class VoidEventChannelSO : ScriptableObject
{
    public event Action OnEventRaised;

    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }
}
