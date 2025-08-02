using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DestroyTime : MonoBehaviour
{
    public float time2Destroy;
    private void Start()
    {
        StartCoroutine(DestroyGame());
    }

    private IEnumerator DestroyGame()
    {
        yield return new WaitForSeconds(time2Destroy);
        PhotonNetwork.Destroy(gameObject);
    }
}
