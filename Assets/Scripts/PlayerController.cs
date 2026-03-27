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

    private Coroutine countDownRoutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
        smashAction = InputSystem.actions.FindAction("Smash");
        breakAction = InputSystem.actions.FindAction("Break");
    }

    // Update is called once per frame
    void Update()
    {
        var  move = moveAction.ReadValue<Vector2>();
        rb.AddForce(move.y * speed * FocalPoint.forward );

        if (breakAction.IsPressed())
        {
            rb.linearVelocity = Vector3.zero;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("PowerUp"))
        {   hasPowerUp = true;
            Destroy(other.gameObject);
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
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Eenmy"))
        {
            if (!hasPowerUp == true)
            {
                var rb = collision.gameObject.GetComponent<Rigidbody>();
                var dir = collision.transform.position - transform.position;
                rb.AddForce(100 * dir.normalized, ForceMode.Impulse);
            }
        }
    }
    
}
