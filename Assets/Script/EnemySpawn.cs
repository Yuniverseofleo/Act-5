using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    public Transform player;

    [Header("Spawn Settings")]
    public float spawnInterval = 3f;
    public float spawnDistance = 25f;
    public float spawnHeight = 0f;
    public int maxEnemies = 10;

    private float spawnTimer = 0f;

    void Start()
    {
        // Find the player automatically
        if (player == null)
        {
            PlayerMovement playerScript =
                FindFirstObjectByType<PlayerMovement>();

            if (playerScript != null)
            {
                player = playerScript.transform;
            }
        }
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
        // Count existing enemies
        EnemyChase[] enemies =
            FindObjectsByType<EnemyChase>(
                FindObjectsSortMode.None
            );

        if (enemies.Length >= maxEnemies)
            return;

        // Pick a random direction around the player
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        if (randomDirection.sqrMagnitude < 0.01f)
            randomDirection = Vector2.right;

        Vector3 spawnPosition = player.position + new Vector3(
            randomDirection.x * spawnDistance,
            spawnHeight,
            randomDirection.y * spawnDistance
        );

        // Spawn the enemy
        GameObject newEnemy = Instantiate(
            enemyPrefab,
            spawnPosition,
            Quaternion.identity
        );

        // Assign the player as the target
        EnemyChase chase = newEnemy.GetComponent<EnemyChase>();

        if (chase != null)
        {
            chase.player = player;
        }
    }
}