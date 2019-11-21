using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    GameObject blackHolePrefab;
    [SerializeField]
    GameObject missilePrefab;
    [SerializeField]
    GameObject shieldPrefab;
    [SerializeField]
    GameObject fuelPrefab;
    [SerializeField]
    float leadDistance = 15;
    [SerializeField]
    Vector2 yRange = new Vector2(-5,5);
    [SerializeField]
    float minSpacing = 2;
    [SerializeField]
    float StartingSpacing = 10;
    [SerializeField]
    float spacingDiff = 0.2f;
    Transform playerTransform;
    float playerXpos;
    float lastSpawnPos;
    [SerializeField]
    float currentSpacing;
    private void Start() 
    {
        playerTransform = GameObject.Find("Player").transform;
        playerXpos = playerTransform.position.x;
        currentSpacing = StartingSpacing;
        SpawnBlackHole();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform!=null)
        {
            playerXpos = playerTransform.position.x;
            if(playerXpos > lastSpawnPos+currentSpacing)
            {
                SpawnObject();
            }
        }
        
    }

    void SpawnBlackHole()
    {
        Vector3 blackHolePosition = new Vector3 (playerXpos+leadDistance,Random.Range(yRange.x,yRange.y),-1);
        GameObject newBlackHole = GameObject.Instantiate(blackHolePrefab);
        newBlackHole.transform.position = blackHolePosition;
    }

    void SpawnMissile()
    {
        Vector3 blackHolePosition = new Vector3 (playerXpos+leadDistance,Random.Range(yRange.x,yRange.y),-1);
        GameObject newBlackHole = GameObject.Instantiate(missilePrefab);
        newBlackHole.transform.position = blackHolePosition;
    }

    void SpawnShield()
    {
        Vector3 blackHolePosition = new Vector3 (playerXpos+leadDistance,Random.Range(yRange.x,yRange.y),-1);
        GameObject newBlackHole = GameObject.Instantiate(shieldPrefab);
        newBlackHole.transform.position = blackHolePosition;
    }

    void SpawnFuel()
    {
        Vector3 blackHolePosition = new Vector3 (playerXpos+leadDistance,Random.Range(yRange.x,yRange.y),-1);
        GameObject newBlackHole = GameObject.Instantiate(fuelPrefab);
        newBlackHole.transform.position = blackHolePosition;
    }

    void SpawnObject()
    {
        //When there are multiple objects to spawn, this method will randomise the process.
        float randNumber = Random.Range(0f,10f);
        if (randNumber < 2)
        {
            SpawnBlackHole();
        }
        else if (randNumber >= 9)
        {
            SpawnShield();
        }
        else if (randNumber > 8)
        {
            SpawnFuel();
        }
        else 
        {
            SpawnMissile();
        }
        
        lastSpawnPos = playerXpos;
        if (currentSpacing>minSpacing)
        {
            currentSpacing -= spacingDiff;
        }
        else
        {
            currentSpacing = minSpacing;
        }
    }
}
