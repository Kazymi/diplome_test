using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private EnemyConfiguration _enemyConfiguration;

    public void Damage()
    {
        FindAnyObjectByType<PlayerHealth>().TakeDamage(_enemyConfiguration.Damage);
    }
}