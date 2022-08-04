using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectReferences : MonoBehaviour
{
    //Singleton
    public static EffectReferences Instance { get; set; }

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

    //Effects - Player
    public GameObject playerJump;
    public GameObject playerDie;
    public GameObject playerTransform;

    //Effects - Fridge
    public GameObject fridgeExplode;
    public GameObject[] fridgeHitEffects;

    //Effects - Weapons
    public GameObject[] hitEffects;

    //Effects - Enemy
    public GameObject enemyDie;
    public GameObject popcornDie;
    public GameObject meatballDie;
}
