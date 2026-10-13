
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    public float stoppingDistance = 2f;

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
        if (player == null)
            return;

        Vector3 direction = player.position - transform.position;

        // Keep enemy movement horizontal
        direction.y = 0f;

        float distance = direction.magnitude;

        if (distance <= stoppingDistance)
            return;

        // Rotate toward the player
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // Move toward the player
        transform.position +=
            direction.normalized * moveSpeed * Time.deltaTime;
    }
}