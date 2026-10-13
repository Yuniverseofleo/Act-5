using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject missilePrefab;
    public Camera mainCamera;

    public float spawnInterval = 3f; // Time between spawn waves
    private float spawnTimer = 0f;
    
    private float survivalTimer = 0f;
    private int missileSpawnCount = 1; // Starts with 1 missile per wave

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;
        survivalTimer += deltaTime;
        spawnTimer += deltaTime;

        // Scaling Difficulty: Increase missile count every 10 seconds survived
        // e.g., 0-10s = 1 missile, 10-20s = 2 missiles, 20-30s = 3 missiles, etc.
        missileSpawnCount = 1 + Mathf.FloorToInt(survivalTimer / 10f);

        // Spawn wave at interval
        if (spawnTimer >= spawnInterval)
        {
            SpawnMissileWave(missileSpawnCount);
            spawnTimer = 0f;
        }
    }

    void SpawnMissileWave(int count)
    {
        if (missilePrefab == null || mainCamera == null) return;

        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPos = GetOffscreenPosition();
            Instantiate(missilePrefab, spawnPos, Quaternion.identity);
        }
        
        Debug.Log(spawnImageSpawnInfo(count));
    }

    string spawnImageSpawnInfo(int count) => $"Spawned wave of {count} missile(s) outside camera view. Total Survival Time: {Mathf.Floor(survivalTimer)}s";

    Vector3 GetOffscreenPosition()
    {
        // Generates a random spawn position strictly outside the camera's viewport view
        float randomSide = Random.value;
        Vector3 viewportPos = Vector3.zero;

        // Choose which edge of the screen to spawn outside of
        if (randomSide < 0.25f) // Top
            viewportPos = new Vector3(Random.Range(0f, 1f), 1.2f, Random.Range(5f, 15f));
        else if (randomSide < 0.5f) // Bottom
            viewportPos = new Vector3(Random.Range(0f, 1f), -0.2f, Random.Range(5f, 15f));
        else if (randomSide < 0.75f) // Left
            viewportPos = new Vector3(-0.2f, Random.Range(0f, 1f), Random.Range(5f, 15f));
        else // Right
            viewportPos = new Vector3(1.2f, Random.Range(0f, 1f), Random.Range(5f, 15f));

        // Convert viewport coordinates to world coordinates relative to the player/camera
        Vector3 worldPos = mainCamera.ViewportToWorldPoint(viewportPos);
        return worldPos;
    }
}