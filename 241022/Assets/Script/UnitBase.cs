using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unit.UnitDataContainer;

public class UnitBase : MonoBehaviour
{
    private WalkState walkState;
    private DeadState deadState;
    private IUnitState currentState;

    public float moveSpeed = 2.0f;
    [SerializeField] Rigidbody2D rigid;

    [SerializeField] public eUnitType unitType = eUnitType.Swordmaster;
    [SerializeField] Weapon.eWeaponType weaponType = Weapon.eWeaponType.Sword; 
    [SerializeField] public Weapon weapon;
    [SerializeField] Sprite image;

    [SerializeField] public Animator anim = new Animator();

    public int ID;
    public float attackDistance;
    public float attackCT = 3;
    public float attackDelay;
    public int ATK;

    public UnitBase curTarget;
    public bool isMyUnit = true;
    public bool isBullet = false;
    public bool isAttack = false;

    public int curHP;

    public int GetUnitID() { return ID; }


    void Start()
    {
        walkState = new WalkState(this);
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
    public void Initialize()
    {
        gameObject.SetActive(true); 
        anim.Rebind();       
        anim.Update(0f);

        ChangeState(walkState);
    }   

    public void SetSpawn(UnitsData data)
    {
        ID = data.unitID;
        moveSpeed = data.unitSpeed;
        attackDistance = data.attackDistance;
        attackCT = 2f;
        curTarget = null;
        curHP = data.hp;
        ATK = data.atk;
        unitType = (eUnitType)data.unitType;
        weapon.SetWeapon(unitType, this);
        ChangeState(walkState);


        //스프라이트 교체
        image = data.spriteImg;

        //애니메이션 교체
        anim.runtimeAnimatorController = data.animCtrl;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (isMyUnit ? -1 : 1);
        transform.localScale = scale;
    }
    public float DistanceToTarget() =>
      curTarget == null ? float.MaxValue : Vector2.Distance(transform.position, curTarget.transform.position);


    public NexusInfo FindNexus()
    {
        NexusInfo nexusTarget = isMyUnit ? SpawnUnitManager.Instance.enemyNexus : SpawnUnitManager.Instance.myNexus;

        if (nexusTarget != null)
        {
            float distance = Vector2.Distance(transform.position, nexusTarget.transform.position);
            if (distance < attackDistance)
                return nexusTarget;
        }
        return null;
    }

    public UnitBase FindNearestEnemy()
    {
        List<UnitBase> enemies = isMyUnit ? SpawnUnitManager.Instance.enemyUnitList : SpawnUnitManager.Instance.myUnitList;

        if (enemies == null || enemies.Count == 0) return null;

        UnitBase nearest = null;

        foreach (UnitBase enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < attackDistance)
            {
                nearest = enemy;
            }
        }
        return nearest;
    }

    public void TakeDMG(int dmg)
    {
        if (curHP <= 0) return;
        curHP -= dmg;
        //hpBar.SetFill((float)currentHP / maxHP);
        //ShowDamageText(damage);

        if (curHP <= 0)
            ChangeState(deadState);
    }

    public void SetUnit()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (isMyUnit ? -1 : 1);
        transform.localScale = scale;
    }

    public void Deactivate()
    {
        string tag = isMyUnit ? ID.ToString() : "Enemy";
        ObjectPoolManager.Instance.ReturnToPool(ObjectPoolManager.ePoolingObj.MyUnit, gameObject);
    }
}

