using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpawn : MonoBehaviour
{
    //Singleton
    public static EnemySpawn Instance { get; set; }

    //Enemy prefabs
    public GameObject bean;
    public GameObject eggplant;
    public GameObject corn;
    public GameObject hotdog;
    public GameObject chicken;
    public GameObject meatball;

    //Spawn positions
    public Transform vegSpawn;
    public Transform meatSpawn;
    public Transform bothSpawn;
    public Transform[] airSpawns;

    //For testing only
    private Transform[] spawns;
    private int currentSpawnIndex = -1;

    //Level
    public TMP_Text levelText;
    public float level = 0;

    private bool isSpawning= false;

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
    }

    //Instantly spawn enemy - is used by an on-screen button for testing
    private void SpawnEnemy(GameObject enemyToSpawn, Vector3 positionToSpawn, int flip)
    {
        GameObject newEnemy = Instantiate(enemyToSpawn, positionToSpawn, Quaternion.identity);
        newEnemy.GetComponent<EnemyScript>().SetSpeed(flip);
    }

    private void SpawnAirEnemy(GameObject enemyToSpawn, Vector3 positionToSpawn, int flip)
    {
        GameObject newEnemy = Instantiate(enemyToSpawn, positionToSpawn, Quaternion.identity);
        newEnemy.GetComponent<EnemyFlyingScript>().SetSpeed(flip);
    }

    private void Start()
    {
        //StartCoroutine(SpawnWave(bean, eggplant, vegSpawn.position, -1));
        //StartCoroutine(SpawnWave(hotdog, chicken, meatSpawn.position, 1));
        //StartCoroutine(SpawnMid());
        //StartCoroutine(SpawnAir());
    }

    //Spawn a wave of only tier 1 enemy (0 = bean, 1 = hotdog)
    public void Tier1(int type, float waitTime)
    {
        if (type == 0)
        {
            StartCoroutine(SpawnWaveTier1(bean, vegSpawn.position, -1, waitTime));
        }
        else if (type == 1)
        {
            StartCoroutine(SpawnWaveTier1(hotdog, meatSpawn.position, 1, waitTime));
        }
    }

    //Spawn a wave that can have both tier 1 and tier 2 enemies (0 = veg, 1 = meat)
    public void BothTier(int type, float waitTime)
    {
        if (type == 0)
        {
            StartCoroutine(SpawnWave(bean, eggplant, vegSpawn.position, -1, waitTime));
        }
        else if (type == 1)
        {
            StartCoroutine(SpawnWave(hotdog, chicken, meatSpawn.position, 1, waitTime));
        }
    }

    //Spawn a wave in the middle spawn (should have different timing from the normal spawns)
    public void Mid(float waitTime)
    {
        StartCoroutine(SpawnMid(waitTime));
    }

    //Spawn a wave of flying enemies
    public void Flying(float waitTime)
    {
        StartCoroutine(SpawnAir(waitTime));
    }

    IEnumerator SpawnWaveTier1(GameObject tier1, Vector3 spawnPos, int flip, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        for (int i = 0; i < 3; i++)
        {
            SpawnEnemy(tier1, spawnPos, flip);

            yield return new WaitForSeconds(1f);
        }

        if (tier1 == bean)
        {
            GameStateScript.Instance.canSpawnTier1_1 = true;
        }
        else
        {
            GameStateScript.Instance.canSpawnTier1_2 = true;
        }
    }

    IEnumerator SpawnWave(GameObject tier1, GameObject tier2, Vector3 spawnPos, int flip, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        for (int i = 0; i < 3; i++)
        {
            int rnd = Random.Range(0, 100);
            if (rnd < 70)
            {
                SpawnEnemy(tier1, spawnPos, flip);
            }
            else
            {
                SpawnEnemy(tier2, spawnPos, flip);
            }

            yield return new WaitForSeconds(1f);
        }

        if (tier1 == bean)
        {
            GameStateScript.Instance.canSpawnTier2_1 = true;
        }
        else
        {
            GameStateScript.Instance.canSpawnTier2_2 = true;
        }
    }

    IEnumerator SpawnMid(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        for (int i = 0; i < 4; i++)
        {
            int rndTier = Random.Range(0, 100);
            int rndType = Random.Range(0, 100);

            if (rndTier < 70)
            {
                if (rndType < 50)
                {
                    SpawnEnemy(bean, bothSpawn.position, -1);
                }
                else
                {
                    SpawnEnemy(hotdog, bothSpawn.position, 1);
                }
            }
            else
            {
                if (rndType < 50)
                {
                    SpawnEnemy(eggplant, bothSpawn.position, -1);
                }
                else
                {
                    SpawnEnemy(chicken, bothSpawn.position, 1);
                }
            }

            yield return new WaitForSeconds(1f);
        }

        GameStateScript.Instance.canSpawnMid = true;
    }

    IEnumerator SpawnAir(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        int rnd1 = Random.Range(0, airSpawns.Length);

        SpawnAirEnemy(corn, airSpawns[rnd1].position, 1);

        yield return new WaitForSeconds(1f);

        int rnd2 = Random.Range(0, airSpawns.Length);

        SpawnAirEnemy(meatball, airSpawns[rnd2].position, 1);

        yield return new WaitForSeconds(1f);

        GameStateScript.Instance.canSpawnAir = true;
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

        Transform spawnPosition = spawns[currentSpawnIndex];

        SpawnTest(spawnPosition);
    }

    public void ActiveAutoSpawn()
    {
        if (!isSpawning)
        {
            InvokeRepeating("AutoSpawn", 1.0f, 3.0f - level); //Active 1 second after clicked, is called every 3 seconds
            isSpawning = true;
        }
    }

    public void EndAutoSpawn()
    {
        CancelInvoke();

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Meat");
        for (int i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i]);
        }

        isSpawning = false;
    }

    public void StopSpawning()
    {
        StopAllCoroutines();
    }

    public void ChangeLevel()
    {
        if (level < 2)
        {
            level++;
        }
        else
        {
            level = 0;
        }
        levelText.text = level.ToString();
    }
}
