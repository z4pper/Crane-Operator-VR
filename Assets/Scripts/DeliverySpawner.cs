using UnityEngine;
using UnityEngine.AI;

public class DeliverySpawner : MonoBehaviour
{
    [SerializeField] private GameObject containerDeliveryPrefab;
    [SerializeField] private Transform deliverySpawnPosition;
    [SerializeField] private Transform deliveryTargetPosition;

    private NavMeshAgent currentAgent;

    private void Start()
    {
        SpawnDelivery();
    }

    private void SpawnDelivery()
    {

    }
}
