using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{

    public List<Enemy> enemies = new List<Enemy>();
    public int currWave;
    private int waveValue;
    public List<GameObject> enemiesToSpawn = new List<GameObject>();
    public GameObject Boss;

    public Transform[] spawnLocation;
    public int spawnIndex;

    public int waveDuration;
    private float waveTimer;
    private float spawnInterval = 0.5f;
    private float spawnTimer;

    public List<GameObject> spawnedEnemies = new List<GameObject>();

    public bool isBossWave = false;

    public static WaveSpawner instance;

    void Start()
    {
        instance = this;
    }

    void FixedUpdate()
    {
        if (GameController.instance.hasGameStarted)
        {
            if (spawnTimer <= 0)
            {
                if (enemiesToSpawn.Count > 0)
                {
                    GameObject enemy = (GameObject)Instantiate(enemiesToSpawn[0], spawnLocation[spawnIndex].position, Quaternion.identity);
                    enemiesToSpawn.RemoveAt(0);
                    spawnedEnemies.Add(enemy);
                    spawnTimer = spawnInterval;

                    if (spawnIndex + 1 <= spawnLocation.Length - 1)
                    {
                        spawnIndex++;
                    }
                    else
                    {
                        spawnIndex = 0;
                    }
                }
                else
                {
                    waveTimer = 0;
                }
            }
            else
            {
                spawnTimer -= Time.fixedDeltaTime;
                waveTimer -= Time.fixedDeltaTime;
            }

            if (waveTimer <= 0 && spawnedEnemies.Count <= 0)
            {
                if (currWave < 15)
                {
                    currWave++;
                    GameController.instance.updateScore(100 * GameController.instance.combo);
                    GenerateWave();
                    isBossWave = true;
                }
                else if (isBossWave)
                {
                    generateBossWave();
                    isBossWave = false;
                }
            }
        }
    }

    public void GenerateWave()
    {
        waveValue = currWave * 10;
        GenerateEnemies();

        if (currWave >= 5)
        {
            spawnInterval = 1f;
        }
        else if (currWave >= 10)
        {
            spawnInterval = .5f;
        }
        else
        {
            spawnInterval = 1.5f;
        }
        waveTimer = waveDuration;
    }

    public void generateBossWave()
    {
        waveValue = 999;
        spawnInterval = 2;
        waveTimer = waveDuration;
        bossWave();
        GenerateEnemies();
    }

    public void GenerateEnemies()
    {
        List<GameObject> generatedEnemies = new List<GameObject>();
        while (waveValue > 0 || generatedEnemies.Count < 50)
        {
            int randEnemyId = Random.Range(0, enemies.Count);
            int randEnemyCost = enemies[randEnemyId].cost;

            if (waveValue - randEnemyCost >= 0)
            {
                generatedEnemies.Add(enemies[randEnemyId].enemyPrefab);
                waveValue -= randEnemyCost;
            }
            else if (waveValue <= 0)
            {
                break;
            }
        }
        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
    }

    public void bossWave()
    {
        Instantiate(Boss, new Vector2(22, 0), Quaternion.identity);
        GameController.instance.currLevelText.text = "LEVEL: BOSS";
    }

}

[System.Serializable]
public class Enemy
{
    public GameObject enemyPrefab;
    public int cost;
}

