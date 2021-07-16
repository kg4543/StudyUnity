using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int weaponIndex = -1;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    public GameObject[] grenades;
    public int hasGrenade;
    public int maxhasGrenade;
    public GameObject grenadeObj;
    public Transform GrenadePos;

    public float speed;
    public float jumpPower;
    public int ammo;
    int reAmmo;
    public int coin;
    public int health;

    public int maxammo;
    public int maxcoin;
    public int maxhealth;

    float hAxis;
    float vAxis;
    float fireDelay;

    bool wDown;
    bool cDown;
    bool xDown;
    bool aDown;
    bool zDown;
    bool vDown;
    bool gDown;
    bool sDown1;
    bool sDown2;
    bool sDown3;

    bool isJump;
    bool isDodge;
    bool isSwap;
    bool isReload;
    bool isFireReady = true;
    bool isBorder;
    bool isDamage;

    Vector3 moveVec;
    Vector3 dodgeVec;

    Rigidbody rigid;
    Animator anime;
    MeshRenderer[] meshes;

    GameObject haveObject;
    Weapon nowWeapon;

    private void Awake()
    {
        isJump = false;

        anime = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        meshes = GetComponentsInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();

        Move();

        Turn();

        Attack();

        Reload();

        ThrowGrenade();

        Jump();

        Dodge();

        Swap();

        Interation();

    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        cDown = Input.GetButton("Jump");
        xDown = Input.GetButton("Dodge");
        aDown = Input.GetButton("Interation");
        zDown = Input.GetButton("Attack");
        gDown = Input.GetButton("Grenade");
        vDown = Input.GetButton("Reload");
        sDown1 = Input.GetButton("Swap1");
        sDown2 = Input.GetButton("Swap2");
        sDown3 = Input.GetButton("Swap3");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized; //normalized

        if (isDodge)
        {
            moveVec = dodgeVec;
        }

        if (!isFireReady || isReload)
        {
            moveVec = Vector3.zero;
        }

        if (!isBorder)
        {
            transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;
        }

        if (wDown)
        {
            transform.position += moveVec * speed * 0.3f * Time.deltaTime;
        }
        else
        {
            transform.position += moveVec * speed * Time.deltaTime;
        }

        anime.SetBool("isRun", moveVec != Vector3.zero);
        anime.SetBool("isWalk", wDown);
    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec);
    }

    void Jump()
    {
        if (cDown && !isJump && !isDodge)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            anime.SetBool("isJump", true);
            anime.SetTrigger("doJump");
            isJump = true;
        }
    }

    void Dodge()
    {
        if (xDown && moveVec != Vector3.zero &&!isJump && !isDodge)
        {
            dodgeVec = moveVec;
            speed *= 2;
            anime.SetTrigger("doDodge");
            isDodge = true;
            Invoke("DodgeOut", 0.5f); // 시간차 함수
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }

    void Interation()
    {
        if (aDown && haveObject != null)
        {
            if (haveObject.tag == "Weapon")
            {
                Item item = haveObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;

                Destroy(haveObject);
            }
        }
    }

    void Swap()
    {
        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;
        if (sDown3) weaponIndex = 2;

        if ((sDown1 || sDown2 || sDown3) && hasWeapons[weaponIndex] == true)
        {
            if (nowWeapon != null)
            {
                nowWeapon.gameObject.SetActive(false);

            }
            nowWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            nowWeapon.gameObject.SetActive(true);

            if (moveVec == Vector3.zero && !isJump)
            {
                anime.SetTrigger("doSwap");
                isSwap = true;

                Invoke("SwapOut", 0.5f); // 시간차 함수
            }
        }
    }

    void SwapOut()
    {
        isSwap = false;

    }

    void Attack()
    {
        if (nowWeapon == null)
        {
            return;
        }

        fireDelay += Time.deltaTime;
        isFireReady = nowWeapon.rate < fireDelay;

        if (zDown && isFireReady && !isDodge && !isSwap)
        {
            nowWeapon.Use();
            anime.SetTrigger(nowWeapon.type == Weapon.WeaponType.Malee ? "doSwing" : "doShot");
            fireDelay = 0;
        }
    }

    void Reload()
    {
        if (nowWeapon == null)
        {
            return;
        }
        if (nowWeapon.type == Weapon.WeaponType.Malee)
        {
            return;
        }
        if (ammo == 0)
        {
            return;
        }
        if (vDown && !isSwap && isFireReady)
        {
            anime.SetTrigger("doReload");
            reAmmo = ammo < nowWeapon.maxAmmo ? ammo : nowWeapon.maxAmmo - ammo;
            nowWeapon.curAmmo += reAmmo;
            ammo -= reAmmo;
            isReload = true;
            Invoke("ReloadOut", 3f);
        }
    }

    void ReloadOut()
    {
        isReload = false;
    }

    void ThrowGrenade()
    {
        if (hasGrenade == 0)
        {
            return;
        }
        if (gDown && !isReload && !isSwap)
        {
            GameObject instantGrenade = Instantiate(grenadeObj, GrenadePos.position, GrenadePos.rotation);
            Rigidbody rigidGrenade = instantGrenade.GetComponent<Rigidbody>();
            Vector3 gvec;
            if (moveVec == Vector3.zero)
            {
                gvec = GrenadePos.forward * 10 + Vector3.up * 8;
                rigidGrenade.AddForce(gvec, ForceMode.Impulse);
            }
            else
            {
                gvec = GrenadePos.forward * -3;
                rigidGrenade.AddForce(gvec, ForceMode.Impulse);
            }
            

            hasGrenade--;
            grenades[hasGrenade].SetActive(false);
        }
    }

    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }

    void stopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.white);
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }

    private void FixedUpdate()
    {
        FreezeRotation();
        stopToWall();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            anime.SetBool("isJump", false);
            isJump = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.ItemType.Ammo:
                    ammo += item.value;
                    if (ammo >= maxammo)
                    {
                        ammo = maxammo;
                    }
                    break;
                case Item.ItemType.Coin:
                    coin += item.value;
                    if (coin >= maxcoin)
                    {
                        coin = maxcoin;
                    }
                    break;
                case Item.ItemType.Grenade:
                    grenades[hasGrenade].SetActive(true);
                    hasGrenade += item.value;
                    if (hasGrenade >= maxhasGrenade)
                    {
                        hasGrenade = maxhasGrenade;
                    }
                    break;
                case Item.ItemType.Heart:
                    health += item.value;
                    if (health >= maxhealth)
                    {
                        health = maxhealth;
                    }
                    break;
                case Item.ItemType.Weapon:
                    ammo += item.value;
                    if (ammo >= maxammo)
                    {
                        ammo = maxammo;
                    }
                    break;
            }
            Destroy(other.gameObject);
        }
        else if (other.tag == "EnemyBullet")
        {
            if (!isDamage)
            {
                Bullet enemyBullet = other.GetComponent<Bullet>();
                health -= enemyBullet.damage;

                bool isBossAtk = other.name == "BossMaleeArea";
                StartCoroutine(OnDamage(isBossAtk));
            }

            if (other.GetComponent<Rigidbody>() != null)
            {
                Destroy(other.gameObject);
            }
        }
    }

    IEnumerator OnDamage(bool isBossAtk)
    {
        isDamage = true;
        foreach (var mesh in meshes)
        {
            mesh.material.color = Color.cyan;
        }

        if (isBossAtk)
        {
            rigid.AddForce(transform.forward * -25, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(1f);

        isDamage = false;
        foreach (var mesh in meshes)
        {
            mesh.material.color = Color.white;
        }

        if (isBossAtk)
        {
            rigid.velocity = Vector3.zero;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon")
        {
            haveObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        haveObject = null;
    }
}