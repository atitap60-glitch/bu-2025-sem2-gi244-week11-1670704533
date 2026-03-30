using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    private Rigidbody rb;
    private GameObject player;

    private bool isStunned = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    void Start()
    {

    }

    void Update()
    {
        if (isStunned) return;

        // Fixed: was transform.forward, should be transform.position
        Vector3 dir = player.transform.position - transform.position;
        dir.Normalize();
        rb.AddForce(dir * speed);
    }

    public void Stun()
    {
        isStunned = true;
        rb.linearVelocity = Vector3.zero;
    }

    public void Unstun()
    {
        isStunned = false;
    }
}