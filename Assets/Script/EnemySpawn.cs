
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;
    public Transform spawnPoint;

    public float spawnDistance = 15f;
    public float spawnInterval = 3f;
    public int maxEnemies = 10;
    public float enemyMoveSpeed = 3f;

    private float spawnTimer;

    void Start()
    {
        if (player == null)
        {
            PlayerMovement playerScript =
                FindAnyObjectByType<PlayerMovement>();

            if (playerScript != null)
                player = playerScript.transform;
        }

        if (enemyPrefab == null)
            Debug.LogError("Assign an Enemy Prefab in the Inspector!", this);
    }

    void Update()
    {
        if (player == null || enemyPrefab == null)
            return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        EnemyFollower[] enemies =
            FindObjectsByType<EnemyFollower>();

        if (enemies.Length >= maxEnemies)
            return;

        Vector2 spawnPosition;

        if (spawnPoint != null)
        {
            spawnPosition = spawnPoint.position;
        }
        else
        {
            Vector2 randomDirection =
                Random.insideUnitCircle.normalized;

            if (randomDirection.sqrMagnitude < 0.01f)
                randomDirection = Vector2.right;

            spawnPosition =
                (Vector2)player.position
                + randomDirection * spawnDistance;
        }

        GameObject enemy = Instantiate(
            enemyPrefab,
            spawnPosition,
            Quaternion.identity
        );

        EnemyFollower follower =
            enemy.GetComponent<EnemyFollower>();

        if (follower == null)
            follower = enemy.AddComponent<EnemyFollower>();

        follower.player = player;
        follower.moveSpeed = enemyMoveSpeed;
    }
}

public class EnemyFollower : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;

    void Update()
    {
        if (player == null)
            return;

        Vector2 direction =
            (Vector2)player.position -
            (Vector2)transform.position;

        if (direction.sqrMagnitude > 0.01f)
        {
            direction.Normalize();

            transform.position +=
                (Vector3)(direction * moveSpeed * Time.deltaTime);
        }
    }
}