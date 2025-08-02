using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BossHealth : HealthBase
{
    public override void ApplyDamOrHealing(float amt)
    {
        curHealth += amt;
        if (curHealth > maxHealth)
            curHealth = maxHealth;
        if (curHealth <= 0 && !hasDied)
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
                        PhotonNetwork.Destroy(gameObject);
                    }
                }
            }
        }
    }
}
