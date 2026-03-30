using System.Collections;
using UnityEngine;

public class StunPowerUp : MonoBehaviour
{
    public float stunDuration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            StartStun();
        }
    }

    private void StartStun()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemies)
        {
            enemy.Stun();
        }

        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null)
        {
            player.StartCoroutine(UnstunAfterDelay(stunDuration));
        }
    }

    private IEnumerator UnstunAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemies)
        {
            enemy.Unstun();
        }
    }
}