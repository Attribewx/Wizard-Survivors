using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class EnemyWalkScript : MonoBehaviour
{
    [SerializeField, Header("Stats")] private float movSpeed;
    [SerializeField] private Transform target;
    private CharacterMovement[] targets = new CharacterMovement[5];
    private Rigidbody2D rig;
    private Vector3 dir;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        targets = FindObjectsOfType<CharacterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        targets = FindObjectsOfType<CharacterMovement>();
        target = FindClosestPlayer();
        dir = target.position - transform.position;

    }

    void FixedUpdate()
    {
        rig.velocity = dir.normalized * movSpeed * Time.deltaTime;
    }

    public void SetMoveSpeedMultiplier(float f)
    {
        movSpeed = movSpeed * f;
    }

    private Transform FindClosestPlayer()
    {
        Transform Closest = FindObjectOfType<CharacterMovement>().transform;
        for (int i = 0; i < targets.Length; i++)
        {
            if(Vector3.Distance(targets[i].transform.position, transform.position) < Vector3.Distance(Closest.position, transform.position))
            {
                Closest = targets[i].transform;
            }
        }
        return Closest;
    }

    void AddPlayer(Transform trans)
    {
        
    }
}
