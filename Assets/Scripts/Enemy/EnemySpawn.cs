using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    //Enemy prefabs
    public GameObject bean;
    public GameObject hotdog;

    //Spawn positions
    public Transform vegSpawn;
    public Transform meatSpawn;
    public Transform[] spawns;

    //Instantly spawn enemy - is used by an on-screen button for testing
    private void SpawnEnemy(GameObject enemyToSpawn, Vector3 positionToSpawn)
    {
        GameObject newEnemy = Instantiate(enemyToSpawn, positionToSpawn, Quaternion.identity);
    }

    //Supplement function for auto spawning enemies
    public void SpawnTest(Transform spawn)
    {
        //Determine the way the enemy is facing randomly
        int rnd = Random.Range(0, 99);
        int flip;
        if (rnd < 50)
        {
            flip = -1;
        }
        else
        {
            flip = 1;
        }

        GameObject newEnemy = Instantiate(hotdog, spawn.position, Quaternion.identity);
        newEnemy.GetComponent<EnemyScript>().SetSpeed(flip);
    }

    //Spawn enemies automatically - is used by an on-screen button for testing
    public void AutoSpawn()
    {
        //Determine the spawn position randomly
        int rnd = Random.Range(0, 79);
        Transform spawnPosition = gameObject.transform;
        if (rnd < 10)
        {
            spawnPosition = spawns[0];
        }
        else if (rnd < 20)
        {
            spawnPosition = spawns[1];
        }
        else if (rnd < 30)
        {
            spawnPosition = spawns[2];
        }
        else if (rnd < 40)
        {
            spawnPosition = spawns[3];
        }
        else if (rnd < 50)
        {
            spawnPosition = spawns[4];
        }
        else if (rnd < 60)
        {
            spawnPosition = spawns[5];
        }
        else if (rnd < 70)
        {
            spawnPosition = spawns[6];
        }
        else if (rnd < 80)
        {
            spawnPosition = spawns[7];
        }
        SpawnTest(spawnPosition);
    }

    public void ActiveAutoSpawn()
    {
        InvokeRepeating("AutoSpawn", 1.0f, 3.0f); //Active 1 second after clicked, is called every 3 seconds
    }

    public void EndAutoSpawn()
    {
        CancelInvoke();
    }
}
