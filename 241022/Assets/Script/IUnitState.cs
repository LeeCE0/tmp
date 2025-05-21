using System.Collections;
using System.Collections.Generic;
using Unit;
using UnityEngine;
public interface IUnitState
{
    MyInfo.eUnitState StateType { get; }
    void Enter();
    void Update();
    void Exit();
}

public class WalkState : IUnitState
{
    private UnitInfo unit;
    public MyInfo.eUnitState StateType => MyInfo.eUnitState.Walk;

    public WalkState(UnitInfo unit) => this.unit = unit;

    public void Enter()
    {
        unit.attackDelay = 0f;
        unit.isAttack = false;
    }

    public void Update() 
    {
        if (unit.curTarget == null)
        {
            unit.anim.SetFloat("RunState", 0.5f);
            float dir = unit.isMyUnit ? 1f : -1f;
            unit.transform.Translate(Vector2.right * dir * unit.moveSpeed * Time.deltaTime);
            unit.curTarget = unit.FindNearestEnemy();
        }

        if (unit.curTarget != null)
        {
            Debug.LogError("Find Enemy");
            unit.ChangeState(new AttackState(unit));
        }
    }

    public void Exit()
    {
        unit.anim.SetFloat("RunState", 0f);
    }
}

public class AttackState : IUnitState
{
    private UnitInfo unit;
    public MyInfo.eUnitState StateType => MyInfo.eUnitState.Attack;

    public AttackState(UnitInfo unit) => this.unit = unit;

    public void Enter()
    {
        unit.isAttack = true; 
        unit.attackDelay = unit.attackCT;
    }

    public void Update()
    {
        if (unit.curTarget == null || unit.DistanceToTarget() > unit.attackDistance)
        {
            unit.curTarget = null;
            unit.ChangeState(new WalkState(unit));
            return;
        }

        unit.attackDelay += Time.deltaTime;
        if (unit.attackDelay >= unit.attackCT)
        {
            unit.attackDelay = 0f;
            PerformAttack();
        }
    }

    public void Exit()
    {
        unit.isAttack = false;
    }

    private void PerformAttack()
    {
        if (unit.curTarget == null) return;
        unit.weapon.SetWeapon(unit.unitType);
        unit.anim.SetTrigger("Attack");
        unit.anim.SetFloat("AttackState", 0f);

        switch (unit.unitType)
        {
            case MyInfo.eUnitType.Swordsman:
                unit.anim.SetFloat("NormalState", 0f);
                unit.anim.SetFloat("RunState", 0f);
                break;
            case MyInfo.eUnitType.Bower:
                unit.anim.SetFloat("NormalState", 0.5f);
                unit.anim.SetFloat("RunState", 0f);
                break;
            case MyInfo.eUnitType.Magician:
                unit.anim.SetFloat("NormalState", 1.0f);
                unit.anim.SetFloat("RunState", 0f);
                break;
        }

        unit.weapon.StartAttack();
    }
}
public class DeadState : IUnitState
{
    public MyInfo.eUnitState StateType => MyInfo.eUnitState.Dead;
    private UnitInfo unit;
     public DeadState(UnitInfo unit) => this.unit = unit;
    public void Enter()
    {

    }
    public void Update()
    {

    }
    public void Exit()
    {
       
    }
}