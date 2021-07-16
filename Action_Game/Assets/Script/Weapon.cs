using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WeaponType { Malee, Range };
    public WeaponType type;
    public int damage;
    public float rate;
    public BoxCollider maleeArea;
    public TrailRenderer trailEffect;
    public Transform bulletPos;
    public GameObject bullet;
    public Transform bulletCasePos;
    public GameObject bulletCase;
    public int maxAmmo;
    public int curAmmo;

    public void Use()
    {
        if (type == WeaponType.Malee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
        else if (type == WeaponType.Range && curAmmo > 0)
        {
            curAmmo--;
            StartCoroutine("Shot");
        }
    }

    IEnumerator Swing()
    {
        //1
        yield return new WaitForSeconds(0.3f); // 0.3초 대기
        maleeArea.enabled = true;
        trailEffect.enabled = true;
        
        //2
        yield return new WaitForSeconds(0.3f); 
        maleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f); 
        trailEffect.enabled = false;
        
        // yield break;
        // yield : 결과를 전달; 코루틴에서 1개 이상 필요
    }

    //Attack() main 루틴 -> Swing() sub 루틴 -> Attack()
    //Attack() main 루틴 + Swing() 코(함께)루틴

    IEnumerator Shot()
    {
        // 총알 발사
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 50;

        yield return null;

        // 탄피 배출
        GameObject intantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRigid = intantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        caseRigid.AddForce(caseVec, ForceMode.Impulse);
        caseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);
    }
}
