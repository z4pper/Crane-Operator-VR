using UnityEngine;

public class CraneCablePlate : MonoBehaviour
{
    [SerializeField] private Transform cablePlate;
    [SerializeField] private float cablePlateMoveSpeed;
    [SerializeField] private float maxCablePlatePosZ;
    [SerializeField] private float minCablePlatePosZ;
    [SerializeField] private AudioSource cablePlateAudioSource;
    [SerializeField] private BooleanSO isCraneMotorStarted;


    private void Update()
    {
        if (!isCraneMotorStarted.Value && cablePlateAudioSource.isPlaying)
        {
            cablePlateAudioSource.Stop();
        }
    }

    public void MoveCablePlate(float direction)
    {
        if (direction == 0)
        {
            cablePlateAudioSource.Stop();
            return;
        }
        if (!cablePlateAudioSource.isPlaying)
        {
            cablePlateAudioSource.Play();
        }

        var pos = cablePlate.localPosition;
        pos.z -= direction * cablePlateMoveSpeed * Time.deltaTime;

        if (pos.z > maxCablePlatePosZ) pos.z = maxCablePlatePosZ;
        if (pos.z < minCablePlatePosZ) pos.z = minCablePlatePosZ;

        cablePlate.localPosition = pos;
    }
}
