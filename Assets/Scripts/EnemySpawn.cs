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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SpawnEnemy(bean, vegSpawn.position);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnEnemy(hotdog, meatSpawn.position);
        }
    }

    private void SpawnEnemy(GameObject enemyToSpawn, Vector3 positionToSpawn)
    {
        GameObject newEnemy = Instantiate(enemyToSpawn, positionToSpawn, Quaternion.identity);
    }

    public void SpawnTest(Transform spawn)
    {
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

    public void AutoSpawn()
    {
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
        InvokeRepeating("AutoSpawn", 1.0f, 3.0f);
    }

    public void EndAutoSpawn()
    {
        CancelInvoke();
    }
}
