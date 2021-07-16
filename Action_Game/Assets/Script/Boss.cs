using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemy
{
    public GameObject missile;
    public Transform missilePosA;
    public Transform missilePosB;

    Vector3 lookVec;
    Vector3 tauntVec;
    public bool islook;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxcollider = GetComponent<BoxCollider>();
        meshes = GetComponentsInChildren<MeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anime = GetComponentInChildren<Animator>();

        nav.isStopped = true;
        StartCoroutine(Think());
    }


    void Update()
    {
        if (isDead)
        {
            StopAllCoroutines();
            return;
        }

        if (islook)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lookVec = new Vector3(h, 0, v) * 5f;
            transform.LookAt(target.position + lookVec);
        }
        else
        {
            nav.SetDestination(tauntVec);
        }
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);

        int ranAction = Random.Range(1, 4);

        switch (ranAction)
        {
            case 1:
                //미사일 발사 패턴
                StartCoroutine(MissileShot());
                break;
            case 2:
                // 점프 공격 패턴
                StartCoroutine(Taunt());
                break;
            case 3:
                //돌 굴러가는 패턴
                StartCoroutine(RockShot());
                break;
        }
    }

    IEnumerator MissileShot()
    {
        anime.SetTrigger("doShot");
        yield return new WaitForSeconds(0.2f);

        GameObject instantMissileA = Instantiate(missile, missilePosA.position, missilePosA.rotation);
        BossMissile bossMissileA = instantMissileA.GetComponent<BossMissile>();
        bossMissileA.target = target;
        
        yield return new WaitForSeconds(0.3f);

        GameObject instantMissileB = Instantiate(missile, missilePosB.position, missilePosB.rotation);
        BossMissile bossMissileB = instantMissileB.GetComponent<BossMissile>();
        bossMissileB.target = target;

        yield return new WaitForSeconds(2f);

        StartCoroutine(Think());
    }

    IEnumerator RockShot()
    {
        islook = false;
        anime.SetTrigger("doBigshot");
        Instantiate(bullet, transform.position, transform.rotation);
        yield return new WaitForSeconds(3f);
        islook = true;
        StartCoroutine(Think());
    }

    IEnumerator Taunt()
    {
        tauntVec = target.position + lookVec;

        islook = false;
        nav.isStopped = false;
        boxcollider.enabled = false;

        anime.SetTrigger("doTaunt");
        yield return new WaitForSeconds(1.5f);
        maleeArea.enabled = true;
        yield return new WaitForSeconds(2f);
        maleeArea.enabled = false;

        yield return new WaitForSeconds(1f);
        islook = true;
        nav.isStopped = true;
        boxcollider.enabled = true;

        StartCoroutine(Think());
    }
}
