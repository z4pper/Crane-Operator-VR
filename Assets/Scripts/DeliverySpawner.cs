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
        var go = Instantiate(containerDeliveryPrefab);
        go.transform.position = deliverySpawnPosition.position;

        currentAgent = go.GetComponent<NavMeshAgent>();
        currentAgent.destination = deliveryTargetPosition.position;
    }

    private void Update()
    {
        if (currentAgent != null)
        {
            Debug.Log(currentAgent.remainingDistance);
        }
    }
}
