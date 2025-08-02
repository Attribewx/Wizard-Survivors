using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterMovement : MonoBehaviour
{
    private float lateralMove;
    private float verticalMove;
    private Rigidbody2D rig;
    private PhotonView view;
    [SerializeField] private GameObject userInterface;
    [SerializeField] private GameObject basicAttack;
    [SerializeField] private GameObject Arrow;
    [SerializeField] private GameObject Speer;
    [SerializeField] private GameObject Flamer;
    [SerializeField] private int[] levelReqs;
    private int currentLevel;
    private int actualLevel;
    private Vector3 attackDir;
    public List<Wpn> weapons;

    [Header("Stats"), SerializeField] private float movSpeed;
    [SerializeField] private float attackRate;
    [SerializeField] private float damageMultiplier = 1;
    private float timeTillNextAttack;
    private float timeTillNextArrow;
    private float timeTillNextSpeer;
    private float timeTillNextFlame;
    private int clockTiming;
    private float XP;
    private float XPTillLevelUp;
    private bool firstArrow;
    private bool firstSpeer;
    private bool firstFlame;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        timeTillNextAttack = Time.time + timeTillNextAttack;
        timeTillNextArrow = Time.time + timeTillNextArrow;
    }

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        attackDir = new Vector3(1, 0, 0).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            if(XP >= levelReqs[currentLevel])
            {
                if(currentLevel < levelReqs.Length - 1)
                {
                    currentLevel++;
                    movSpeed += .1f;
                    attackRate -= .02f;
                    damageMultiplier += .1f;
                }
            }

            if(Input.GetKey(KeyCode.Q))
            {
                Time.timeScale = 100f;
            }else
            {
                Time.timeScale = 1f;
            }


            lateralMove = Input.GetAxisRaw("Horizontal");
            verticalMove = Input.GetAxisRaw("Vertical");
            Vector3 walkDir = new Vector3(lateralMove, verticalMove,0).normalized;
            if (walkDir != Vector3.zero)
                attackDir = walkDir;
            if (timeTillNextAttack < Time.time)
            {
                GameObject bAtack = PhotonNetwork.Instantiate(basicAttack.name,transform.position + attackDir * 2, Quaternion.Euler(0, 0, GetAngleFromVectorFloat(attackDir)));
                Projectile proj = bAtack.GetComponent<Projectile>();
                if(proj != null)
                {
                    proj.SetVelocity(attackDir);
                }
                timeTillNextAttack += attackRate;
            }

            if(Contains(weapons, Weapons.Axe))
            {
                if(timeTillNextFlame < Time.time)
                {
                    int clockAngle = clockTiming * 30;
                    Vector3 vecForAng = new Vector3(Mathf.Sin(Mathf.Deg2Rad * clockAngle), Mathf.Cos(Mathf.Deg2Rad * clockAngle), 0);
                    Debug.Log(vecForAng);
                    GameObject bAtack = PhotonNetwork.Instantiate(Flamer.name, transform.position + vecForAng * 2, Quaternion.Euler(0, 180, clockAngle + 90));
                    Projectile proj = bAtack.GetComponent<Projectile>();
                    if (proj != null)
                    {
                        proj.SetVelocity(vecForAng);
                        if(clockTiming < 12)
                        {
                            clockTiming++;
                        }
                        else
                        {
                            clockTiming = 1;
                        }
                    }
                    if (!firstFlame)
                    {
                        timeTillNextFlame = timeTillNextAttack;
                        firstFlame = true;
                    }
                    else
                    {
                        timeTillNextFlame += attackRate;
                        proj.SetDamage(damageMultiplier);
                    }
                    if (WeaponLvl(Weapons.Axe) >= 4)
                    {
                        timeTillNextFlame -= attackRate * 5 / 6;
                    }
                    else if (WeaponLvl(Weapons.Axe) >= 3)
                    {
                        timeTillNextFlame -= attackRate / 2;
                    }
                    else if (WeaponLvl(Weapons.Axe) >= 2)
                    {
                        timeTillNextFlame -= attackRate / 3;
                    }
                }
            }

            if(Contains(weapons, Weapons.Speer))
            {
                if (timeTillNextSpeer < Time.time)
                {
                    float randX = Random.Range(-1f, 1f);
                    float randY = Random.Range(-1f, 1f);
                    Vector3 vec = new Vector3(randX, randY, 0).normalized;
                    GameObject bAtack = PhotonNetwork.Instantiate(Speer.name, transform.position + vec * 2, Quaternion.Euler(0, 0, GetAngleFromVectorFloat(vec)));
                    Projectile proj = bAtack.GetComponent<Projectile>();
                    if (proj != null)
                    {
                        proj.SetVelocity(vec);
                    }
                    if (!firstSpeer)
                    {
                        timeTillNextSpeer = timeTillNextAttack;
                        firstSpeer = true;
                    }
                    else
                    {
                        timeTillNextSpeer += attackRate;
                    }
                    if(WeaponLvl(Weapons.Speer) >= 4)
                    {
                        timeTillNextSpeer -= attackRate * 4 / 5;
                        proj.IncreasePierce(3);
                    }
                    else if (WeaponLvl(Weapons.Speer) >= 3)
                    {
                        timeTillNextSpeer -= attackRate * 2 / 5;
                        proj.IncreasePierce(2);
                    }
                    else if (WeaponLvl(Weapons.Speer) >= 2)
                    {
                        timeTillNextSpeer -= attackRate * 1 / 5;
                        proj.IncreasePierce(1);
                    }
                }
            }

            if(Contains(weapons, Weapons.Arrow))
            {
                if(timeTillNextArrow < Time.time)
                {
                    GameObject bAtack = PhotonNetwork.Instantiate(Arrow.name, transform.position + -attackDir * 2, Quaternion.Euler(0, 0, GetAngleFromVectorFloat(-attackDir)));
                    Projectile proj = bAtack.GetComponent<Projectile>();
                    if (proj != null)
                    {
                        proj.SetVelocity(-attackDir);
                    }
                    if (!firstArrow)
                    {
                        timeTillNextArrow = timeTillNextAttack;
                        firstArrow = true;
                    }else
                    {
                        timeTillNextArrow += attackRate;
                    }
                    if (WeaponLvl(Weapons.Arrow) >= 4)
                    {
                        timeTillNextArrow -= attackRate * .9f;
                    }
                    else if (WeaponLvl(Weapons.Arrow) >= 3)
                    {
                        timeTillNextArrow -= attackRate * .8f;
                    }
                    else if (WeaponLvl(Weapons.Arrow) >= 2)
                    {
                        timeTillNextArrow -= attackRate / 2;
                    }
                }

            }
        }
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    public void addXP(int amt)
    {
        XP += amt;
    }

    public void levelActualLevel()
    {
        actualLevel++;
    }

    public bool CharacterLevelUpReady()
    {
        if (actualLevel < currentLevel)
            return true;
        else
            return false;
    }

    private void FixedUpdate()
    {
        rig.velocity = new Vector2(lateralMove, verticalMove).normalized * movSpeed;
    }

    public void EquipWeapon(Wpn w)
    {
        bool hasItemAlready = false;
        for (int i = 0; i < 6; i++)
        {
            if(weapons[i].weapon == w.weapon)
            {
                weapons[i].weaponLvl += 1;
                hasItemAlready = true;
                i = 6;
            }
        }

        if (!hasItemAlready)
        {
            for (int i = 0; i < 6; i++)
            {
                if (weapons[i].weapon == Weapons.Nothing)
                {
                    weapons[i] = w;
                    weapons[i].weaponLvl += 1;
                    i = 6;
                }
            }
        }
    }

    private bool Contains(List<Wpn> wpns, Weapons wpn)
    {
        for (int i = 0; i < wpns.Count; i++)
        {
            if (wpns[i].weapon == wpn)
                return true;
        }
        return false;
    }

    private float WeaponLvl(Weapons wep)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if(weapons[i].weapon == wep)
            {
                return weapons[i].weaponLvl;
            }
        }
        return -1;
    }

    public int GetXP()
    {
        return (int)XP;
    }

    public int GetLvl()
    {
        return currentLevel;
    }

    public int GetLvlReq(int lvl)
    {
        return levelReqs[lvl];
    }

    public PhotonView getView()
    {
        return view;
    }
}
