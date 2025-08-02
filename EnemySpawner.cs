using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemySpawner : MonoBehaviour
{
    private static EnemySpawner instance;
    public SpawnPlayers allPlayers;
    [SerializeField]private GameObject enemyPrefab;
    [SerializeField] private GameObject bossPrefab;
    private Vector3 spawnPos;
    private float rngX;
    private float rngY;

    [SerializeField, Header("Game Rules")] private int maxEnemies;
    [SerializeField] private float SpawnRadiusX;
    [SerializeField] private float SpawnRadiusY;
    [SerializeField] private float SpawnRate;
    private float timeToSpawn;
    public static int currentEnemies;
    private int curMaxEnemies;
    private float bossTimer;
    private bool bossSpawned;
    private int minutesPassed;

    public static bool ready;
    public event EventHandler OnDeath;

    private LevelUpController levelUpController;

    void Start()
    {
        levelUpController = FindObjectOfType<LevelUpController>();
        allPlayers = FindObjectOfType<SpawnPlayers>();
        rngX = UnityEngine.Random.Range(-SpawnRadiusX, SpawnRadiusX);
        rngY = UnityEngine.Random.Range(-SpawnRadiusY, SpawnRadiusY);
    }

    // Update is called once per frame
    void Update()
    {
        curMaxEnemies = maxEnemies * allPlayers.players.Count;
        if(Input.GetButtonDown("Ready"))
        {
            ready = true;
            currentEnemies = FindObjectsOfType<EnemyWalkScript>().Length;
        }

        if (ready)
        {
            timerCode();
            if(currentEnemies < curMaxEnemies && timeToSpawn < Time.time)
            {
                timeToSpawn = Time.time + SpawnRate;
                PositionFind();
                GameObject enemy = PhotonNetwork.Instantiate(enemyPrefab.name, PositionFind(), Quaternion.identity);
                HealthBase helth = enemy.GetComponent<HealthBase>();
                EnemyWalkScript movement = enemy.GetComponent<EnemyWalkScript>();
                if (helth)
                    helth.SetMaxHealth(1f + (float)minutesPassed / 5f);
                if (movement)
                    movement.SetMoveSpeedMultiplier(1f + (float)minutesPassed / 5f);
                ChangeEnemyCount(1);
                OnDeath?.Invoke(this, EventArgs.Empty);
            }
            if(bossTimer >= 300 && bossSpawned == false)
            {
                bossSpawned = true;
                PhotonNetwork.Instantiate(bossPrefab.name, PositionFind(), Quaternion.identity);
            }

            if(minutesPassed <= bossTimer / 60)
            {
                Debug.Log(bossTimer / 60);
                minutesPassed++;
                maxEnemies++;
                if(SpawnRate > .1f)
                SpawnRate -= .02f;
            }
        }
    }

    private Vector3 PositionFind()
    {
        rngX = UnityEngine.Random.Range(-SpawnRadiusX, SpawnRadiusX);
        rngY = UnityEngine.Random.Range(-SpawnRadiusY, SpawnRadiusY);
        float x = 0;
        float y = 0;
        int rngPlayer = UnityEngine.Random.Range(0, allPlayers.players.Count);
        x = Mathf.Sin(rngX) * SpawnRadiusX + allPlayers.players[rngPlayer].transform.position.x;
        y = Mathf.Cos(rngY) * SpawnRadiusY + allPlayers.players[rngPlayer].transform.position.y;
        spawnPos = new Vector3(x, y, 0);
        for (int i = 0; i < allPlayers.players.Count; i++)
        {
            if(Vector3.Distance(allPlayers.players[i].transform.position,spawnPos) < SpawnRadiusX)
            {
                spawnPos = PositionFind();
            }
        }
        return spawnPos;
    }

    private void timerCode()
    {
        bossTimer += Time.deltaTime;
        int mins = (int)bossTimer / 60;
        float secs = bossTimer;
        if (secs >= 60)
        {
            secs = secs % 60;
        }
        string s = mins + ":" + (int)secs;
        if(secs <= 10)
        {
            s = mins + ":0" + (int)secs;
        }
        if (mins < 10)
        {
            s = "0" + s;
        }
        levelUpController.updateTimer(s);
    }
    public static void ChangeEnemyCount(int amt)
    {
        currentEnemies += amt;
    }

}
