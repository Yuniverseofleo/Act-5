using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject missilePrefab;
    public Camera mainCamera;

    public float spawnInterval = 3f;
    private float spawnTimer = 0f;

    private float survivalTimer = 0f;
    private int missileSpawnCount = 1;

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

        missileSpawnCount = 1 + Mathf.FloorToInt(survivalTimer / 10f);

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

        Debug.Log($"Spawned wave of {count} missile(s) outside camera view. Total Survival Time: {Mathf.Floor(survivalTimer)}s");
    }

    Vector3 GetOffscreenPosition()
    {
        float randomSide = Random.value;

        // FIX: Changed from Vector2.zero to Vector3.zero
        Vector3 viewportPos = Vector3.zero;

        float zDepth = -mainCamera.transform.position.z;

        if (randomSide < 0.25f)
            viewportPos = new Vector3(Random.Range(0f, 1f), 1.2f, zDepth);
        else if (randomSide < 0.5f)
            viewportPos = new Vector3(Random.Range(0f, 1f), -0.2f, zDepth);
        else if (randomSide < 0.75f)
            viewportPos = new Vector3(-0.2f, Random.Range(0f, 1f), zDepth);
        else
            viewportPos = new Vector3(1.2f, Random.Range(0f, 1f), zDepth);

        Vector3 worldPos = mainCamera.ViewportToWorldPoint(viewportPos);
        worldPos.z = 0f;
        return worldPos;
    }
}