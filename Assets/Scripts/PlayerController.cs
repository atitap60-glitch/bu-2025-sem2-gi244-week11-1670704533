using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform FocalPoint;
    public float speed = 5f;

    private Rigidbody rb;

    private InputAction moveAction;
    private InputAction smashAction;
    private InputAction breakAction;

    public bool hasPowerUp = false;

    public GameObject powerUpIndicator;

    private Coroutine countDownRoutine;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
        smashAction = InputSystem.actions.FindAction("Smash");
        breakAction = InputSystem.actions.FindAction("Break");
    }

    void Start()
    {
        // Hide indicator at start
        if (powerUpIndicator != null)
        {
            powerUpIndicator.SetActive(false);
        }
    }

    void Update()
    {
        var move = moveAction.ReadValue<Vector2>();
        rb.AddForce(move.y * speed * FocalPoint.forward);

        if (breakAction.IsPressed())
        {
            rb.linearVelocity = Vector3.zero;
        }

        // Make indicator follow player position
        if (powerUpIndicator != null && powerUpIndicator.activeSelf)
        {
            powerUpIndicator.transform.position = transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            hasPowerUp = true;
            Destroy(other.gameObject);

            // Show indicator
            if (powerUpIndicator != null)
            {
                powerUpIndicator.SetActive(true);
            }

            if (countDownRoutine != null)
            {
                StopCoroutine(countDownRoutine);
            }
            countDownRoutine = StartCoroutine(powerCoolDown());
        }
    }

    IEnumerator powerCoolDown()
    {
        yield return new WaitForSeconds(10);
        hasPowerUp = false;

        // Hide indicator
        if (powerUpIndicator != null)
        {
            powerUpIndicator.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (hasPowerUp)
            {
                var rb = collision.gameObject.GetComponent<Rigidbody>();
                var dir = collision.transform.position - transform.position;
                rb.AddForce(100 * dir.normalized, ForceMode.Impulse);
            }
        }
    }
}