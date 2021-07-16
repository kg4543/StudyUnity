using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { A, B, C, D};
    public Type enemyType;
    public int maxHealth;
    public int curHealth;
    public Transform target;
    public BoxCollider maleeArea;
    public bool isChase;
    public bool isAttack;
    public bool isDead;
    public GameObject bullet;
    public Rigidbody rigid;
    public BoxCollider boxcollider;
    public MeshRenderer[] meshes;
    public NavMeshAgent nav;
    public Animator anime;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxcollider = GetComponent<BoxCollider>();
        meshes = GetComponentsInChildren<MeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anime = GetComponentInChildren<Animator>();

        if (enemyType != Type.D)
        {
            Invoke("ChaseStart", 2);
        }
    }

    void ChaseStart()
    {
        isChase = true;
        anime.SetBool("isWalk", true);
    }

    void Update()
    {
        if (nav.enabled && enemyType != Type.D)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }
    }
    private void FixedUpdate()
    {
        Targeting();
        FreezeVelocity();
    }

    void FreezeVelocity()
    {
        if (isChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }
    void Targeting()
    {

        if (!isDead && enemyType != Type.D)
        {
            float targetRadius = 0;
            float targetRange = 0;

            switch (enemyType)
            {
                case Type.A:
                    targetRadius = 1.5f;
                    targetRange = 2.5f;
                    break;
                case Type.B:
                    targetRadius = 1f;
                    targetRange = 12f;
                    break;
                case Type.C:
                    targetRadius = 0.5f;
                    targetRange = 25f;
                    break;
            }

            RaycastHit[] rayHits =
                Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

            if (rayHits.Length > 0 && !isAttack)
            {
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        anime.SetBool("isAttack",true);

        switch (enemyType)
        {
            case Type.A:
                yield return new WaitForSeconds(1f);
                maleeArea.enabled = true;

                yield return new WaitForSeconds(0.3f);
                maleeArea.enabled = false;

                yield return new WaitForSeconds(0.3f);
                break;
            case Type.B:
                yield return new WaitForSeconds(0.3f);
                rigid.AddForce(transform.forward * 30, ForceMode.Impulse);
                maleeArea.enabled = true;

                yield return new WaitForSeconds(0.5f);
                rigid.velocity = Vector3.zero;
                maleeArea.enabled = false;

                yield return new WaitForSeconds(2f);
                break;
            case Type.C:
                yield return new WaitForSeconds(0.5f);
                GameObject instantBullet = Instantiate(bullet, transform.position, transform.rotation);
                Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
                //rigidBullet.velocity = Vector3.back * 20;
                rigidBullet.AddForce(transform.forward * 30, ForceMode.Impulse);

                yield return new WaitForSeconds(2f);
                break;
            default:
                break;
        }

        
        isChase = true;
        isAttack = false;
        anime.SetBool("isAttack", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Malee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec, false));
        }
        else if(other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;
            Vector3 reactVec = transform.position - other.transform.position;

            Destroy(other.gameObject);
            StartCoroutine(OnDamage(reactVec, false));
        }
    }

    public void HitByGrenade(Vector3 explosionPos)
    {
        curHealth -= 100;
        Vector3 reactVec = transform.position - explosionPos;
        StartCoroutine(OnDamage(reactVec, true));
    }

    IEnumerator OnDamage(Vector3 reactVec, bool isGrenade)
    {
        foreach (var mesh in meshes)
        {
            mesh.material.color = Color.red;
        }

        yield return new WaitForSeconds(0.1f);

        if (curHealth > 0)
        {
            foreach (var mesh in meshes)
            {
                mesh.material.color = Color.white;
            }
        }
        else
        {
            foreach (var mesh in meshes)
            {
                mesh.material.color = Color.gray;
            }
            gameObject.layer = 12;
            isDead = true;
            isChase = false;
            nav.enabled = false;
            anime.SetTrigger("doDie");

            if (isGrenade)
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up * 2;
                rigid.freezeRotation = false;
                rigid.AddForce(reactVec * 3, ForceMode.Impulse);
                rigid.AddTorque(reactVec * 10, ForceMode.Impulse);
            }
            else
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.forward;
                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            }
            Destroy(gameObject,4);
        }
    }
}
