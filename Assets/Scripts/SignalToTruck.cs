using UnityEngine;

public class SignalToTruck : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO signalToTruckEventChannel;
    
    private AudioSource _audioSource;
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.Three))
        {
            Debug.Log("Test");
            _audioSource.PlayOneShot(_audioSource.clip);
            signalToTruckEventChannel.RaiseEvent();
        }
    }
}
