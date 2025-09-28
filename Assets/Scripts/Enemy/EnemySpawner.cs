using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Настройки спауна")] [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private float minDistanceToPlayer = 3f;
    [SerializeField] private Transform player;
    [SerializeField] private int maxTries = 30;
    

    public void SpawnEnemy()
    {
        if (enemyPrefab == null || player == null) return;

        Vector2 spawnPosition = GetValidSpawnPosition();

        var enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemy.GetComponent<EnemyStateMachine>().Initialize(player);
    }

    private Vector2 GetValidSpawnPosition()
    {
        for (int i = 0; i < maxTries; i++)
        {
            Vector2 randomPos = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
            if (Vector2.Distance(randomPos, player.position) >= minDistanceToPlayer)
            {
                return randomPos;
            }
        }

        return (Vector2)transform.position + (Random.insideUnitCircle.normalized * spawnRadius);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(player.position, minDistanceToPlayer);
        }
    }
}