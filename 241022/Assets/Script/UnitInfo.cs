using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using Unity.VisualScripting;
using System;
using UnityEngine.UIElements;
using UnityEngine.SocialPlatforms;

public class UnitInfo : MonoBehaviour
{
    private WalkState walkState;
    private AttackState attackState;
    private DeadState deadState;
    private IUnitState currentState;

    public float moveSpeed = 2.0f;
    [SerializeField] Rigidbody2D rigid;

    [SerializeField] public MyInfo.eUnitType unitType = MyInfo.eUnitType.Swordsman;
    [SerializeField] Weapon.eWeaponType weaponType = Weapon.eWeaponType.Sword;
    [SerializeField] public Weapon weapon;

    [SerializeField] public Animator anim = new Animator();

    public int ID;
    public float attackDistance;
    public float attackCT = 3;
    public float attackDelay;

    public GameObject curTarget;
    public bool isMyUnit = true;
    public bool isBullet = false;
    public bool isAttack = false;   


    public int GetUnitID() { return ID; }


    void Start()
    {
        walkState = new WalkState(this);
        attackState = new AttackState(this);
        deadState = new DeadState(this);
        ChangeState(walkState);
    }

    void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(IUnitState nextState)
    {
        currentState?.Exit();
        currentState = nextState;
        currentState?.Enter();
    }

    // 오브젝트 풀에서 꺼낼 때 호출할 초기화
    public void Initialize(MyInfo.UnitData data)
    {
        gameObject.SetActive(true); // 풀에서 꺼낼 때 활성화

        ID = data.UnitID;
        moveSpeed = data.UnitSpeed;
        attackDistance = data.AttackDistance;
        curTarget = null;
        attackDelay = 0f;
        isAttack = false;

        anim.Rebind();       
        anim.Update(0f);

        ChangeState(new WalkState(this));
    }   

    public void SetSpawn(MyInfo.UnitData data)
    {
        ID = data.UnitID;
        moveSpeed = data.UnitSpeed;
        attackDistance = data.AttackDistance;
        attackCT = 2f;
        curTarget = null;

       ChangeState(new WalkState(this));

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (isMyUnit ? -1 : 1);
        transform.localScale = scale;
    }
    public float DistanceToTarget() =>
      curTarget == null ? float.MaxValue : Vector2.Distance(transform.position, curTarget.transform.position);

    public GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(isMyUnit ? "Enemy" : "Mine");
        if (enemies == null || enemies.Length == 0) return null;

        GameObject nearest = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < attackDistance)
            {
                nearest = enemy;
            }
        }
        return nearest;
    }
}
