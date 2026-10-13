using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 8f;
    public float reverseSpeed = 4f;
    public float turnSpeed = 200f;
    public float deceleration = 5f;

    [Header("Sprite Orientation")]
    public bool spriteFacesRight = false;

    [Header("Shooting Settings")]
    public GameObject missilePrefab;
    public Transform firePoint;

    private float currentSpeed = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        HandleMovement2D();
        HandleShooting2D();
    }

    void HandleMovement2D()
    {
        // W and S control turning
        float turnInput = Input.GetAxis("Vertical");

        transform.Rotate(
            0f,
            0f,
            -turnInput * turnSpeed * Time.deltaTime
        );

        // D moves forward, A moves backward
        float movementInput = Input.GetAxis("Horizontal");

        if (movementInput > 0f)
        {
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                forwardSpeed * movementInput,
                forwardSpeed * Time.deltaTime * 3f
            );
        }
        else if (movementInput < 0f)
        {
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                -reverseSpeed * Mathf.Abs(movementInput),
                forwardSpeed * Time.deltaTime * 3f
            );
        }
        else
        {
            // Slow down when no movement key is pressed
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                0f,
                deceleration * Time.deltaTime
            );
        }

        // Move in the direction the player is facing
        Vector3 moveDirection = spriteFacesRight
            ? transform.right
            : transform.up;

        transform.position +=
            moveDirection * currentSpeed * Time.deltaTime;
    }

    void HandleShooting2D()
    {
        // Press Space to shoot
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootMissile2D();
        }
    }

    void ShootMissile2D()
    {
        if (missilePrefab == null)
        {
            Debug.LogWarning(
                "Missile Prefab is missing in the Inspector!"
            );
            return;
        }

        Transform spawnPoint = firePoint != null
            ? firePoint
            : transform;

        Instantiate(
            missilePrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );
    }
}