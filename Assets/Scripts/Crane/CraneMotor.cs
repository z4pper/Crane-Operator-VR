using System.Collections;
using UnityEngine;

public class CraneMotor : MonoBehaviour
{
    [SerializeField] private BooleanSO isCraneMotorStarted;
    [SerializeField] private AudioSource craneMotorAudioSource;
    [SerializeField] private AudioClip craneMotorStartingSound;
    [SerializeField] private AudioClip craneMotorShuttingDownSound;
    [SerializeField] private AudioClip craneMotorRunningSound;

    private Coroutine _craneMotorRunningSoundRoutine;
    
    public void ToggleCraneMotor()
    {
        if (isCraneMotorStarted.Value)
        {
            isCraneMotorStarted.Value = false;
            craneMotorAudioSource.loop = false;
            
            if (_craneMotorRunningSoundRoutine != null)
            {
                StopCoroutine(_craneMotorRunningSoundRoutine);
            }
            
            craneMotorAudioSource.Stop();
            craneMotorAudioSource.PlayOneShot(craneMotorShuttingDownSound);
        }
        else
        {
            isCraneMotorStarted.Value = true;
            craneMotorAudioSource.PlayOneShot(craneMotorStartingSound);
            _craneMotorRunningSoundRoutine = StartCoroutine(StartCraneMotorIdleSound());
        }
    }

    private IEnumerator StartCraneMotorIdleSound()
    {
        var elapsed = 0f;
        var clipLength = craneMotorStartingSound.length;
        while (clipLength > elapsed)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        craneMotorAudioSource.loop = true;
        craneMotorAudioSource.clip = craneMotorRunningSound;
        craneMotorAudioSource.Play();
    }
}
