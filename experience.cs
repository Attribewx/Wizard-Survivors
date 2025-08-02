using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class experience : HealthBase
{
    [SerializeField] private int expPoints;
    [SerializeField] private float radius;
    [SerializeField] private GameObject bigExp;
    private EnemySpawner spwan;
    public GameObject[] games;

    public override void Start()
    {
        spwan = FindObjectOfType<EnemySpawner>();
        if(spwan)
            spwan.OnDeath += Spwan_OnDeath;
        base.Start();
    }

    private void Spwan_OnDeath(object sender, System.EventArgs e)
    {
        if(bigExp != null)
        {

            RaycastHit2D[] exps = Physics2D.CircleCastAll(transform.position, radius, Vector2.up, 0, 1 << 8);
            experience[] experiences = new experience[0];
            experience[] allEXP = new experience[exps.Length];

            for (int i = 0; i < exps.Length; i++)
            {
                allEXP[i] = exps[i].transform.GetComponent<experience>();
                if(allEXP[i].expPoints == expPoints)
                {
                    experiences = addEXP(experiences, allEXP[i]);
                }
            }
            if (experiences.Length > 4)
            {
                if (experiences[0] != null)
                    experiences[0].unsub();
                if (experiences[1] != null)
                    experiences[1].unsub();
                if (experiences[2] != null)
                    experiences[2].unsub();
                if (experiences[3] != null)
                    experiences[3].unsub();
                if (experiences[4] != null)
                    experiences[4].unsub();
                PhotonNetwork.Instantiate(bigExp.name, transform.position, Quaternion.identity);
                if(view.AmOwner)
                {

                PhotonNetwork.Destroy(experiences[0].transform.gameObject);
                PhotonNetwork.Destroy(experiences[1].transform.gameObject);
                PhotonNetwork.Destroy(experiences[2].transform.gameObject);
                PhotonNetwork.Destroy(experiences[3].transform.gameObject);
                PhotonNetwork.Destroy(experiences[4].transform.gameObject);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterMovement chara = collision.GetComponent<CharacterMovement>();
        if(chara != null)
        {
            if(spwan != null)
                spwan.OnDeath -= Spwan_OnDeath;
            chara.addXP(expPoints);
            if(view != null)
            {
                if(view.AmOwner)
                    PhotonNetwork.Destroy(gameObject);

            }
        }
    }

    private void unsub()
    {
        if(spwan != null)
        spwan.OnDeath -= Spwan_OnDeath;
    }

    public override void ApplyDamOrHealing(float amt)
    {
        
    }

    private experience[] addEXP(experience[] extra, experience additive)
    {
        experience[] temp;
        temp = new experience[extra.Length + 1];
        for (int i = 0; i < extra.Length; i++)
        {
            temp[i] = extra[i];
        }
        temp[temp.Length - 1] = additive;
        return temp;
    }
}
