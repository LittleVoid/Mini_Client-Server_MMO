using Unity.Netcode;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 2f, 6f);
    }

    private void SpawnEnemy()
    {
        Enemy instantiatedEnemy = Instantiate(enemy);
        instantiatedEnemy.GetComponent<NetworkObject>().Spawn();
    }
}
