using UnityEngine;

public class CraneCablePlate : MonoBehaviour
{
    [SerializeField] private Transform cablePlate;
    [SerializeField] private float cablePlateMoveSpeed;
    [SerializeField] private float maxCablePlatePosZ;
    [SerializeField] private float minCablePlatePosZ;
    
    public void MoveCablePlate(float direction)
    {
        if (direction == 0) return;

        var pos = cablePlate.localPosition;
        pos.z -= direction * cablePlateMoveSpeed * Time.deltaTime;

        if (pos.z > maxCablePlatePosZ) pos.z = maxCablePlatePosZ;
        if (pos.z < minCablePlatePosZ) pos.z = minCablePlatePosZ;

        cablePlate.localPosition = pos;
    }
}
