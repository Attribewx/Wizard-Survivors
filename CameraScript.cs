using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraScript : MonoBehaviour
{
    private SpawnPlayers allPlayers;
    public Transform curPlayerSpectating;

    private void Start()
    {
        allPlayers = FindObjectOfType<SpawnPlayers>();
    }

    private void Update()
    {
        for (int i = 0; i < allPlayers.players.Count; i++)
        {
            if (allPlayers.players[i].GetPhotonView().IsMine)
            {
                transform.position = new Vector3(allPlayers.players[i].transform.position.x, allPlayers.players[i].transform.position.y, -10);
            }
        }
    }
}
