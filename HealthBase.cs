using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public abstract class HealthBase : MonoBehaviour
{

    [HideInInspector] public float curHealth;
    [SerializeField] public float maxHealth;
    [SerializeField] private float startHealth;
    [SerializeField] public GameObject EXP;
    [SerializeField] private GameObject damageIndicator;
    private bool isPosioned;
    private bool isSlowed;
    [HideInInspector]public PhotonView view;
    [HideInInspector] public bool hasDied;

    public virtual void Start()
    {
        curHealth = startHealth;
        view = GetComponent<PhotonView>();
    }

    public void SetMaxHealth(float f)
    {
        maxHealth = maxHealth * f;
        startHealth = maxHealth;
    }

    public virtual void ApplyDamOrHealing(float amt)
    {
        curHealth += amt;
        GameObject damIndie = PhotonNetwork.Instantiate(damageIndicator.name, transform.position, Quaternion.identity);
        TMP_Text text = damIndie.GetComponentInChildren<TMP_Text>();
        if (text)
            text.text = Mathf.Abs((int)amt).ToString();
        if (curHealth > maxHealth)
            curHealth = maxHealth;
        if(curHealth <= 0 && !hasDied)
        {
            if (view != null)
            {
                if (view.IsMine)
                {
                    if (EXP != null)
                    {
                        PhotonNetwork.Instantiate(EXP.name, transform.position, Quaternion.identity);
                    }
                    if (gameObject != null)
                    {
                        hasDied = true;
                        EnemySpawner.ChangeEnemyCount(-1);
                        PhotonNetwork.Destroy(gameObject);
                    }
                }
            }
        }
    }
}
