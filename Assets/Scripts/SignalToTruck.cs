using UnityEngine;

public class SignalToTruck : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO signalToTruckEventChannel;
    [SerializeField] private AudioSource audioSource;
    
    public void SignalTruck()
    {
        audioSource.PlayOneShot(audioSource.clip);
        signalToTruckEventChannel.RaiseEvent();
    }
}
