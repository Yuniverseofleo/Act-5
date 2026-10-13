using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    private Transform playerTransform;

    public float speed = 8f;
    public float rotationSpeed = 3f;
    public float lifetime = 5f;
    public float proximityDestroyDistance = 1.0f;

    [HideInInspector] public bool CanDealDamage = true;

    private float timer = 0f;

    void Start()
    {
        // Find the player using the updated Unity method
        PlayerMovement player = FindFirstObjectByType<PlayerMovement>();

        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        // Count down the missile's lifetime
        timer += Time.deltaTime;

        if (timer >= lifetime)
        {
            DestroySelf();
            return;
        }

        // Home in on the player
        if (playerTransform != null)
        {
            Vector3 directionToPlayer =
                (playerTransform.position - transform.position).normalized;

            if (directionToPlayer != Vector3.zero)
            {
                Quaternion targetRotation =
                    Quaternion.LookRotation(directionToPlayer);

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }

            // Destroy the missile if it gets too close
            if (Vector3.Distance(
                transform.position,
                playerTransform.position
            ) <= proximityDestroyDistance)
            {
                DestroySelf();
                return;
            }
        }

        // Move forward in the direction the missile faces
        transform.Translate(
            Vector3.forward * speed * Time.deltaTime,
            Space.Self
        );
    }

    public void DestroySelf()
    {
        CanDealDamage = false;
        Destroy(gameObject);
    }
}