using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 10f;
    public float turnSpeed = 90f;
    public float maxBankAngle = 30f;

    [Header("Rotation")]
    public Vector3 angles = new Vector3(30f, 45f, 0f);

    [Header("Lives")]
    public int maxLives = 5;
    public int currentLives;

    [Header("Missile Detection")]
    public float hitRadius = 1.5f;

    void Start()
    {
        currentLives = maxLives;

        // Apply initial rotation using Vector3 angles
        transform.rotation = Quaternion.Euler(angles);
    }

    void Update()
    {
        float horizontalInput = 0f;
        float verticalInput = 0f;

        // WASD controls
        if (Input.GetKey(KeyCode.A))
            horizontalInput = -1f;

        if (Input.GetKey(KeyCode.D))
            horizontalInput = 1f;

        if (Input.GetKey(KeyCode.W))
            verticalInput = 1f;

        if (Input.GetKey(KeyCode.S))
            verticalInput = -1f;

        // Forward and backward movement
        transform.Translate(
            Vector3.forward * verticalInput *
            forwardSpeed * Time.deltaTime,
            Space.Self
        );

        // Turn left and right on the Y axis
        float turnAmount =
            horizontalInput * turnSpeed * Time.deltaTime;

        transform.Rotate(Vector3.up, turnAmount, Space.World);

        // Bank while turning
        float currentYRotation = transform.eulerAngles.y;
        float currentXRotation = transform.eulerAngles.x;
        float targetBank = -horizontalInput * maxBankAngle;

        float currentZRotation = Mathf.LerpAngle(
            transform.eulerAngles.z,
            targetBank,
            Time.deltaTime * 5f
        );

        transform.rotation = Quaternion.Euler(
            currentXRotation,
            currentYRotation,
            currentZRotation
        );

        CheckMissileProximity();
    }

    void CheckMissileProximity()
    {
        HomingMissile[] activeMissiles =
            FindObjectsByType<HomingMissile>(
                FindObjectsSortMode.None
            );

        foreach (HomingMissile missile in activeMissiles)
        {
            if (missile != null && missile.CanDealDamage)
            {
                float distance = Vector3.Distance(
                    transform.position,
                    missile.transform.position
                );

                if (distance <= hitRadius)
                {
                    TakeHit();
                    missile.DestroySelf();
                    break;
                }
            }
        }
    }

    public void TakeHit()
    {
        if (currentLives <= 0)
            return;

        currentLives--;

        Debug.Log(
            $"Player hit! Lives remaining: {currentLives}"
        );

        if (currentLives <= 0)
        {
            RestartGame();
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }
}