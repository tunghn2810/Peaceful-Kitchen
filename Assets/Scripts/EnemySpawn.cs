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
}
