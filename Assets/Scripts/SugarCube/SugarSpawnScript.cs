using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SugarSpawnScript : MonoBehaviour
{
    //Singleton
    public static SugarSpawnScript Instance { get; set; }

    //Sugar cube prefab
    public GameObject sugarCube;

    //Spawn positions
    public Transform[] spawns;
    private int currentSpawnIndex = -1;

    private void Awake()
    {
        //Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //Spawn the first cube
        SpawnCube();
    }

    //Spawn a cube at a random spawn position
    public void SpawnCube()
    {
        //Determine the spawn position randomly
        while (true)
        {
            int rnd = Random.Range(0, spawns.Length);
            
            //Avoid repeating position
            if (rnd != currentSpawnIndex)
            {
                currentSpawnIndex = rnd;
                break;
            }
        }

        Vector3 spawnPosition = spawns[currentSpawnIndex].position;

        GameObject newCube = Instantiate(sugarCube, spawnPosition, Quaternion.identity);
    }
}
