using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int amountEnemy;
    [SerializeField] private EnemySpawner spawner;

    private void Awake()
    {
        StartGame();
    }

    public void StartGame()
    {
        StartCoroutine(SpawnEnemy());
        spawner.SetAmountEnemy(amountEnemy);
    }

    private IEnumerator SpawnEnemy()
    {
        for (int i = 0; i < amountEnemy; i++)
        {
            yield return new WaitForSeconds(2f);
            spawner.SpawnEnemy();
        }
    }
}