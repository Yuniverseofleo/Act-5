using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public enum MissileType
    {
        PlayerMissile,
        EnemyHomingMissile
    }

    [Header("Type Settings")]
    public MissileType type = MissileType.EnemyHomingMissile;

    [Header("Movement Settings")]
    public float speed = 12f;
    public float rotationSpeed = 200f;
    public float lifetime = 5f;
    public float proximityDestroyDistance = 0.5f;

    [HideInInspector] public bool CanDealDamage = true;

    private Transform targetTransform;
    private float timer;

    void Start()
    {
        if (type == MissileType.EnemyHomingMissile)
        {
            PlayerMovement playerTarget =
                FindAnyObjectByType<PlayerMovement>();

            if (playerTarget != null)
                targetTransform = playerTarget.transform;
        }
        else if (type == MissileType.PlayerMissile)
        {
            EnemyFollower enemyTarget =
                FindAnyObjectByType<EnemyFollower>();

            if (enemyTarget != null)
                targetTransform = enemyTarget.transform;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= lifetime)
        {
            DestroySelf();
            return;
        }

        if (targetTransform != null)
        {
            Vector2 direction =
                (Vector2)targetTransform.position
                - (Vector2)transform.position;

            if (direction != Vector2.zero)
            {
                float targetAngle =
                    Mathf.Atan2(direction.y, direction.x)
                    * Mathf.Rad2Deg - 90f;

                Quaternion targetRotation =
                    Quaternion.Euler(0f, 0f, targetAngle);

                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }

            if (Vector2.Distance(
                transform.position,
                targetTransform.position
            ) <= proximityDestroyDistance)
            {
                DestroySelf();
                return;
            }
        }

        transform.Translate(
            Vector3.up * speed * Time.deltaTime,
            Space.Self
        );
    }

    public void DestroySelf()
    {
        CanDealDamage = false;
        Destroy(gameObject);
    }
}