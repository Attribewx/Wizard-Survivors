using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    public GameObject playerPref;
    public GameObject enemySpawnerPref;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    public List<GameObject> players;
    void Start()
    {
        Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        players.Add(PhotonNetwork.Instantiate(playerPref.name, randomPosition, Quaternion.identity));
        PhotonNetwork.Instantiate(enemySpawnerPref.name, Vector3.zero, Quaternion.identity);
    }

    private void Update()
    {
        CharacterMovement[] chars = FindObjectsOfType<CharacterMovement>();
        for (int i = 0; i < chars.Length; i++)
        {
            if (players.Count > i)
                players[i] = chars[i].gameObject;
            else
                players.Add(chars[i].gameObject);
        }
    }
}
