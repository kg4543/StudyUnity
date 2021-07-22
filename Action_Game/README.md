# ACTION GAME

<kbd>[![Action](/Capture/Action.gif "Action")](https://github.com/kg4543/StudyUnity/tree/main/Action_Game/Assets/Script)</kbd> </br>

## CAMERA

- '쿼터 뷰'로써 메인카메라가 플레이어 상공에서 플레이어를 따라 다닌다. 
- 서브 카메라의 경우 플레이어가 보고 있는 시점을 하단에 설정하여 보여 준다. (LookAt 사용)
```C#
    public Transform target; //Player 설정
    public Vector3 offset; //Player와 카메라 사이의 간견 설정
    float rotationX = 0.0f;
    float rotationY = 0.0f;

    public GameObject see //Player 보다 앞으로 설정하여 'see' 객체를 보게함;
    
void Update()
    {
        transform.position = target.position + offset;
        transform.eulerAngles = new Vector3(-rotationY,rotationX, 0.0f);
        transform.LookAt(see.transform.position);
    }
```

[MainCamera Code](https://github.com/kg4543/StudyUnity/blob/main/Action_Game/Assets/Script/CameraFollow.cs) </br>
[SubCamera Code](https://github.com/kg4543/StudyUnity/blob/main/Action_Game/Assets/Script/SubCamera.cs) </br>

## PLAYER

<kbd>[![Player](/Capture/Player.PNG "Player")](https://github.com/kg4543/StudyUnity/blob/main/Action_Game/Assets/Script/Player.cs)</kbd> </br>
(Click the Image) </br>

- Input Manager를 활용하여 기본 동작 키를 받아옴
- 기본 행동 : 전방위 이동 / 점프 / 구르기 / 무기 교체
- 각 행동은 bool값을 설정하여 동시에 여러 행동이 안되도록 방지
- anime.SetBool(상태)와 anime.SetTrigger(상황)로 animation 동작
```C#
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
```
- Bool형 배열을 통해 무기 소지 여부 판단
- 무기 배열을 통해 index 값으로 무기 구분 및 무기 교체
```C#
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
```
- 무기 타입(근접 / 원거리)에 따른 공격 속도 및 Animation 지정 
- 탄환의 갯수에 따른 공격 불가 및 탄환 추가 제약
- 수류탄 배열을 통한 갯수 파악 및 투척 Animation 설정 
- 아이템 및 적 공격 충돌 이벤트 지정

## WEAPON

<kbd>[![Weapon](/Capture/Weapon.PNG "Weapon")](https://github.com/kg4543/StudyUnity/blob/main/Action_Game/Assets/Script/Weapon.cs)</kbd> </br>
(Click the Image) </br>

- 무기 종류(근접 / 단발 / 연발)에 공격 이벤트 지정
- 공격 시 공격 범위 및 이펙트 효과 활성화 (Coroutine 사용)
```C#
public void Use()
    {
        if (type == WeaponType.Malee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
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
```
- 원거리 공격 시 총알 및 탄피 배출 Animation 지정 

## ITEM

<kbd>[![Item](/Capture/Item.PNG "Item")](https://github.com/kg4543/StudyUnity/blob/main/Action_Game/Assets/Script/Item.cs)</kbd> </br>
(Click the Image) </br>

- 'Player'와 충돌 이벤트 범위 및 'Ground' 및 'Wall'에서 물리적으로 구분되는 범위 지정
- 'Player'에서 'ITEM' 충돌 시 이벤트 활성
```C#
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
    }

```

## ENEMY

<kbd>[![ENEMY](/Capture/Enemy.PNG "Enemy")](https://github.com/kg4543/StudyUnity/blob/main/Action_Game/Assets/Script/Enemy.cs)</kbd> </br>
(Click the Image) </br>

- 타입별 공격 Animation 지정
```C#
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
```
- Player 공격 종류 별 데미지 계산 및 피격 시 Animation 지정
- 사망 시  Animaion 및 일정 시간 뒤 객체 소멸
- Boss의 경우 Player 위치를 지속적으로 파악 / Random하게 공격 Animation이 활성화

```C#
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
```
- Boss의 미사일 공격은 유도 기능 탑재 (NavMeshAgent 활용)
```C#
void Update()
    {
        nav.SetDestination(target.position);
    }
```
- Boss의 바위 공격은 회전 및 크기 확대 Animation 지정

참고 자료 : https://www.youtube.com/channel/UCw_N-Q_eYJo-IJFbNkhiYDA </br>
