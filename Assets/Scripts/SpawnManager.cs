using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject enemyPrefab;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
        //InvokeRepeating(nameof(randomSpawn), 0, 5);
        //StartCoroutine(Helllo());
        //StartCoroutine(GoodBye());
    }
    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(5);
        while (true)
        {
            randomSpawn();
            yield return new WaitForSeconds(3);
        }
    }
    void randomSpawn()
    { 
        var index = Random.Range(0, spawnPoints.Length);
        var spawn = spawnPoints[index];
        Instantiate(enemyPrefab, spawn.position, Quaternion.identity);
    }
    IEnumerator GoodBye()
    {
        //yield return new WaitForSeconds(1);
        Debug.Log("Bye " + Time.frameCount + "" + Time.time);
        yield return null;
        //yield break;
        //StartCoroutine(Helllo());
        //yield return Helllo();
    }
    IEnumerator Helllo()
    {
        Debug.Log("Hellow " + Time.frameCount);
        yield return null;
    }
}
