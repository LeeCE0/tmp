using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using Unity.VisualScripting;
using System;
using UnityEngine.UIElements;

public class UnitInfo : MonoBehaviour
{

    public float moveSpeed = 2.0f;
    [SerializeField] Rigidbody2D rigid;

    [SerializeField] int ID;
    [SerializeField] string Name;
    [SerializeField] int HP;
    [SerializeField] int cost;
    [SerializeField] float attackDistance;
    [SerializeField] GameObject curTarget;
    [SerializeField] float attackCT;
    [SerializeField] float attackDelay;


    [SerializeField] MyInfo.eUnitState unitState = MyInfo.eUnitState.Idle;
    [SerializeField] MyInfo.eUnitType unitType = MyInfo.eUnitType.Swordsman;
    [SerializeField] Weapon.eWeaponType weaponType = Weapon.eWeaponType.Sword;
    [SerializeField] Weapon weapon;
    [SerializeField] bool isMyUnit = true;

    [SerializeField] Animator anim = new Animator();
    public bool isBullet = false;
    public bool isAttack = false;   


    public int GetUnitID() { return ID; }

    void Start()
    {
        SetState(unitState);
    } 
    
    // 오브젝트 풀에서 꺼낼 때 호출할 초기화 메서드
    public void Initialize(MyInfo.UnitData data)
    {
        gameObject.SetActive(true); // 풀에서 꺼낼 때 활성화

        // 데이터 세팅
        ID = data.UnitID;
        Name = data.UnitName;
        HP = data.HP;
        cost = data.Cost;
        moveSpeed = data.UnitSpeed;
        curTarget = null;

        // 애니메이션 초기화
        //anim.Play("Idle", 0, 0f);

        SetState(MyInfo.eUnitState.Walk);
    }
    void Update()
    {
        switch (unitState)
        {
            case MyInfo.eUnitState.Walk:
                TryFindEnemy();
                break;
            case MyInfo.eUnitState.Attack:
                HandleAttack();
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

        SetState(MyInfo.eUnitState.Walk);
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (isMyUnit ? -1 : 1);
        transform.localScale = scale;
    }

    public void SetState(MyInfo.eUnitState state)
    {
        if (state == unitState) return;
        bool EditChk = true;
        unitState = state;
        switch (state)
        {
            case MyInfo.eUnitState.Idle:
                anim.SetFloat("RunState", 0f);
                break;

            case MyInfo.eUnitState.Walk: //Run
                anim.SetFloat("RunState", 0.5f);
                break;

            case MyInfo.eUnitState.Dead: //Death
                anim.SetTrigger("Die");
                anim.SetBool("EditChk", EditChk);
                break;
           
            case MyInfo.eUnitState.Attack:
                {
                    if (unitType == MyInfo.eUnitType.Swordsman) //Attack Sword
                    {
                        anim.SetTrigger("Attack");
                        anim.SetFloat("AttackState", 0.0f);
                        anim.SetFloat("NormalState", 0.0f);
                        break;
                    }
                    else if (unitType == MyInfo.eUnitType.Bower)
                    {
                        anim.SetTrigger("Attack");
                        anim.SetFloat("AttackState", 0.0f);
                        anim.SetFloat("NormalState", 0.5f);
                        break;
                    }
                    else if(unitType == MyInfo.eUnitType.Magician)
                    {
                        anim.SetTrigger("Attack");
                        anim.SetFloat("AttackState", 0.0f);
                        anim.SetFloat("NormalState", 1.0f);
                        break;
                    }
                    break;
                }

                //case 3: //Stun
                //    anim.SetFloat("RunState", 1.0f);
                //    break;

                //case MyInfo.eUnitState.Attack: //Skill Sword
                //    anim.SetTrigger("Attack");
                //    anim.SetFloat("AttackState", 1.0f);
                //    anim.SetFloat("SkillState", 0.0f);
                //    break;

                //case MyInfo.eUnitState.Attack: //Skill Bow
                //    anim.SetTrigger("Attack");
                //    anim.SetFloat("AttackState", 1.0f);
                //    anim.SetFloat("SkillState", 0.5f);
                //    break;

                //case MyInfo.eUnitState.Attack: //Skill Magic
                //    anim.SetTrigger("Attack");
                //    anim.SetFloat("AttackState", 1.0f);
                //    anim.SetFloat("SkillState", 1.0f);
                //    break;
        }
    }
    private void TryFindEnemy()
    {
        if (curTarget == null)
        {
            float direction = isMyUnit ? 1 : -1;
            transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);
            curTarget = FindNearestEnemy();

            if (curTarget != null)
            {
                SetState(MyInfo.eUnitState.Attack);
            }
        }
    }

    private void HandleAttack()
    {
        if (curTarget == null)
        {
            SetState(MyInfo.eUnitState.Walk);
            return;
        }

        float distance = Vector2.Distance(transform.position, curTarget.transform.position);
        if (distance > attackDistance)
        {
            curTarget = null;
            SetState(MyInfo.eUnitState.Walk);
            return;
        }

        attackDelay += Time.deltaTime;
        if (attackDelay >= attackCT)
        {
            attackDelay = 0f;
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        if (curTarget == null) return;
        weapon.SetWeapon(unitType);
        weapon.StartAttack();
    }

    private GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(isMyUnit ? "Enemy" : "Mine");
        if (enemies == null || enemies.Length == 0) return null;

        GameObject nearest = null;
        float minDistance = attackDistance;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                nearest = enemy;
                minDistance = distance;
            }
        }
        return nearest;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
