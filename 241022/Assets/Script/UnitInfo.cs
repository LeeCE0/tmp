using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;
using UnityEditor.Animations;
using UnityEngine.SocialPlatforms;

public class UnitInfo : MonoBehaviour
{

    public float moveSpeed = 2.0f;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] Animator[] anim;
    [SerializeField] SpriteRenderer[] spriteRD;

    [SerializeField] int ID;
    [SerializeField] string Name;
    [SerializeField] int HP;
    [SerializeField] int cost;
    [SerializeField] float attackDistance;
    [SerializeField] GameObject curTarget;
    [SerializeField] float attackCT;
    [SerializeField] float attackDelay;


    [SerializeField] MyInfo.eUnitState unitState = MyInfo.eUnitState.Idle;
    [SerializeField] MyInfo.eUnitType unitType = MyInfo.eUnitType.boomer;
    [SerializeField] Weapon weapon;
    [SerializeField] bool isMyUnit = true;
    public bool isBullet = false;
    public bool isAttack = false;   


    public int GetUnitID() { return ID; }

    void Start()
    {
        SetState(unitState); // 초기 상태를 idle로 설정
    }
    void Update()
    {
        switch (unitState)
        {
            case MyInfo.eUnitState.Idle:
                break;
            case MyInfo.eUnitState.Walk:
                UpdateWalk();
                break;
            case MyInfo.eUnitState.Attack:
                UpdateAttack();
                break;
            case MyInfo.eUnitState.Dead:
                // 죽은 상태는 보통 별도 동작 없음
                break;
        }
    }

    public void SetSpawn(MyInfo.UnitData data)
    {
        ID = data.UnitID;
        Name = data.UnitName;
        HP = data.HP;
        cost = data.Cost;
        moveSpeed = data.UnitSpeed;
        curTarget = null;
        unitState = MyInfo.eUnitState.Walk;
        SetState(unitState);
    }

    public void SetState(MyInfo.eUnitState state)
    {
        unitState = state;

        for (int i = 0; i < anim.Length; i++)
        {
            if (anim[i] != null)
            {
                switch (unitState)
                {
                    case MyInfo.eUnitState.Idle:
                        anim[i].SetTrigger("Idle");
                        break;
                    case MyInfo.eUnitState.Walk:
                        anim[i].SetBool("Walk", true);
                        break;
                    case MyInfo.eUnitState.Attack:
                        anim[i].SetTrigger("Attack");
                        break;
                    case MyInfo.eUnitState.Dead:
                        break;
                }
            }
        }
    }
    // Walk 상태 동작
    private void UpdateWalk()
    {
        if (curTarget == null)
        {
            // 이동 로직
            float direction = isMyUnit ? 1 : -1;
            transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

            // 가장 가까운 적 찾기
            curTarget = FindNearestEnemy();
        }
        else
        {
            // 적이 있다면 공격 상태로 전환
            SetState(MyInfo.eUnitState.Attack);
        }
    }

    // Attack 상태 동작
    private void UpdateAttack()
    {
        if (curTarget == null)
        {
            // 적이 없으면 다시 걷기 상태로 전환
            SetState(MyInfo.eUnitState.Walk);
            return;
        }

        // 공격 딜레이 관리
        attackDelay += Time.deltaTime;
        if (attackDelay >= attackCT)
        {
            Debug.LogError("Attack");
            attackDelay = 0f;
            PerformAttack();
        }

        // 적이 감지 범위를 벗어나면 Walk로 돌아감
        float distance = Vector2.Distance(transform.position, curTarget.transform.position);
        if (distance > attackDistance)
        {
            curTarget = null;
            SetState(MyInfo.eUnitState.Walk);
        }
    }

    // 실제 공격 수행 로직
    private void PerformAttack()
    {
        if (curTarget != null)
        {
            Debug.Log("Performing attack on " + curTarget.name);
            Weapon ob = Instantiate(weapon.gameObject).GetComponent<Weapon>();
            weapon.Launch(weapon.transform.position, curTarget.transform.position, Weapon.eWeaponType.Boom);
        }
    }

    // 가장 가까운 적 찾기
    private GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(isMyUnit ? "Enemy" : "Mine");

        if (enemies == null || enemies.Length == 0)
            return null;

        GameObject nearestEnemy = null;
        float minDistance = attackDistance;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                nearestEnemy = enemy;
                minDistance = distance;
            }
        }
        return nearestEnemy;
    }
}
