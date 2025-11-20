using System.Collections;
using UnityEngine;

public class MobileSpawner : MonoBehaviour
{
    public GameObject bubblePrefab;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));

            // 1. Get the screen width automatically
            float screenHeight = Camera.main.orthographicSize;
            float screenWidth = screenHeight * Camera.main.aspect;

            // 2. Spawn within the visible width (padding of 0.5 so they don't spawn ON the edge)
            float randomX = Random.Range(-screenWidth + 0.5f, screenWidth - 0.5f);

            // 3. Spawn at the bottom of the screen
            Vector3 spawnPos = new Vector3(randomX, -screenHeight - 1f, 0);

            Instantiate(bubblePrefab, spawnPos, Quaternion.identity);
        }
    }
}