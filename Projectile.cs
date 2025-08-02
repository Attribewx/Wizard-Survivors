using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float projSpeed;
    [SerializeField] private bool rotateTowards;
    [SerializeField] private float lifetime;
    [SerializeField] private int pierceAmt = 1;
    private Rigidbody2D rig;
    private float curLifetime;
    private PhotonView view;

    void Awake()
    {
        view = GetComponent<PhotonView>();
        rig = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        //transform.eulerAngles = new Vector3(0,0,CharacterMovement.GetAngleFromVectorFloat(rig.velocity));
        curLifetime = Time.time + lifetime;
    }

    private void Update()
    {
        if (Time.time > curLifetime)
        {
            if(view.IsMine)
            { 
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        HealthBase health = collision.GetComponent<HealthBase>();
        if (health != null)
        {
            health.ApplyDamOrHealing(-damage);
            pierceAmt -= 1;
            if(pierceAmt <= 0)
            {
                if(view.IsMine)
                {
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }
    }

    public void SetVelocity(Vector3 vec)
    {
        rig.velocity = vec * projSpeed;
    }

    public void IncreasePierce(int p)
    {
        pierceAmt += p;
    }

    public void SetDamage(float dam)
    {
        damage *= dam;
    }
}
