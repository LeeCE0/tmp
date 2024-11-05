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
    [SerializeField] bool isMyUnit = true;

    void Update()
    {
        if (curTarget == null)
        {
            SetState(MyInfo.eUnitState.Walk);
            // 적 스폰지점으로 이동
            if (isMyUnit)
                transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            else
                transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

            // 감지 범위 내에서 가장 가까운 적을 찾음
            curTarget = FIndNearEnemy();
        }

        else
        {
            attackDelay += Time.deltaTime;
            if (attackDelay >= attackCT)
            {
                SetState(MyInfo.eUnitState.Attack);
                Debug.LogError("Attack");
                attackDelay = 0f;
            }
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

    }

    public void SetState(MyInfo.eUnitState state)
    {
        unitState = state;
        
        for (int i = 0; i < anim.Length; i++)
        {
            switch (state)
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

    public void SetAnim()
    {
        switch (unitState)
        {
            case MyInfo.eUnitState.Idle:
                for(int i = 0; i < anim.Length; i++)
                {
                }
                break;
            case MyInfo.eUnitState.Walk:

                break;
            case MyInfo.eUnitState.Attack:
                break;
            case MyInfo.eUnitState.Dead:
                break;
        }

    }

    public GameObject FIndNearEnemy()
    {
        GameObject[] enemies = null;
        if (isMyUnit)
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
        else
            enemies = GameObject.FindGameObjectsWithTag("Mine");

        if (enemies == null)
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
