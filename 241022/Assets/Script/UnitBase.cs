using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unit.UnitDataContainer;

public class UnitBase : MonoBehaviour
{
    private WalkState walkState;
    private DeadState deadState;
    public IUnitState currentState;

    public float moveSpeed = 2.0f;
    [SerializeField] Rigidbody2D rigid;

    [SerializeField] public eUnitType unitType = eUnitType.Swordmaster;
    [SerializeField] Weapon.eWeaponType weaponType = Weapon.eWeaponType.Sword; 
    [SerializeField] public Weapon weapon;
    [SerializeField] public SpriteRenderer image;
    [SerializeField] HPbar hpImg;

    [SerializeField] public Animator anim = new Animator();

    public int ID;
    public float attackDistance;
    public float attackCT = 3;
    public float attackDelay;
    public int ATK;

    public UnitBase curTarget;
    public bool isMyUnit = true;
    public bool isAttack = false;
    public bool isDead = false;

    public int curHP;
    public int maxHP;

    public int GetUnitID() { return ID; }


    private void Awake()
    {
        walkState = new WalkState(this);
        deadState = new DeadState(this);
    }

    void Start()
    {
        isDead = false;
        ChangeState(walkState);
    }

    void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(IUnitState nextState)
    {
        currentState?.Exit();

        if (isDead)
            return;
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
        maxHP = data.hp;
        curHP = data.hp;
        ATK = data.atk;
        unitType = (eUnitType)data.unitType;
        weapon.SetWeapon(unitType, this);
        ChangeState(walkState);

        image.sprite = data.spriteImg;
        image.flipX = isMyUnit;

        anim.runtimeAnimatorController = data.animCtrl;
    }

    public float DistanceToTarget() =>
      curTarget == null ? float.MaxValue : Vector2.Distance(transform.position, curTarget.transform.position);


    public NexusInfo FindTargetNexus()
    {
        NexusInfo nexusTarget = isMyUnit ? StageManager.Instance.spawner.enemyNexus : StageManager.Instance.spawner.myNexus;

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
        List<UnitBase> enemies = isMyUnit ? StageManager.Instance.spawner.enemyUnitList : StageManager.Instance.spawner.myUnitList;

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
        if (curHP <= 0 || isDead) return;
        curHP -= dmg;
        hpImg.UpdateBar(curHP, maxHP);
        //ShowDamageText(damage);

        if (curHP <= 0)
        {
            ChangeState(deadState);

            isDead = true;
        }
    }

    public void HealHP(int amount)
    {
        if (curHP <= 0 || isDead) return;
        if (curHP + amount > maxHP)
            curHP = maxHP;
        else
            curHP += amount;

        hpImg.UpdateBar(curHP, maxHP);
    }

    public void LaunchArrow()
    {
        weapon.LaunchArrow(gameObject, curTarget.transform.position, ATK, curTarget);
    }

    public void Deactivate()
    {
        ObjectPoolManager.Instance.ReturnToPool(isMyUnit ? ObjectPoolManager.ePoolingObj.MyUnit : ObjectPoolManager.ePoolingObj.Enemy, gameObject);
    }
}

